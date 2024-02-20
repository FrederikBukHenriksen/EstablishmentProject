using DMIOpenData;
using System.Diagnostics.CodeAnalysis;
using WebApplication1.Application_Layer.CommandsQueriesHandlersReturns.EstablishmentHandlers;
using WebApplication1.Application_Layer.Handlers.ItemHandler;
using WebApplication1.Application_Layer.Handlers.SalesHandlers;
using WebApplication1.Application_Layer.Services;
using WebApplication1.Application_Layer.Services.CommandHandlerServices;
using WebApplication1.CommandHandlers;
using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Domain_Layer.Services.Repositories;
using WebApplication1.Infrastructure.Data;
using WebApplication1.Middelware;
using WebApplication1.Services;

namespace WebApplication1.Program
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtension
    {
        public static void AddServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddTransient<ApplicationDbContext>();
            serviceCollection.AddScoped<IAuthService, AuthService>();
            serviceCollection.AddScoped<IUserContextService, ContextService>();
            serviceCollection.AddScoped<UserContextMiddleware>();

            serviceCollection.AddScoped<ITestDataCreatorService, TestDataCreatorService>();
            serviceCollection.AddScoped<IWeatherApi, DmiWeatherApi>();
            serviceCollection.AddTransient<IDataFetcingAndStoringService, DataFetcingAndStoringService>();
        }

        public static void AddRepositories(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IEstablishmentRepository, EstablishmentRepository>();
            serviceCollection.AddScoped<IUserRepository, UserRepository>();

        }

        public static void AddCommandHandlers(this IServiceCollection serviceCollection)
        {
            //CommandHandlerServices
            serviceCollection.AddScoped<ICommandValidatorService, HandlerService>();
            serviceCollection.AddScoped<IVerifyEstablishmentCommandService, VerifyEstablishmentCommandService>();
            serviceCollection.AddScoped<IVerifySalesCommandService, VerifySalesCommandService>();
            serviceCollection.AddScoped<IVerifyItemsCommandService, VerifyItemsCommandService>();
            serviceCollection.AddScoped<IVerifyTablesCommandService, VerifyTablesCommandService>();


            //Login
            serviceCollection.AddTransient<IHandler<LoginCommand, LoginReturn>, LoginCommandHandler>();

            //Correlation
            serviceCollection.AddTransient<IHandler<CorrelationCommand, CorrelationReturn>, CorrelationHandler>();

            //Clustering
            serviceCollection.AddTransient<IHandler<Clustering_TimeOfVisit_TotalPrice_Command, ClusteringReturn>, Clustering_TimeOfVisitVSTotalPrice>();
            serviceCollection.AddTransient<IHandler<Clustering_TimeOfVisit_LengthOfVisit_Command, ClusteringReturn>, Clustering_TimeOfVisitVSLengthOfVisit>();

            //Establishment

            serviceCollection.AddTransient<IHandler<GetEstablishmentsCommand, GetEstablishmentsIdReturn>, GetMultipleEstablishmentsHandler<GetEstablishmentsIdReturn>>();
            serviceCollection.AddTransient<IHandler<GetEstablishmentsCommand, GetEstablishmentsEntityReturn>, GetMultipleEstablishmentsHandler<GetEstablishmentsEntityReturn>>();
            serviceCollection.AddTransient<IHandler<GetEstablishmentsCommand, GetEstablishmentsDTOReturn>, GetMultipleEstablishmentsHandler<GetEstablishmentsDTOReturn>>();

            //Sale
            serviceCollection.AddTransient<IHandler<GetSalesCommand, GetSalesReturn>, GetSalesHandler<GetSalesReturn>>();
            serviceCollection.AddTransient<IHandler<GetSalesCommand, GetSalesRawReturn>, GetSalesHandler<GetSalesRawReturn>>();
            serviceCollection.AddTransient<IHandler<GetSalesCommand, GetSalesDTOReturn>, GetSalesHandler<GetSalesDTOReturn>>();

            //Item
            serviceCollection.AddTransient<IHandler<GetItemsCommand, GetItemsIdReturn>, GetItemsHandler<GetItemsIdReturn>>();
            serviceCollection.AddTransient<IHandler<GetItemsCommand, GetItemsEntityReturn>, GetItemsHandler<GetItemsEntityReturn>>();
            serviceCollection.AddTransient<IHandler<GetItemsCommand, GetItemsDTOReturn>, GetItemsHandler<GetItemsDTOReturn>>();







        }
    }
}