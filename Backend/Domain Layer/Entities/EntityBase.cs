namespace WebApplication1.Domain_Layer.Entities
{
    public abstract class EntityBase 
    {

        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

    }


}
