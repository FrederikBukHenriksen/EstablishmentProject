using WebApplication1.Application_Layer.Services;
using WebApplication1.CommandHandlers;
using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Utils;

namespace WebApplication1.Application_Layer.Handlers.SalesHandlers
{
    public class GetSalesCommand : CommandBase, ICmdField_EstablishmentId
    {
        public Guid EstablishmentId { get; set; }
        public SalesSorting SalesSorting { get; set; }
    }

    public class GetSalesReturn : ReturnBase
    {
        public List<Guid> Sales { get; set; }

        public GetSalesReturn(List<Sale> sales)
        {
            this.Sales = sales.Select(x => x.Id).ToList();
        }
    }

    public class GetSalesHandler : HandlerBase<GetSalesCommand, GetSalesReturn>
    {
        private IUnitOfWork unitOfWork;

        public GetSalesHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async override Task<GetSalesReturn> Handle(GetSalesCommand command)
        {
            Establishment establishment = this.unitOfWork.establishmentRepository.IncludeSales().IncludeSalesItems().GetById(command.EstablishmentId)!;
            List<Sale> sales = establishment.GetSales();
            List<Sale> filteredSales = SalesSortingParametersExecute.SortSales(sales, command.SalesSorting);
            return new GetSalesReturn(filteredSales);


        }
    }
}