using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Application_Layer.Services;
using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Services
{

    public interface IAuthenticationService
    {
        public User Login(string username, string password);
        public User? ExtractUserFromJwt(HttpContext httpContext);
        internal string GenerateJwtToken(Guid id);
    }

    public class AuthenticationService : IAuthenticationService
    {
        public AuthenticationService([FromServices] IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        private static readonly string _securityKey = "this is my custom Secret key for authentication";

        private readonly IUnitOfWork unitOfWork;

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
            User? user = this.unitOfWork.userRepository.Find(x => ((x.Email.ToLower()) == (username.ToLower())) && x.Password == password);
            if (user == null) throw new Exception("User could not be logged in based on the given credentials");
            return user;
        }
        public User? ExtractUserFromJwt(HttpContext httpContext)
        {
            string? token = httpContext.Request.Cookies["jwt"];
            if (token.IsNullOrEmpty())
            {
                return null;
            }
            string usernameClaim = GetClaimValue(token, "username");
            User? user = this.unitOfWork.userRepository.IncludeUserRoles().GetById(Guid.Parse(usernameClaim));
            if (usernameClaim == null || user == null)
            {
                return null;
            }
            return user;
        }

        private static string GetClaimValue(string token, string claimType)
        {
            JwtSecurityToken securityToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var claim = securityToken.Claims.FirstOrDefault(c => c.Type == claimType);
            return claim.Value;
        }
    }
}
