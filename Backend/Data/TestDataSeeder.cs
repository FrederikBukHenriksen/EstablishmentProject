using Microsoft.EntityFrameworkCore;
using WebApplication1.Data.DataModels;
using WebApplication1.Repositories;
using static WebApplication1.Data.DataModels.UserRole;
using System;
using System.Collections.Generic;
using MathNet.Numerics.Distributions;
using System.Security.Cryptography;

namespace WebApplication1.Data
{
    public class TestDataSeeder
    {


        public static void SeedDataBase(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                GetUsers()
            );
            modelBuilder.Entity<Establishment>().HasData(
                GetEstablishments()
            );

            var item = new Item { Name = "Espresso", Price = 25 };

            modelBuilder.Entity<UserRole>().HasData(
                new
                {
                    Id = Guid.NewGuid(),
                    Role = Role.Admin,
                    EstablishmentId = new Guid("00000000-0000-0000-0000-000000000001"),
                    UserId = new Guid("00000000-0000-0000-0000-000000000001"),
                }
            ); ;
        }

        private static List<User> GetUsers()
        {

            return new List<User>() {
                new User
            {
                Id = new Guid("00000000-0000-0000-0000-000000000001"),
                Username = "Frederik",
                Password = "1234",
            },
                            new User
            {
                Id = new Guid("00000000-0000-0000-0000-000000000002"),
                Username = "Lydia",
                Password = "1234",
            }};
        }

        private static List<Establishment> GetEstablishments()
        {
            return new List<Establishment>() {

                new Establishment
            {
                Id = new Guid("00000000-0000-0000-0000-000000000001"),
                Name = "My Establishment",
            },
        };
        }

        private static List<Item> GetItems()
        {
            return new List<Item>()
            {
                new Item {Name = "Nævesuppe", Price = 69 },
            };
        }


        //private static List<Sale> GetSale()
        //{
        //    return new List<Sale>()
        //    {
        //        new Sale {Establishment = GetEstablishments().First(), Items = new List<Item>() {GetItems().Find(x => x.Name == "Espresso") } }
        //    };
        //}

    }
    
}
