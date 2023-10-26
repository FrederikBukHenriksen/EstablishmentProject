namespace WebApplication1.Models
{
    public class Table : EntityBase
    {
        public Establishment Establishment { get; set; }
        public Guid EstablishmentId { get; set; }
        public string Name { get; set; }
    }
}
