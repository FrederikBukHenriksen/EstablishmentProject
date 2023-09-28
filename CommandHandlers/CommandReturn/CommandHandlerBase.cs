namespace WebApplication1.CommandHandlers.CommandReturn
{
    public abstract class CommandHandlerBase : ICommandHandlerReturn
    {
        public object _value = null;

        public object value { get { return _value; } set { _value = value; } }
    }
}
