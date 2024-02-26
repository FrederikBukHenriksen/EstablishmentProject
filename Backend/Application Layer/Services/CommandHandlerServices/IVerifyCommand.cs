
using WebApplication1.CommandsHandlersReturns;

namespace WebApplication1.Application_Layer.Services.CommandHandlerServices
{
    public interface IVerifyCommand
    {
        void Verify(ICommand command);

    }
}
