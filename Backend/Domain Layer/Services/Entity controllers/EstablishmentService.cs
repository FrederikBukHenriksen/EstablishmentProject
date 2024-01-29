using WebApplication1.Application_Layer.Services.Entity_controllers;
using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Domain_Layer.Services.Entity_controllers
{
    public interface IEstablishmentService
    {
        public IEstablishmentService AddItem(Item item);
        public IEstablishmentService RemoveItem(Item item);
        public IEstablishmentService AddTable(Table table);
        public IEstablishmentService RemoveTable(Table table);
        public IEstablishmentService AddSale(Sale sale);
        public IEstablishmentService RemoveSale(Sale sale);
    }

    public class EstablishmentService : ControllerServiceBase<Establishment>, IEstablishmentService
    {
        public EstablishmentService()
        {
        }

        public IEstablishmentService AddItem(Item item)
        {
            //this.Entity?.AddItem(item);
            return this;
        }

        public IEstablishmentService AddSale(Sale sale)
        {
            //this.Entity?.AddSale(sale);
            return this;
        }

        public IEstablishmentService AddTable(Table table)
        {
            //this.Entity?.AddTable(table);
            return this;
        }

        public IEstablishmentService RemoveItem(Item item)
        {
            this.Entity?.RemoveItem(item);
            return this;
        }

        public IEstablishmentService RemoveSale(Sale sale)
        {
            this.Entity?.RemoveSale(sale);
            return this;
        }

        public IEstablishmentService RemoveTable(Table table)
        {
            this.Entity?.RemoveTable(table);
            return this;
        }
    }
}
