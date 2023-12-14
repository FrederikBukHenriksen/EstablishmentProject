using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using WebApplication1.Data.DataModels;
using WebApplication1.Domain.Entities;
using WebApplication1.Domain.Services.Repositories;

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

        public override void ReadPropertiesOfEntity(Sale entity)
        {
            this.builderTimestampArrival = entity.TimestampArrival;
            this.builderTimestampPayment = entity.TimestampPayment;
            this.builderSalesItems = entity.SalesItems;
            this.builderTable = entity.Table;
        }

        public override void WritePropertiesOfEntity(Sale Entity)
        {
            Entity.TimestampArrival = (DateTime?)this.builderTimestampArrival;
            Entity.TimestampPayment = (DateTime)this.builderTimestampPayment;
            Entity.SalesItems = (List<SalesItems>)this.builderSalesItems;
            Entity.Table = (Table)this.builderTable;
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
            var projectedToSalesItems = itemsAndQuantities.Select(x => new SalesItemsBuilder().WithSale(this.Entity).WithItem(x.item).WithQuantity(x.quantity).Build());
            this.builderSalesItems = projectedToSalesItems.ToList();
            return this;
        }

        public override bool Validation()
        {
            if (!this.DoesSaleHavePaymentTimestamp()) throw new System.Exception("Sale must have a timestamp for payment");
            return true;
        }

        private bool DoesSaleHavePaymentTimestamp()
        {
            return builderTimestampPayment != null;
        }


    }
}
