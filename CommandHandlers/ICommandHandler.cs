using WebApplication1.Commands;

namespace WebApplication1.CommandHandlers
{
    public interface ICommandHandler<TCommand, TReturn>
        where TCommand : ICommand
    {
        public Task<TReturn> ExecuteAsync(TCommand command, CancellationToken cancellationToken);
    }
}