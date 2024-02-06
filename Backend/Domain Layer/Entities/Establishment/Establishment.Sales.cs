using Microsoft.IdentityModel.Tokens;

namespace WebApplication1.Domain_Layer.Entities
{
    public interface IEstablishment_Sale
    {
        Sale AddSale(Sale sale);
        Sale CreateSale(DateTime timestampPayment, Table? table = null, List<(Item, int)>? itemAndQuantity = null, DateTime? timestampArrival = null);
        List<Sale> GetSales();
        void RemoveSale(Sale sale);
        Sale UpdateSale(Sale sale);
    }

    public partial class Establishment : EntityBase, IEstablishment_Sale
    {
        //CRUD
        public Sale CreateSale(DateTime timestampPayment, Table? table = null, List<(Item, int)>? itemAndQuantity = null, DateTime? timestampArrival = null)
        {
            Sale sale = new Sale(timestampPayment, timestampArrival, table: table, salesItems: itemAndQuantity);
            this.AddSale(sale);
            return sale;
        }

        public List<Sale> GetSales()
        {
            return this.Sales.ToList();
        }

        public Sale UpdateSale(Sale sale)
        {
            if (this.DoesSaleAlreadyExist(sale))
            {
                this.RemoveSale(sale);
                this.AddSale(sale);
            }
            return sale;
        }

        public void RemoveSale(Sale sale)
        {
            if (this.DoesSaleAlreadyExist(sale))
            {
                this.Sales.Remove(sale);
            }
        }


        public Sale AddSale(Sale sale)
        {
            if (!sale.SalesItems.IsNullOrEmpty())
            {
                if (this.IsItemsRegisteredToEstablishment(sale))
                {
                    throw new Exception("Not all items are registered to establishment");
                }
            }
            this.Sales.Add(sale);
            return sale;
        }

        //Checkers and validators
        private bool DoesSaleAlreadyExist(Sale sale)
        {
            return this.Sales.Any(x => x.Id == sale.Id);
        }

        private bool IsItemsRegisteredToEstablishment(Sale sale)
        {
            bool itemNotRegistered = false;
            foreach (var salesItem in sale.SalesItems)
            {
                if (!this.GetItems().Any(x => x.Id == salesItem.Item.Id))
                {
                    itemNotRegistered = true;
                }
            }
            return itemNotRegistered;
        }
    }
}
