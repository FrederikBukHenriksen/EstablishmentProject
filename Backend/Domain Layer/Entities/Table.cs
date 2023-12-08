using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApplication1.Domain.Entities
{
    public class Table : EntityBase
    {
        //public Establishment Establishment { get; set; }
        public string Name { get; set; }

    }


    //public class TableConfiguration : IEntityTypeConfiguration<Establishment>
    //{
    //    public void Configure(EntityTypeBuilder<Establishment> builder)
    //    {
    //        builder.ToTable(nameof(Establishment));

    //        builder.Property(e => e.Name).IsRequired();

    //        builder.HasMany(e => e.Sales)
    //        .WithOne(e => e.Establishment)
    //        .IsRequired();
    //    }
    //}
}
