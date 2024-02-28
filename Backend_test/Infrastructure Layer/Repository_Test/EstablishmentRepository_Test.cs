using EstablishmentProject.test.TestingCode;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Data;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Domain_Layer.Services.Repositories;

namespace EstablishmentProject.test.Infrastructure_Layer
{
    public class EstablishmentRepository_Test : IntegrationTest
    {
        private ApplicationDbContext applicationDbContext;
        private EstablishmentRepository establishmentRepository;
        private Establishment establishment;

        public EstablishmentRepository_Test() : base([DatabaseTestContainer.CreateAsync().Result])
        {
            applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            establishmentRepository = new EstablishmentRepository(applicationDbContext);
            establishment = new Establishment("Test Establishment");
            var item = establishment.CreateItem("Test Item", 1);
            establishment.AddItem(item);
            var table = establishment.CreateTable("Test table");
            establishment.AddTable(table);
            var sale = establishment.CreateSale(DateTime.Now);
            establishment.AddSale(sale);
            var SalesItems = establishment.CreateSalesItem(sale, item, 1);
            establishment.AddSalesItems(sale, SalesItems);
            var SalesTables = establishment.CreateSalesTables(sale, table);
            establishment.AddSalesTables(sale, SalesTables);
            establishmentRepository.Add(establishment);
            applicationDbContext.SaveChanges();
        }


        [Fact]
        public async Task UsingLazyLoading_WithSalesItems_ShouldBeAbleToAccessRelatedData()
        {
            // Arrange

            // Act
            Establishment establishmentWithoutItems = establishmentRepository.GetById(establishment.Id);

            // Assert
            Assert.NotNull(establishmentWithoutItems.GetSales()[0].GetSalesItems());

        }

        [Fact]
        public async Task IncludeItems_WithItem_ShouldIncludeItems()
        {
            // Arrange

            // Act
            establishmentRepository.IncludeItems();
            Establishment establishmentWithItems = establishmentRepository.GetById(establishment.Id);

            // Assert
            Assert.NotNull(establishmentWithItems.GetItems());
        }

        [Fact]
        public async Task IncludeTables_WithTable_ShouldIncludeTables()
        {
            // Arrange

            // Act
            establishmentRepository.IncludeTables();
            Establishment establishmentWithTables = establishmentRepository.GetById(establishment.Id);

            // Assert
            Assert.NotNull(establishmentWithTables.GetTables());
        }


        [Fact]
        public async Task IncludeSales_WithSale_ShouldIncludeSales()
        {
            // Arrange

            // Act
            establishmentRepository.IncludeSales();
            Establishment establishmentWithSales = establishmentRepository.GetById(establishment.Id);

            // Assert
            Assert.NotNull(establishmentWithSales.GetSales());
        }

        [Fact]
        public async Task IncludeSalesItems_WithSalesItems_ShouldIncludeSalesItems()
        {
            // Arrange

            // Act
            establishmentRepository.IncludeSalesItems();
            Establishment establishmentWithSalesItems = establishmentRepository.GetById(establishment.Id);

            // Assert
            Assert.NotNull(establishmentWithSalesItems.Sales);
            Assert.NotNull(establishmentWithSalesItems.GetSales()[0].GetSalesTables());
        }
        [Fact]
        public async Task IncludeSalesTables_WithSalesTables_ShouldIncludeSalesTables()
        {
            // Arrange

            // Act
            establishmentRepository.IncludeSalesTables();
            Establishment establishmentWithSalesTables = establishmentRepository.GetById(establishment.Id);

            // Assert
            Assert.NotNull(establishmentWithSalesTables.Sales);
            Assert.NotNull(establishmentWithSalesTables.GetSales()[0].GetSalesItems());
        }
    }
}
