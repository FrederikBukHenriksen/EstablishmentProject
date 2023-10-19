using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.CommandHandlers;
using WebApplication1.Commands;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/analysis")]
    public class AnalysisController
    {
        [AllowAnonymous]
        [HttpPost("coffee-correlation")]
        public void LogIn()
        {
            



            return;
        }
    }
}
