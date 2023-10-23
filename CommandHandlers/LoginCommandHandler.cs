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
        private readonly IUserRepository userRepository;

        public LoginCommandHandler(
            [FromServices] IAuthService authService, IUserRepository userRepository)
        {
            this.authService = authService;
            this.userRepository = userRepository;
        }

            public override async Task<string> ExecuteAsync(LoginCommand command, CancellationToken cancellationToken)
            {
                bool loginFound = authService.Login(command.Username, command.Password);
                User? user = await userRepository.Queryable.Where(x => x.Username == command.Username).FirstOrDefaultAsync(cancellationToken);
                if (!loginFound || user == null)
                {
                    throw new Exception("Login failed");
                }

                var result = authService.GenerateJwtToken(user.Id);
                return result;
            }
    }
}