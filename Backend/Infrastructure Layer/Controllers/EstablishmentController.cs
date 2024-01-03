using Microsoft.AspNetCore.Mvc;
using WebApplication1.Domain.Entities;
using WebApplication1.Domain.Services.Repositories;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/establishment/")]
    public class EstablishmentController : ControllerBase
    {
        private readonly IEstablishmentRepository _establishmentRepository;
        private readonly IUserContextService _userContextService;

        public EstablishmentController(
            IEstablishmentRepository establishmentRepository,
            IUserContextService userContextService
            )
        {
            _establishmentRepository = establishmentRepository;
            _userContextService = userContextService;
        }

        [HttpGet]
        [Route("get")]
        public Establishment? Get(Guid establishmentId)
        {
            return _establishmentRepository.Find(x => x.Id == establishmentId);
        }

        [Route("get-all")]
        public List<Establishment>? GetAll()
        {
            return _userContextService.GetAccessibleEstablishments().ToList();
        }

        [HttpGet("items/get-all")]
        public ICollection<Item> ItemGetAll([FromServices] IUserContextService userContextService,
        [FromServices] IEstablishmentRepository establishmentRepository)
        {
            Establishment activeEstablishment = userContextService.GetActiveEstablishment();
            var res = establishmentRepository.GetEstablishmentItems(activeEstablishment.Id)!;
            return res;
        }
    }
}