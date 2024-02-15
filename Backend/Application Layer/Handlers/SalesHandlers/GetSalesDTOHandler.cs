using WebApplication1.Application_Layer.Handlers.SalesHandlers;
using WebApplication1.CommandHandlers;
using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Domain_Layer.Services.Repositories;
using WebApplication1.Services;

namespace WebApplication1.Application_Layer.CommandsQueriesHandlersReturns.SalesHandlers
{
    public class GetSalesDTOCommand : CommandBase, ICmdField_EstablishmentId
    {
        public Guid EstablishmentId { get; set; }
        public List<Guid> SalesIds { get; set; }
    }




    public class GetSalesDTOHandler<T> : HandlerBase<GetSalesDTOCommand, T> where T : ISalesReturn, new()

        //public class GetSalesDTOHandler : HandlerBase<GetSalesDTOCommand, GetSalesDTOReturn>
    {
        private ISalesRepository salesRepository;

        public GetSalesDTOHandler(IUserContextService userContextService, ISalesRepository salesRepository)
        {
            this.salesRepository = salesRepository;
        }

        public async override Task<T> Handle(GetSalesDTOCommand command)
        {
            List<Sale> sales = this.salesRepository.GetFromIds(command.SalesIds);
            sales = sales.Where(x => command.SalesIds.Contains(x.Id)).ToList();

            return (T)(new T()).Create(sales);

        }
    }
}
