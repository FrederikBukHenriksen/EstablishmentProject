using WebApplication1.Application_Layer.Services;
using WebApplication1.CommandHandlers;
using WebApplication1.CommandsHandlersReturns;
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
            //var ok1 = this.unitOfWork.establishmentRepository.IncludeSales().GetById(command.EstablishmentId);

            //var ok1 = this.unitOfWork.establishmentRepository.DisableTracking().test2();
            var ok1 = this.unitOfWork.establishmentRepository.IncludeSales().GetById(command.EstablishmentId);

            //IEnumerable<Establishment> ok3 = this.unitOfWork.establishmentRepository.GetAll().ToList();

            var hej1 = ok1.Items.Count();
            var hej2 = ok1.Sales.Count();

            //IEnumerable<Sale> sales = this.unitOfWork.establishmentRepository.GetById(command.EstablishmentId)!.GetSales();
            //IEnumerable<Sale> filteredSales = SalesSortingParametersExecute.SortSales(sales, command.SalesSorting);
            var filteredSales = ok1.Sales;
            List<Guid> salesIds = filteredSales.Select(x => x.Id).ToList();
            return new GetSalesReturn { Sales = salesIds };
            //return new GetSalesReturn { };


        }
    }
}