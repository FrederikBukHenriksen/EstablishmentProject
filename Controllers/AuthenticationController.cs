using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Commands;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/login")]
    public class AuthenticationController : ControllerBase
    {
        public AuthenticationController(
            )
        {
        }

        [HttpGet]
    //    public string Login()
    //    {
    //        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom Secret key for authentication"));
    //        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    //        var claims = new[]
    //        {
    //    new Claim(JwtRegisteredClaimNames.Sub, "Frederik"),
    //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    //    new Claim(ClaimTypes.Role,"admin")
    //}
    //        ;

    //        var token = new JwtSecurityToken(
    //            issuer: "issuer",
    //            audience: "audience",
    //            claims: claims,
    //            expires: DateTime.UtcNow.AddMinutes(60),
    //            signingCredentials: credentials
    //        );

    //        return new JwtSecurityTokenHandler().WriteToken(token);


        //}


        [HttpPost]
        public IActionResult Loginv2()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom Secret key for authentication"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim(ClaimTypes.Role,"admin")
    }
            ;

            var token = new JwtSecurityToken(
                //issuer: "issuer",
                //audience: "audience",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: credentials
            );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            this.HttpContext.Response.Cookies.Append("jwt", jwt, new CookieOptions {  HttpOnly = true, Secure = true, IsEssential = true, SameSite = SameSiteMode.None});
            return Ok();
        }

    }
}