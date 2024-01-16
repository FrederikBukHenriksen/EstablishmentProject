namespace WebApplication1.CommandsHandlersReturns
{
    public interface ICommand
    {
    }
    public abstract class CommandBase : ICommand
    {
    }

    public interface IEstablishmentCommandField : ICommand
    {
        public Guid EstablishmentId { get; set; }
    }
}
