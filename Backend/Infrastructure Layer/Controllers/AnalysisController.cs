using Microsoft.AspNetCore.Mvc;
using WebApplication1.CommandHandlers;
using WebApplication1.Domain.Entities;
using static WebApplication1.CommandHandlers.MeanSales;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/sales-analysis/")]
    public class AnalysisController
    {
        [HttpPost("sales")]
        public SalesQueryReturn Sales(SalesQuery command,
            [FromServices] IHandler<SalesQuery, SalesQueryReturn> handler
            )
        {
            //var ok = new SalesQuery { TimePeriod = new Utils.TimePeriod(start: DateTime.Now.AddDays(-7), end: DateTime.Now), TimeResolution = Utils.TimeResolution.Date };
            return handler.Handle(command);
        }

        [HttpPost("sales-mean")]
        public MeanItemSalesReturn MeanSales(MeanItemSalesCommand command,
        [FromServices] IHandler<MeanItemSalesCommand, MeanItemSalesReturn> handler
        )
        {
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
        public List<(TimeSpan, double)> CorrelationCoefficientAndLag(CorrelationBetweenSalesAndWeatherCommand command,
            [FromServices] IHandler<CorrelationBetweenSalesAndWeatherCommand, List<(TimeSpan, double)>> handler)
        {
            return handler.Handle(command);

        }


        [HttpPost("mean-shift-clustering")]
        public List<(TimeSpan, double)> MeanShiftClustering(CorrelationBetweenSalesAndWeatherCommand command,
            [FromServices] IHandler<CorrelationBetweenSalesAndWeatherCommand, List<(TimeSpan, double)>> handler)
        {
            return handler.Handle(command);

        }
    }
}