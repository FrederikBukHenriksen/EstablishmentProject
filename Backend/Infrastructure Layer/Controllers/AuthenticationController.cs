using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using WebApplication1.CommandHandlers;
using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Data.DataModels;
using WebApplication1.Domain.Entities;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/authentication")]
    public class AuthenticationController : ControllerBase
    {

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult LogIn(
            [FromBody] LoginCommand loginCommand,
            [FromServices] IHandler<LoginCommand, LoginReturn> loginCommandHandler
            )
        {
            try
            {
                LoginReturn loginReturn = loginCommandHandler.Handle(loginCommand);
                this.HttpContext.Response.Cookies.Append("jwt", loginReturn.Token, new CookieOptions { HttpOnly = true, Secure = true, IsEssential = true, SameSite = SameSiteMode.None });
                return Ok();
            }
            catch (Exception e)
            {
                //this.HttpContext.Response.StatusCode = 401;
                return Unauthorized(e);
            }
        }

        [AllowAnonymous]
        [HttpPost("logout")]
        public void LogOut([FromServices] IAuthService authenticationService)
        {
            this.HttpContext.Response.Cookies.Delete("jwt");
            return;
        }


        [AllowAnonymous]
        [HttpGet("is-logged-in")]
        public bool IsLoggedIn([FromServices] IAuthService authenticationService)
        {
            return authenticationService.GetUserFromHttp(this.HttpContext) != null;
        }

        [AllowAnonymous]
        [HttpGet("get-logged-in-user")]
        public User GetLoggedInUser([FromServices] IUserContextService userContextService)
        {
            return userContextService.GetUser();
        }
    }
}