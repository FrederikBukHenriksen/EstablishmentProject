using WebApplication1.Domain_Layer.Services.Entity_builders;

namespace WebApplication1.Domain_Layer.Entities
{
    public interface IItemBuilderService : IEntityBuilder<Item>
    {
        IItemBuilderService withName(string name);
        IItemBuilderService withPrice(double price, Currency? currency = null);
    }

    public class ItemBuilderService : EntityBuilderBase<Item>, IItemBuilderService
    {
        private string? builderName = null;
        private Price? builderPrice = null;


        public override void ConstructEntity(Item entity)
        {
            this.Entity = new Item(this.builderName, this.builderPrice);
        }

        public IItemBuilderService withName(string name)
        {
            this.builderName = name;
            return this;
        }

        public IItemBuilderService withPrice(double price, Currency? currency = null)
        {
            currency ??= Currency.DKK;
            this.builderPrice = new Price(price, currency!);
            return this;
        }

    }
}

