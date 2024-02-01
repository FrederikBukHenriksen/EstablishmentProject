namespace WebApplication1.Domain_Layer.Entities
{
    public class RetrivedEntitiesJoiningTable : EntityBase
    {
        public Guid EntityId { get; set; }
        public string ForeignId { get; set; }
    }
}
