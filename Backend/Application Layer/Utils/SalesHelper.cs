﻿using Microsoft.IdentityModel.Tokens;
using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Utils
{

    public enum SaleAttributes
    {
        Table,
        Items,
        TimestampPayment,
        TimestampArrival,
    }

    public class SalesSorting
    {
        public List<Guid>? Any { get; set; }
        public List<Guid>? Excatly { get; set; }
        public List<Guid>? All { get; set; }
        public List<(DateTime start, DateTime end)>? WithinTimeperiods { get; set; }
        public List<SaleAttributes>? MustContainAllAttributes { get; set; }

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
            return sales;
        }
    }

    public static class SalesHelper
    {

        public static List<Sale> AnyFilter(this List<Sale> sales, List<Guid> itemIds)
        {
            return sales.Where(sale => sale.SalesItems.Any(salesItem => itemIds.Contains(salesItem.Item.Id))).ToList();
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

        public static List<Sale> FilterSalesByNotNullAttribute(this List<Sale> sales, SaleAttributes saleAttributes)
        {
            return sales.Where(sale =>
            {
                switch (saleAttributes)
                {
                    case SaleAttributes.Table:
                        return sale.Table != null;

                    case SaleAttributes.Items:
                        return sale.SalesItems != null;

                    case SaleAttributes.TimestampPayment:
                        return sale.TimestampPayment != null;

                    case SaleAttributes.TimestampArrival:
                        return sale.TimestampArrival != null;

                    default:
                        return false;
                }
            }).ToList();
        }

    }
}
