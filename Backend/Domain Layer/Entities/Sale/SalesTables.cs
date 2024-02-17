using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApplication1.Domain_Layer.Entities
{
    public interface ISalesTables
    {

    }

    public partial class SalesTables : EntityBase, ISalesTables
    {

        public virtual Sale Sale { get; set; }
        public virtual Table Table { get; set; }

        public SalesTables()
        {

        }

        public SalesTables(Sale sale, Table Table)
        {
            this.Sale = sale;
            this.Table = Table;
        }

        public class SalesTablesConfiguration : IEntityTypeConfiguration<SalesTables>
        {
            public void Configure(EntityTypeBuilder<SalesTables> builder)
            {


                builder.Property<Guid>("SalesId");
                builder.Property<Guid>("TableId");

                builder.HasKey(new string[] { "SalesId", "TableId" });

                builder.HasIndex("SalesId");
                builder.HasIndex("TableId");

                builder.HasOne(e => e.Sale)
                    .WithMany(e => e.SalesTables)
                    .HasForeignKey("SalesId");

                builder.HasOne(e => e.Table)
                    .WithMany()
                    .HasForeignKey("TableId");
            }
        }

    }
}
