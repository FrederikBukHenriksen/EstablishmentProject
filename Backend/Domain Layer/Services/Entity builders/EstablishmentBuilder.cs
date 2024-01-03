using Microsoft.IdentityModel.Tokens;
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
        private string? builderName = null;
        private ICollection<Item> builderItems = new List<Item>();
        private ICollection<Table> builderTables = new List<Table>();
        private ICollection<Sale> builderSales = new List<Sale>();

        public EstablishmentBuilder()
        {
        }

        public override void ReadPropertiesOfEntity(Establishment entity)
        {
            this.builderName = entity.Name;
            this.builderItems = entity.GetItems();
            this.builderTables = entity.Tables;
            this.builderSales = entity.Sales;
        }

        public override void WritePropertiesOfEntity(Establishment Entity)
        {
            Entity.Name = (string)this.builderName;
            Entity.AddItems((ICollection<Item>)this.builderItems);
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
            if (!this.doesEstablishmentHaveName(this.builderName)) throw new System.Exception("Establishment must have a name");
            if (!this.isItemsInSalesIsAssignedToTheEstablishment(this.builderSales)) throw new System.Exception("Sold items in sales must exist in the establishment");
            return true;
        }

        private bool doesEstablishmentHaveName(string name)
        {
            return !name.IsNullOrEmpty();
        }

        private bool isItemsInSalesIsAssignedToTheEstablishment(ICollection<Sale> sales)
        {
            if (!sales.IsNullOrEmpty())
            {
                ICollection<Item> allItemsFromSales = this.builderSales
                    .SelectMany(sale => sale?.SalesItems?.Select(saleItem => saleItem?.Item) ?? Enumerable.Empty<Item>())
                    .ToList() ?? new List<Item>();

                ICollection<Item> establishmentItems = this.builderItems;
                bool establishmentHasAllItems = allItemsFromSales.All(item => establishmentItems.Contains(item));
                return establishmentHasAllItems;
            }
            else
            {
                return true;
            }
        }


    }
}

