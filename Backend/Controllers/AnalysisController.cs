using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.CommandHandlers;
using WebApplication1.Commands;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/analysis/correlation")]
    public class AnalysisController
    {
        [AllowAnonymous]
        [HttpPost("product-weather-correlation")]
        public void ProductWeatherCorrelation(GetProductSalesQuery command)
        {
            return;
        }
    }
}
