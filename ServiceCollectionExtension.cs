using WebApplication1.Repositories;
using WebApplication1.Services;

namespace WebApplication1
{
    public static class ServiceCollectionExtension
    {

        public static void AddServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IAuthenticationService, AuthenticationService>();
        }
        public static void AddRepositories(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IEstablishmentRepository, EstablishmentRepository>();
            serviceCollection.AddScoped<ILocationRepository, LocationRepository>();
        }
    }
}
