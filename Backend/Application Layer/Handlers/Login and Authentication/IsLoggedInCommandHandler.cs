using WebApplication1.Application_Layer.Services.Authentication_and_login;
using WebApplication1.CommandHandlers;
using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Application_Layer.Handlers.Login_and_Authentication
{
    public class IsLoggedInCommand : ICommand
    {
        public string JWT { get; set; }
    }

    public class IsLoggedInReturn : IReturn
    {
        public bool isLoggedIn { get; set; }
    }

    public class IsLoggedInCommandHandler : HandlerBase<IsLoggedInCommand, IsLoggedInReturn>
    {
        private readonly IJWTService JWTService;

        public IsLoggedInCommandHandler(
             IJWTService JWTService)
        {
            this.JWTService = JWTService;
        }

        public override async Task<IsLoggedInReturn> Handle(IsLoggedInCommand command)
        {
            User? user = this.JWTService.ExtractUserFromJWT(command.JWT);
            if (user == null) return new IsLoggedInReturn { isLoggedIn = false };
            return new IsLoggedInReturn { isLoggedIn = true };
        }
    }
}
