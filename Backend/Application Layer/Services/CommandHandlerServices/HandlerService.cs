using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using WebApplication1.Application_Layer.Services;
using WebApplication1.Application_Layer.Services.CommandHandlerServices;
using WebApplication1.CommandHandlers;

namespace WebApplication1.CommandsHandlersReturns
{
    public interface ICommandValidatorService
    {
        Task<ActionResult<Return>> Service<Command, Return>(IHandler<Command, Return> handler, Command command)
            where Command : ICommand
            where Return : IReturn;
    }

    [ExcludeFromCodeCoverage]
    public class HandlerService : ICommandValidatorService
    {
        private VerifyEstablishmentCommandService verifyEstablishmentCommandService;
        private VerifySalesCommandService verifySalesCommandService;
        private VerifyItemsCommandService verifyItemsCommandService;
        private VerifyTablesCommandService verifyTablesCommandService;

        public HandlerService([FromServices] VerifyEstablishmentCommandService verifyEstablishmentCommandService, [FromServices] VerifySalesCommandService verifySalesCommandService, [FromServices] VerifyItemsCommandService verifyItemsCommandService, [FromServices] VerifyTablesCommandService verifyTablesCommandService)
        {
            this.verifyEstablishmentCommandService = verifyEstablishmentCommandService;
            this.verifySalesCommandService = verifySalesCommandService;
            this.verifyItemsCommandService = verifyItemsCommandService;
            this.verifyTablesCommandService = verifyTablesCommandService;

        }

        public async Task<ActionResult<Return>> Service<Command, Return>(IHandler<Command, Return> handler, Command command)
            where Command : ICommand
            where Return : IReturn
        {
            try
            {
                this.verifyEstablishmentCommandService.Verify(command);
                this.verifySalesCommandService.Verify(command);
                this.verifyItemsCommandService.Verify(command);
                this.verifyTablesCommandService.Verify(command);
                var res = await handler.Handle(command);
                return new OkObjectResult(res);
            }
            catch (UnauthorizedAccessException exception)
            {
                return new UnauthorizedResult();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}
