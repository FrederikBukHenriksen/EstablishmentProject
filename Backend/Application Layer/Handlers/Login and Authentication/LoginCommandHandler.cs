using WebApplication1.Application_Layer.Services;
using WebApplication1.Application_Layer.Services.Authentication_and_login;
using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.CommandHandlers
{
    public class LoginCommand : ICommand
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginReturn : IReturn
    {
        public string JWT { get; set; }
    }

    public class LoginCommandHandler : HandlerBase<LoginCommand, LoginReturn>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IJWTService JWTService;

        public LoginCommandHandler(
            IUnitOfWork unitOfWork,
             IJWTService JWTService)
        {
            this.unitOfWork = unitOfWork;
            this.JWTService = JWTService;
        }

        public override async Task<LoginReturn> Handle(LoginCommand command)
        {
            User? user = this.unitOfWork.userRepository.Find(x => ((x.Email.ToLower()) == (command.Username.ToLower())) && x.Password == command.Password);
            if (user == null) throw new Exception("User could not be logged in based on the given credentials");
            string token = this.JWTService.GenerateJwtTokenForUser(user);
            return new LoginReturn { JWT = token };
        }
    }
}