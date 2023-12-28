using EstablishmentProject.test;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Data;
using WebApplication1.Program;

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
        webApplicationFactory = factory;
        httpClient = webApplicationFactory.CreateDefaultClient();
    }

    protected void clearDatabase()
    {
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();
    }
}