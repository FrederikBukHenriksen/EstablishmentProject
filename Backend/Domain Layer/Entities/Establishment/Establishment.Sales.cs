namespace WebApplication1.Domain.Entities
{
    public partial class Establishment
    {
        public void AddSale(Sale sale)
        {
            if (this.AddSaleValidation(sale))
            {
                this.Sales.Add(sale);
            }
        }

        public bool AddSaleValidation(Sale sale)
        {
            if (!sale.SalesItems.All(x => this.IsItemRegisteredToEstablishment(x.Item))) throw new Exception("Sold item from sale is not registered to establishment");
            return true;
        }

        public void RemoveSale(Sale sale)
        {
            this.Sales.Remove(sale);
        }

        public ICollection<Sale> GetSales()
        {
            return this.Sales;
        }

        //Valdiators
        private bool IsItemRegisteredToEstablishment(Item item)
        {
            return this.Items.Contains(item);
        }
    }
}
