using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace WebApplication1.ControllersBui
{
    [ExcludeFromCodeCoverage]
    [ApiController]
    [Route("api/weather/")]
    public class WeatherController : ControllerBase
    {
        //[HttpGet("get-temperature")]
        //public List<Establishment> GetWeatherTemperature([FromServices] IUserContextService userContextService)
        //{
        //    var establishments = userContextService.GetAccessibleEstablishments();
        //    return establishments;
        //}
    }
}