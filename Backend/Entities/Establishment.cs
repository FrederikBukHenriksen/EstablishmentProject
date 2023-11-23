using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication1.Data.DataModels;

namespace WebApplication1.Models
{
    public class Establishment : EntityBase
    {
        public string Name { get; set; }
        //public Location? Location { get; set; } = null;
        public ICollection<Item> Items { get; set; } = new List<Item>();
        //public ICollection<Table>? Tables { get; set; } = null;
        public ICollection<Sale> Sales { get; set; } = new List<Sale>();
        //public Location Location { get; set; }
        internal Establishment()
        {
            
        }
    }


    public class EstablishmentConfiguration : IEntityTypeConfiguration<Establishment>
    {
        public void Configure(EntityTypeBuilder<Establishment> builder)
        {
            builder.Property(x => x.Name);
            //builder.Property(x => x.Location);

        }
    }
}
