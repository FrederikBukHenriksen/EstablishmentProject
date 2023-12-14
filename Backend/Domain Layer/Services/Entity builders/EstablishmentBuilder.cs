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

        private string? builderName = null;
        private ICollection<Item>? builderItems = null;
        private ICollection<Table>? builderTables = null;
        private ICollection<Sale>? builderSales = null;

        public EstablishmentBuilder([FromServices] IEstablishmentRepository establishmentRepository)
        {
            this.establishmentRepository = establishmentRepository;
        }
        public override void ReadPropertiesOfEntity(Establishment entity)
        {
            this.builderName = entity.Name;
            this.builderItems = entity.Items;
            this.builderTables = entity.Tables;
            this.builderSales = entity.Sales;
        }

        public override void WritePropertiesOfEntity(Establishment Entity)
        {
            Entity.Name = (string)this.builderName;
            Entity.Items = (ICollection<Item>)this.builderItems;
            Entity.Tables = (ICollection<Table>)this.builderTables;
            Entity.Sales = (ICollection<Sale>)this.builderSales;
        }

        public IEstablishmentBuilder WithName(string name)
        {
            this.builderName = name;
            return this;
        }

        public IEstablishmentBuilder WithItems(ICollection<Item> items)
        {
            this.builderItems = items;
            return this;
        }

        public IEstablishmentBuilder WithTables(ICollection<Table> tables)
        {
            this.builderTables = tables;
            return this;
        }

        public IEstablishmentBuilder WithSales(ICollection<Sale> sales)
        {
            this.builderSales = sales;
            return this;
        }

        public override bool Validation()
        {
            if (!this.doesEstablishmentHaveAName()) throw new System.Exception("Establishment must have a name");
            if (!this.isItemsInSalesIsAssignedToTheEstablishment()) throw new System.Exception("Items in sales must exist in the establishment");
            return true;
        }

        private bool doesEstablishmentHaveAName()
        {
            return this.builderName.IsNullOrEmpty();
        }

        private bool isItemsInSalesIsAssignedToTheEstablishment()
        {
            if (!builderSales.IsNullOrEmpty())
            {
                ICollection<Item> allItemsFromSales = builderSales.SelectMany(sale => sale.SalesItems.Select(saleItem => saleItem.Item)).ToList();
                ICollection<Item> establishmentItems = builderItems;
                bool establishmentHasAllItems = allItemsFromSales.All(item => establishmentItems.Contains(item));
                return establishmentHasAllItems;
            } else
            {
                return true;
            }
        }


    }
}

