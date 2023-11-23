using Microsoft.AspNetCore.Mvc;
using WebApplication1.CommandHandlers;
using WebApplication1.Commands;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/analysis/")]
    public class AnalysisController
    {
        [HttpPost("sales-line-chart")]
        public GraphDTO ProductSalesChart(
            [FromServices] ICommandHandler<GetProductSalesPerDayQuery, GraphDTO> handler
            )
        {

            var command = new GetProductSalesPerDayQuery() { ItemId = Guid.Empty, Resolution = TimeResolution.halfHour,  StartDate = new DateTime(DateTime.Now.Year, 7, 1, 8, 0, 0), EndDate = new DateTime(DateTime.Now.Year, 7, 1, 18, 0, 0) };
            var result = handler.Execute(command);

            foreach (var item in result.values)
            {
                item.SalesCount = 10+ new Random().Next(-5, 5);
            }
            return result;
        }
    }
}