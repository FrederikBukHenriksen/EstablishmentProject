using WebApplication1.Commands;

namespace WebApplication1.CommandHandlers
{
    public interface ICommandHandler<TCommand, TCommandReturn>
        where TCommand : ICommand
        where TCommandReturn : CommandReturn.ICommandHandlerReturn
    {
        Task<CommandReturn.ICommandHandlerReturn> ExecuteAsync(ICommand command, CancellationToken cancellationToken);
    }

}