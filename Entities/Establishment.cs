using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication1.Data.DataModels;

namespace WebApplication1.Models
{
    public class Establishment : EntityBase
    {
        public string Name { get; set; }
        //public Location? Location { get; set; } = null;
        public List<Table> Tables { get; set; }
        public List<Item> Items { get; set; }
        public List<Sale> Sales { get; set; }
    }

    public class EstablishmentConfiguration : IEntityTypeConfiguration<Establishment>
    {
        public void Configure(EntityTypeBuilder<Establishment> builder)
        {
            builder.ToTable(nameof(Establishment));

            builder.Property(e => e.Name).IsRequired();

            builder.HasMany(e => e.Sales)
            .WithOne(e => e.Establishment)
            .IsRequired();
        }
    }
}
