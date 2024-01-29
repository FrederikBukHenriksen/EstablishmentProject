namespace WebApplication1.Domain_Layer.Entities
{
    public interface ICommon
    {

    }

    public interface IEntity : ICommon
    {
        public Guid Id { get; set; }
    }

    public abstract class EntityBase : IEntity
    {

        [Key]
        public Guid Id { get; set; } = new Guid();
    }
}

//[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
