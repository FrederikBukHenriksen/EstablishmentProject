using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.Domain.Services.Repositories;
using WebApplication1.Domain_Layer.Services.Entity_builders;

    namespace WebApplication1.Domain.Entities
    {
        public interface IEstablishmentBuilder : IEntityBuilder<Establishment>
        {
        IEstablishmentBuilder WithName(string name);
        IEstablishmentBuilder WithItems(ICollection<Item> items);
        IEstablishmentBuilder WithTables(ICollection<Table> tables);
        IEstablishmentBuilder WithSales(ICollection<Sale> sales);
        }

        public class EstablishmentBuilder : EntityBuilderBase<Establishment>, IEstablishmentBuilder
        {
        private IEstablishmentRepository establishmentRepository;

        public EstablishmentBuilder([FromServices] IEstablishmentRepository establishmentRepository)
        {
            this.establishmentRepository = establishmentRepository;
        }

        public IEstablishmentBuilder WithName(string name)
        {
            Entity.Name = name;
            return this;
        }

        public IEstablishmentBuilder WithItems(ICollection<Item> items)
        {
            Entity.Items = items;
            return this;
        }

        public IEstablishmentBuilder WithTables(ICollection<Table> tables)
        {
            Entity.Tables = tables;
            return this;
        }

        public IEstablishmentBuilder WithSales(ICollection<Sale> sales)
        {
            Entity.Sales = sales;
            return this;
        }

        public override bool EntityValidation()
        {
            if (!this.doesEstablishmentHaveAName()) throw new System.Exception("Establishment must have a name");
            if (!this.isItemsInSalesIsAssignedToTheEstablishment()) throw new System.Exception("Items in sales must exist in the establishment");
            return true;
        }

        private bool doesEstablishmentHaveAName()
        {
            return Entity.Name.IsNullOrEmpty();
        }

        private bool isItemsInSalesIsAssignedToTheEstablishment()
        {
            ICollection<Item> allItemsFromSales = Entity.Sales.SelectMany(sale => sale.SalesItems.Select(saleItem => saleItem.Item)).ToList();
            ICollection<Item> establishmentItems = Entity.Items;
            bool establishmentHasAllItems = allItemsFromSales.All(item => establishmentItems.Contains(item));
            return establishmentHasAllItems;

        }
    }
}

