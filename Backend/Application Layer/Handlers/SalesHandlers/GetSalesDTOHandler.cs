using WebApplication1.CommandHandlers;
using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Domain_Layer.Services.Repositories;
using WebApplication1.Infrastructure_Layer.DataTransferObjects;
using WebApplication1.Services;

namespace WebApplication1.Application_Layer.CommandsQueriesHandlersReturns.SalesHandlers
{
    public class GetSalesDTOCommand : CommandBase, ICmdField_EstablishmentId
    {
        public Guid EstablishmentId { get; set; }
        public List<Guid> SalesIds { get; set; }
    }

    public class GetSalesDTOReturn : ReturnBase
    {
        public List<SaleDTO> Sales { get; set; }
    }

    public class GetSalesDTOHandler : HandlerBase<GetSalesDTOCommand, GetSalesDTOReturn>
    {
        private IUserContextService userContextService;
        private ISalesRepository salesRepository;

        public GetSalesDTOHandler(IUserContextService userContextService, ISalesRepository salesRepository)
        {
            this.userContextService = userContextService;
            this.salesRepository = salesRepository;
        }

        public async override Task<GetSalesDTOReturn> Handle(GetSalesDTOCommand command)
        {
            List<Sale> sales = this.salesRepository.GetFromIds(command.SalesIds);
            sales = sales.Where(x => command.SalesIds.Contains(x.Id)).ToList();
            List<SaleDTO> salesDTO = sales.Select(x => new SaleDTO(x)).ToList();

            return new GetSalesDTOReturn { Sales = salesDTO };
        }
    }
}
