using Microsoft.IdentityModel.Tokens;
using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Utils
{

    public enum SaleAttributes
    {
        Tables,
        Items,
        TimestampArrival,
        TimestampPayment,
    }

    public class FilterSales
    {
        public List<(DateTime start, DateTime end)>? ArrivalTimeframe { get; }
        public List<(DateTime start, DateTime end)>? PaymentTimeframe { get; }
        public List<SaleAttributes>? MustContainAllAttributes { get; }

        public FilterSales(
            List<(DateTime start, DateTime end)>? arrivalTimeframe = null,
            List<(DateTime start, DateTime end)>? paymentTimeframe = null,
            List<SaleAttributes>? mustContainAllAttributes = null)
        {
            this.ArrivalTimeframe = arrivalTimeframe;
            this.PaymentTimeframe = paymentTimeframe;
            this.MustContainAllAttributes = mustContainAllAttributes;
        }
    }

    public class FilterSalesBySalesItems
    {
        public List<Guid>? Any { get; }
        public List<Guid>? Excatly { get; }
        public List<Guid>? All { get; }
        //public List<(DateTime start, DateTime end)>? WithinTimeperiods { get; set; }
        //public List<SaleAttributes>? MustContainAllAttributes { get; set; }

        public FilterSalesBySalesItems(
            List<Guid>? any = null,
            List<Guid>? exactly = null,
            List<Guid>? all = null)
        //List<(DateTime start, DateTime end)>? withinTimeperiods = null,
        //List<SaleAttributes>? mustContainAllAttributes = null)
        {
            this.Any = any;
            this.Excatly = exactly;
            this.All = all;
            //this.WithinTimeperiods = withinTimeperiods;
            //this.MustContainAllAttributes = mustContainAllAttributes;
        }
    }

    public class FilterSalesBySalesTables
    {
        public List<Guid>? Any { get; }
        public List<Guid>? Excatly { get; }
        public List<Guid>? All { get; }

        public FilterSalesBySalesTables(
            List<Guid>? any = null,
            List<Guid>? exactly = null,
            List<Guid>? all = null)
        {
            this.Any = any;
            this.Excatly = exactly;
            this.All = all;
        }
    }



    public static class SalesFilterHelper
    {
        public static List<Sale> FilterSalesOnSalesItems(List<Sale> sales, FilterSalesBySalesItems parameter)
        {
            if (!parameter.Any.IsNullOrEmpty())
            {
                sales = sales.Where(sale => sale.SalesItems?.Any(salesItem => salesItem?.Item != null && parameter.Any.Contains(salesItem.Item.Id)) == true).ToList();
            }

            if (!parameter.All.IsNullOrEmpty())
            {
                sales = sales.Where(sale => parameter.All.All(itemId => sale.SalesItems.Any(salesItem => salesItem.Item.Id == itemId))).ToList();
            }

            if (!parameter.Excatly.IsNullOrEmpty())
            {
                sales = sales.Where(sale => sale.SalesItems.Count == parameter.Excatly.Count && parameter.Excatly.All(itemId => sale.SalesItems.Any(salesItem => salesItem.Item.Id == itemId))).ToList();
            }
            return sales;
        }

        public static List<Sale> FilterSalesOnSalesTables(List<Sale> sales, FilterSalesBySalesTables parameter)
        {
            if (!parameter.Any.IsNullOrEmpty())
            {
                sales = sales.Where(sale => sale.SalesTables?.Any(SalesTable => parameter.Any!.Contains(SalesTable.Table.Id)) == true).ToList();
            }

            if (!parameter.All.IsNullOrEmpty())
            {
                sales = sales.Where(sale => parameter.All!.All(itemId => sale.SalesTables.Any(salesItem => salesItem.Table.Id == itemId))).ToList();
            }

            if (!parameter.Excatly.IsNullOrEmpty())
            {
                sales = sales.Where(sale => sale.SalesTables.Count == parameter.Excatly.Count && parameter.Excatly.All(tableId => sale.SalesTables.Any(salesTables => salesTables.Table.Id == tableId))).ToList();
            }
            return sales;
        }

        public static List<Sale> FilterSales(List<Sale> sales, FilterSales parameters)
        {
            if (!parameters.ArrivalTimeframe.IsNullOrEmpty())
            {
                List<DateTimePeriod> periods = parameters.ArrivalTimeframe.Select(timeframe => new DateTimePeriod(timeframe.start, timeframe.end)).ToList();
                foreach (var period in periods)
                {
                    sales = sales.Where(sale => sale.GetTimeOfArrival() >= period.Start && sale.GetTimeOfArrival() <= period.End).ToList();
                }
            }

            if (!parameters.PaymentTimeframe.IsNullOrEmpty())
            {
                List<DateTimePeriod> periods = parameters.PaymentTimeframe.Select(timeframe => new DateTimePeriod(timeframe.start, timeframe.end)).ToList();
                foreach (var period in periods)
                {
                    sales = sales.Where(sale => sale.GetTimeOfPayment() >= period.Start && sale.GetTimeOfPayment() <= period.End).ToList();
                }
            }

            if (!parameters.MustContainAllAttributes.IsNullOrEmpty())
            {
                if (parameters.MustContainAllAttributes.Contains(SaleAttributes.Items))
                {
                    sales = sales.Where(sale => sale.SalesItems.Count > 0).ToList();
                }
                if (parameters.MustContainAllAttributes.Contains(SaleAttributes.Tables))
                {
                    sales = sales.Where(sale => sale.SalesTables.Count > 0).ToList();
                }
                if (parameters.MustContainAllAttributes.Contains(SaleAttributes.TimestampArrival))
                {
                    sales = sales.Where(sale => sale.TimestampArrival != null).ToList();
                }
                if (parameters.MustContainAllAttributes.Contains(SaleAttributes.TimestampPayment))
                {
                    sales = sales.Where(sale => sale.TimestampPayment != null).ToList();
                }
            }
            return sales;
        }




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
            if (saleAttributes.Contains(SaleAttributes.Tables))
            {
                sales = sales.Where(sale => sale.SalesTables != null).ToList();
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
