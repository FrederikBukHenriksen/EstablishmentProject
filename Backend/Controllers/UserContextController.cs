using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/user-context/")]
    public class UserContextController : ControllerBase
    {
        [HttpGet("get-accessible-establishment")]
        public List<Establishment> GetAccessibleEstablishments([FromServices] IUserContextService userContextService)
        {
            return userContextService.GetAccessibleEstablishments();
        }

        //[HttpPost("set-active-establishment")]
        //public void SetActiveEstablishment(Guid EstablishmentId, [FromServices] IUserContextService userContextService)
        //{
        //    userContextService.SetActiveEstablishmentInSession(this.HttpContext,EstablishmentId);
        //}

        //[HttpPost("get-active-establishment")]
        //public Establishment GetActiveEstablishment([FromServices] IUserContextService userContextService)
        //{
        //    return userContextService.GetActiveEstablishment();
        //}
    }
}