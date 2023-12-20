using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Domain.Entities;

namespace EstablishmentProject.test
{
    public static class CreateTestData
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
            establishment.Id = id == null ? establishment.Id : (Guid) id;
            establishment.Name = name == null ? "Establishment " + GetIntFromGuid((Guid) id) : name;
            return establishment;
        }

        //Item
        public static Item CreateItem(Guid? id = null, string? name = null, double? price = null)
        {
            var item = new Item();
            item.Id = id == null ? item.Id : (Guid) id;
            item.Name = name == null ? "Item " + GetIntFromGuid((Guid) id) : name;
            item.Price = price == null ? 0 : (double) price;
            return item;
        }

        //Sale
        public static Sale CreateSale(Guid? id = null, DateTime? timestampEnd = null)
        {
            var sale = new Sale();
            sale.Id = id == null ? sale.Id : (Guid) id;
            sale.TimestampArrival = timestampEnd == null ? DateTime.Now : (DateTime) timestampEnd;
            return sale;
        }

        //User
        public static User CreateUser(Guid? id = null, string username = null, string password = null) {
            var user = new User();
            user.Id = id == null ? user.Id : (Guid) id;
            user.Email = username != null ? username : "User " + GetIntFromGuid((Guid) id);
            user.Password = password != null ? password : GetIntFromGuid((Guid) id).ToString();
            return user;
        }









    }
}
