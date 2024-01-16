using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Application_Layer.CommandsQueriesHandlersReturns.SalesHandlers;
using WebApplication1.Application_Layer.Handlers.SalesHandlers;
using WebApplication1.CommandHandlers;
using WebApplication1.CommandsHandlersReturns;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/establishment/sales")]
    public class SaleController : ControllerBase
    {
        private IHandlerService handlerService;

        public SaleController([FromServices] IHandlerService handlerService)
        {
            this.handlerService = handlerService;
        }

        [Authorize]
        [HttpPost("get-salesDTO")]
        public async Task<GetSalesDTOReturn> GetSalesDTO([FromBody] Application_Layer.CommandsQueriesHandlersReturns.SalesHandlers.GetSalesDTOCommand command, [FromServices] IHandler<Application_Layer.CommandsQueriesHandlersReturns.SalesHandlers.GetSalesDTOCommand, GetSalesDTOReturn> handler)
        {
            return await this.handlerService.Service(handler, command);
        }

        [HttpPost("find")]
        public async Task<GetSalesReturn> GetSales([FromBody] GetSalesCommand command, [FromServices] IHandler<GetSalesCommand, GetSalesReturn> handler)
        {
            return await this.handlerService.Service(handler, command);
        }

        [HttpPost("statistics")]
        public async Task<SalesStatisticsReturn> SaleStaticstics([FromBody] SalesStatisticsCommand command, [FromServices] IHandler<SalesStatisticsCommand, SalesStatisticsReturn> handler)
        {
            return await this.handlerService.Service(handler, command);
        }
    }
}