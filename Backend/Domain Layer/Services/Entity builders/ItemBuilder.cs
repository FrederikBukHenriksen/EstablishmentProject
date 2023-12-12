using Microsoft.IdentityModel.Tokens;
using WebApplication1.Domain_Layer.Services.Entity_builders;

    namespace WebApplication1.Domain.Entities
    {
        public interface IItemBuilder : IEntityBuilder<Item>{
        IItemBuilder WithName(string name);
        IItemBuilder WithPrice(double price);
    }

    public class ItemBuilder : EntityBuilderBase<Item>, IItemBuilder
    {
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

        public override bool EntityValidation()
        {
            if (!this.doesItemHaveAName()) throw new System.Exception("Item must have a name");
            if (!this.doesItemHavePrice()) throw new System.Exception("Item must have a price");
            return true;
        }

        private bool doesItemHavePrice()
        {
            return Entity.Price > 0;
        }

        private bool doesItemHaveAName()
        {
            return Entity.Name.IsNullOrEmpty();
        }

    }
}

