using Microsoft.AspNetCore.Mvc;
using WebApplication1.Commands;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/establishment")]
    public class EstablishmentController : ControllerBase
    {
        private readonly IEstablishmentRepository _establishmentRepository;

        public EstablishmentController(
            IEstablishmentRepository establishmentRepository
            )
        {
            _establishmentRepository = establishmentRepository;
        }

        [HttpGet]
        [Route("/get")]
        public Establishment Get(Guid id)
        {
            return _establishmentRepository.Get(id);
        }

        [HttpGet]
        [Route("/get/sale")]
        public void GetSale(Guid id)
        {
            var sales = _establishmentRepository.Find(x => x.Id == new Guid("00000000-0000-0000-0000-000000000000")).Sales;
            return;
        }

        [Route("/getall")]
        public IEnumerable<Establishment> GetAll()
        {
            return _establishmentRepository.GetAll();
        }

        [HttpPost]
        public void Post(CreateEstablishmentCommand command)
        {
            Establishment est = new Establishment { Name = command.Name };
            _establishmentRepository.Add(est);
        }
    }
}