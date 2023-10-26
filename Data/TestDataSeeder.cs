using Microsoft.EntityFrameworkCore;
using WebApplication1.Repositories;

namespace WebApplication1.Data
{
    public static class TestDataSeeder
    {
        public static void SeedDataBase(ModelBuilder modelBuilder)
        {
            //Establishment estab = new Establishment("Frederiks Cafe");
            //Table table = new Table { Name = "table 1", Establishment = estab };
            //estab.Tables = new List<Table> { table };
            //modelBuilder.Entity<Establishment>().HasData(estab);

            // Create an Establishment
            Establishment estab = new Establishment { Name = "Frederiks Cafe" };

            // Create a Table associated with the Establishment
            Table table = new Table { Name = "table 1", Establishment = estab };
            modelBuilder.Entity<Table>().HasData(table);


            // Add the Table to the Establishment's Tables collection
            estab.Tables = new List<Table> { table };
            modelBuilder.Entity<Establishment>().HasData(estab);
        }
    }
    
}
