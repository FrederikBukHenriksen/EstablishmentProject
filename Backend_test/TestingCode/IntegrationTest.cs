using DMIOpenData;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Testcontainers.PostgreSql;
using WebApplication1.Data;
using WebApplication1.Program;
using WebApplication1.Utils;

namespace EstablishmentProject.test.TestingCode
{

    public interface ITestService
    {
        public void Config(IWebHostBuilder webHostBuilder);
    }

    public interface IIntegrationTest
    {
    }

    public class IntegrationTest : WebApplicationFactory<Program>
    {
        public IServiceScope scope;

        public HttpClient httpClient;

        public List<ITestService> testServices = new List<ITestService>();

        public IntegrationTest(List<ITestService>? testServices = null)
        {
            this.testServices = testServices != null ? testServices : this.testServices;
            scope = Services.CreateScope();
            httpClient = CreateDefaultClient();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            if (!testServices.IsNullOrEmpty())
            {
                foreach (var service in testServices)
                {
                    service.Config(builder);
                }
            }
            if (!testServices.Any(x => x is DatabaseTestContainer))
            {
                removeDatabaseConnection(builder);
            }
        }
        private void removeDatabaseConnection(IWebHostBuilder webHostBuilder)
        {
            webHostBuilder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                if (descriptor is not null)
                {
                    services.Remove(descriptor);

                    string connectionString = "Host=localhost; Database=NoDatabase; Username=postgres; password=postgres";
                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseLazyLoadingProxies(true);
                        options.UseNpgsql(connectionString);
                    });
                }
            }
            );
        }
    }

    public class WeatherMock : IWeather, ITestService
    {
        private readonly Mock<IWeather> mockWeatherApi = new Mock<IWeather>();

        public List<(DateTime, double)> temperature = new List<(DateTime, double)> { };


        private WeatherMock()
        {
            setTemperatureReturn();
        }

        public WeatherMock(List<(DateTime, double)> temperature)
        {
            this.temperature = temperature;
            setTemperatureReturn();
        }


        public void setTemperatureReturn()
        {
            mockWeatherApi.Setup(api => api.GetMeanTemperature(It.IsAny<Coordinates>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<TimeResolution>()))
            .ReturnsAsync(temperature);
        }

        public Task<List<(DateTime, double)>> GetMeanTemperature(Coordinates coordinates, DateTime startTime, DateTime endTime, TimeResolution timeresolution)
        {
            return mockWeatherApi.Object.GetMeanTemperature(coordinates, startTime, endTime, timeresolution);
        }
        public void Config(IWebHostBuilder webHostBuilder)
        {
            webHostBuilder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(s => s.ServiceType == typeof(IWeather));
                if (descriptor is not null)
                {
                    services.Remove(descriptor);
                    var thisInstance = this;
                    services.AddSingleton<IWeather>(thisInstance);
                }

            });
        }
    }

    public class DatabaseTestContainer : IAsyncLifetime, ITestService
    {
        private readonly PostgreSqlContainer _dbContainer;

        private DatabaseTestContainer(PostgreSqlContainer dbContainer)
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

        public static async Task<DatabaseTestContainer> CreateAsync()
        {
            var dbContainer = new PostgreSqlBuilder()
                .WithImage("postgres:latest")
                .WithDatabase("EstablishmentProject")
                .WithUsername("postgres")
                .WithPassword("postgres")
                .Build();

            await dbContainer.StartAsync();

            return new DatabaseTestContainer(dbContainer);
        }
    }
}
