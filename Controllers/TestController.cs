using Microsoft.AspNetCore.Mvc;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IEstablishmentRepository _establishmentRepository;
        private readonly ILocationRepository _locationRepository;

        public TestController(
            IEstablishmentRepository establishmentRepository,
            ILocationRepository locationRepository
            )
        {
            _establishmentRepository = establishmentRepository;
            _locationRepository = locationRepository;
        }

        [HttpGet(Name = "Lolcat")]
        public Location Get()
        {
            var id = 7;

            //Establishment obj = new Establishment { Id = id, Name = "hej" };
            //_establishmentRepository.Add(obj);
            Location loc = new Location { };
            //_locationRepository.Add(loc);

            //var download = _establishmentRepository.Get(id);
            //var download2 = _locationRepository.Get(id+10);

            return loc;
        }
    }
}