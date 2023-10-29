using WebApplication1.Commands;

namespace WebApplication1.CommandHandlers
{

    public abstract class CommandHandlerBase<TCommand, TReturn> : ICommandHandler<TCommand, TReturn>
        where TCommand : ICommand
    {
        public abstract Task<TReturn> ExecuteAsync(TCommand command, CancellationToken cancellationToken);
    }
}
