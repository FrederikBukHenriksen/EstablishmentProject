using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Infrastructure_Layer.DataTransferObjects
{
    public class ItemDTO
    {
        public Guid Id;
        public string Name { get; set; }
        public double Price { get; set; }

        public ItemDTO(Item item)
        {
            this.Id = item.Id;
            this.Name = item.Name;
            this.Price = item.Price;
        }
    }
}
