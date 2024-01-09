using Microsoft.AspNetCore.Mvc;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Domain_Layer.Services.Repositories;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/establishment/")]
    public class EstablishmentController : ControllerBase
    {
        private IEstablishmentRepository _establishmentRepository;
        private IUserContextService _userContextService;

        public EstablishmentController(
            IEstablishmentRepository establishmentRepository,
            IUserContextService userContextService
            )
        {
            this._establishmentRepository = establishmentRepository;
            this._userContextService = userContextService;
        }

        [HttpGet("get")]
        public ActionResult<EstablishmentDTO> GetEstablishment(Guid establishmentId)
        {
            List<UserRole> userRoles = this._userContextService.GetAllUserRoles().ToList();

            if (userRoles.Any(userRole => userRole.Establishment.Id == establishmentId))
            {
                Establishment establishment = this._establishmentRepository.GetById(establishmentId);
                return new EstablishmentDTO(this._establishmentRepository.GetAll().First());
            }
            else
            {
                return this.Unauthorized();
            }
            return this.NotFound();
        }

        [HttpPost("get-multiple")]
        public ActionResult<List<EstablishmentDTO>> GetEstablishments(List<Guid> establishmentIds)
        {
            List<UserRole> userRoles = this._userContextService.GetAllUserRoles().ToList();

            List<EstablishmentDTO> establishments = new List<EstablishmentDTO>();

            foreach (Guid establishmentId in establishmentIds)
            {
                if (userRoles.Any(userRole => userRole.Establishment.Id == establishmentId))
                {
                    Establishment establishment = this._establishmentRepository.GetById(establishmentId);
                    establishments.Add(new EstablishmentDTO(establishment));
                }
                else
                {
                    return this.Unauthorized();
                }
            }
            return establishments;
        }

    }
}