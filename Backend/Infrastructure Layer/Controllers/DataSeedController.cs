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
        private IUserRepository _userRepository;
        private ISalesService salesService;
        private ISalesRepository _salesRepository;
        private IUserRolesRepository _userRolesRepository;
        private IEstablishmentRepository _establishmentRepository;
        private ApplicationDbContext _applicationDbContext;

        public DataSeedController(ISalesService salesService, IEstablishmentRepository establishmentRepository, IUserRepository userRepository, IUserRolesRepository userRolesRepository, ISalesRepository salesRepository)
        {
            this.salesService = salesService;
            this._salesRepository = salesRepository;
            this._userRolesRepository = userRolesRepository;
            this._userRepository = userRepository;
            this._establishmentRepository = establishmentRepository;
        }

        [HttpGet]
        public void SeedDatabase(IFactoryServiceBuilder factoryServiceBuilder, ITestDataCreatorService testDataCreatorService, IEstablishmentRepository establishmentRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            var totalDataTimePeriod = new DateTimePeriod(new DateTime(2021, 1, 1, 0, 0, 0), new DateTime(2021, 12, 30, 23, 0, 0)); //Leave out last day of the year to test null values
            var timelineAllDays = TimeHelper.CreateTimelineAsList(totalDataTimePeriod, TimeResolution.Hour);
            var openingHours = testDataCreatorService.CreateSimpleOpeningHoursForWeek(new LocalTime(8, 0), new LocalTime(16, 0));
            var timelineDaysWithinOpeningHours = testDataCreatorService.FilterDistrubutionBasedOnOpeningHours(timelineAllDays, openingHours);

            Dictionary<DateTime, int> distributionHourly = testDataCreatorService.GenerateDistributionFromTimeline(timelineDaysWithinOpeningHours, x => x.Hour, TestDataCreatorService.GetLinearFuncition(0, 1));
            Dictionary<DateTime, int> distributionDaily = testDataCreatorService.GenerateDistributionFromTimeline(timelineDaysWithinOpeningHours, x => x.Day, TestDataCreatorService.GetLinearFuncition(0, 0));
            Dictionary<DateTime, int> distributionMonthly = testDataCreatorService.GenerateDistributionFromTimeline(timelineDaysWithinOpeningHours, x => x.Month, TestDataCreatorService.GetLinearFuncition(0, 0));

            List<Dictionary<DateTime, int>> allDistributions = new List<Dictionary<DateTime, int>> { distributionHourly, distributionDaily, distributionMonthly };

            bool doesDictionariesHaveTheSameKeys = allDistributions.All(x => x.Keys.SequenceEqual(allDistributions.First().Keys));

            var aggregatedDistribution = testDataCreatorService.AggregateDistributions(allDistributions);

            var water = factoryServiceBuilder.ItemBuilder().WithName("Water").WithPrice(10).Build();
            var coffee = factoryServiceBuilder.ItemBuilder().WithName("Coffee").WithPrice(25).Build();
            var bun = factoryServiceBuilder.ItemBuilder().WithName("Bun").WithPrice(50).Build();


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
                .SetName("Cafe Frederik")
                .AddItems(new List<Item> { coffee, bun, water })
                .AddSales(salesDistribution)
                .Build();


            var user = factoryServiceBuilder.UserBuilder().WithEmail("Frederik@mail.com").WithPassword("12345678").WithUserRoles(new List<(Establishment, Role)> { (establishment, Role.Admin) }).Build();


            using (var uow = unitOfWork)
            {
                uow.establishmentRepository.Add(establishment);
                uow.userRepository.Add(user);

            }
        }

        [HttpGet("lol")]
        public void test(IEstablishmentRepository establishmentRepository, IUnitOfWork unitOfWork)
        {
            using (var uow = unitOfWork)
            {
                var ee = uow.establishmentRepository.GetAll().First();
            }
        }

    }
}