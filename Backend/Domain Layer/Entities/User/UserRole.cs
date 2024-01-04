using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApplication1.Domain_Layer.Entities
{

    public class UserRole : EntityBase
    {
        public virtual User User { get; set; }
        public virtual Establishment Establishment { get; set; }
        public Role Role { get; set; }

        public UserRole()
        {

        }

        public UserRole(User user, Establishment establishment, Role role)
        {
            this.User = user;
            this.Establishment = establishment;
            this.Role = role;
        }
    }

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
                    roleName => (Role)Enum.Parse(typeof(Role), roleName))
            .IsRequired(true);
        }
    }

}
