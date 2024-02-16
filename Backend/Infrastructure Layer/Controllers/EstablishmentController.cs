using Microsoft.AspNetCore.Authorization;
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
        private ICommandValidatorService handlerService;

        public EstablishmentController([FromServices] ICommandValidatorService handlerService)
        {
            this.handlerService = handlerService;
        }

        [Authorize]
        [HttpPost("get-id")]
        public async Task<GetEstablishmentsIdReturn> GetEstablishmentdID([FromBody] GetEstablishmentsCommand command, [FromServices] IHandler<GetEstablishmentsCommand, GetEstablishmentsIdReturn> handler)
        {
            return await this.handlerService.Service(handler, command);
        }

        [Authorize]
        [HttpPost("get-DTO")]
        public async Task<GetEstablishmentsDTOReturn> GetEstablishmentsDTO([FromBody] GetEstablishmentsCommand command, [FromServices] IHandler<GetEstablishmentsCommand, GetEstablishmentsDTOReturn> handler)
        {
            return await this.handlerService.Service(handler, command);
        }
    }
}