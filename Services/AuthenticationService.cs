using System.IdentityModel.Tokens.Jwt;
using YamlDotNet.Core.Tokens;

namespace WebApplication1.Services
{

    public interface IAuthenticationService
    {
        public string getUserInfo(string token);

    }

    public class AuthenticationService : IAuthenticationService
    {
        private JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

        public string getUserInfo(string token)
        {
            var secutiryToken = jwtSecurityTokenHandler.ReadJwtToken(token);
            var name = secutiryToken.Claims.First().Value;
            return name;
        }
    }
}
