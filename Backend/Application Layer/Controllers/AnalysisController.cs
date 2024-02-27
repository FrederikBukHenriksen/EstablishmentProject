using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using WebApplication1.Application_Layer.Handlers.Correlation;
using WebApplication1.Application_Layer.Handlers.MeanShift;
using WebApplication1.CommandHandlers;
using WebApplication1.CommandsHandlersReturns;

namespace WebApplication1.Controllers
{
    [ExcludeFromCodeCoverage]
    [ApiController]
    [Route("api/analysis/")]
    public class AnalysisController
    {
        private ICommandValidatorService handlerService;

        public AnalysisController([FromServices] ICommandValidatorService handlerService)
        {
            this.handlerService = handlerService;
        }



        [HttpPost("Correlation_NumberOfSales_Vs_Temperature")]
        public async Task<ActionResult<CorrelationReturn>> NumberOfSalesVsTemperature([FromBody] Correlation_NumberOfSales_Vs_Temperature_Command command, [FromServices] IHandler<Correlation_NumberOfSales_Vs_Temperature_Command, CorrelationReturn> handler)
        {
            return await handler.Handle(command);
        }


        [HttpPost("Correlation_SeatTime_Vs_Temperature")]
        public async Task<ActionResult<CorrelationReturn>> SeatTimeVsTemperature([FromBody] Correlation_SeatTime_Vs_Temperature_Command command, [FromServices] IHandler<Correlation_SeatTime_Vs_Temperature_Command, CorrelationReturn> handler)
        {
            return await handler.Handle(command);
        }


        [HttpPost("Clustering_TimeOfVisit_Vs_TotalPrice")]
        public async Task<ActionResult<ClusteringReturn>> TimeOfVisitVsTotalPrice(
            [FromBody] Clustering_TimeOfVisit_TotalPrice_Command command,
            [FromServices] IHandler<Clustering_TimeOfVisit_TotalPrice_Command, ClusteringReturn> handler)
        {
            return await this.handlerService.Service(handler, command);
        }

        [HttpPost("Clustering_TimeOfVisit_Vs_SeatTime")]
        public async Task<ActionResult<ClusteringReturn>> TimeOfVisitVsSeatTime(
            [FromBody] Clustering_TimeOfVisit_LengthOfVisit_Command command,
            [FromServices] IHandler<Clustering_TimeOfVisit_LengthOfVisit_Command, ClusteringReturn> handler)
        {
            return await this.handlerService.Service(handler, command);
        }
    }
}