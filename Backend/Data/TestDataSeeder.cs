using Microsoft.EntityFrameworkCore;
using WebApplication1.Data.DataModels;
using WebApplication1.Repositories;
using static WebApplication1.Data.DataModels.UserRole;
using System;
using System.Collections.Generic;
using MathNet.Numerics.Distributions;


namespace WebApplication1.Data
{
    public static class TestDataSeeder
    {
        public static void SeedDataBase(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                GetUsers()
            );
            modelBuilder.Entity<Establishment>().HasData(
                GetEstablishments()
            );

            modelBuilder.Entity<UserRole>().HasData(
                new
                {
                    Id = Guid.NewGuid(),
                    Role = Role.Admin,
                    EstablishmentId = new Guid("00000000-0000-0000-0000-000000000001"),
                    UserId = new Guid("00000000-0000-0000-0000-000000000001"),
                }
            ); ;

            modelBuilder.Entity<Item>().HasData(
                new { Id = Guid.NewGuid(), EstablishmentId = new Guid("00000000-0000-0000-0000-000000000001"), Name = "Espresso", Price = 30.0 }
                );
        }

        private static List<User> GetUsers()
        {
            //double mean = 50.0;
            //double stdDev = 10.0;
            //int numberOfDataPoints = 100;

            //// Create a normal distribution
            //Normal normalDistribution = new Normal(mean, stdDev);

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

        //private static List<Item> GetItems()
        //{
        //    return new List<Item>()
        //    {
        //        new {Id = Guid.NewGuid(),Establishment = new Guid("00000000-0000-0000-0000-000000000001"),Name = "Espresso", Price = 30},
        //        //new Item {Name = "Smoothie", Price = 50},
        //        //new Item {Name = "Bun with cheese", Price = 60}
        //    };
        //}


        //private static List<Sale> GetSale()
        //{
        //    return new List<Sale>()
        //    {
        //        new Sale {Establishment = GetEstablishments().First(), Items = new List<Item>() {GetItems().Find(x => x.Name == "Espresso") } }
        //    };
        //}

    }
    
}
