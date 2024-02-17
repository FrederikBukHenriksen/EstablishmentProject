namespace WebApplication1.Domain_Layer.Entities
{

    public interface ITable
    {
        string GetName();
        string SetName(string name);
    }
    public class Table : EntityBase, ITable
    {
        public Guid EstablishmentId { get; set; }
        public string Name { get; set; }

        public Table()
        {

        }

        public Table(string name)
        {
            this.Name = name;
        }

        public string SetName(string name)
        {
            if (!this.IsTableNameValid(name))
            {
                throw new ArgumentException("Table name is not valid");
            }
            this.Name = name;
            return this.GetName();
        }
        public string GetName()
        {
            return this.Name;
        }

        //Checkers and validators
        public bool IsTableNameValid(string name)
        {
            if (name == "")
            {
                return false;
            }
            return true;
        }

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
