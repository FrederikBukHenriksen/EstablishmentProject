using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
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

        [HttpPost("cross-correlation-with-weather")]
        public async Task<CorrelationReturn> CorrelationCoefficientAndLag([FromBody] CorrelationCommand command, [FromServices] IHandler<CorrelationCommand, CorrelationReturn> handler)
        {
            //var command = new CorrelationCommand { TimePeriod = new DateTimePeriod(start: new DateTime(2021, 1, 1, 0, 0, 0), end: new DateTime(2022, 1, 1).AddTicks(-1)), TimeResolution = TimeResolution.Date };
            var value = handler.Handle(command);
            return await value;
        }

        //[HttpGet("mean-shift-clustering")]
        //public MeanShiftClusteringReturn MeanShiftClustering([FromBody] MeanShiftClusteringCommand command, [FromServices] IHandler<MeanShiftClusteringCommand, MeanShiftClusteringReturn> handler)
        //{
        //    return handler.Handle(command);

        //}

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