namespace WebApplication1.Domain_Layer.Entities
{
    public interface ICommon
    {
        public Guid Id { get; set; }
    }

    public interface IEntity : ICommon
    {
        [Key]
        public Guid Id { get; set; }
    }

    public interface IJoiningTable : ICommon
    {
        [Key]
        public Guid Id { get; set; }
    }

    public abstract class EntityBase : IEntity
    {

        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

    }

    public abstract class JoiningTableBase : IJoiningTable
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}