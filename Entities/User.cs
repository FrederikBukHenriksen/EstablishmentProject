using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Hosting;
using Namotion.Reflection;
using Newtonsoft.Json;
using System.Reflection.Emit;
using System.Reflection.Metadata;

namespace WebApplication1.Models
{
    public class User : EntityBase

    {
        public string Username { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
        public IList<Establishment> Establishment { get; set; }
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

            builder.Property(e => e.Establishment).HasConversion(
                v => JsonConvert.SerializeObject(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                v => JsonConvert.DeserializeObject<IList<Establishment>>(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));


        }
    }
}
