using Microsoft.AspNetCore.Mvc;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Services;

namespace WebApplication1.ControllersBui
{
    [ApiController]
    [Route("api/user-context/")]
    public class UserContextController : ControllerBase
    {
        [HttpGet("get-accessible-establishment")]
        public List<Establishment> GetAccessibleEstablishments([FromServices] IUserContextService userContextService)
        {
            var establishments = userContextService.GetAccessibleEstablishments();
            establishments = establishments.Select(x => new Establishment { Id = x.Id, Name = x.Name }).ToList();
            return establishments;
        }

        [HttpPost("get-active-establishment")]
        public Establishment GetActiveEstablishment([FromServices] IUserContextService userContextService)
        {
            return userContextService.GetActiveEstablishment();
        }
    }
}