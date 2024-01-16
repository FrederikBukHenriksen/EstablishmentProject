using Microsoft.AspNetCore.Mvc;
using WebApplication1.Application_Layer.CommandsQueriesHandlersReturns.EstablishmentHandlers;
using WebApplication1.CommandHandlers;
using WebApplication1.CommandsHandlersReturns;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/establishment/")]
    public class EstablishmentController : ControllerBase
    {
        private IHandlerService handlerService;

        public EstablishmentController([FromServices] IHandlerService handlerService)
        {
            this.handlerService = handlerService;
        }

        [HttpPost("get")]
        public async Task<GetEstablishmentReturn> GetEstablishment([FromBody] GetEstablishmentCommand command, [FromServices] IHandler<GetEstablishmentCommand, GetEstablishmentReturn> handler)
        {
            return await this.handlerService.Service(handler, command);
        }

        [HttpPost("get-multiple")]
        public async Task<GetMultipleEstablishmentsReturn> GetEstablishments([FromBody] GetMultipleEstablishmentsCommand command, [FromServices] IHandler<GetMultipleEstablishmentsCommand, GetMultipleEstablishmentsReturn> handler)
        {
            return await handler.Handle(command);
        }

    }
}