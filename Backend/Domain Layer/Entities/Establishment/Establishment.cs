using NodaTime;
using WebApplication1.Data.DataModels;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Domain_Layer.Entities.Establishment;
using WebApplication1.Utils;

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
