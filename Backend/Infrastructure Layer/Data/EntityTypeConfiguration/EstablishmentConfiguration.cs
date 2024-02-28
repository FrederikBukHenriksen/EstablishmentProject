using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace WebApplication1.Domain_Layer.Entities
{
    [ExcludeFromCodeCoverage]
    public class EstablishmentConfiguration : IEntityTypeConfiguration<Establishment>
    {
        public void Configure(EntityTypeBuilder<Establishment> builder)
        {
            builder.Property(x => x.Name);
        }
    }






}
