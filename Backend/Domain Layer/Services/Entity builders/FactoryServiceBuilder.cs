using WebApplication1.Domain.Entities;
using WebApplication1.Domain.Services.Repositories;

namespace WebApplication1.Domain_Layer.Services.Entity_builders
{
    public class FactoryServiceBuilder
    {
        private IServiceProvider serviceProvider;

        public IEstablishmentBuilder EstablishmentBuilder
        {
            get { return serviceProvider.GetRequiredService<IEstablishmentBuilder>(); }
        }

        public ISaleBuilder SaleBuilder
        {
            get { return serviceProvider.GetRequiredService<ISaleBuilder>(); }
        }

        public IItemBuilder ItemBuilder
        {
            get { return serviceProvider.GetRequiredService<IItemBuilder>(); }
        }

        public FactoryServiceBuilder(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }





    }
}
