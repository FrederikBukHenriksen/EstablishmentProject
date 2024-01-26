using WebApplication1.Data.DataModels;
using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Domain_Layer.Services.Entity_builders
{
    public interface ISalesItemsBuilder : IEntityBuilder<SalesItems>
    {
        ISalesItemsBuilder WithSale(Sale sale);
        ISalesItemsBuilder WithItem(Item item);
        ISalesItemsBuilder WithQuantity(int quantity);
    }
    public class SalesItemsBuilder : EntityBuilderBase<SalesItems>, ISalesItemsBuilder
    {

        private Sale? builderSale = null;
        private Item? builderItem = null;
        private int? builderQuantity = null;

        public override SalesItems Build()
        {
            return new SalesItems(Item: this.builderItem, Quantity: (int)this.builderQuantity);
        }

        public ISalesItemsBuilder WithItem(Item item)
        {
            this.builderItem = item;
            return this;
        }

        public ISalesItemsBuilder WithQuantity(int quantity)
        {
            this.builderQuantity = quantity;
            return this;
        }

        public ISalesItemsBuilder WithSale(Sale sale)
        {
            this.builderSale = sale;
            return this;
        }
    }
}
