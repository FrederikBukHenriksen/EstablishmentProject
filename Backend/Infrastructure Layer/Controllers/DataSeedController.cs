using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NodaTime;
using WebApplication1.Application_Layer.Services;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Domain_Layer.Services.Entity_builders;
using WebApplication1.Domain_Layer.Services.Repositories;
using WebApplication1.Infrastructure.Data;
using WebApplication1.Utils;

namespace WebApplication1.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/test")]
    public class DataSeedController : ControllerBase
    {

        public DataSeedController()
        {
        }

        [HttpGet]
        public void SeedDatabase(IFactoryServiceBuilder factoryServiceBuilder, ITestDataCreatorService testDataCreatorService, IEstablishmentRepository establishmentRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            var totalDataTimePeriod = new DateTimePeriod(new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0).AddMonths(-1), new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0)); //Leave out last day of the year to test null values
            var timelineAllDays = TimeHelper.CreateTimelineAsList(totalDataTimePeriod, TimeResolution.Hour);
            var openingHours = testDataCreatorService.CreateSimpleOpeningHoursForWeek(new LocalTime(8, 0), new LocalTime(16, 0));
            var timelineDaysWithinOpeningHours = testDataCreatorService.SLETTES_DistrubutionBasedOnTimlineAndOpeningHours(timelineAllDays, openingHours);

            Dictionary<DateTime, int> distributionHourly = testDataCreatorService.SLETTES_GenerateDistributionFromTimeline(timelineDaysWithinOpeningHours, x => x.Hour, TestDataCreatorService.GetLinearFuncition(0.5, 1));
            Dictionary<DateTime, int> distributionDaily = testDataCreatorService.SLETTES_GenerateDistributionFromTimeline(timelineDaysWithinOpeningHours, x => x.Day, TestDataCreatorService.GetLinearFuncition(0.5, 0));
            Dictionary<DateTime, int> distributionMonthly = testDataCreatorService.SLETTES_GenerateDistributionFromTimeline(timelineDaysWithinOpeningHours, x => x.Month, TestDataCreatorService.GetLinearFuncition(0, 0));

            List<Dictionary<DateTime, int>> allDistributions = new List<Dictionary<DateTime, int>> { distributionHourly, distributionDaily, distributionMonthly };

            var aggregatedDistribution = testDataCreatorService.AggregateDistributions(allDistributions);

            var water = factoryServiceBuilder.ItemBuilder().withName("Water").withPrice(10).Build();
            var coffee = factoryServiceBuilder.ItemBuilder().withName("Coffee").withPrice(25).Build();
            var bun = factoryServiceBuilder.ItemBuilder().withName("Bun").withPrice(50).Build();


            var salesDistribution = testDataCreatorService.SaleGenerator(new List<(Item, int)> { (coffee, 1), (water, 1) }, aggregatedDistribution);

            Random random = new Random();

            foreach (var sale in salesDistribution)
            {
                int randomNumber = random.Next(2);
                if (randomNumber == 0)
                {
                    sale.TimestampArrival = sale.TimestampPayment.AddMinutes(-60);
                }
                else
                {
                    sale.TimestampArrival = sale.TimestampPayment.AddMinutes(-30);

                }
            }

            var establishment = factoryServiceBuilder
                .EstablishmentBuilder()
                .withName("Cafe Frederik")
                .withItems(new List<Item> { coffee, bun, water })
                .withSales(salesDistribution)
                .Build();

            var TestEstab = establishment!.Sales.First()!;
            TestEstab.Id = new Guid("ba9fcc86-1a5c-4ffa-a550-2c9c567e1965");


            var user = factoryServiceBuilder.UserBuilder().WithEmail("Frederik@mail.com").WithPassword("12345678").WithUserRoles(new List<(Establishment, Role)> { (establishment, Role.Admin) }).Build();


            using (var uow = unitOfWork)
            {
                uow.establishmentRepository.Add(establishment);
                uow.userRepository.Add(user);

            }
        }
    }
}