using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApplication1.Data.DataModels
{
    public class UserLinkEstablishment : EntityBase
    {
        public User User { get; set; }
        public Establishment Establishment { get; set; }
        public Role role { get; set; }

        public UserLinkEstablishment(User user, Establishment establishment, Role role)
        {
            this.User = user;
            this.Establishment = establishment;
            this.role = role;
        }
    }

    public class UserLinkEstablishmentConfiguration : IEntityTypeConfiguration<UserLinkEstablishment>
    {
        public void Configure(EntityTypeBuilder<UserLinkEstablishment> builder)
        {
            builder.ToTable(nameof(Establishment));

            builder.Property(x => x.role)
                .HasConversion(
                    role => role.ToString(),
                    roleName => (Role)Enum.Parse(typeof(Role), roleName))
            .IsRequired(true);

        }
    }

}
