using WebApplication1.Domain.Entities;
using WebApplication1.Domain.Services.Repositories;

namespace WebApplication1.Domain_Layer.Services.Entity_builders
{
    public class FactoryServiceBuilder
    {
        private IServiceProvider serviceProvider;

        public FactoryServiceBuilder(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IEstablishmentBuilder Establishment()
        {
            return serviceProvider.GetRequiredService<IEstablishmentBuilder>();
        }

        public ISaleBuilder Sale()
        {
            return serviceProvider.GetRequiredService<ISaleBuilder>();
        }



    }
}
