using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.CommandHandlers;
using WebApplication1.CommandHandlers.CommandReturn;
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
            [FromServices] ICommandHandler<LoginCommand,ICommandHandlerReturn> loginCommandHandler,
            [FromServices] IAuthenticationService authenticationService)
        {

            var ok = loginCommandHandler.ExecuteAsync(loginCommand, new CancellationToken());


            if (!(loginCommand.Username.Equals("frederik") && loginCommand.Password.Equals("1234")))
            {
                return StatusCode(500);
            }
            string jwt = authenticationService.GenerateJwtToken("frederik", new List<Role> { Role.Admin });

            HttpContext.Response.Cookies.Append("jwt", jwt, new CookieOptions { HttpOnly = true, Secure = true, IsEssential = true, SameSite = SameSiteMode.None });
            return Ok();
        }
        [Authorize(Roles = "Admin, User")]
        [HttpGet("get-user-info")]
        public User getUser([FromServices] IAuthenticationService authenticationService)
        {
            string? cookie = HttpContext.Request.Cookies["jwt"];

            return new User { Username = "frederik", Role = authenticationService.GetUserInfo(cookie) };
        }

    }
}