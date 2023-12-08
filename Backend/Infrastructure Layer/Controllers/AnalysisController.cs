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
        [HttpPost("sales-mean")]
        public SalesMeanQueryReturn MeanSales(SalesMeanOverTime command,[FromServices] IHandler<SalesMeanOverTime, SalesMeanQueryReturn> handler
        )
        {
            //var manualCommand = new MeanSalesCommand
            //{
            //    TimeResolution = TimeResolution.Date,
            //    Timeline = new TimePeriod(start: System.DateTime.Now.AddDays(-1), end: System.DateTime.Now),
            //    //UseDataFromTimeframePeriods = new System.Collections.Generic.List<TimePeriod> { new TimePeriod(start: System.DateTime.Now.AddDays(-7), end: System.DateTime.Now) }
            //};
            return handler.Handle(command);
        }


        //[HttpPost("sales-median")]
        //public GraphDTO MedianSales(GetProductSalesPerDayQuery command,
        //[FromServices] IHandler<GetProductSalesPerDayQuery, GraphDTO> handler
        //)
        //{
        //    return handler.Execute(command);
        //}

        //[HttpPost("sales-variance")]
        //public GraphDTO VarianceSales(GetProductSalesPerDayQuery command,
        //[FromServices] IHandler<GetProductSalesPerDayQuery, GraphDTO> handler
        //)
        //{
        //    return handler.Execute(command);
        //}


        [HttpPost("cross-correlation-with-weather")]
        public List<(TimeSpan, double)> CorrelationCoefficientAndLag(CorrelationCommand command,
            [FromServices] IHandler<CorrelationCommand, List<(TimeSpan, double)>> handler)
        {
            return handler.Handle(command);
        }
        [Route("clustering/")]

        [HttpPost("mean-shift-clustering")]
        public MeanShiftClusteringReturn MeanShiftClustering(MeanShiftClusteringCommand command,[FromServices] IHandler<MeanShiftClusteringCommand, MeanShiftClusteringReturn> handler)
        {
            return handler.Handle(command);

        }
    }
}