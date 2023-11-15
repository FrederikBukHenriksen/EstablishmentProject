using WebApplication1.Commands;

namespace WebApplication1.CommandHandlers
{
    public interface ICommandHandler<TCommand, TReturn>
        where TCommand : ICommand
    {
        public TReturn Execute(TCommand command);
    }
}