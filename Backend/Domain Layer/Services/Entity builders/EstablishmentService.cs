using WebApplication1.Domain_Layer.Services.Entity_builders;

namespace WebApplication1.Domain_Layer.Entities
{
    public interface IEstablishmentService : IEntityBuilder<Establishment>
    {
        IEstablishmentService SetName(string name);
        IEstablishmentService AddItems(ICollection<Item> items);
        IEstablishmentService AddTables(ICollection<Table> tables);
        IEstablishmentService AddSales(ICollection<Sale> sales);
    }

    public class EstablishmentService : EntityBuilderBase<Establishment>, IEstablishmentService
    {
        private string? name = null;
        private ICollection<Item> items = new List<Item>();
        private ICollection<Table> tables = new List<Table>();
        private ICollection<Sale> sales = new List<Sale>();

        public override bool Validation()
        {
            return true;
        }

        public IEstablishmentService SetName(string name)
        {
            this.name = name;
            return this;
        }

        public IEstablishmentService AddItems(ICollection<Item> items)
        {
            this.items = items;
            return this;
        }

        public IEstablishmentService AddTables(ICollection<Table> tables)
        {
            this.tables = tables;
            return this;
        }

        public IEstablishmentService AddSales(ICollection<Sale> sales)
        {
            this.sales = sales;
            return this;
        }

        public override void WritePropertiesOfEntity(Establishment entity)
        {
            entity.Name = (string)this.name;
            foreach (var item in this.items)
            {
                this.Entity.AddItem(item);
            }
            foreach (var table in this.tables)
            {
                this.Entity.AddTable(table);
            }
            foreach (var sale in this.sales)
            {
                this.Entity.AddSale(sale);
            }
        }

        public override void ReadPropertiesOfEntity(Establishment entity)
        {
            this.name = entity.Name;
            this.items = entity.Items;
            this.tables = entity.Tables;
            this.sales = entity.Sales;
        }
    }
}
