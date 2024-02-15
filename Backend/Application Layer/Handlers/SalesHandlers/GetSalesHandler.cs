using Microsoft.IdentityModel.Tokens;
using WebApplication1.Application_Layer.Services;
using WebApplication1.CommandHandlers;
using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Infrastructure_Layer.DataTransferObjects;
using WebApplication1.Utils;

namespace WebApplication1.Application_Layer.Handlers.SalesHandlers
{
    public class GetSalesCommand : CommandBase, ICmdField_EstablishmentId, ICmdField_SalesIds
    {
        public Guid EstablishmentId { get; set; }
        public List<Guid> SalesIds { get; set; } = new List<Guid>();
        public SalesSorting? SalesSorting { get; set; }
    }

    public interface ISalesReturn : IReturn
    {
        ISalesReturn Create(List<Sale> sales);
    }

    public class GetSalesReturn : ReturnBase, ISalesReturn
    {
        public List<Guid> Sales { get; set; } = new List<Guid>();

        public ISalesReturn Create(List<Sale> sales)
        {
            this.Sales = sales.Select(x => x.Id).ToList();
            return this;
        }
    }

    public class GetSalesDTOReturn : ReturnBase, ISalesReturn
    {
        public List<SaleDTO> Sales { get; set; }


        public ISalesReturn Create(List<Sale> sales)
        {
            this.Sales = sales.Select(x => new SaleDTO(x)).ToList();
            return this;
        }
    }

    public class GetSalesRawReturn : ReturnBase, ISalesReturn
    {
        public List<Sale> Sales { get; set; }


        public ISalesReturn Create(List<Sale> sales)
        {
            this.Sales = sales;
            return this;
        }
    }

    public class GetSalesHandler<T> : HandlerBase<GetSalesCommand, T> where T : ISalesReturn, new()
    {
        private IUnitOfWork unitOfWork;

        public GetSalesHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async override Task<T> Handle(GetSalesCommand command)
        {
            Establishment establishment = this.unitOfWork.establishmentRepository.IncludeSales().IncludeSalesItems().GetById(command.EstablishmentId)!;
            List<Sale> sales = establishment.GetSales();
            if (!command.SalesIds.IsNullOrEmpty())
            {
                sales = sales.Where(sale => command.SalesIds.Any(saleId => saleId == sale.Id)).ToList();
            }
            if (command.SalesSorting != null)
            {
                sales = SalesSortingParametersExecute.SortSales(sales, command.SalesSorting);
            }
            return (T)(new T()).Create(sales);
        }
    }
}
