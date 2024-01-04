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

        public override void ReadPropertiesOfEntity(SalesItems entity)
        {
            this.builderSale = entity.Sale;
            this.builderItem = entity.Item;
            this.builderQuantity = entity.Quantity;
        }

        public override void WritePropertiesOfEntity(SalesItems Entity)
        {
            Entity.Sale = (Sale)this.builderSale;
            Entity.Item = (Item)this.builderItem;
            Entity.Quantity = (int)this.builderQuantity;
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
        public override bool BuildValidation()
        {
            if (builderQuantity <= 0) throw new Exception("Quantity must be larger than zero");
            return true;
        }
    }
}
