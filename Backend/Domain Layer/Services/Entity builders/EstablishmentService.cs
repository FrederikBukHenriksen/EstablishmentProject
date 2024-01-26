using WebApplication1.Domain_Layer.Services.Entity_builders;

namespace WebApplication1.Domain_Layer.Entities
{
    public interface IEstablishmentService : IEntityBuilder<Establishment>
    {
        IEstablishmentService withName(string name);
        IEstablishmentService withItems(List<Item> items);
        IEstablishmentService withTables(List<Table> tables);
        IEstablishmentService withSales(List<Sale> sales);
    }

    public class EstablishmentService : EntityBuilderBase<Establishment>, IEstablishmentService
    {
        private string? name = null;
        private List<Item> items = new List<Item>();
        private List<Table> tables = new List<Table>();
        private List<Sale> sales = new List<Sale>();

        public IEstablishmentService withName(string name)
        {
            this.name = name;
            return this;
        }

        public IEstablishmentService withItems(List<Item> items)
        {
            this.items = items;
            return this;
        }

        public IEstablishmentService withTables(List<Table> tables)
        {
            this.tables = tables;
            return this;
        }

        public IEstablishmentService withSales(List<Sale> sales)
        {
            this.sales = sales;
            return this;
        }

        public override Establishment Build()
        {
            return new Establishment(name: this.name, items: this.items, tables: this.tables, sales: this.sales);
        }
    }
}
