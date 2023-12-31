﻿using WebApplication1.Domain.Entities;
using WebApplication1.Domain.Services.Repositories;

namespace WebApplication1.Domain_Layer.Services.Entity_builders
{

    public interface IFactoryServiceBuilder
    {
        IEstablishmentBuilder EstablishmentBuilder();
        IEstablishmentBuilder EstablishmentBuilder(Establishment establishment);
        IItemBuilder ItemBuilder();
        IItemBuilder ItemBuilder(Item item);
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

        public IEstablishmentBuilder EstablishmentBuilder()
        {
            return serviceProvider.GetRequiredService<IEstablishmentBuilder>();
        }

        public IEstablishmentBuilder EstablishmentBuilder(Establishment establishment)
        {
            var builderService = this.EstablishmentBuilder();
            builderService.UseExistingEntity(establishment);
            return builderService;
        }

        public ISaleBuilder SaleBuilder()
        {
            return serviceProvider.GetRequiredService<ISaleBuilder>();
        }

        public ISaleBuilder SaleBuilder(Sale sale)
        {
            var builderService = this.SaleBuilder();
            builderService.UseExistingEntity(sale);
            return builderService;
        }

        public IItemBuilder ItemBuilder()
        {
            return serviceProvider.GetRequiredService<IItemBuilder>();
        }

        public IItemBuilder ItemBuilder(Item item)
        {
            var builderService = this.ItemBuilder();
            builderService.UseExistingEntity(item);
            return builderService;
        }

        public IUserBuilder UserBuilder()
        {
            return serviceProvider.GetRequiredService<IUserBuilder>();
        }

        public IUserBuilder UserBuilder(User user)
        {
            var builderService = this.UserBuilder();
            builderService.UseExistingEntity(user);
            return builderService;
        }
    }
}
