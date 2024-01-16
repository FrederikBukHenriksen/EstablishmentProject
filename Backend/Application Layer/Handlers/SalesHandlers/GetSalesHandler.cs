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
        public SalesSorting SalesSortingParameters { get; set; }
    }

    public class GetSalesReturn : ReturnBase
    {
        public List<Guid> Sales { get; set; }
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
            IEnumerable<Sale> sales = this.unitOfWork.salesRepository.FindAll(x => x.EstablishmentId == command.EstablishmentId);
            IEnumerable<Sale> filteredSales = SalesSortingParametersExecute.SortSales(sales, command.SalesSortingParameters);
            List<Guid> salesIds = filteredSales.Select(x => x.Id).ToList();
            return new GetSalesReturn { Sales = salesIds };

        }
    }
}
