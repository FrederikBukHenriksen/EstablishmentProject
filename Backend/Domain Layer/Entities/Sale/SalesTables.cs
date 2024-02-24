namespace WebApplication1.Domain_Layer.Entities
{
    public interface ISalesTables
    {

    }

    public partial class SalesTables : JoiningTableBase, ISalesTables
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
    }
}
