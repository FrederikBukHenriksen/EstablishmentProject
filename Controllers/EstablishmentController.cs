using Microsoft.AspNetCore.Mvc;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/establishment")]
    public class EstablishmentController : ControllerBase
    {
        private readonly IEstablishmentRepository _establishmentRepository;
        private readonly ILocationRepository _locationRepository;

        public EstablishmentController(
            IEstablishmentRepository establishmentRepository,
            ILocationRepository locationRepository
            )
        {
            _establishmentRepository = establishmentRepository;
            _locationRepository = locationRepository;
        }

        [HttpPost]
        public Establishment Post()
        {


            Establishment est = new Establishment { Name = "hej" };
            _establishmentRepository.Add(est);



            var download = _establishmentRepository.Get(est.Id);

            Location loc = new Location { Address = "Vesterbrogade", Establishment = download };
            _locationRepository.Add(loc);

            return download;
        }
    }
}