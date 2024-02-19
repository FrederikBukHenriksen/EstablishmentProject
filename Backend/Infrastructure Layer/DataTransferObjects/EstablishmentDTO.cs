using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Controllers
{
    public class EstablishmentDTO
    {
        public Guid Id { get; }
        public string Name { get; } = "";
        public List<Guid> Items { get; } = new List<Guid>();
        public List<Guid> Tables { get; } = new List<Guid>();
        public List<Guid> Sales { get; } = new List<Guid>();

        public EstablishmentDTO(Establishment establishment)
        {
            this.Id = establishment.Id;
            this.Name = establishment.Name!;
            this.Items = establishment.Items.Select(item => item.Id).ToList();
            this.Tables = establishment.Tables.Select(table => table.Id).ToList();
            this.Sales = establishment.Sales.Select(sale => sale.Id).ToList();
        }
    }
}