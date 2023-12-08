using WebApplication1.Data.DataModels;
using WebApplication1.Domain.Entities;

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
        //public override IEntityBuilder<SalesItems> UseExistingEntity(SalesItems entity)
        //{
        //    this.Entity = entity;
        //    return this;
        //}

        public ISalesItemsBuilder WithItem(Item item)
        {
            Entity.Item = item;
            return this;
        }

        public ISalesItemsBuilder WithQuantity(int quantity)
        {
            Entity.Quantity = quantity;
            return this;
        }

        public ISalesItemsBuilder WithSale(Sale sale)
        {
            Entity.Sale = sale;
            return this;
        }
    }
}
