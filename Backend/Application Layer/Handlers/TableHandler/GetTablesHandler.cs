using WebApplication1.Application_Layer.Services;
using WebApplication1.CommandHandlers;
using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Infrastructure_Layer.DataTransferObjects;

namespace WebApplication1.Application_Layer.Handlers.SalesHandlers
{
    public class GetTablesCommand : CommandBase, ICmdField_EstablishmentId, ICmdField_TablesIds
    {
        public Guid EstablishmentId { get; set; }
        public List<Guid>? TablesIds { get; set; } = null;
    }

    public interface ITablesReturn : IReturn
    {
        ITablesReturn Create(List<Table> tables);
    }

    public class GetTablesIdReturn : ReturnBase, ITablesReturn
    {
        public List<Guid> Tables { get; set; } = new List<Guid>();

        public ITablesReturn Create(List<Table> tables)
        {
            this.Tables = tables.Select(x => x.Id).ToList();
            return this;
        }
    }

    public class GetTablesDTOReturn : ReturnBase, ITablesReturn
    {
        public List<TableDTO> Tables { get; set; }


        public ITablesReturn Create(List<Table> tables)
        {
            this.Tables = tables.Select(x => new TableDTO(x)).ToList();
            return this;
        }
    }

    public class GetTablesRawReturn : ReturnBase, ITablesReturn
    {
        public List<Table> Tables { get; set; } = new List<Table>();


        public ITablesReturn Create(List<Table> tables)
        {
            this.Tables = tables;
            return this;
        }
    }

    public class GetTablesHandler<T> : HandlerBase<GetTablesCommand, T> where T : ITablesReturn, new()
    {
        private IUnitOfWork unitOfWork;

        public GetTablesHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async override Task<T> Handle(GetTablesCommand command)
        {
            Establishment establishment = this.unitOfWork.establishmentRepository.GetById(command.EstablishmentId)!;
            List<Table> tables = establishment.GetTables();
            return (T)(new T()).Create(tables);
        }
    }
}
