using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Extensions;
using Namotion.Reflection;
using System.Data;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Commands;
using WebApplication1.Repositories;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/authentication")]
    public class AuthenticationController : ControllerBase
    {
        public AuthenticationController()
        {}

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult login([FromBody] LoginCommand command)
        {
            if (!(command.Username.Equals("frederik") && command.Password.Equals("1234")))
            {
                return StatusCode(500);
            }
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom Secret key for authentication"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {new Claim("roles","Admin")};


            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: credentials
            );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            this.HttpContext.Response.Cookies.Append("jwt", jwt, new CookieOptions {  HttpOnly = true, Secure = true, IsEssential = true, SameSite = SameSiteMode.None});
            return Ok();
        }
        [Authorize(Roles = "Admin, User")]
        [HttpGet("get-user-info")]
        public User getUser([FromServices] IAuthenticationService authenticationService)
        {
            var cookie = this.HttpContext.Request.Cookies["jwt"];

            return new User { Username = "frederik", Role = authenticationService.getUserInfo(cookie) };
        }

    }
}