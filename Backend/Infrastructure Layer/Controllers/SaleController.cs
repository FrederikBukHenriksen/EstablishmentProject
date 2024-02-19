using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using WebApplication1.Application_Layer.Handlers.SalesHandlers;
using WebApplication1.CommandHandlers;
using WebApplication1.CommandsHandlersReturns;

namespace WebApplication1.Controllers
{
    [ExcludeFromCodeCoverage]
    [ApiController]
    [Route("api/establishment/sales")]
    public class SaleController : ControllerBase
    {
        private ICommandValidatorService handlerService;

        public SaleController([FromServices] ICommandValidatorService handlerService)
        {
            this.handlerService = handlerService;
        }

        [Authorize]
        [HttpPost("find-sales")]
        public async Task<ActionResult<GetSalesReturn>> GetSales([FromBody] GetSalesCommand command, [FromServices] IHandler<GetSalesCommand, GetSalesReturn> handler)
        {
            return await this.handlerService.Service(handler, command);
        }


        //[Authorize]
        //[HttpPost("statistics")]
        //public async Task<SalesStatisticsReturn> SaleStaticstics([FromBody] SalesStatisticsCommand command, [FromServices] IHandler<SalesStatisticsCommand, SalesStatisticsReturn> handler)
        //{
        //    return await this.handlerService.Service(handler, command);
        //}
    }
}