﻿namespace WebApplication1.Domain_Layer.Entities
{

    public interface IEstablishment_Item
    {
        void RemoveItem(Item item);
        List<Item> GetItems();
        void AddItem(Item item);
        Item CreateItem(string name, double price);
        void SetItemName(Item item, string name);
    }
    public partial class Establishment : EntityBase, IEstablishment_Item
    {
        public Item CreateItem(string name, double price)
        {
            this.ItemNameMustBeUnique(name);
            Item item = new Item(name, price);
            return item;
        }

        public void AddItem(Item item)
        {
            this.ItemMustBeCreatedForEstablishment(item);
            this.ItemMustNotAlreadyExist(item);
            this.ItemNameMustBeUnique(item.Name);
            this.Items.Add(item);
        }

        public List<Item> GetItems()
        {
            return this.Items.ToList();
        }

        public void RemoveItem(Item item)
        {
            this.ItemMustExist(item);
            this.ItemMustNotBeUsedInSales(item);
            this.Items.Remove(item);
        }

        public void SetItemName(Item item, string name)
        {
            this.ItemNameMustBeUnique(name);
            item.SetName(name);
        }


        //Checkers and validators
        protected void ItemMustBeCreatedForEstablishment(Item item)
        {
            if (!this.isItemCreatedForEstablishment(item))
            {
                throw new InvalidOperationException("Item is not created for establishment");
            }
        }
        protected bool isItemCreatedForEstablishment(Item item)
        {
            return item.EstablishmentId == this.Id;
        }

        protected bool ItemNameMustBeUnique(string name)
        {
            if (this.isNameAlreadyInUse(name))
            {
                throw new ArgumentException("Name is already in use");
            }
            return true;
        }

        protected bool isNameAlreadyInUse(string name)
        {
            return this.GetItems().Any(x => x.Name == name);
        }
        protected void ItemMustNotBeUsedInSales(Item item)
        {
            if (this.IsItemUsedInSales(item))
            {
                throw new InvalidOperationException("Item is used in sales, and therefore cannot be deleted");
            }
        }

        protected bool IsItemUsedInSales(Item item)
        {
            return this.GetSales().Any(x => x.SalesItems.Any(y => y.Item == item));
        }

        protected bool ItemMustExist(Item item)
        {
            if (!this.doesItemAlreadyExist(item))
            {
                throw new InvalidOperationException("Item does not exist within the establishment");
            }
            return true;
        }

        protected bool ItemMustNotAlreadyExist(Item item)
        {
            if (this.doesItemAlreadyExist(item))
            {
                throw new InvalidOperationException("Item already exists within the establishment");
            }
            return true;
        }

        protected bool doesItemAlreadyExist(Item item)
        {
            return this.Items.Any(x => x.Name == item.Name);
        }


    }
}
