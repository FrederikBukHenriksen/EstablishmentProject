using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data.DataModels;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Domain_Layer.Services.Repositories;

namespace WebApplication1.Domain_Layer.Services.Entity_builders
{
    public interface ISaleBuilder : IEntityBuilder<Sale>
    {
        ISaleBuilder WithTimestampArrival(DateTime? timestampArrival);
        ISaleBuilder WithTimestampPayment(DateTime timestampPayment);
        ISaleBuilder WithSoldItems(List<(Item item, int quantity)> itemsAndQuantities);
        ISaleBuilder WithTable(Table table);
    }

    public class SaleBuilder : EntityBuilderBase<Sale>, ISaleBuilder
    {
        private IEstablishmentRepository itemRepository;

        private DateTime? builderTimestampArrival = null;
        private DateTime? builderTimestampPayment = null;
        private List<SalesItems> builderSalesItems = null;
        private Table? builderTable = null;
        public SaleBuilder([FromServices] IEstablishmentRepository itemRepository)
        {
            this.itemRepository = itemRepository;
        }



        public ISaleBuilder WithTimestampArrival(DateTime? timestampArrival)
        {
            this.builderTimestampArrival = timestampArrival;
            return this;
        }

        public ISaleBuilder WithTimestampPayment(DateTime timestampPayment)
        {
            this.builderTimestampPayment = timestampPayment;
            return this;
        }

        public ISaleBuilder WithTable(Table table)
        {
            this.builderTable = table;
            return this;
        }

        public ISaleBuilder WithSoldItems(List<(Item item, int quantity)> itemsAndQuantities)
        {
            this.builderSalesItems = itemsAndQuantities.Select(x => new SalesItems(x.item, x.quantity)).ToList();
            return this;
        }

        public override Sale Build()
        {
            return new Sale(timestampPayment: (DateTime)this.builderTimestampPayment, salesItems: this.builderSalesItems, timestampArrival: this.builderTimestampArrival, table: this.builderTable);

        }
    }
}
