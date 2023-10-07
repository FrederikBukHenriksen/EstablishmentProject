using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApplication1.Models
{
    public class User : EntityBase
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(nameof(User));

            builder.Property(x => x.Username);

            builder.Property(x => x.Password);

        }
    }
}
