using WebApplication1.Domain_Layer.Services.Entity_builders;

namespace WebApplication1.Domain_Layer.Entities
{
    public interface IEstablishmentBuilder : IEntityBuilder<Establishment>
    {
        IEstablishmentBuilder WithName(string name);
        IEstablishmentBuilder WithItem(Item item);
        IEstablishmentBuilder WithItems(ICollection<Item> items);
        IEstablishmentBuilder WithTable(Table table);
        IEstablishmentBuilder WithTables(ICollection<Table> tables);
        IEstablishmentBuilder WithSale(Sale sale);
        IEstablishmentBuilder WithSales(ICollection<Sale> sales);
    }

    public class EstablishmentBuilder : EntityBuilderBase<Establishment>, IEstablishmentBuilder
    {
        public override bool BuildValidation()
        {
            return true;
        }

        IEstablishmentBuilder IEstablishmentBuilder.WithName(string name)
        {
            this.Entity.SetName(name);
            return this;
        }

        IEstablishmentBuilder IEstablishmentBuilder.WithItems(ICollection<Item> items)
        {
            foreach (Item item in items)
            {
                this.WithItem(item);
            }
            return this;
        }

        IEstablishmentBuilder IEstablishmentBuilder.WithTables(ICollection<Table> tables)
        {
            foreach (Table table in tables)
            {
                this.WithTable(table);
            }
            return this;
        }

        IEstablishmentBuilder IEstablishmentBuilder.WithSales(ICollection<Sale> sales)
        {
            foreach (var sale in sales)
            {
                this.WithSale(sale);
            }
            return this;
        }

        public IEstablishmentBuilder WithItem(Item item)
        {
            this.Entity.AddItem(item);
            return this;
        }

        public IEstablishmentBuilder WithTable(Table table)
        {
            this.Entity.AddTable(table);
            return this;
        }

        public IEstablishmentBuilder WithSale(Sale sale)
        {
            this.Entity.AddSale(sale);
            return this;
        }
    }
}

