using MathNet.Numerics.Distributions;
using MathNet.Numerics.Random;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using WebApplication1.Application_Layer.Services;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Infrastructure.Data;
using WebApplication1.Utils;

namespace WebApplication1.Controllers
{
    [ExcludeFromCodeCoverage]
    [AllowAnonymous]
    [ApiController]
    [Route("api/test")]
    public class DataSeedController : ControllerBase
    {

        public DataSeedController()
        {
        }

        [HttpGet("above-each-other")]
        public void aboveeachother(IUnitOfWork unitOfWork)
        {
            var testDataBuilder = new TestDataBuilder();

            //ARRANGE
            var establishment = new Establishment("Cafe 1");
            var testItem = establishment.CreateItem("test", 1);
            establishment.AddItem(testItem);

            Func<double, double> linearFirstDistribution = TestDataBuilder.GetLinearFuncition(2, -8 * 2);
            Func<double, double> linearSecondDistribution = TestDataBuilder.GetLinearFuncition(-2, 32);

            var firstSalesDistribution = testDataBuilder.FINALgenerateDistrubution(DateTime.Today.AddDays(-1), DateTime.Today, linearFirstDistribution, TimeResolution.Hour);
            var firstSales = testDataBuilder.FINALFilterOnOpeningHours(8, 12, firstSalesDistribution);
            var secondSalesDistribution = testDataBuilder.FINALgenerateDistrubution(DateTime.Today.AddDays(-1), DateTime.Today, linearSecondDistribution, TimeResolution.Hour);
            var secondSales = testDataBuilder.FINALFilterOnOpeningHours(12, 16, secondSalesDistribution);

            var aggregate = testDataBuilder.FINALAggregateDistributions([firstSales, secondSales]);

            var normalRandomSeed = new SystemRandomSource(1);

            Normal normal = new Normal(0, 5, normalRandomSeed);
            foreach (var distribution in aggregate.ToList())
            {
                for (int i = 0; i < distribution.Value; i++)
                {
                    var randomNormalDistributionNumber = normal.RandomSource.Next(0, 100);
                    var sale = establishment.CreateSale(distribution.Key);
                    establishment.AddSale(sale);
                    var salesItems = establishment.CreateSalesItem(sale, testItem, randomNormalDistributionNumber);
                    establishment.AddSalesItems(sale, salesItems);
                }
            }

            foreach (var distribution in aggregate.ToList())
            {
                for (int i = 0; i < distribution.Value; i++)
                {
                    var randomNormalDistributionNumber = normal.RandomSource.Next(200, 300);
                    var sale = establishment.CreateSale(distribution.Key);
                    establishment.AddSale(sale);
                    var salesItems = establishment.CreateSalesItem(sale, testItem, randomNormalDistributionNumber);
                    establishment.AddSalesItems(sale, salesItems);
                }
            }

            var user = new User("Frederik@mail.com", "12345678");
            var userRole = user.CreateUserRole(establishment, user, Role.Admin);
            user.AddUserRole(userRole);

            using (var uow = unitOfWork)
            {
                uow.establishmentRepository.Add(establishment);
                uow.userRepository.Add(user);

            }
        }

        [HttpGet]
        public void SeedDatabase(IUnitOfWork unitOfWork)
        {

            var testDataCreator = new TestDataBuilder();
            Dictionary<DateTime, int> distributionHourly = testDataCreator.FINALgenerateDistrubution(DateTime.Today.AddDays(-30), DateTime.Today, TestDataBuilder.GetLinearFuncition(0.25, 1), TimeResolution.Hour);
            Dictionary<DateTime, int> distributionDaily = testDataCreator.FINALgenerateDistrubution(DateTime.Today.AddDays(-30), DateTime.Today, TestDataBuilder.GetLinearFuncition(0.0, 0), TimeResolution.Date);
            Dictionary<DateTime, int> distributionMonthly = testDataCreator.FINALgenerateDistrubution(DateTime.Today.AddDays(-30), DateTime.Today, TestDataBuilder.GetLinearFuncition(0, 0), TimeResolution.Month);
            Dictionary<DateTime, int> aggregatedDistribution = testDataCreator.FINALAggregateDistributions(new List<Dictionary<DateTime, int>> { distributionHourly, distributionDaily, distributionMonthly });

            var establishment = new Establishment("Cafe Frederik");
            var water = establishment.CreateItem("Water", 10);
            establishment.AddItem(water);
            var coffee = establishment.CreateItem("Coffee", 25);
            establishment.AddItem(coffee);
            var esrepsso = establishment.CreateItem("Espresso", 30);
            establishment.AddItem(esrepsso);
            var latte = establishment.CreateItem("Latte", 40);
            establishment.AddItem(latte);
            var bun = establishment.CreateItem("Bun", 50);
            establishment.AddItem(bun);

            List<List<(Item, int)>> baskets = new List<List<(Item, int)>> {
                new List<(Item, int)> { (esrepsso, 1), (water, 1) },
                new List<(Item, int)> { (esrepsso, 2), (bun, 2) },
                new List<(Item, int)> { (coffee, 3) },
                new List<(Item, int)> { (coffee, 1), (bun, 1), (esrepsso, 1) },
                new List<(Item, int)> { (latte, 2), (esrepsso,2), (bun, 2) }
            };

            Random random = new Random();
            List<Sale> salesDistribution = new List<Sale>();

            foreach (KeyValuePair<DateTime, int> entry in aggregatedDistribution)
            {
                for (int i = 0; i < entry.Value; i++)
                {
                    int randomNumber = random.Next(0, baskets.Count);
                    Sale newSale = establishment.CreateSale(timestampPayment: entry.Key, itemAndQuantity: baskets[randomNumber]);
                    establishment.AddSale(newSale);
                    salesDistribution.Add(newSale);
                }
            }

            foreach (var sale in salesDistribution)
            {
                int randomNumber = random.Next(-30, 31);
                sale.TimestampPayment = sale.TimestampPayment.AddMinutes(randomNumber);
            }

            var user = new User("Frederik@mail.com", "12345678");
            var userRole = user.CreateUserRole(establishment, user, Role.Admin);
            user.AddUserRole(userRole);

            using (var uow = unitOfWork)
            {
                uow.establishmentRepository.Add(establishment);
                uow.userRepository.Add(user);

            }
        }
    }
}