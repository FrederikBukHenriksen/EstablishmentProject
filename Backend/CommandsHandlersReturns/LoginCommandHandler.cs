using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Repositories;
using WebApplication1.Services;


namespace WebApplication1.CommandHandlers
{
    public class LoginCommand : ICommand
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginCommandHandler : HandlerBase<LoginCommand, string>
    {
        private readonly IAuthService authService;

        public LoginCommandHandler(
            [FromServices] IAuthService authService)
        {
            this.authService = authService;
        }

            public override string Execute(LoginCommand command)
            {
                User user = authService.Login(command.Username, command.Password);
                var result = authService.GenerateJwtToken(user.Id);
                return result;
            }
    }
}