namespace WebApplication1.Models
{
    public class Table : EntityBase
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Establishment Establishment { get; set; }
        public string Name { get; set; }
    }
}
