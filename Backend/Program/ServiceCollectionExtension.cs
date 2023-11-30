using MathNet.Numerics;
using WebApplication1.Application_Layer.Services;
using WebApplication1.CommandHandlers;
using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Data;
using WebApplication1.Domain.Services.Repositories;
using WebApplication1.Middelware;
using WebApplication1.Services;
using static WebApplication1.CommandHandlers.MeanSales;

namespace WebApplication1.Program
{
    public static class ServiceCollectionExtension
    {
        public static void AddServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IAuthService, AuthService>();
            serviceCollection.AddScoped<IUserContextService, UserContextService>();
            serviceCollection.AddScoped<UserContextMiddleware>();
            serviceCollection.AddScoped<ISalesService, SalesService>();

            serviceCollection.AddScoped<IDatabaseContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        }

        public static void AddRepositories(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IEstablishmentRepository, EstablishmentRepository>();
            serviceCollection.AddScoped<IUserRepository, UserRepository>();
            serviceCollection.AddScoped<IUserRolesRepository, UserRolesRepository>();
            serviceCollection.AddScoped<ISalesRepository, SalesRepository>();
            serviceCollection.AddScoped<IItemRepository, ItemRepository>();

        }

        public static void AddCommandHandlers(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IHandler<LoginCommand, LoginReturn>, LoginCommandHandler>();
            serviceCollection.AddTransient<IHandler<SalesQuery, SalesQueryReturn>, SalesQueryHandler>();
            serviceCollection.AddTransient<IHandler<CorrelationBetweenSalesAndWeatherCommand, CorrelationBetweenSalesAndWeatherReturn>, CorrelationBetweenSoldItemsAndWeatherCommandHandler>();
            serviceCollection.AddTransient<IHandler<CorrelationGraphCommand, CorrelationGraphReturn>, CorrelationGraphHandler>();
            serviceCollection.AddTransient<IHandler<MeanItemSalesCommand, MeanItemSalesReturn>, MeanItemSalesHandler>();
            serviceCollection.AddTransient<IHandler<MeanShiftClusteringCommand, MeanShiftClusteringReturn>, MeanShiftClusteringHandler>();


        }
    }
}