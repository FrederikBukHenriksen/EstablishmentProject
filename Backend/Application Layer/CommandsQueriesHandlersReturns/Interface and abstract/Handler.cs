using WebApplication1.CommandsHandlersReturns;

namespace WebApplication1.CommandHandlers
{
    public interface IHandler<TCommand, TReturn>
        where TCommand : ICommand
    {
        public TReturn Handle(TCommand command);
    }

    public abstract class HandlerBase<TCommand, TReturn> : IHandler<TCommand, TReturn>
    where TCommand : ICommand
    {
        public abstract TReturn Handle(TCommand command);
    }
}