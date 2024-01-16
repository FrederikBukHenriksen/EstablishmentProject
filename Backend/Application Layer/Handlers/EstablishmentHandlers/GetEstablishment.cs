using WebApplication1.Application_Layer.Services;
using WebApplication1.CommandHandlers;
using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Controllers;
using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Application_Layer.CommandsQueriesHandlersReturns.EstablishmentHandlers
{
    public class GetEstablishmentCommand : CommandBase, ICmdField_EstablishmentId
    {
        public Guid EstablishmentId { get; set; }
    }

    public class GetEstablishmentReturn : ReturnBase
    {
        public EstablishmentDTO EstablishmentDTO { get; set; }
    }

    public class GetEstablishmentHandler : HandlerBase<GetEstablishmentCommand, GetEstablishmentReturn>
    {
        private IUnitOfWork unitOfWork;

        public GetEstablishmentHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public override async Task<GetEstablishmentReturn> Handle(GetEstablishmentCommand command)
        {
            Establishment establishment = this.unitOfWork.establishmentRepository.GetById(command.EstablishmentId);
            EstablishmentDTO establishmentDTO = new EstablishmentDTO(establishment);
            return new GetEstablishmentReturn { EstablishmentDTO = establishmentDTO };
        }
    }
}
