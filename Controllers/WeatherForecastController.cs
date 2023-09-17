using Microsoft.AspNetCore.Mvc;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IEstablishmentRepository _establishmentRepository;
        private readonly ILocationRepository _locationRepository;



        public WeatherForecastController(
            ILogger<WeatherForecastController> logger,
            IEstablishmentRepository establishmentRepository,
            ILocationRepository locationRepository
            )
        {
            _logger = logger;
            _establishmentRepository = establishmentRepository;
            _locationRepository = locationRepository;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            var id = 7;

            Establishment obj = new Establishment { Id = id, Name = "hej" };
            _establishmentRepository.Add(obj);
            Location loc = new Location { Id = id + 10, EstablishmentId = obj.Id };
            _locationRepository.Add(loc);

            var download = _establishmentRepository.Get(id);
            var download2 = _locationRepository.Get(id+10);



            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}