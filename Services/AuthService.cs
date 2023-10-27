using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Repositories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WebApplication1.Services
{

    public interface IAuthService
    {

        public bool Login(string username, string password);

        public Guid? GetUserGuid(HttpContext httpContext);

        public string GenerateJwtToken(Guid id);
    }

    public class AuthService : IAuthService
    {
        public AuthService([FromServices] IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        private static readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        private static readonly string _tokenName = "jwt";
        private static readonly string _securityKey = "this is my custom Secret key for authentication";

        private readonly IUserRepository userRepository;

        public string GenerateJwtToken(Guid id)
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_securityKey));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


            Claim[] claims = new[] {new Claim("username", id.ToString())};

            JwtSecurityToken token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: credentials
            );
            string jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        public bool Login(string username, string password)
        {
            return userRepository.Contains(x => x.Username == username && x.Password == password);
        }

        public Guid? GetUserGuid(HttpContext httpContext)
        {
            string? token = httpContext.Request.Cookies["jwt"];
            string? usernameClaim = GetClaimValue(token, "username");
            if (usernameClaim == null || usernameClaim == null) {
                return null;
            }
            return new Guid(usernameClaim);
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
