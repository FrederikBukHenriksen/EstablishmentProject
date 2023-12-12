using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using WebApplication1.Utils;

namespace WebApplication1.Domain.Entities
{
    public class EstablishmentConfiguration : IEntityTypeConfiguration<Establishment>
    {
        public void Configure(EntityTypeBuilder<Establishment> builder)
        {
            builder.Property(x => x.Name);
            builder.HasOne(x => x.EstablishmentInformation);
        }
    }




}
