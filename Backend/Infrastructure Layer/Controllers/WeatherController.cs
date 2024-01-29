using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.ControllersBui
{
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