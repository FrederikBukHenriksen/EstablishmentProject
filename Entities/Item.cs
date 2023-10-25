namespace WebApplication1.Models
{
    public class Item : EntityBase
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public float Price { get; set; }
    }
}
