using WebApplication1.Data.DataModels;

namespace WebApplication1.Domain.Entities
{
    public class Establishment : EntityBase
    {
        public string? Name { get; set; }
        public Location? Location { get; set; }
        public ICollection<Item> Items { get; set; } = new List<Item>();
        public ICollection<Table> Tables { get; set; } = new List<Table>();
        public ICollection<Sale> Sales { get; set; } = new List<Sale>();

    }




}
