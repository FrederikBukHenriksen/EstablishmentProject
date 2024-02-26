using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using WebApplication1.Application_Layer.Handlers.Login_and_Authentication;
using WebApplication1.Application_Layer.Services.Authentication_and_login;
using WebApplication1.CommandHandlers;

namespace WebApplication1.Controllers
{
    [ExcludeFromCodeCoverage]
    [ApiController]
    [Route("api/authentication")]
    public class AuthenticationController : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> LogIn(
            [FromBody] LoginCommand command,
            [FromServices] IHandler<LoginCommand, LoginReturn> handler
            )
        {
            LoginReturn loginReturn = await handler.Handle(command);
            this.HttpContext.Response.Cookies.Append("jwt", loginReturn.JWT, new CookieOptions { HttpOnly = true, Secure = true, IsEssential = true, SameSite = SameSiteMode.None });
            return this.Ok();
        }


        [AllowAnonymous]
        [HttpGet("is-logged-in")]
        public async Task<ActionResult<IsLoggedInReturn>> IsLoggedIn([FromServices] IJWTService JWTService, IHandler<IsLoggedInCommand, IsLoggedInReturn> handler)
        {
            string JWT = JWTService.ExtractJwtFromRequest(this.HttpContext);
            IsLoggedInCommand command = new IsLoggedInCommand { JWT = JWT };
            return await handler.Handle(command);
        }

        [Authorize]
        [HttpGet("logout")]
        public ActionResult LogOut()
        {
            this.HttpContext.Response.Cookies.Delete("jwt");
            return this.Ok();
        }
    }
}