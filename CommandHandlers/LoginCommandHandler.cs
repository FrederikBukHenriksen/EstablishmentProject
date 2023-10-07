using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApplication1.Commands;
using WebApplication1.Repositories;
using WebApplication1.Services;

namespace WebApplication1.CommandHandlers
{
    public class LoginCommandHandler : CommandHandlerBase<LoginCommand, string>
    {
        private readonly IAuthService authService;
            
        public LoginCommandHandler(
            [FromServices] IAuthService authService)
        {
            this.authService = authService;
        }

        public override Task<string> ExecuteAsync(LoginCommand command, CancellationToken cancellationToken)
        {
            var login = authService.Login(command.Username, command.Password);
            if (login != null)
            {
                throw new Exception("Could not be signed in");
            }
            var result = authService.GenerateJwtToken(login.Username, new List<Role> { Role.Admin });
            return Task.Run(() => { return result; });
        }
    }
}