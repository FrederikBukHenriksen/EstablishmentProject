using WebApplication1.CommandHandlers.CommandReturn;
using WebApplication1.Commands;

namespace WebApplication1.CommandHandlers
{

    public abstract class CommandHandlerBase : ICommandHandler<ICommand, ICommandHandlerReturn>
    {
        public Task<CommandReturn.ICommandHandlerReturn> ExecuteAsync(ICommand command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
