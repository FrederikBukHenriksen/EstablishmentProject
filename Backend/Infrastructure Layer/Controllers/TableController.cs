using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using WebApplication1.Application_Layer.Handlers.SalesHandlers;
using WebApplication1.CommandHandlers;
using WebApplication1.CommandsHandlersReturns;

namespace WebApplication1.Controllers
{
    [ExcludeFromCodeCoverage]
    [ApiController]
    [Route("api/establishment/tables")]
    public class TableController : ControllerBase
    {
        private ICommandValidatorService handlerService;

        public TableController([FromServices] ICommandValidatorService handlerService)
        {
            this.handlerService = handlerService;
        }

        [HttpPost("get")]
        public async Task<ActionResult<GetTablesIdReturn>> GetTables([FromBody] GetTablesCommand command, IHandler<GetTablesCommand, GetTablesIdReturn> handler)
        {
            return await this.handlerService.Service(handler, command);
        }

        [HttpPost("get-DTO")]
        public async Task<ActionResult<GetTablesDTOReturn>> GetTablesDTO([FromBody] GetTablesCommand command, IHandler<GetTablesCommand, GetTablesDTOReturn> handler)
        {
            return await this.handlerService.Service(handler, command);
        }

    }
}