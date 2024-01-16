namespace WebApplication1.Domain_Layer.Entities
{
    public class ItemCategory : EntityBase
    {
        public string Name { get; set; }
        public virtual ICollection<Item> Items { get; set; } = new List<Item>();
    }
}
