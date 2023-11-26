using Microsoft.AspNetCore.Mvc;
using WebApplication1.CommandHandlers;
using WebApplication1.Commands;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/sales-analysis/")]
    public class AnalysisController
    {
        [HttpPost("sales")]
        public GraphDTO Sales(GetProductSalesPerDayQuery command,
            [FromServices] IHandler<GetProductSalesPerDayQuery, GraphDTO> handler
            )
        {
            return handler.Execute(command);
        }

        [HttpPost("sales-mean")]
        public GraphDTO MeanSales(GetProductSalesPerDayQuery command,
        [FromServices] IHandler<GetProductSalesPerDayQuery, GraphDTO> handler
        )
        {
            return handler.Execute(command);
        }


        [HttpPost("sales-median")]
        public GraphDTO MedianSales(GetProductSalesPerDayQuery command,
        [FromServices] IHandler<GetProductSalesPerDayQuery, GraphDTO> handler
        )
        {
            return handler.Execute(command);
        }

        [HttpPost("sales-variance")]
        public GraphDTO VarianceSales(GetProductSalesPerDayQuery command,
        [FromServices] IHandler<GetProductSalesPerDayQuery, GraphDTO> handler
        )
        {
            return handler.Execute(command);
        }


        [HttpPost("cross-correlation-with-weather")]
        public List<(TimeSpan, double)> CorrelationCoefficientAndLag(CorrelationBetweenSalesAndWeatherCommand command,
            [FromServices] IHandler<CorrelationBetweenSalesAndWeatherCommand, List<(TimeSpan, double)>> handler)
        {
            return handler.Execute(command);

        }


        [HttpPost("mean-shift-clustering")]
        public List<(TimeSpan, double)> MeanShiftClustering(CorrelationBetweenSalesAndWeatherCommand command,
            [FromServices] IHandler<CorrelationBetweenSalesAndWeatherCommand, List<(TimeSpan, double)>> handler)
        {
            return handler.Execute(command);

        }
    }
}