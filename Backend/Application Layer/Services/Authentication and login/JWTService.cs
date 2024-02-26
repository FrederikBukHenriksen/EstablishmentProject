using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Application_Layer.Services.Authentication_and_login
{

    public interface IJWTService
    {
        public User? ExtractUserFromRequest(HttpContext httpContext);
        public string? ExtractJwtFromRequest(HttpContext httpContext);

        public User? ExtractUserFromJWT(string JWT);
        internal string GenerateJwtTokenForUser(User user);
    }
    public class JWTService : IJWTService
    {
        private IUnitOfWork unitOfWork;

        private static readonly string _securityKey = "this is my custom Secret key for authentication";
        private const string _usernameClaimType = "username";

        public JWTService([FromServices] IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public string GenerateJwtTokenForUser(User user)
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_securityKey));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


            Claim[] claims = [
                new Claim(_usernameClaimType, user.Id.ToString())
            ];

            JwtSecurityToken token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: credentials
            );
            string jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        public User? ExtractUserFromRequest(HttpContext httpContext)
        {
            string? JWT = httpContext.Request.Cookies["jwt"];
            if (JWT.IsNullOrEmpty())
            {
                return null;
            }
            return this.ExtractUserFromJWT(JWT);
        }

        private static string GetClaimValue(string token, string claimType)
        {
            JwtSecurityToken securityToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var claim = securityToken.Claims.FirstOrDefault(c => c.Type == claimType);
            return claim.Value;
        }

        public User? ExtractUserFromJWT(string JWT)
        {
            string usernameClaim = GetClaimValue(JWT, _usernameClaimType);
            User? user = this.unitOfWork.userRepository.IncludeUserRoles().GetById(Guid.Parse(usernameClaim));
            if (usernameClaim == null || user == null)
            {
                return null;
            }
            return user;
        }

        public string? ExtractJwtFromRequest(HttpContext httpContext)
        {
            string? JWT = httpContext.Request.Cookies["jwt"];
            if (JWT.IsNullOrEmpty())
            {
                return null;
            }
            return JWT;
        }
    }
}
