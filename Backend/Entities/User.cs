using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Namotion.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Data.DataModels;

namespace WebApplication1.Models
{
    public class User : EntityBase
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(nameof(User));

            builder.HasIndex(u => u.Username)
            .IsUnique();

            builder.Property(x => x.Username).IsRequired(true);


            builder.Property(x => x.Password).IsRequired(true);

            builder.HasMany(u => u.UserRoles)
            .WithOne(ur => ur.User)
            .HasForeignKey("UserId");


        }
    }
}
