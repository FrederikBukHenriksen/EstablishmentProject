using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Namotion.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.Domain.Entities
{
    public class User : EntityBase
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        //Constructors
        public User()
        {
            
        }


        public User(string username, string password) {
            this.Username = username;
            this.Password = password;
        }

        public User(string username, string password, UserRole userRole)
        {
            this.Username = username;
            this.Password = password;
            this.UserRoles.Add(userRole);
        }

        public User(string username, string password, ICollection<UserRole> userRoles)
        {
            this.Username = username;
            this.Password = password;
            this.UserRoles = userRoles;
        }
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
