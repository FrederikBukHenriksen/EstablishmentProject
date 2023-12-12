using WebApplication1.Domain.Entities;
using static WebApplication1.Utils.SalesHelper;
using WebApplication1.Data.DataModels;
using static WebApplication1.CommandHandlers.MeanSales.MeanSalesHandler;
using Microsoft.IdentityModel.Tokens;

namespace WebApplication1.Utils
{

    public class SalesSortingParameters
    {
        public List<Guid>? MustContaiedItems { get; set; }
        public List<DateTimePeriod>? UseDataFromTimeframePeriods { get; set; }
    }

    public static class SalesSortingParametersExecute
    {
        public static List<Sale> SortSales(this List<Sale> sales, SalesSortingParameters salesSortingParameters)
        {
            if (!salesSortingParameters.UseDataFromTimeframePeriods.IsNullOrEmpty())
            {
                sales = SalesHelper.SortSalesByTimePeriods(sales, salesSortingParameters.UseDataFromTimeframePeriods);
            }
            if (!salesSortingParameters.MustContaiedItems.IsNullOrEmpty())
            {
                sales = SalesHelper.SortSalesByItems(sales, salesSortingParameters.MustContaiedItems);
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

        public static List<Sale> SortSalesByItems(this List<Sale> sales, List<Item> mustContainedItems)
        {
            return sales.Where(sale => mustContainedItems.All(item => sale.SalesItems.Any(x => x.Item == item))).ToList();
        }

        public static List<Sale> SortSalesByItems(this List<Sale> sales, List<Guid> mustContainedItems)
        {
            return sales.Where(sale => mustContainedItems.All(item => sale.SalesItems.Any(x => x.Item.Id == item))).ToList();
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
