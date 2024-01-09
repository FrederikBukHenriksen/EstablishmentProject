namespace WebApplication1.Domain_Layer.Entities
{

    public enum EntityType
    {
        Sale,
        Item,
        Table,
        Employee,
    }
    public class EntityFetchinfo
    {
        public EntityType EntityType { get; set; }
        public Guid EntityId { get; set; }
        public string PosId { get; set; }
    }
}

