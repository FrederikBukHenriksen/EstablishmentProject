using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication1.Data.DataModels;

namespace WebApplication1.Models
{
    public class Establishment : EntityBase
    {
        public string Name { get; set; }
        public Location? Location { get; set; } = null;
        public ICollection<Table>? Tables { get; set; }
        public ICollection<Item>? Items { get; set; }
        public ICollection<Sale>? Sales { get; set; }

        public Establishment()
        { 
        }

        public Establishment(string Name)
        {
            this.Name = Name;
        }

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
