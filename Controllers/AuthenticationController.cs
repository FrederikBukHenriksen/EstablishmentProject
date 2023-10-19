using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.CommandHandlers;
using WebApplication1.Commands;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/authentication")]
    public class AuthenticationController : ControllerBase
    {

        [AllowAnonymous]
        [HttpPost("login")]
        public async void LogIn(
            [FromBody] LoginCommand loginCommand,
            [FromServices] ICommandHandler<LoginCommand,string> loginCommandHandler
            )
        {
            var jwtTokenString = await loginCommandHandler.ExecuteAsync(loginCommand, new CancellationToken());
            this.HttpContext.Response.Cookies.Append("jwt", jwtTokenString, new CookieOptions { HttpOnly = true, Secure = true, IsEssential = true, SameSite = SameSiteMode.None });
            return;
        }

        [Authorize]
        [HttpPost("logout")]
        public async void LogOut([FromServices] IAuthService authenticationService)
        {
            this.HttpContext.Response.Cookies.Delete("jwt");
            return;
        }


        [Authorize]
        [HttpGet("is-logged-in")]
        public bool IsLoggedIn([FromServices] IAuthService authenticationService)
        {
            return true;
        }


        [Authorize(Roles = "Admin, User")]
        [HttpGet("get-user-info")]
        public User GetLoggedInUser([FromServices] IAuthService authenticationService)
        {
            string? cookie = HttpContext.Request.Cookies["jwt"];
            User user = authenticationService.GetUserInfo(cookie);
            if (user == null)
            {
                throw new Exception("User not found");
            };
            return user;
        }



    }
}