using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YamlDotNet.Core.Tokens;

namespace WebApplication1.Models
{
    public class Location : EntityBase
    {
        public string Address { get; set; }
        public Guid EstablishmentId { get; set; }
        public Establishment Establishment { get; set; }
    }

    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.ToTable(nameof(Location));  

            builder.Property(e => e.Address);

            //builder.HasOne(s => s.Establishment)
            //.WithOne(a => a.Location)
            //.HasForeignKey<Location>(a => a.Id);
        }
    }


}