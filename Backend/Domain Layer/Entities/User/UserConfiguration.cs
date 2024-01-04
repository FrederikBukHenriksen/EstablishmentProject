using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApplication1.Domain_Layer.Entities
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(nameof(User));

            builder.HasIndex(u => u.Email)
            .IsUnique();

            builder.Property(x => x.Email).IsRequired(true);

            builder.Property(x => x.Password).IsRequired(true);

            builder.HasMany(u => u.UserRoles)
            .WithOne(ur => ur.User)
            .HasForeignKey("UserId");


        }
    }
}
