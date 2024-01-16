using WebApplication1.Application_Layer.CommandsQueriesHandlersReturns.EstablishmentHandlers;
using WebApplication1.Application_Layer.Services;
using WebApplication1.CommandHandlers;
using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Domain_Layer.Services.Entity_builders;
using WebApplication1.Domain_Layer.Services.Repositories;
using WebApplication1.Infrastructure.Data;
using WebApplication1.Middelware;
using WebApplication1.Services;
using static WebApplication1.CommandHandlers.MeanSales;

namespace WebApplication1.Program
{
    public static class ServiceCollectionExtension
    {
        public static void AddServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddTransient<ApplicationDbContext>();
            serviceCollection.AddScoped<IAuthService, AuthService>();
            serviceCollection.AddScoped<IUserContextService, ContextService>();
            serviceCollection.AddScoped<UserContextMiddleware>();
            serviceCollection.AddTransient<IHandlerService, HandlerService>();

            serviceCollection.AddScoped<VerifyEstablishmentCommandService>();

            serviceCollection.AddScoped<ISalesService, SalesService>();
            //serviceCollection.AddScoped<IDatabaseContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

            //Entity services
            serviceCollection.AddTransient<IEstablishmentService, EstablishmentService>();
            serviceCollection.AddTransient<ISaleBuilder, SaleBuilder>();
            serviceCollection.AddTransient<IItemBuilderService, ItemBuilderService>();
            serviceCollection.AddTransient<IUserBuilder, UserBuilder>();


            serviceCollection.AddScoped<IFactoryServiceBuilder, FactoryServiceBuilder>();
            serviceCollection.AddScoped<ITestDataCreatorService, TestDataCreatorService>();


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
            serviceCollection.AddTransient<IHandler<SalesMeanOverTime, SalesMeanQueryReturn>, SalesMeanOverTimeQueryHandler>();

            serviceCollection.AddTransient<IHandler<CorrelationCommand, CorrelationReturn>, CorrelationHandler>();
            serviceCollection.AddTransient<IHandler<CorrelationGraphCommand, CorrelationGraphReturn>, CorrelationGraphHandler>();
            serviceCollection.AddTransient<IHandler<MeanSalesCommand, MeanSalesReturn>, MeanSalesHandler>();
            //Clustering
            serviceCollection.AddTransient<IHandler<MeanShiftClusteringCommand, MeanShiftClusteringReturn>, salesClustering>();
            serviceCollection.AddTransient<IHandler<Clustering_TimeOfVisit_TotalPrice_Command, ClusteringReturn>, Clustering_TimeOfVisitVSTotalPrice>();
            serviceCollection.AddTransient<IHandler<Clustering_TimeOfVisit_LengthOfVisit_Command, ClusteringReturn>, Clustering_TimeOfVisitVSLengthOfVisit>();


            //Establishment
            serviceCollection.AddTransient<IHandler<GetEstablishmentCommand, GetEstablishmentReturn>, GetEstablishmentHandler>();
            serviceCollection.AddTransient<IHandler<GetMultipleEstablishmentsCommand, GetMultipleEstablishmentsReturn>, GetMultipleEstablishmentsHandler>();






        }
    }
}