using WebApplication1.CommandHandlers;
using WebApplication1.Commands;
using WebApplication1.Repositories;
using WebApplication1.Services;

namespace WebApplication1
{
    public static class ServiceCollectionExtension
    {
        public static void AddServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IAuthService, AuthService>();
        }
        public static void AddRepositories(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IEstablishmentRepository, EstablishmentRepository>();
            serviceCollection.AddScoped<ILocationRepository, LocationRepository>();
            serviceCollection.AddScoped<IUserRepository, UserRepository>();
        }

        public static void AddCommandHandlers(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ICommandHandler<LoginCommand, string>, LoginCommandHandler>();
        }
    }
}
