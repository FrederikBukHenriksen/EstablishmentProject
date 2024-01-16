using Microsoft.AspNetCore.Mvc;
using WebApplication1.Application_Layer.Handlers.ItemHandler;
using WebApplication1.CommandHandlers;
using WebApplication1.CommandsHandlersReturns;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/establishment/item")]
    public class ItemController : ControllerBase
    {
        private IHandlerService handlerService;

        public ItemController([FromServices] IHandlerService handlerService)
        {
            this.handlerService = handlerService;
        }

        [HttpPost("get")]
        public async Task<GetItemDTOReturn> GetItems([FromBody] GetItemDTOCommand command, IHandler<GetItemDTOCommand, GetItemDTOReturn> handler)
        {
            return await this.handlerService.Service(handler, command);
        }

    }
}