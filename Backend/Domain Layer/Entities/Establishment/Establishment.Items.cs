namespace WebApplication1.Domain_Layer.Entities
{

    public interface IEstablishment_Item
    {
        Item CreateItem(string name, double price, Currency currency);
        void RemoveItem(Item item);
        List<Item> GetItems();
        Item UpdateItem(Item item);
        Item AddItem(Item item);
    }
    public partial class Establishment : EntityBase, IEstablishment_Item
    {
        //CRUD
        public Item CreateItem(string name, double price, Currency currency)
        {
            Item item = new Item(name, price, currency);
            return item;
        }

        public Item AddItem(Item item)
        {
            this.Items.Add(item);
            return item;
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

        private bool doesItemAlreadyExist(Item item)
        {
            return this.Items.Any(x => x.Name == item.Name);
        }

        public Item UpdateItem(Item item)
        {
            if (this.doesItemAlreadyExist(item))
            {
                this.RemoveItem(item);
                this.AddItem(item);
            }
            return item;
        }
    }
}
