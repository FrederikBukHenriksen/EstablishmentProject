namespace WebApplication1.Domain_Layer.Entities
{
    public partial class Establishment : EntityBase
    {
        public string? Name { get; set; }
        public virtual EstablishmentInformation? Information { get; set; }
        public virtual ICollection<Item> Items { get; set; } = new List<Item>();
        public virtual ICollection<Table> Tables { get; set; } = new List<Table>();
        public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();

        public Establishment()
        {

        }
        public Establishment(
            string name,
            List<Item>? items = null,
            List<Table>? tables = null,
            List<Sale>? sales = null)
        {
            this.SetName(name);
            items?.ForEach(this.AddItem);
            tables?.ForEach(this.AddTable);
            sales?.ForEach(this.AddSale);
        }

        public void SetName(string name)
        {
            this.Name = name;
        }
    }
}
