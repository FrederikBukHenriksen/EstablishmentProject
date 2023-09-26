using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {

        public TestController(
            )
        {
        }
        [Authorize("admin")]
        [HttpGet(Name = "testEndPoint")]
        public Location Get()
        {

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