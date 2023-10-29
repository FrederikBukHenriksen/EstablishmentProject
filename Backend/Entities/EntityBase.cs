namespace WebApplication1.Models
{
    public abstract class EntityBase
    {

        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
    }


}
