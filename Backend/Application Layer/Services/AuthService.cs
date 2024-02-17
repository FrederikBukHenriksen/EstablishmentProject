using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Domain_Layer.Services.Repositories;

namespace WebApplication1.Services
{

    public interface IAuthService
    {
        public User Login(string username, string password);
        public User? GetUserFromHttp(HttpContext httpContext);
        internal string GenerateJwtToken(Guid id);
    }

    public class AuthService : IAuthService
    {
        public AuthService([FromServices] IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        private static readonly string _tokenName = "jwt";
        private static readonly string _securityKey = "this is my custom Secret key for authentication";

        private readonly IUserRepository userRepository;

        public string GenerateJwtToken(Guid id)
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_securityKey));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


            Claim[] claims = new[] { new Claim("username", id.ToString()),
            };

            JwtSecurityToken token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: credentials
            );
            string jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        public User Login(string username, string password)
        {
            User? user = this.userRepository.Find(x => ((x.Email.ToLower()) == (username.ToLower())) && x.Password == password);
            if (user == null) throw new Exception("User could not be logged in based on the given credentials");
            return user;
        }
        public User? GetUserFromHttp(HttpContext httpContext)
        {
            string? token = httpContext.Request.Cookies["jwt"];
            if (token.IsNullOrEmpty())
            {
                return null;
            }
            string? usernameClaim = GetClaimValue(token, "username");
            User? user = this.userRepository.IncludeUserRoles().GetById(Guid.Parse(usernameClaim));
            if (usernameClaim == null || user == null)
            {
                return null;
            }
            return user;
        }

        private static string? GetClaimValue(string token, string claimType)
        {
            JwtSecurityToken securityToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var claim = securityToken.Claims.FirstOrDefault(c => c.Type == claimType);
            if (claim == null)
            {
                return null;
            }
            return claim.Value;
        }
    }
}
