using Microsoft.IdentityModel.Tokens;
using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Utils
{

    public enum SaleAttributes
    {
        Table,
        Employee,
        Items,
        TimestampPayment,
        TimestampCreation,
        EstablishmentId,
    }

    public class SalesSorting
    {
        public List<Guid>? Any { get; set; }
        public List<Guid>? Contains { get; set; }
        public List<Guid>? All { get; set; }
        public List<DateTimePeriod>? WithinTimeperiods { get; set; }
        public List<SaleAttributes>? MustContainAllAttributes { get; set; }
    }

    public static class SalesSortingParametersExecute
    {
        public static IEnumerable<Sale> SortSales(this IEnumerable<Sale> sales, SalesSorting salesSortingParameters)
        {
            if (!salesSortingParameters.WithinTimeperiods.IsNullOrEmpty())
            {
                sales = SalesHelper.SortSalesByTimePeriods(sales.ToList(), salesSortingParameters.WithinTimeperiods);
            }
            if (!salesSortingParameters.Any.IsNullOrEmpty())
            {
                sales = SalesHelper.SortSalesByItems(sales, salesSortingParameters.Any);
            }
            return sales;
        }
    }
    public static partial class SalesHelper
    {

        public static List<Sale> SortSalesByTimePeriod(this List<Sale> sales, DateTimePeriod timePeriods)
        {
            return sales.SortEntitiesWithinDateTimePeriod(timePeriods, (x) => x.TimestampPayment);
        }

        public static List<Sale> SortSalesByTimePeriods(this List<Sale> sales, List<DateTimePeriod> timePeriods)
        {
            return sales.SortEntitiesWithinTimePeriods(timePeriods, (x) => x.TimestampPayment);
        }

        public static IEnumerable<Sale> SortSalesByItems(this IEnumerable<Sale> sales, IEnumerable<Item> mustContainedItems)
        {
            return sales.Where(sale => mustContainedItems.All(item => sale.SalesItems.Any(x => x.Item == item))).ToList();
        }

        public static IEnumerable<Sale> SortSalesByItems(this IEnumerable<Sale> sales, IEnumerable<Guid> mustContainedItems)
        {
            return sales.Where(sale => mustContainedItems.All(item => sale.SalesItems.Any(x => x.Item.Id == item))).ToList();
        }

        public static IEnumerable<Sale> SortSalesByRequiredConatinedItems(this IEnumerable<Sale> sales, IEnumerable<Item> mustContainedItems)
        {
            return sales.Where(sale => mustContainedItems.All(item => sale.SalesItems.Any(x => x.Item == item))).ToList();
        }
        public static IEnumerable<Sale> SortSalesByTables(this IEnumerable<Sale> sales, IEnumerable<Table> possibleTables)
        {
            return sales.Where(sale => possibleTables.Contains(sale.Table)).ToList();
        }
    }
}
