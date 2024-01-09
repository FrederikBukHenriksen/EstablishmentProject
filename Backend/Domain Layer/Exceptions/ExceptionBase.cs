namespace WebApplication1.Domain_Layer.Exceptions
{
    public abstract class ExceptionBase : Exception, IException
    {
        public string CustomMessage { get; private set; }

        public ExceptionBase()
        {

        }

        protected ExceptionBase(string message)
            : base(message)
        {
            this.CustomMessage = message;
        }

        protected ExceptionBase(string message, Exception innerException)
            : base(message, innerException)
        {
            this.CustomMessage = message;
        }
    }



}
