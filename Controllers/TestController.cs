using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {

        public TestController(
            )
        {
        }

    }
}