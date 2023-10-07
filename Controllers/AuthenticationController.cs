using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult login(
            [FromBody] LoginCommand loginCommand,
            [FromServices] ICommandHandler<LoginCommand,string> loginCommandHandler
            )
        {
            var jwt = loginCommandHandler.ExecuteAsync(loginCommand, new CancellationToken()).Result;
            this.HttpContext.Response.Cookies.Append("jwt", jwt, new CookieOptions { HttpOnly = true, Secure = true, IsEssential = true, SameSite = SameSiteMode.None });
            return Ok();
        }
        [Authorize(Roles = "Admin, User")]
        [HttpGet("get-user-info")]
        public User getUser([FromServices] IAuthService authenticationService)
        {
            string? cookie = HttpContext.Request.Cookies["jwt"];

            return new User { Username = "frederik", Role = authenticationService.GetUserInfo(cookie) };
        }

    }
}