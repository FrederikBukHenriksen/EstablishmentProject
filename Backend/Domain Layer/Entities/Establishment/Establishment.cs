using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Domain.Entities
{
    public class Establishment : EntityBase
    {
        public string? Name { get; set; }
        public Information? Information { get; set; }
        public ICollection<Item> Items { get; set; } = new List<Item>();
        public ICollection<Table> Tables { get; set; } = new List<Table>();
        public ICollection<Sale> Sales { get; set; } = new List<Sale>();


    }


}
