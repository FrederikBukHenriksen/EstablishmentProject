﻿using WebApplication1.Domain.Entities;

namespace WebApplication1.Domain.Services.Repositories
{

    public interface ISalesRepository : IRepository<Sale>
    {
        List<Sale> GetSalesFromEstablishment(Establishment establishment);
        List<Item> GetSoldItems(Sale sale);
        Sale IncludeSalesItems(Sale sale);
        List<Sale> IncludeSalesItems(List<Sale> sales);

    }

    public class SalesRepository : Repository<Sale>, ISalesRepository
    {
        public SalesRepository(ApplicationDbContext context) : base(context)
        {
        }
        public List<Sale> GetSalesFromEstablishment(Establishment establishment)
        {
            var res = context
                .Set<Sale>()

                .Include(x => x.Establishment)

                .Where(x => x.Establishment == establishment)
                .ToList();

            return res;
        }

        public List<Item> GetSoldItems(Sale sale)
        {
            var res = context
                .Set<Sale>()

                .Include(x => x.SalesItems)
                .ThenInclude(x => x.Item)

                .Where(x => x == sale).First()
                .SalesItems
                .Select(x => x.Item)
                .ToList();

            return res;
        }

        public Sale IncludeSalesItems(Sale sale)
        {
            sale.SalesItems = context
                .Set<Sale>()
                .Include(x => x.SalesItems)
                .ThenInclude(x => x.Item)
                .Where(x => x == sale).First()
                .SalesItems
                .ToList();

            return sale;
        }

        public List<Sale> IncludeSalesItems(List<Sale> sales)
        {
            List<Sale> updatedSalesList = new List<Sale>();

            foreach (var sale in sales)
            {
                Sale updatedSale = IncludeSalesItems(sale);
                updatedSalesList.Add(updatedSale);
            }

            return updatedSalesList;
        }
    }
}


