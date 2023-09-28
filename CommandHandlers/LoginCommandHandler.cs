using WebApplication1.CommandHandlers.CommandReturn;
using WebApplication1.Commands;

namespace WebApplication1.CommandHandlers
{
    public class LoginCommandHandler : CommandHandlerBase
    {
        public Task<ICommandHandlerReturn> ExecuteAsync(LoginCommand command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
