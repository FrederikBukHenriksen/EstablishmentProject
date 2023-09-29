using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApplication1.Services
{

    public interface IAuthenticationService
    {
        public string GetUserInfo(string token);

        public string GenerateJwtToken(string username, List<Role> roles);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private static readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

        private static readonly string _securityKey = "this is my custom Secret key for authentication";

        public string GenerateJwtToken(string username, List<Role> roles)
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_securityKey));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            string rolesString = string.Join(", ",
                roles.Select(x => x.ToString()).ToList()
                );

            Claim[] claims = new[] {
                new Claim("username", username), new Claim("roles", rolesString)};

            JwtSecurityToken token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: credentials
            );
            string jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        public string GetUserInfo(string token)
        {
            JwtSecurityToken secutiryToken = _jwtSecurityTokenHandler.ReadJwtToken(token);
            string name = secutiryToken.Claims.First().Value;
            return name;
        }
    }
}
