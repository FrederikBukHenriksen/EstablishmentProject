﻿using WebApplication1.Data.DataModels;

namespace WebApplication1.Domain_Layer.Entities
{
    public interface ISale_SalesItems
    {
        void AddSalesItems(SalesItems salesItem);
        SalesItems CreateSalesItems(Item item, int quantity);
        List<SalesItems> GetSalesItems();
        void RemoveSalesItems(SalesItems salesItem);
    }

    public partial class Sale : ISale_SalesItems
    {
        public void AddSalesItems(SalesItems salesItem)
        {
            this.SalesItemsMustBeCreatedForSale(salesItem);
            this.SalesItemsMustNotAlreadyExist(salesItem);
            this.SalesItems.Add(salesItem);
        }

        public SalesItems CreateSalesItems(Item item, int quantity)
        {
            return new SalesItems(this, item, quantity);
        }

        public List<SalesItems> GetSalesItems()
        {
            return this.SalesItems.ToList();
        }

        public void RemoveSalesItems(SalesItems salesItem)
        {
            this.SalesItemsMustExist(salesItem);
            this.SalesItems.Remove(salesItem);

        }

        //Checkers and validators

        protected void SalesItemsMustExist(SalesItems salesItem)
        {
            if (!this.DoesSalesItemExistInSale(salesItem))
            {
                throw new InvalidOperationException("SalesItem does not exist within sale");
            }
        }

        protected void SalesItemsMustNotAlreadyExist(SalesItems salesItem)
        {
            if (this.DoesSalesItemExistInSale(salesItem))
            {
                throw new InvalidOperationException("SalesItem already exists within sale");
            }
        }

        protected void SalesItemsMustBeCreatedForSale(SalesItems salesItems)
        {
            if (!this.IsSalesItemsCreatedForSale(salesItems))
            {
                throw new InvalidOperationException("SalesItems is not created for sale");
            }
        }
        private bool DoesSalesItemExistInSale(SalesItems salesItem)
        {
            return this.GetSalesItems().Any(x => x.Item == salesItem.Item);
        }

        private bool IsSalesItemsCreatedForSale(SalesItems salesItems)
        {
            return salesItems.Sale == this;
        }

    }
}
