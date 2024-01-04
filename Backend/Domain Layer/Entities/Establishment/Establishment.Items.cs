namespace WebApplication1.Domain_Layer.Entities
{
    public partial class Establishment
    {
        public void AddItem(Item item)
        {
            this.Items.Add(item);
        }

        //public void AddItems(ICollection<Item> items)
        //{
        //    foreach (var item in items)
        //    {
        //        this.AddItem(item);
        //    }
        //}

        public void RemoveItem(Item item)
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
