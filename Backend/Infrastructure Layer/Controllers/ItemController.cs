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
        private ICommandValidatorService handlerService;

        public ItemController([FromServices] ICommandValidatorService handlerService)
        {
            this.handlerService = handlerService;
        }

        [HttpPost("get-items-dto")]
        public async Task<GetItemDTOReturn> GetItemsDTO([FromBody] GetItemDTOCommand command, IHandler<GetItemDTOCommand, GetItemDTOReturn> handler)
        {
            return await this.handlerService.Service(handler, command);
        }

        [HttpPost("get-items")]
        public async Task<GetItemsReturn> GetItems([FromBody] GetItemsCommand command, IHandler<GetItemsCommand, GetItemsReturn> handler)
        {
            return await this.handlerService.Service(handler, command);
        }

    }
}