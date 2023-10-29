using WebApplication1.CommandHandlers;
using WebApplication1.Commands;
using WebApplication1.Data;
using WebApplication1.Middelware;
using WebApplication1.Repositories;

namespace WebApplication1.Services
{
    public static class ServiceCollectionExtension
    {
        public static void AddServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IAuthService, AuthService>();
            serviceCollection.AddScoped<IUserContextService, UserContextService>();
            serviceCollection.AddScoped<UserContextMiddleware>();

            serviceCollection.AddScoped<IDatabaseContext>(provider =>  provider.GetRequiredService<ApplicationDbContext>());

        }
        public static void AddRepositories(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IEstablishmentRepository, EstablishmentRepository>();
            serviceCollection.AddScoped<IUserRepository, UserRepository>();
            serviceCollection.AddScoped<IUserRolesRepository, UserRolesRepository>();

            serviceCollection.AddScoped<ISalesRepository, SalesRepository>();
        }

        public static void AddCommandHandlers(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ICommandHandler<LoginCommand, string>, LoginCommandHandler>();
        }
    }
}