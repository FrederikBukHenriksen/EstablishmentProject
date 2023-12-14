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
        private string? builderName = null;
        private double? builderPrice = null;


        public override void ReadPropertiesOfEntity(Item entity)
        {
            this.builderName = entity.Name;
            this.builderPrice = entity.Price;
        }

        public override void WritePropertiesOfEntity(Item entity)
        {
            entity.Name = (string)this.builderName;
            entity.Price = (double)this.builderPrice;
        }

        public IItemBuilder WithName(string name)
        {
            this.builderName = name;
            return this;
        }

        public IItemBuilder WithPrice(double price)
        {
            this.builderPrice = price;
            return this;
        }

        public override bool Validation()
        {
            if (!this.doesItemHaveAName()) throw new Exception("Item must have a name");
            if (!this.doesItemHavePrice()) throw new Exception("Item must have a price");
            return true;
        }

        private bool doesItemHavePrice()
        {
            return this.builderPrice >= 0;
        }

        private bool doesItemHaveAName()
        {
            return !(this.builderName.IsNullOrEmpty());
        }
    }
}

