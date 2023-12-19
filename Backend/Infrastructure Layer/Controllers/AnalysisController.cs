using Microsoft.AspNetCore.Mvc;
using WebApplication1.CommandHandlers;
using WebApplication1.Domain.Entities;
using WebApplication1.Utils;
using static WebApplication1.CommandHandlers.MeanSales;
using static WebApplication1.CommandHandlers.MeanSales.MeanSalesHandler;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/analysis/")]
    public class AnalysisController
    {
        [HttpPost("sales")]
        public SalesQueryReturn Sales(SalesQuery command,
            [FromServices] IHandler<SalesQuery, SalesQueryReturn> handler
            )
        {
            //var ok = new SalesQuery { TimePeriod = new Utils.TimePeriod(start: DateTime.Now.AddDays(-7), end: DateTime.Now), TimeResolution = Utils.TimeResolution.Date };
            //command.salesSortingParameters.MustContaiedItems = new System.Collections.Generic.List<System.Guid> { new System.Guid("00000000-0000-0000-0000-000000000002") };
            return handler.Handle(command);
        }

        [HttpPost("average-visits")]    
        public SalesMeanQueryReturn MeanSales([FromBody] SalesMeanOverTime command, [FromServices] IHandler<SalesMeanOverTime, SalesMeanQueryReturn> handler
        )
        {
            return handler.Handle(command);
        }


        [HttpPost("average-spend")] //SalesMeanOverTimeAverageSpend
        public SalesMeanQueryReturn MeanSalesAverageSpend([FromBody] SalesMeanOverTime command, [FromServices] IHandler<SalesMeanOverTime, SalesMeanQueryReturn> handler
        )
        {
            return handler.Handle(command);
        }


        //[HttpPost("sales-median")]
        //public SalesMeanQueryReturn MediainSalesAverageSpend([FromBody] SalesMeanOverTime command, [FromServices] IHandler<SalesMeanOverTime, SalesMeanQueryReturn> handler
        //)
        //{
        //    return handler.Handle(command);
        //}

        //[HttpPost("sales-variance")]
        //public GraphDTO VarianceSales(GetProductSalesPerDayQuery command,
        //[FromServices] IHandler<GetProductSalesPerDayQuery, GraphDTO> handler
        //)
        //{
        //    return handler.Execute(command);
        //}


        [HttpPost("cross-correlation-with-weather")]
        public CorrelationReturn CorrelationCoefficientAndLag([FromServices] IHandler<CorrelationCommand, CorrelationReturn> handler)
        {
            var command = new CorrelationCommand { TimePeriod = new DateTimePeriod(start: new DateTime(2021, 1, 1,0,0,0), end: new DateTime(2022, 1,1).AddTicks(-1)), TimeResolution = TimeResolution.Date };
            var value = handler.Handle(command);
            return value;
        }
        [Route("clustering/")]

        [HttpPost("mean-shift-clustering")]
        public MeanShiftClusteringReturn MeanShiftClustering(MeanShiftClusteringCommand command,[FromServices] IHandler<MeanShiftClusteringCommand, MeanShiftClusteringReturn> handler)
        {
            return handler.Handle(command);

        }
    }
}