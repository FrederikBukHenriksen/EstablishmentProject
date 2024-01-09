using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Domain_Layer.Services.Entity_builders
{

    public interface IFactoryServiceBuilder
    {
        IEstablishmentService EstablishmentBuilder();
        IEstablishmentService EstablishmentBuilder(Establishment establishment);
        IItemBuilderService ItemBuilder();
        IItemBuilderService ItemBuilder(Item item);
        ISaleBuilder SaleBuilder();
        ISaleBuilder SaleBuilder(Sale sale);
        IUserBuilder UserBuilder();
        IUserBuilder UserBuilder(User user);
    }

    public class FactoryServiceBuilder : IFactoryServiceBuilder
    {
        private IServiceProvider serviceProvider;

        public FactoryServiceBuilder(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IEstablishmentService EstablishmentBuilder()
        {
            return this.serviceProvider.GetRequiredService<IEstablishmentService>();
        }

        public IEstablishmentService EstablishmentBuilder(Establishment establishment)
        {
            var builderService = this.EstablishmentBuilder();
            builderService.UseExistingEntity(establishment);
            return builderService;
        }

        public ISaleBuilder SaleBuilder()
        {
            return this.serviceProvider.GetRequiredService<ISaleBuilder>();
        }

        public ISaleBuilder SaleBuilder(Sale sale)
        {
            var builderService = this.SaleBuilder();
            builderService.UseExistingEntity(sale);
            return builderService;
        }

        public IItemBuilderService ItemBuilder()
        {
            return this.serviceProvider.GetRequiredService<IItemBuilderService>();
        }

        public IItemBuilderService ItemBuilder(Item item)
        {
            var builderService = this.ItemBuilder();
            builderService.UseExistingEntity(item);
            return builderService;
        }

        public IUserBuilder UserBuilder()
        {
            return this.serviceProvider.GetRequiredService<IUserBuilder>();
        }

        public IUserBuilder UserBuilder(User user)
        {
            var builderService = this.UserBuilder();
            builderService.UseExistingEntity(user);
            return builderService;
        }
    }
}
