using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Infrastructure_Layer.DataTransferObjects
{
    public class SaleDTO
    {
        public Guid id { get; set; }
        public DateTime? TimestampArrival { get; set; } = null;
        public DateTime TimestampPayment { get; set; }
        public List<(Guid, int)> SalesItems { get; set; } = new List<(Guid, int)>();
        public List<Guid> SalesTables { get; set; } = new List<Guid>();
        public Guid? Employee { get; set; } = null;


        public SaleDTO(Sale sale)
        {
            this.id = sale.Id;
            this.TimestampArrival = sale.TimestampArrival;
            this.TimestampPayment = sale.TimestampPayment;
            this.SalesItems = sale.SalesItems.Select(x => (x.Item.Id, x.quantity)).ToList();
            this.SalesTables = sale.SalesItems.Select(x => x.Id).ToList();
        }
    }
}
