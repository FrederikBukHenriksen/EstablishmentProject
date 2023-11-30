using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using WebApplication1.Domain.Entities;

namespace WebApplication1.Utils
{

    public static partial class SalesHelper
    {


        public static List<Sale> SortSalesByTimePeriod(this List<Sale> sales, TimePeriod timePeriods)
        {
            return sales.SortEntitiesWithinTimePeriod(timePeriods, (x) => x.TimestampPayment);
        }

        public static List<Sale> SortSalesByTimePeriods(this List<Sale> sales, List<TimePeriod> timePeriods)
        {
            return sales.SortEntitiesWithinTimePeriods(timePeriods, (x) => x.TimestampPayment);
        }

        public static List<Sale> SortSalesByItems(this List<Sale> sales, List<Item> mustContainedItems)
        {
            return sales.Where(sale => mustContainedItems.Any(item => sale.SalesItems.Any(x => x.Item == item))).ToList();
        }

        public static List<Sale> SortSalesByRequiredConatinedItems(this List<Sale> sales, List<Item> mustContainedItems)
        {
            return sales.Where(sale => mustContainedItems.All(item => sale.SalesItems.Any(x => x.Item == item))).ToList();
        }

        public static List<Sale> SortSalesByTables(this List<Sale> sales, List<Table> possibleTables)
        {
            return sales.Where(sale => possibleTables.Contains(sale.Table)).ToList();
        }
    }
}
