namespace WebApplication1.Domain_Layer.Entities
{

    public interface IEstablishment_Item
    {
        Item CreateItem(string name, double price, Currency currency);
        void RemoveItem(Item item);
        List<Item> GetItems();
        //void AddItem(Item item);
    }
    public partial class Establishment : EntityBase, IEstablishment_Item
    {
        //CRUD
        public Item CreateItem(string name, double price, Currency currency)
        {
            Item item = new Item(name, price, currency);
            this.AddItem(item);
            return item;
        }

        private void AddItem(Item item)
        {
            this.Items.Add(item);
        }

        public List<Item> GetItems()
        {
            return this.Items.ToList();
        }

        public void RemoveItem(Item item)
        {
            if (this.IsItemUsedInSales(item))
            {
                throw new Exception("Item is used in sales, and therefore cannot be deleted");
            }
            this.Items.Remove(item);
        }

        //Checkers and validators
        public bool IsItemUsedInSales(Item item)
        {
            return this.GetSales().Any(x => x.SalesItems.Any(y => y.Item == item));
        }

    }
}
