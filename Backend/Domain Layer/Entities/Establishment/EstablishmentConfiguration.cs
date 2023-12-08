using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApplication1.Domain.Entities
{
    public class EstablishmentConfiguration : IEntityTypeConfiguration<Establishment>
    {
        public void Configure(EntityTypeBuilder<Establishment> builder)
        {
            builder.Property(x => x.Name);
            builder.HasOne(x => x.Location);

        }
    }




}
