namespace WebApplication1.Domain_Layer.Entities
{
    public interface IEstablishment : IEstablishment_Sale, IEstablishment_Table, IEstablishment_Item, IEstablishment_Information
    {
        string GetName();
        string SetName(string name);
    }

    public partial class Establishment : EntityBase
    {
        public string? Name { get; set; }
        public virtual EstablishmentInformation Information { get; set; } = new EstablishmentInformation();
        public virtual EstablishmentSettings Settings { get; set; } = new EstablishmentSettings();
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
            items?.ForEach(x => this.AddItem(x));
            tables?.ForEach(x => this.AddTable(x));
            sales?.ForEach(x => this.AddSale(x));
        }

        public string SetName(string name)
        {
            if (this.IsNameValid(name)) { throw new ArgumentException("Name is not valid"); }
            this.Name = name;
            return name;
        }

        public string GetName()
        {
            return this.Name;
        }

        //Checkers and validators
        public bool IsNameValid(string name)
        {
            if (name == "")
            {
                return false;
            }
            return true;
        }

    }
}
