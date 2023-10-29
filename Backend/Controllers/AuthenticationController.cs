using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using WebApplication1.CommandHandlers;
using WebApplication1.Commands;
using WebApplication1.Models;
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
            try
            {
                var jwtTokenString = loginCommandHandler.ExecuteAsync(loginCommand, new CancellationToken());

                this.HttpContext.Response.Cookies.Append("jwt", jwtTokenString.Result, new CookieOptions { HttpOnly = true, Secure = true, IsEssential = true, SameSite = SameSiteMode.None });
                return;
            }
            catch (Exception e)
            {
                this.HttpContext.Response.StatusCode = 401;
                return;
            }
        }

        [AllowAnonymous]
        [HttpPost("logout")]
        public async void LogOut([FromServices] IAuthService authenticationService)
        {
            this.HttpContext.Response.Cookies.Delete("jwt");
            return;
        }


        [AllowAnonymous]
        [HttpGet("is-logged-in")]
        public bool IsLoggedIn([FromServices] IAuthService authenticationService)
        {
            return authenticationService.GetUserFromGuid(this.HttpContext) != null;
        }


        [Authorize]
        [HttpGet("get-user-info")]
        public User GetLoggedInUser([FromServices] IUserContextService userContextService)
        {
            string? cookie = HttpContext.Request.Cookies["jwt"];
            try
            {
                return userContextService.GetUser();
            } catch (Exception e) {
                this.HttpContext.Response.StatusCode = 401;
                return null;
            };
        }



    }
}