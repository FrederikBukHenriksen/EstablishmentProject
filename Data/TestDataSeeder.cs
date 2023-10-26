using Microsoft.EntityFrameworkCore;
using WebApplication1.Data.DataModels;
using WebApplication1.Repositories;
using static WebApplication1.Data.DataModels.UserRole;

namespace WebApplication1.Data
{
    public static class TestDataSeeder
    {
        public static void SeedDataBase(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Establishment>().HasData(
                GetEstablishment()
            );

            modelBuilder.Entity<User>().HasData(
                GetUser()
            );




            modelBuilder.Entity<UserRole>().HasData(
                new
                {
                    Id = Guid.NewGuid(),
                    Role = Role.Admin,
                    EstablishmentId = GetEstablishment().Id,
                    //Establishment = GetEstablishment(),
                    UserId = GetUser().Id,
                    //User = GetUser(),
                }
            );

            modelBuilder.Entity<Table>().HasData(
                new Table
                {
                    Name = "Table 1",
                    EstablishmentId = new Guid("00000000-0000-0000-0000-000000000001"),
                },
                new Table
                {
                    Name = "Table 2",
                    EstablishmentId = new Guid("00000000-0000-0000-0000-000000000001"),
                },
                new Table
                {
                    Name = "Table 3",
                    EstablishmentId = new Guid("00000000-0000-0000-0000-000000000001"),
                }
            );

            static User GetUser()
            {
                return new User
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000002"),
                    Username = "Frederik",
                    Password = "1234",
                };
            }
        }

        private static Establishment GetEstablishment()
        {
            return new Establishment
            {
                Id = new Guid("00000000-0000-0000-0000-000000000001"),
                Name = "My Establishment",
            };
        }
    }
    
}
