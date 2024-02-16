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

        [HttpPost("get")]
        public async Task<GetItemsIdReturn> GetItems([FromBody] GetItemsCommand command, IHandler<GetItemsCommand, GetItemsIdReturn> handler)
        {
            return await this.handlerService.Service(handler, command);
        }

        [HttpPost("get-DTO")]
        public async Task<GetItemsDTOReturn> GetItemsDTO([FromBody] GetItemsCommand command, IHandler<GetItemsCommand, GetItemsDTOReturn> handler)
        {
            return await this.handlerService.Service(handler, command);
        }

    }
}