using WebApplication1.CommandsHandlersReturns;

namespace WebApplication1.CommandHandlers
{
    public interface IHandler<TCommand, TReturn>
        where TCommand : ICommand
        where TReturn : IReturn
    {
        Task<TReturn> Handle(TCommand command);
    }

    public abstract class HandlerBase<TCommand, TReturn> : IHandler<TCommand, TReturn>
        where TCommand : ICommand
        where TReturn : IReturn
    {
        public abstract Task<TReturn> Handle(TCommand command);
    }
}
