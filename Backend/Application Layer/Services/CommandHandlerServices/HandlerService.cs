using Microsoft.AspNetCore.Mvc;
using WebApplication1.Application_Layer.Services;
using WebApplication1.Application_Layer.Services.CommandHandlerServices;
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
        private IVerifyEstablishmentCommandService verifyEstablishmentCommandService;
        private IVerifySalesCommandService verifySalesCommandService;
        private IVerifyItemsCommandService verifyItemsCommandService;

        public HandlerService([FromServices] IVerifyEstablishmentCommandService verifyEstablishmentCommandService, [FromServices] IVerifySalesCommandService verifySalesCommandService, [FromServices] IVerifyItemsCommandService verifyItemsCommandService)
        {
            this.verifyEstablishmentCommandService = verifyEstablishmentCommandService;
            this.verifySalesCommandService = verifySalesCommandService;
            this.verifyItemsCommandService = verifyItemsCommandService;
        }

        public async Task<Return> Service<Command, Return>(IHandler<Command, Return> handler, Command command)
            where Command : ICommand
            where Return : IReturn
        {
            try
            {
                this.verifyEstablishmentCommandService.VerifyEstablishment(command);
                this.verifySalesCommandService.VerifySales(command);
                this.verifyItemsCommandService.VerifyItems(command);
                return await handler.Handle(command);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}
