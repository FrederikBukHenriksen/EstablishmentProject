using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Infrastructure_Layer.Data.EntityTypeConfiguration
{

    public class EntityFetchinfoConfiguration : IEntityTypeConfiguration<EntityFetchinfo>
    {
        public void Configure(EntityTypeBuilder<EntityFetchinfo> builder)
        {
            //builder.Property<Guid>("UserId");
            //builder.Property<Guid>("EstablishmentId");

            //builder.HasKey(new string[] { "UserId", "EstablishmentId" });

            //builder.HasIndex("UserId");
            //builder.HasIndex("EstablishmentId");

            //builder.HasOne(e => e.User)
            //    .WithMany(e => e.UserRoles)
            //    .HasForeignKey("UserId");

            //builder.HasOne(e => e.Establishment)
            //    .WithMany()
            //    .HasForeignKey("EstablishmentId");

        }
    }
}
