using WebApplication1.CommandHandlers.CommandReturn;
using WebApplication1.Commands;

namespace WebApplication1.CommandHandlers
{
    public class LoginCommandHandler : ICommandHandler<LoginCommand,ICommandHandlerReturn>
    {

        public Task<ICommandHandlerReturn> ExecuteAsync(ICommand command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
