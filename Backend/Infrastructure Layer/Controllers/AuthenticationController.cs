using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.CommandHandlers;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/authentication")]
    public class AuthenticationController : ControllerBase
    {

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LogIn(
            [FromBody] LoginCommand loginCommand,
            [FromServices] IHandler<LoginCommand, LoginReturn> loginCommandHandler
            )
        {
            try
            {
                LoginReturn loginReturn = await loginCommandHandler.Handle(loginCommand);
                this.HttpContext.Response.Cookies.Append("jwt", loginReturn.Token, new CookieOptions { HttpOnly = true, Secure = true, IsEssential = true, SameSite = SameSiteMode.None });
                return this.Ok();
            }
            catch (Exception e)
            {
                //this.HttpContext.Response.StatusCode = 401;
                return this.Unauthorized(e);
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
    }
}