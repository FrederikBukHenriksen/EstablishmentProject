using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication1.Data.DataModels;

namespace WebApplication1.Models
{
    public class Establishment : EntityBase
    {
        public string Name { get; set; }
        //public Location? Location { get; set; } = null;
        public ICollection<Item> Items { get; set; }
        //public ICollection<Table>? Tables { get; set; } = null;
        //public ICollection<Sale>? Sales { get; set; } = null;

        internal Establishment()
        {
            
        }

        public Establishment(string name)
        {
            Name = name;
        }

    }


    public class EstablishmentConfiguration : IEntityTypeConfiguration<Establishment>
    {
        public void Configure(EntityTypeBuilder<Establishment> builder)
        {

        }
    }
}
