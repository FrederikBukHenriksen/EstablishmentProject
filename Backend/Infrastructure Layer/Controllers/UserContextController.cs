using Microsoft.AspNetCore.Mvc;
using WebApplication1.Domain.Entities;
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

        [HttpPost("get-active-establishment")]
        public Establishment GetActiveEstablishment([FromServices] IUserContextService userContextService)
        {
            return userContextService.GetActiveEstablishment();
        }
    }
}