using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Domain.Entities;
using WebApplication1.Domain.Services.Repositories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WebApplication1.Services
{

    public interface IAuthService
    {
        public User Login(string username, string password);
        public Guid? GetUserFromHttp(HttpContext httpContext);
        public string GenerateJwtToken(Guid id);
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
            User? user = userRepository.Find(x => ((x.Username.ToLower()) == (username.ToLower())) && x.Password == password);
            if (user == null) throw new Exception("User could not be logged in based on the given credentials");
            return user;
        }

        public Guid? GetUserFromHttp(HttpContext httpContext)
        {
            string? token = httpContext.Request.Cookies["jwt"];
            if (token == null)
            {
                return null;
            }
            string? usernameClaim = GetClaimValue(token, "username");
            User? user = userRepository.Find(x => x.Id == Guid.Parse(usernameClaim));
            if (usernameClaim == null || user == null)
            {
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
