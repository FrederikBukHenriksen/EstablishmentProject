using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Namotion.Reflection;

namespace WebApplication1.Data.DataModels
{

    public class UserRole : EntityBase
    {
        public User User { get; set; }
        public Establishment Establishment { get; set; }
        public Establishment Role { get; set; }

        public class UserRolesConfiguration : IEntityTypeConfiguration<UserRole>
        {
            public void Configure(EntityTypeBuilder<UserRole> builder)
            {
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
                        roleName => (Establishment)Enum.Parse(typeof(Establishment), roleName))
                .IsRequired(true);
            }
        }

    }
}
