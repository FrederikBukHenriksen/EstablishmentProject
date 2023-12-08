using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Data;

namespace Establishment.Test
{
    public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
    {
        public IServiceScope scope;
        public ApplicationDbContext dbContext;


        public WebApplicationFactory<WebApplication1.Program> webApplicationFactory;
        public HttpClient httpClient;

        public BaseIntegrationTest(IntegrationTestWebAppFactory factory)
        {
            scope = factory.Services.CreateScope();
            dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            webApplicationFactory = new WebApplicationFactory<WebApplication1.Program>();
            httpClient = webApplicationFactory.CreateDefaultClient();
            clearDatabase();

        }   

        protected void clearDatabase()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
        }
    }


}
