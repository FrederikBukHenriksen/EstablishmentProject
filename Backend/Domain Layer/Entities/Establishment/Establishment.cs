namespace WebApplication1.Domain_Layer.Entities
{
    public partial class Establishment : EntityBase
    {
        public string? Name { get; set; }
        public virtual EstablishmentInformation? Information { get; set; }
        protected virtual ICollection<Item> Items { get; set; } = new List<Item>();
        public virtual ICollection<Table> Tables { get; set; } = new List<Table>();
        public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

        public void SetName(string name)
        {
            this.Name = name;
        }








    }

}
