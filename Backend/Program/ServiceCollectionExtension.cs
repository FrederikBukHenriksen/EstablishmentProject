using DMIOpenData;
using WebApplication1.Application_Layer.CommandsQueriesHandlersReturns.EstablishmentHandlers;
using WebApplication1.Application_Layer.CommandsQueriesHandlersReturns.SalesHandlers;
using WebApplication1.Application_Layer.Handlers.ItemHandler;
using WebApplication1.Application_Layer.Handlers.SalesHandlers;
using WebApplication1.Application_Layer.Services;
using WebApplication1.Application_Layer.Services.CommandHandlerServices;
using WebApplication1.CommandHandlers;
using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Domain_Layer.Services.Repositories;
using WebApplication1.Infrastructure.Data;
using WebApplication1.Middelware;
using WebApplication1.Services;

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
            serviceCollection.AddScoped<ISalesService, SalesService>();

            //serviceCollection.AddScoped<IDatabaseContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

            //Entity services
            serviceCollection.AddTransient<IEstablishmentService, EstablishmentService>();
            //serviceCollection.AddTransient<ISaleBuilder, SaleBuilder>();
            //serviceCollection.AddTransient<IItemBuilderService, ItemBuilderService>();
            //serviceCollection.AddTransient<IUserBuilder, UserBuilder>();

            serviceCollection.AddScoped<ITestDataCreatorService, TestDataCreatorService>();
            serviceCollection.AddScoped<IWeatherApi, DmiWeatherApi>();
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
            //CommandHandlerServices
            serviceCollection.AddScoped<ICommandValidatorService, HandlerService>();
            serviceCollection.AddScoped<IVerifyEstablishmentCommandService, VerifyEstablishmentCommandService>();
            serviceCollection.AddScoped<IVerifySalesCommandService, VerifySalesCommandService>();
            serviceCollection.AddScoped<IVerifyItemsCommandService, VerifyItemsCommandService>();

            //Login
            serviceCollection.AddTransient<IHandler<LoginCommand, LoginReturn>, LoginCommandHandler>();

            //Correlation
            serviceCollection.AddTransient<IHandler<CorrelationCommand, CorrelationReturn>, CorrelationHandler>();
            serviceCollection.AddTransient<IHandler<CorrelationGraphCommand, CorrelationGraphReturn>, CorrelationGraphHandler>();

            //Clustering
            serviceCollection.AddTransient<IHandler<MeanShiftClusteringCommand, MeanShiftClusteringReturn>, salesClustering>();
            serviceCollection.AddTransient<IHandler<Clustering_TimeOfVisit_TotalPrice_Command, ClusteringReturn>, Clustering_TimeOfVisitVSTotalPrice>();
            serviceCollection.AddTransient<IHandler<Clustering_TimeOfVisit_LengthOfVisit_Command, ClusteringReturn>, Clustering_TimeOfVisitVSLengthOfVisit>();


            //Establishment
            serviceCollection.AddTransient<IHandler<GetEstablishmentCommand, GetEstablishmentReturn>, GetEstablishmentHandler>();
            serviceCollection.AddTransient<IHandler<GetMultipleEstablishmentsCommand, GetMultipleEstablishmentsReturn>, GetMultipleEstablishmentsHandler>();


            //Sale
            serviceCollection.AddTransient<IHandler<GetSalesDTOCommand, GetSalesDTOReturn>, GetSalesDTOHandler<GetSalesDTOReturn>>();
            serviceCollection.AddTransient<IHandler<GetSalesDTOCommand, GetSalesRawReturn>, GetSalesDTOHandler<GetSalesRawReturn>>();

            serviceCollection.AddTransient<IHandler<GetSalesCommand, GetSalesReturn>, GetSalesHandler<GetSalesReturn>>();
            serviceCollection.AddTransient<IHandler<GetSalesCommand, GetSalesRawReturn>, GetSalesHandler<GetSalesRawReturn>>();
            serviceCollection.AddTransient<IHandler<GetSalesCommand, GetSalesDTOReturn>, GetSalesHandler<GetSalesDTOReturn>>();

            //Item
            serviceCollection.AddTransient<IHandler<GetItemDTOCommand, GetItemDTOReturn>, GetItemDTOHandler>();
            serviceCollection.AddTransient<IHandler<GetItemsCommand, GetItemsReturn>, GetItemsHandler>();






        }
    }
}