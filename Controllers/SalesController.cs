using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.CommandHandlers;
using WebApplication1.Commands;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/sales")]
    public class SalesController
    {

        [Authorize]
        [HttpGet("product")]
        public void getPoductSales()
        {




            return;
        }

    }
}
