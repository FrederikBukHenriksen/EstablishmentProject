using Microsoft.AspNetCore.Mvc;
using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Domain.Entities;
using WebApplication1.Services;


namespace WebApplication1.CommandHandlers
{
    public class LoginCommand : ICommand
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginReturn : IReturn
    {
        public string Token { get; set; }
    }

    public class LoginCommandHandler : HandlerBase<LoginCommand, LoginReturn>
    {
        private readonly IAuthService authService;

        public LoginCommandHandler(
            [FromServices] IAuthService authService)
        {
            this.authService = authService;
        }

        public override LoginReturn Handle(LoginCommand command)
        {
            User user = this.authService.Login(command.Username, command.Password);
            var result = this.authService.GenerateJwtToken(user.Id);
            return new LoginReturn { Token = result };
        }
    }
}