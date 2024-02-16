using WebApplication1.Application_Layer.Services;
using WebApplication1.CommandHandlers;
using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Controllers;
using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Application_Layer.CommandsQueriesHandlersReturns.EstablishmentHandlers
{
    public class GetEstablishmentsCommand : CommandBase, ICmdField_EstablishmentIds
    {
        public List<Guid> EstablishmentIds { get; set; } = new List<Guid>();
    }

    public interface IEstablishmentReturn : IReturn
    {
        IEstablishmentReturn Create(List<Establishment> establishments);
    }

    public class GetEstablishmentsIdReturn : ReturnBase, IEstablishmentReturn
    {
        public List<Guid> ids { get; set; } = new List<Guid>();

        public IEstablishmentReturn Create(List<Establishment> establishments)
        {
            this.ids = establishments.Select(x => x.Id).ToList();
            return this;
        }
    }
    public class GetEstablishmentsDTOReturn : ReturnBase, IEstablishmentReturn
    {
        public List<EstablishmentDTO> dtos { get; set; } = new List<EstablishmentDTO>();

        public IEstablishmentReturn Create(List<Establishment> establishments)
        {
            this.dtos = establishments.Select(x => new EstablishmentDTO(x)).ToList();
            return this;
        }
    }

    public class GetEstablishmentsEntityReturn : ReturnBase, IEstablishmentReturn
    {
        public List<Establishment> entities { get; set; } = new List<Establishment>();

        public IEstablishmentReturn Create(List<Establishment> establishments)
        {
            this.entities = establishments;
            return this;
        }
    }

    public class GetMultipleEstablishmentsHandler<T> : HandlerBase<GetEstablishmentsCommand, T> where T : IEstablishmentReturn, new()
    {
        private IUnitOfWork unitOfWork;

        public GetMultipleEstablishmentsHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public override async Task<T> Handle(GetEstablishmentsCommand command)
        {
            List<Establishment> establishment = this.unitOfWork.establishmentRepository.GetAll().Where(x => command.EstablishmentIds.Any(y => y == x.Id)).ToList();
            return (T)new T().Create(establishment);
        }
    }
}
