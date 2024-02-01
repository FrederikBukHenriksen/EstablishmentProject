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
        public async Task<ActionResult> LogIn(
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
                return this.Unauthorized(e);
            }
        }

        [AllowAnonymous]
        [HttpGet("is-logged-in")]
        public ActionResult<bool> IsLoggedIn([FromServices] IAuthService authenticationService)
        {
            return this.Ok(authenticationService.GetUserFromHttp(this.HttpContext) != null);
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