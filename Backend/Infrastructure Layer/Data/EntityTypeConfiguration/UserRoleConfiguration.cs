using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;
using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Infrastructure_Layer.Data.EntityTypeConfiguration
{
    [ExcludeFromCodeCoverage]
    public class UserRolesConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

            builder.Property<Guid>("UserId");
            builder.Property<Guid>("EstablishmentId");

            builder.HasKey(new string[] { "UserId", "EstablishmentId" });

            builder.HasIndex("UserId");
            builder.HasIndex("EstablishmentId");

            builder.HasOne(e => e.User)
                .WithMany(e => e.UserRoles)
                .HasForeignKey("UserId");

            builder.HasOne(e => e.Establishment)
                .WithMany()
                .HasForeignKey("EstablishmentId");

            builder.Property(x => x.Role)
                .HasConversion(
                    role => role.ToString(),
                    roleName => (Role)Enum.Parse(typeof(Role), roleName))
            .IsRequired(true);
        }
    }

}
