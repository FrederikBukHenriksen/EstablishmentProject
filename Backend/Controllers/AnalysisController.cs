using Microsoft.AspNetCore.Mvc;
using WebApplication1.CommandHandlers;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/analysis/")]
    public class AnalysisController
    {
        [HttpPost("sales-line-chart")]
        public LineChartData ProductSalesChart([FromServices] GetProductSalesChartQueryHandler handler)
        {
            var command = new GetProductSalesChartQuery() { ItemId = Guid.Empty, StartDate = DateTime.Today.AddDays(-1), EndDate = DateTime.Today.AddDays(1) };
            var result = handler.ExecuteAsync(command, new CancellationToken());
            return result.Result;
        }
    }
}

