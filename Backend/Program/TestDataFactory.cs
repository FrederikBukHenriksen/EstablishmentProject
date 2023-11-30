using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Data.DataModels;
using WebApplication1.Domain.Entities;

namespace WebApplication1.Program
{
    public static class TestDataFactory
    {
        //Helper methods
        public static Guid CreateGuidFromInt(int number)
        {
            byte[] bytes = BitConverter.GetBytes(number);
            return new Guid(bytes);
        }

        public static int GetIntFromGuid(Guid id)
        {
            string guidString = id.ToString();
            string numericString = guidString.Replace("-", "");
            return int.Parse(numericString);
        }
        //Establishment
        public static Establishment CreateEstablishment(Guid? id = null, string? name = null)
        {
            var establishment = new Establishment();
            establishment.Id = id == null ? establishment.Id : (Guid)id;
            establishment.Name = name == null ? "Establishment " + establishment.Id : establishment.Name;
            return establishment;
        }

        //Item
        public static Item CreateItem(Guid? id = null, string? name = null, double? price = null)
        {
            var item = new Item();
            item.Id = id == null ? item.Id : (Guid)id;
            item.Name = name == null ? item.Name : name + item.Id;
            item.Price = 69;
            return item;
        }

        //Sale
        public static Sale CreateSale(Guid? id = null, DateTime? timestampEnd = null, List<SalesItems>? items = null)
        {
            var sale = new Sale();
            sale.Id = id == null ? sale.Id : (Guid)id;
            sale.TimestampPayment = timestampEnd == null ? DateTime.Now : (DateTime)timestampEnd;
            sale.SalesItems = items == null ? new List<SalesItems>() : items;
            return sale;
        }

        public static SalesItems CreateSalesItems(Sale sale, Item item, int quantity)
        {
            SalesItems salesItems = new SalesItems();
            salesItems.Sale = sale;
            salesItems.Item = item;
            salesItems.Quantity = quantity;
            return salesItems;
        }

        public static User CreateUser(Guid? id = null, string username = null, string password = null)
        {
            var user = new User("", "");
            user.Username = username != null ? username : "User " + user.Id;
            user.Password = password != null ? password : GetIntFromGuid(id.GetValueOrDefault()).ToString();
            user.Id = id == null ? user.Id : (Guid)id;
            return user;
        }

        public static UserRole CreateUserRole(Establishment establishment, User user, Role role, Guid? id = null)
        {
            var userRole = new UserRole(user: user, establishment: establishment, role:role);
            userRole.Id = id == null ? userRole.Id : (Guid)id;
            return userRole;
        }










    }
}
