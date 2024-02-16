using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NodaTime;
using System.Diagnostics.CodeAnalysis;
using WebApplication1.Application_Layer.Services;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Domain_Layer.Services.Repositories;
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

        [HttpGet]
        public void SeedDatabase(ITestDataCreatorService testDataCreatorService, IEstablishmentRepository establishmentRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            var totalDataTimePeriod = new DateTimePeriod(new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0).AddMonths(-3), new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0)); //Leave out last day of the year to test null values
            var timelineAllDays = TimeHelper.CreateTimelineAsList(totalDataTimePeriod.Start, totalDataTimePeriod.End, TimeResolution.Hour);
            var openingHours = testDataCreatorService.CreateSimpleOpeningHoursForWeek(new LocalTime(8, 0), new LocalTime(16, 0));
            var timelineDaysWithinOpeningHours = testDataCreatorService.SLETTES_DistrubutionBasedOnTimlineAndOpeningHours(timelineAllDays, openingHours);

            Dictionary<DateTime, int> distributionHourly = testDataCreatorService.GenerateDistributionFromTimeline(timelineDaysWithinOpeningHours, x => x.Hour, TestDataCreatorService.GetLinearFuncition(0.5, 1));
            Dictionary<DateTime, int> distributionDaily = testDataCreatorService.GenerateDistributionFromTimeline(timelineDaysWithinOpeningHours, x => x.Day, TestDataCreatorService.GetLinearFuncition(0.5, 0));
            Dictionary<DateTime, int> distributionMonthly = testDataCreatorService.GenerateDistributionFromTimeline(timelineDaysWithinOpeningHours, x => x.Month, TestDataCreatorService.GetLinearFuncition(0, 0));

            Dictionary<DateTime, int> aggregatedDistribution = testDataCreatorService.AggregateDistributions(new List<Dictionary<DateTime, int>> { distributionHourly, distributionDaily, distributionMonthly });

            var establishment = new Establishment("Cafe Frederik");
            var water = establishment.AddItem(establishment.CreateItem("Water", 10, Currency.DKK));
            var coffee = establishment.AddItem(establishment.CreateItem("Coffee", 25, Currency.DKK));
            var esrepsso = establishment.AddItem(establishment.CreateItem("Espresso", 30, Currency.DKK));
            var latte = establishment.AddItem(establishment.CreateItem("Latte", 40, Currency.DKK));
            var bun = establishment.AddItem(establishment.CreateItem("Bun", 50, Currency.DKK));

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
                    Sale newSale = establishment.AddSale(establishment.CreateSale(timestampPayment: entry.Key, itemAndQuantity: baskets[randomNumber]));
                    salesDistribution.Add(newSale);
                }
            }



            //var salesDistribution1 = testDataCreatorService.SaleGenerator(new List<(Item, int)> { (coffee, 1), (water, 2) }, distributionHourly);
            //var salesDistribution2 = testDataCreatorService.SaleGenerator(new List<(Item, int)> { (coffee, 2), (water, 1), (bun, 2) }, distributionDaily);

            //var salesDistribution = salesDistribution1.Concat(salesDistribution2).ToList();

            foreach (var sale in salesDistribution)
            {
                int randomNumber = random.Next(-30, 31);
                sale.TimestampPayment = sale.TimestampPayment.AddMinutes(randomNumber);
            }

            //var establishmentBack = factoryServiceBuilder
            //    .EstablishmentBuilder()
            //    .withName("Cafe Frederik")
            //    .withItems(new List<Item> { coffee, latte, esrepsso, bun, water })
            //    .withSales(salesDistribution)
            //    .Build();

            //var user = factoryServiceBuilder.UserBuilder().WithEmail("Frederik@mail.com").WithPassword("12345678").WithUserRoles(new List<(Establishment, Role)> { (establishment, Role.Admin) }).Build();
            var user = new User("Frederik@mail.com", "12345678", new List<(Establishment, Role)> { (establishment, Role.Admin) });

            using (var uow = unitOfWork)
            {
                uow.establishmentRepository.Add(establishment);
                uow.userRepository.Add(user);

            }
        }
    }
}