using DMIOpenData;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Testcontainers.PostgreSql;
using WebApplication1.Data;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Program;
using WebApplication1.Utils;

namespace EstablishmentProject.test.TestingCode
{

    public interface ITestService
    {
        public void Config(IWebHostBuilder webHostBuilder);
    }

    public interface IBaseTest
    {
    }

    public class BaseTest : WebApplicationFactory<Program>
    {
        public IServiceScope scope;

        public List<ITestService> testServices = new List<ITestService> { };



        public BaseTest(List<ITestService>? testServices = null)
        {
            if (testServices != null)
            {
                this.testServices = testServices;
            }
            scope = Services.CreateScope();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            if (testServices.Count > 0)
            {
                foreach (var service in testServices)
                {
                    service.Config(builder);
                }
            }
        }
    }

    public class WeatherMock : IWeatherApi, ITestService
    {
        private readonly Mock<IWeatherApi> mockWeatherApi = new Mock<IWeatherApi>();

        public List<(DateTime, double)> returnValue = new List<(DateTime, double)> { };

        public WeatherMock()
        {
            mockWeatherApi.Setup(api => api.GetTemperature(It.IsAny<Coordinates>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<TimeResolution>()))
                .ReturnsAsync(returnValue);
        }

        public Task<List<(DateTime, double)>> GetTemperature(Coordinates coordinates, DateTime startTime, DateTime endTime, TimeResolution timeresolution)
        {
            return mockWeatherApi.Object.GetTemperature(coordinates, startTime, endTime, timeresolution);
        }
        public void Config(IWebHostBuilder webHostBuilder)
        {
            webHostBuilder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(s => s.ServiceType == typeof(IWeatherApi));
                if (descriptor is not null)
                {
                    services.Remove(descriptor);
                }
                services.AddScoped<IWeatherApi, WeatherMock>();

            });
        }
    }



    //public class ByPassUserLoginMock : IVerifyEstablishmentCommandService, ITestService
    //{

    //    private readonly Mock<IVerifyEstablishmentCommandService> verifyEstablishmentCommandService = new Mock<IVerifyEstablishmentCommandService>();

    //    public ByPassUserLoginMock()
    //    {
    //        verifyEstablishmentCommandService.Setup(api => api.VerifyEstablishment(It.IsAny<ICommand>()))
    //            .Callback<ICommand>(command => { return; });

    //    }
    //    public void VerifyEstablishment(ICommand command)
    //    {
    //        return;
    //    }



    //    public void Config(IWebHostBuilder webHostBuilder)
    //    {
    //        webHostBuilder.ConfigureTestServices(services =>
    //        {
    //            var descriptor = services.SingleOrDefault(s => s.ServiceType == typeof(IVerifyEstablishmentCommandService));
    //            if (descriptor is not null)
    //            {
    //                services.Remove(descriptor);
    //            }
    //            services.AddScoped<IVerifyEstablishmentCommandService, ByPassUserLoginMock>();
    //        });
    //    }


    //}

    public class TestContainer : IAsyncLifetime, ITestService
    {
        private readonly PostgreSqlContainer _dbContainer;

        // Private constructor, use static method CreateAsync to create an instance
        private TestContainer(PostgreSqlContainer dbContainer)
        {
            _dbContainer = dbContainer;
        }

        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();
        }

        public async Task DisposeAsync()
        {
            await _dbContainer.StopAsync();
        }

        public void Config(IWebHostBuilder webHostBuilder)
        {
            webHostBuilder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                if (descriptor is not null)
                {
                    services.Remove(descriptor);
                }
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseNpgsql(_dbContainer.GetConnectionString());
                });
            });
        }

        // Static method to create an instance of TestContainer
        public static async Task<TestContainer> CreateAsync()
        {
            var dbContainer = new PostgreSqlBuilder()
                .WithImage("postgres:latest")
                .WithDatabase("EstablishmentProject")
                .WithUsername("postgres")
                .WithPassword("postgres")
                .Build();

            // Initialize the container
            await dbContainer.StartAsync();

            // Create an instance of TestContainer
            return new TestContainer(dbContainer);
        }
    }

}
