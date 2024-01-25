using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Domain_Layer.Services.Repositories;
using WebApplication1.Services;

namespace WebApplication1.Application_Layer.Services
{

    public interface ISalesService
    {
        //List<Sale> GetSalesWhichContainItems(List<Guid>? mustContaiedItemIds);
        //List<Sale> GetSalesAssosicatedWithTables(List<Guid>? allowedTables);
        object GetAttributeValue(Sale sale, Func<Sale, object> selector);
    }



    public class SalesService : ISalesService
    {
        private readonly IEstablishmentRepository establishmentRepository;
        private readonly IUserContextService userContextService;

        public SalesService(IEstablishmentRepository establishmentRepository, IUserContextService userContextService)
        {
            this.establishmentRepository = establishmentRepository;
            this.userContextService = userContextService;
        }

        //public List<Sale> GetSalesWhichContainItems(List<Guid>? mustContaiedItemIds)
        //{
        //    List<Sale> sales = new List<Sale>();

        //    if (!mustContaiedItemIds.IsNullOrEmpty())
        //    {
        //        var activeEstablishment = this.userContextService.GetActiveEstablishment();

        //        List<Item> establishmentItems = this.establishmentRepository.GetEstablishmentItems(activeEstablishment.Id).ToList();

        //        sales = this.establishmentRepository.GetEstablishmentSales(activeEstablishment.Id).ToList();

        //        List<Item> mustBeConatineditems = establishmentItems.Where(x => mustContaiedItemIds.Contains(x.Id)).ToList();

        //        sales.SortSalesByRequiredConatinedItems(mustBeConatineditems);
        //    }
        //    return sales;
        //}

        //public List<Sale> GetSalesAssosicatedWithTables(List<Guid>? allowedTables)
        //{
        //    List<Sale> sales = new List<Sale>();

        //    if (!allowedTables.IsNullOrEmpty())
        //    {
        //        var activeEstablishment = this.userContextService.GetActiveEstablishment();

        //        List<Table> establishmentTables = this.establishmentRepository.GetEstablishmentTables(activeEstablishment.Id).ToList();

        //        sales = this.establishmentRepository.GetEstablishmentSales(activeEstablishment.Id).ToList();

        //        List<Table> possibleTables = establishmentTables.Where(x => allowedTables.Contains(x.Id)).ToList();

        //        sales.SortSalesByTables(possibleTables);
        //    }
        //    return sales;

        //}

        public object GetAttributeValue(Sale sale, Func<Sale, object> selector)
        {
            return selector(sale);
        }

    }
}
