using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Controllers
{
    public class EstablishmentDTO
    {
        public Guid id { get; set; }
        public string name { get; set; } = "";
        public List<Guid> items { get; set; } = new List<Guid>();
        public List<Guid> tables { get; set; } = new List<Guid>();
        public List<Guid> sales { get; set; } = new List<Guid>();

        public EstablishmentDTO(Establishment establishment)
        {
            this.id = establishment.Id;
            this.name = establishment.Name!;
            this.items = establishment.Items.Select(item => item.Id).ToList();
            this.tables = establishment.Tables.Select(table => table.Id).ToList();
            this.sales = establishment.Sales.Select(sale => sale.Id).ToList();
        }
    }
}