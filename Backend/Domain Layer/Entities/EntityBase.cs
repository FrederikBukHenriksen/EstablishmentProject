namespace WebApplication1.Domain.Entities
{
    public abstract class EntityBase
    {

        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
    }


}
