using Microsoft.IdentityModel.Tokens;
using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Utils
{

    public enum SaleAttributes
    {
        Table,
        Items,
        TimestampArrival,
        TimestampPayment,

    }

    public class SalesSorting
    {
        public List<Guid>? Any { get; set; }
        public List<Guid>? Excatly { get; set; }
        public List<Guid>? All { get; set; }
        public List<(DateTime start, DateTime end)>? WithinTimeperiods { get; set; }
        public List<SaleAttributes>? MustContainAllAttributes { get; set; }

        public SalesSorting(
            List<Guid>? any = null,
            List<Guid>? exactly = null,
            List<Guid>? all = null,
            List<(DateTime start, DateTime end)>? withinTimeperiods = null,
            List<SaleAttributes>? mustContainAllAttributes = null)
        {
            this.Any = any;
            this.Excatly = exactly;
            this.All = all;
            this.WithinTimeperiods = withinTimeperiods;
            this.MustContainAllAttributes = mustContainAllAttributes;
        }


    }

    public static class SalesSortingParametersExecute
    {
        public static List<Sale> SortSales(List<Sale> sales, SalesSorting salesSortingParameters)
        {
            if (!salesSortingParameters.Any.IsNullOrEmpty())
            {
                sales = SalesHelper.AnyFilter(sales, salesSortingParameters.Any);
            }
            if (!salesSortingParameters.All.IsNullOrEmpty())
            {
                sales = SalesHelper.AllFilter(sales, salesSortingParameters.All);
            }
            if (!salesSortingParameters.Excatly.IsNullOrEmpty())
            {
                sales = SalesHelper.ExcatlyFilter(sales, salesSortingParameters.Excatly);
            }
            if (!salesSortingParameters.WithinTimeperiods.IsNullOrEmpty())
            {
                sales = SalesHelper.SortSalesByTimePeriod(sales, salesSortingParameters.WithinTimeperiods);
            }
            if (!salesSortingParameters.MustContainAllAttributes.IsNullOrEmpty())
            {
                sales = SalesHelper.FilterSalesByNotNullAttribute(sales, salesSortingParameters.MustContainAllAttributes);
            }
            return sales;
        }
    }

    public static class SalesHelper
    {

        public static List<Sale> AnyFilter(this List<Sale> sales, List<Guid> itemIds)
        {
            return sales.Where(sale => sale.SalesItems?.Any(salesItem => salesItem?.Item != null && itemIds.Contains(salesItem.Item.Id)) == true).ToList();
        }

        public static List<Sale> AllFilter(this List<Sale> sales, List<Guid> itemIds)
        {
            return sales.Where(sale => itemIds.All(itemId => sale.SalesItems.Any(salesItem => salesItem.Item.Id == itemId))).ToList();
        }

        public static List<Sale> ExcatlyFilter(this List<Sale> sales, List<Guid> itemIds)
        {
            return sales.Where(sale => sale.SalesItems.Count == itemIds.Count && itemIds.All(itemId => sale.SalesItems.Any(salesItem => salesItem.Item.Id == itemId))).ToList();
        }

        public static List<Sale> SortSalesByTimePeriod(this List<Sale> sales, List<(DateTime start, DateTime end)> timeframes)
        {
            List<DateTimePeriod> periods = timeframes.Select(timeframe => new DateTimePeriod(timeframe.start, timeframe.end)).ToList();
            foreach (var period in periods)
            {
                sales = sales.Where(sale => sale.GetTimeOfSale() >= period.Start && sale.GetTimeOfSale() <= period.End).ToList();
            }
            return sales;
        }

        public static List<Sale> FilterSalesByNotNullAttribute(this List<Sale> sales, List<SaleAttributes> saleAttributes)
        {
            if (saleAttributes.Contains(SaleAttributes.Items))
            {
                sales = sales.Where(sale => sale.SalesItems != null).ToList();
            }
            if (saleAttributes.Contains(SaleAttributes.Table))
            {
                sales = sales.Where(sale => sale.Table != null).ToList();
            }
            if (saleAttributes.Contains(SaleAttributes.TimestampArrival))
            {
                sales = sales.Where(sale => sale.TimestampArrival != null).ToList();
            }
            if (saleAttributes.Contains(SaleAttributes.TimestampPayment))
            {
                sales = sales.Where(sale => sale.TimestampPayment != null).ToList();
            }
            return sales;
        }
    }
}
