namespace WebApplication1.Domain_Layer.Entities
{
    public partial class Establishment : EntityBase
    {
        public void AddItem(Item item)
        {
            this.Items.Add(item);
        }

        internal void RemoveItem(Item item)
        {
            if (this.IsItemUsedInSales(item)) throw new Exception("Item is used in sales, and therefore cannot be deleted");
            this.Items.Remove(item);
        }

        public ICollection<Item> GetItems()
        {
            return this.Items;
        }

        public bool IsItemUsedInSales(Item item)
        {
            return this.GetSales().Any(x => x.SalesItems.Any(y => y.Item == item));
        }
    }
}
