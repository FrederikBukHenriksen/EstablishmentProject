using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Data;
using WebApplication1.Program;

namespace EstablishmentProject.Test
{
    public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
    {
        public IServiceScope scope;
        public ApplicationDbContext dbContext;

        public WebApplicationFactory<Program> webApplicationFactory;
        public HttpClient httpClient;

        public BaseIntegrationTest(IntegrationTestWebAppFactory factory)
        {
            scope = factory.Services.CreateScope();
            dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var connection = dbContext.Database.GetDbConnection();
            webApplicationFactory = factory;  // Use the instance provided by the IntegrationTestWebAppFactory
            httpClient = webApplicationFactory.CreateDefaultClient();
            clearDatabase();
        }

        protected void clearDatabase()
        {
            //dbContext.Database.EnsureDeleted();
            //dbContext.Database.EnsureCreated();
        }
    }
}
