using Microsoft.IdentityModel.Tokens;
using WebApplication1.Domain_Layer.Services.Entity_builders;

namespace WebApplication1.Domain_Layer.Entities
{
    public interface IItemBuilderService : IEntityBuilder<Item>
    {
        IItemBuilderService WithName(string name);
        IItemBuilderService WithPrice(double price);
    }

    public class ItemBuilderService : EntityBuilderBase<Item>, IItemBuilderService
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

        public IItemBuilderService WithName(string name)
        {
            this.builderName = name;
            return this;
        }

        public IItemBuilderService WithPrice(double price)
        {
            this.builderPrice = new Price(price, Currency.DKK);
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

