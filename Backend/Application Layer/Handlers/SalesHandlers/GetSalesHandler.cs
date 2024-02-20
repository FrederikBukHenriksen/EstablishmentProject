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
        public List<Guid>? SalesIds { get; set; } = null;
        public FilterSales? FilterSales { get; set; } = null;
        public FilterSalesBySalesItems? FilterSalesBySalesItems { get; set; } = null;
        public FilterSalesBySalesTables? FilterSalesBySalesTables { get; set; } = null;
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
        public List<Sale> Sales { get; set; } = new List<Sale>();


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
            Establishment establishment = this.unitOfWork.establishmentRepository.IncludeSales().IncludeSalesItems().IncludeSalesTables().GetById(command.EstablishmentId)!;
            List<Sale> sales = establishment.GetSales();
            if (!command.SalesIds.IsNullOrEmpty())
            {
                sales = sales.Where(sale => command.SalesIds.Any(saleId => saleId == sale.Id)).ToList();
            }
            if (command.FilterSalesBySalesItems != null)
            {
                sales = SalesFilterHelper.FilterSalesOnSalesItems(sales, command.FilterSalesBySalesItems);
            }
            if (command.FilterSalesBySalesTables != null)
            {
                sales = SalesFilterHelper.FilterSalesOnSalesTables(sales, command.FilterSalesBySalesTables);
            }
            if (command.FilterSales != null)
            {
                sales = SalesFilterHelper.FilterSales(sales, command.FilterSales);
            }
            return (T)(new T()).Create(sales);
        }
    }
}
