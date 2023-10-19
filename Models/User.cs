using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApplication1.Models
{
    public class User : EntityBase

    {
        public string Username { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
        //public Establishment? Establishment { get; set; } = null;
        public User(string username, string password, Role role)
        {
            this.Username = username;
            this.Password = password;
            this.Role = role;
        }
    }

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(nameof(User));

            builder.Property(x => x.Username).IsRequired(true);

            builder.Property(x => x.Password).IsRequired(true);

            builder.Property(x => x.Role)
                .HasConversion(
                    role => role.ToString(),
                    roleName => (Role)Enum.Parse(typeof(Role), roleName))
                .IsRequired(true);

            //builder.Property(x => x.Establishment)
            //    .IsRequired(false);
        }
    }
}
