using WebApplication1.CommandsHandlersReturns;

namespace WebApplication1.CommandHandlers
{
    //public interface IHandler<TCommand, TReturn>
    //    where TCommand : ICommand
    //    where TReturn : IReturn
    //{
    //    public TReturn Handle(TCommand command);
    //}

    //public abstract class HandlerBase<TCommand, TReturn> : IHandler<TCommand, TReturn>
    //    where TCommand : ICommand
    //    where TReturn : IReturn
    //{
    //    public abstract TReturn Handle(TCommand command);
    //}

    //public interface IHandlerAsync<TCommand, TReturn>
    //    where TCommand : ICommand
    //    where TReturn : IReturn
    //{
    //    public Task<TReturn> HandleAsync(TCommand command);
    //}

    //public abstract class HandlerAsyncBase<TCommand, TReturn> : IHandlerAsync<TCommand, TReturn>
    //    where TCommand : ICommand
    //    where TReturn : IReturn
    //{
    //    public abstract Task<TReturn> HandleAsync(TCommand command);
    //}

    public interface IHandler<TCommand, TReturn>
    where TCommand : ICommand
    where TReturn : IReturn
    {
        public Task<TReturn> Handle(TCommand command);
    }

    public abstract class HandlerBase<TCommand, TReturn> : IHandler<TCommand, TReturn>
        where TCommand : ICommand
        where TReturn : IReturn
    {
        public abstract Task<TReturn> Handle(TCommand command);
    }
}