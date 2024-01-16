using WebApplication1.Application_Layer.Services;
using WebApplication1.CommandHandlers;
using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Controllers;
using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Application_Layer.CommandsQueriesHandlersReturns.EstablishmentHandlers
{
    public class GetMultipleEstablishmentsCommand : CommandBase, IEstablishmentCommandField
    {
        public Guid EstablishmentId { get; set; }
        public List<Guid> EstablishmentIds { get; set; }
    }

    public class GetMultipleEstablishmentsReturn : ReturnBase
    {
        public List<EstablishmentDTO> EstablishmentDTOs { get; set; }
    }

    public class GetMultipleEstablishmentsHandler : HandlerBase<GetMultipleEstablishmentsCommand, GetMultipleEstablishmentsReturn>
    {
        private IUnitOfWork unitOfWork;

        public GetMultipleEstablishmentsHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public override GetMultipleEstablishmentsReturn Handle(GetMultipleEstablishmentsCommand command)
        {
            List<Establishment> establishment = this.unitOfWork.establishmentRepository.GetAll().Where(x => command.EstablishmentIds.Any(y => y == x.Id)).ToList();
            List<EstablishmentDTO> establishmentDTO = establishment.Select(x => new EstablishmentDTO(x)).ToList();
            return new GetMultipleEstablishmentsReturn { EstablishmentDTOs = establishmentDTO };
        }
    }
}
