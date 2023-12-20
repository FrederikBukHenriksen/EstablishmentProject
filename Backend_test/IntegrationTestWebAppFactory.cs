using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Renci.SshNet;
using Testcontainers.PostgreSql;
using WebApplication1.Data;
using WebApplication1.Domain.Services.Repositories;
using WebApplication1.Program;
using Xunit;

namespace EstablishmentProject.Test
{
    public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        public PostgreSqlContainer _dbContainer;
        public IntegrationTestWebAppFactory()
        {
            _dbContainer =
            new PostgreSqlBuilder()
            .WithImage("postgres:latest")
            .WithDatabase("test")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();
        }

        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            await _dbContainer.StopAsync();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Replace the DbContext registration with a version that uses the PostgreSQL container connection string
                services.RemoveAll(typeof(ApplicationDbContext));
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseNpgsql(_dbContainer.GetConnectionString());
                });
            });
        }




    }
}