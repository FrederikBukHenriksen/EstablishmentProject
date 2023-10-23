using Microsoft.EntityFrameworkCore.Metadata.Builders;
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
        public ICollection<UserLinkEstablishment> UserRoleEstablishment { get; set; }
        public User(string username, string password, Role role)
        {
            this.Username = username;
            this.Password = password;
        }
    }

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(nameof(User));

            builder.Property(x => x.Username).IsRequired(true);

            builder.Property(x => x.Password).IsRequired(true);


        }
    }
}
