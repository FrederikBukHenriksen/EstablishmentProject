    using WebApplication1.Domain_Layer.Services.Entity_builders;

    namespace WebApplication1.Domain.Entities
    {
        public interface IItemBuilder : IEntityBuilder<Item>
        {        IItemBuilder WithName(string name);
        IItemBuilder WithPrice(double price);
    }

    public class ItemBuilder : EntityBuilderBase<Item>, IItemBuilder
    {
        //public override IEntityBuilder<Item> UseExistingEntity(Item entity)
        //{
        //    this.Entity = entity;
        //    return this;
        //}

        public IItemBuilder WithName(string name)
        {
            Entity.Name = name;
            return this;
        }

        public IItemBuilder WithPrice(double price)
        {
            Entity.Price = price;
            return this;
        }
    }
}

