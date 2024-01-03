using WebApplication1.Domain_Layer.Services.Entity_builders;

namespace WebApplication1.Domain.Entities
{
    public interface IEstablishmentBuilder2 : IEntityBuilder2<Establishment>
    {
        IEstablishmentBuilder2 WithName(string name);
        IEstablishmentBuilder2 WithItem(Item item);
        IEstablishmentBuilder2 WithItems(ICollection<Item> items);
        IEstablishmentBuilder2 WithTable(Table table);
        IEstablishmentBuilder2 WithTables(ICollection<Table> tables);
        IEstablishmentBuilder2 WithSale(Sale sale);
        IEstablishmentBuilder2 WithSales(ICollection<Sale> sales);
    }

    public class EstablishmentBuilder2 : EntityBuilderBase2<Establishment>, IEstablishmentBuilder2
    {
        public EstablishmentBuilder2()
        {
        }

        IEstablishmentBuilder2 IEstablishmentBuilder2.WithName(string name)
        {
            this.Entity.SetName(name);
            return this;
        }

        IEstablishmentBuilder2 IEstablishmentBuilder2.WithItems(ICollection<Item> items)
        {
            foreach (Item item in items)
            {
                this.WithItem(item);
            }
            return this;
        }

        IEstablishmentBuilder2 IEstablishmentBuilder2.WithTables(ICollection<Table> tables)
        {
            foreach (Table table in tables)
            {
                this.WithTable(table);
            }
            return this;
        }

        IEstablishmentBuilder2 IEstablishmentBuilder2.WithSales(ICollection<Sale> sales)
        {
            foreach (var sale in sales)
            {
                this.WithSale(sale);
            }
            return this;
        }

        public override bool BuildValidation()
        {
            throw new NotImplementedException();
        }

        public IEstablishmentBuilder2 WithItem(Item item)
        {
            this.Entity.AddItem(item);
            return this;
        }

        public IEstablishmentBuilder2 WithTable(Table table)
        {
            this.Entity.AddTable(table);
            return this;
        }

        public IEstablishmentBuilder2 WithSale(Sale sale)
        {
            this.Entity.AddSale(sale);
            return this;
        }
    }
}

