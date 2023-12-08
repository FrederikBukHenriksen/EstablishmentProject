using Microsoft.AspNetCore.Mvc;
using WebApplication1.Domain.Services.Repositories;
using WebApplication1.Domain_Layer.Services.Entity_builders;

    namespace WebApplication1.Domain.Entities
    {
        public interface IEstablishmentBuilder : IEntityBuilder<Establishment>
        {
        IEstablishmentBuilder WithId(Guid id);
        IEstablishmentBuilder WithName(string name);
        IEstablishmentBuilder WithLocation(Location location);
        IEstablishmentBuilder WithItems(ICollection<Item> items);
        IEstablishmentBuilder WithTables(ICollection<Table> tables);
        IEstablishmentBuilder WithSales(ICollection<Sale> sales);
        IEstablishmentBuilder UseEntity(Establishment entity);
    }

        public class EstablishmentBuilder : EntityBuilderBase<Establishment>, IEstablishmentBuilder
        {
        private IEstablishmentRepository establishmentBuilder;

        public EstablishmentBuilder([FromServices] IEstablishmentRepository establishmentBuilder)
        {
            this.establishmentBuilder = establishmentBuilder;
        }
        public IEstablishmentBuilder WithId(Guid id)
        {
            Entity.Id = id;
            return this;
        }

        public IEstablishmentBuilder WithName(string name)
        {
            Entity.Name = name;
            return this;
        }

        public IEstablishmentBuilder WithLocation(Location location)
        {
            Entity.Location = location;
            return this;
        }

        public IEstablishmentBuilder WithItems(ICollection<Item> items)
        {
            Entity.Items = items;
            return this;
        }

        public IEstablishmentBuilder WithTables(ICollection<Table> tables)
        {
            Entity.Tables = tables;
            return this;
        }

        public IEstablishmentBuilder WithSales(ICollection<Sale> sales)
        {
            Entity.Sales = sales;
            return this;
        }

        public IEstablishmentBuilder UseEntity(Establishment entity)
        {
            Entity = entity;
            return this;
        }


        //public override IEntityBuilder<Establishment> CreateNewEntity()
        //{
        //    throw new NotImplementedException();
        //}
    }
}

