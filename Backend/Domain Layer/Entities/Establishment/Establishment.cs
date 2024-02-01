using NodaTime;

namespace WebApplication1.Domain_Layer.Entities
{
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
            tables?.ForEach(this.AddTable);
            sales?.ForEach(x => this.AddSale(x));
        }

        public Establishment SetName(string name)
        {
            this.Name = name;
            return this;
        }

        public Establishment SetOpeningHours(DayOfWeek dayOfWeek, LocalTime open, LocalTime close)
        {
            OpeningHours openingHours = new OpeningHours(dayOfWeek, open, close);
            this.Information.setOpeningHour(openingHours);
            return this;
        }
    }
}
