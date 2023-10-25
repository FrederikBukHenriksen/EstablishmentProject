namespace WebApplication1.Entities
{
    public class Location : EntityBase
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Country { get; set; }
    }
}
