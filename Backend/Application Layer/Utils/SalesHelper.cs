using Microsoft.IdentityModel.Tokens;
using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Utils
{

    public class SalesSorting
    {
        public List<Guid>? MustContainSomeItems { get; set; }
        public List<Guid>? MustContainAllItems { get; set; }
        public List<Guid>? MustContainSomeTables { get; set; }
        public List<Guid>? MustContainAllTables { get; set; }
        public List<DateTimePeriod>? UseDataFromTimeframePeriods { get; set; }
    }

    public static class SalesSortingParametersExecute
    {
        public static IEnumerable<Sale> SortSales(this IEnumerable<Sale> sales, SalesSorting salesSortingParameters)
        {
            if (!salesSortingParameters.UseDataFromTimeframePeriods.IsNullOrEmpty())
            {
                sales = SalesHelper.SortSalesByTimePeriods(sales.ToList(), salesSortingParameters.UseDataFromTimeframePeriods);
            }
            if (!salesSortingParameters.MustContainSomeItems.IsNullOrEmpty())
            {
                sales = SalesHelper.SortSalesByItems(sales, salesSortingParameters.MustContainSomeItems);
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
