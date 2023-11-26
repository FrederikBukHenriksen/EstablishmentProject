using System.Windows.Input;
using WebApplication1.CommandHandlers;
using WebApplication1.CommandsHandlersReturns;

namespace WebApplication1.Utils
{
    public static class CommandHandlerReturnHelper
    {
        public static TReturn ExecuteCommand<TCommand, TReturn>(TCommand command, IHandler<TCommand, TReturn> handler)
            where TCommand : CommandsHandlersReturns.ICommand
        {
            return handler.Execute(command);
        }
    }

}
