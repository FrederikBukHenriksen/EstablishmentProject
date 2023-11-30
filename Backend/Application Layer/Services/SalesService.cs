using Microsoft.IdentityModel.Tokens;
using WebApplication1.Domain.Entities;
using WebApplication1.Domain.Services.Repositories;
using WebApplication1.Services;
using WebApplication1.Utils;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WebApplication1.Application_Layer.Services
{

    public interface ISalesService
    {
        List<Sale> GetSalesWhichContainItems(List<Guid>? mustContaiedItemIds);
        List<Sale> GetSalesAssosicatedWithTables(List<Guid>? allowedTables);
        object GetAttributeValue(Sale sale, Func<Sale, object> selector);
    }



    public class SalesService : ISalesService
    {
        private readonly IEstablishmentRepository establishmentRepository;
        private readonly IUserContextService userContextService;

        private readonly Establishment activeEstablishment;
        public SalesService(IEstablishmentRepository establishmentRepository, IUserContextService userContextService)
        {
            this.establishmentRepository = establishmentRepository;
            this.userContextService = userContextService;
            this.activeEstablishment = this.userContextService.GetActiveEstablishment();
        }
        
        public List<Sale> GetSalesWhichContainItems(List<Guid>? mustContaiedItemIds)
        {
            List<Sale> sales = new List<Sale>();

            if (!mustContaiedItemIds.IsNullOrEmpty())
            {
                List<Item> establishmentItems = establishmentRepository.GetEstablishmentItems(activeEstablishment.Id).ToList();

                sales = establishmentRepository.GetEstablishmentSales(activeEstablishment.Id).ToList();

                List<Item> mustBeConatineditems = establishmentItems.Where(x => mustContaiedItemIds.Contains(x.Id)).ToList();

                sales.SortSalesByRequiredConatinedItems(mustBeConatineditems);
            }
            return sales;
        }

        public List<Sale> GetSalesAssosicatedWithTables(List<Guid>? allowedTables)
        {
            List<Sale> sales = new List<Sale>();

            if (!allowedTables.IsNullOrEmpty())
            {
                List<Table> establishmentTables = establishmentRepository.GetEstablishmentTables(activeEstablishment.Id).ToList();

                sales = establishmentRepository.GetEstablishmentSales(activeEstablishment.Id).ToList();

                List<Table> possibleTables = establishmentTables.Where(x => allowedTables.Contains(x.Id)).ToList();

                sales.SortSalesByTables(possibleTables);
            }
            return sales;

        }

        public object GetAttributeValue(Sale sale, Func<Sale,object> selector)
        {
            return selector(sale);
        }

    }
}
