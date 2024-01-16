using Microsoft.AspNetCore.Mvc;
using WebApplication1.Application_Layer.Services;
using WebApplication1.CommandHandlers;

namespace WebApplication1.CommandsHandlersReturns
{
    public interface IHandlerService
    {
        Task<Return> Service<Command, Return>(IHandler<Command, Return> handler, Command command)
            where Command : ICommand
            where Return : IReturn;
    }

    public class HandlerService : IHandlerService
    {
        private VerifyEstablishmentCommandService verifyEstablishmentCommandService;

        public HandlerService([FromServices] VerifyEstablishmentCommandService verifyEstablishmentCommandService)
        {
            this.verifyEstablishmentCommandService = verifyEstablishmentCommandService;
        }

        public async Task<Return> Service<Command, Return>(IHandler<Command, Return> handler, Command command)
            where Command : ICommand
            where Return : IReturn
        {
            this.verifyEstablishmentCommandService.VerifyEstablishment(command);
            try
            {
                return await handler.Handle(command);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
