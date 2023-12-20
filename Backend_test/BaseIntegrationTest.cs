using EstablishmentProject.test;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
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
        this.scope = factory.Services.CreateScope();
        dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        webApplicationFactory = factory;  // Use the instance provided by the IntegrationTestWebAppFactory
        httpClient = webApplicationFactory.CreateDefaultClient();
    }

    protected void clearDatabase()
    {
        dbContext.Database.EnsureDeleted();
        //dbContext.Database.EnsureCreated();
    }
}