using WebApplication1.CommandsHandlersReturns;

namespace WebApplication1.CommandHandlers
{
    public interface IHandler<TCommand, TReturn>
        where TCommand : ICommand
    {
        public TReturn Execute(TCommand command);
    }

    public abstract class HandlerBase<TCommand, TReturn> : IHandler<TCommand, TReturn>
    where TCommand : ICommand
    {
        public abstract TReturn Execute(TCommand command);
    }
}