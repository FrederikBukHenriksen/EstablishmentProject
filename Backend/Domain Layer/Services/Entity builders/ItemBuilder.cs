using Microsoft.IdentityModel.Tokens;
using WebApplication1.Domain_Layer.Services.Entity_builders;

namespace WebApplication1.Domain_Layer.Entities
{
    public interface IItemBuilder : IEntityBuilder<Item>
    {
    }

    public class ItemBuilder : EntityBuilderBase<Item>, IItemBuilder
    {
        private string? builderName = null;
        private Price? builderPrice = null;


        public override void ReadPropertiesOfEntity(Item entity)
        {
            this.builderName = entity.Name;
            this.builderPrice = entity.Price;
        }

        public override void WritePropertiesOfEntity(Item entity)
        {
            entity.Name = (string)this.builderName;
            entity.Price = this.builderPrice;
        }

        public IItemBuilder WithName(string name)
        {
            this.builderName = name;
            return this;
        }

        public IItemBuilder WithPrice(Price price)
        {
            this.builderPrice = price;
            return this;
        }

        public override bool Validation()
        {
            if (!this.doesItemHaveAName()) throw new Exception("Item must have a name");
            return true;
        }


        private bool doesItemHaveAName()
        {
            return !(this.builderName.IsNullOrEmpty());
        }
    }
}

