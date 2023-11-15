using WebApplication1.Commands;

namespace WebApplication1.CommandHandlers
{

    public abstract class CommandHandlerBase<TCommand, TReturn> : ICommandHandler<TCommand, TReturn>
        where TCommand : ICommand
    {
        public abstract TReturn Execute(TCommand command);
    }
}
