using Microsoft.AspNetCore.Mvc;
using WebApplication1.Domain.Entities;
using WebApplication1.Services;

namespace WebApplication1.ControllersBui
{
    [ApiController]
    [Route("api/user-context/")]
    public class WeatherController : ControllerBase
    {
        [HttpGet("get-temperature")]
        public List<Establishment> GetWeatherTemperature([FromServices] IUserContextService userContextService)
        {
            var establishments = userContextService.GetAccessibleEstablishments();
            establishments = establishments.Select(x => new Establishment { Id = x.Id, Name = x.Name }).ToList();
            return establishments;
        }
    }
}