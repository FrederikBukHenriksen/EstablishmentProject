﻿using Microsoft.AspNetCore.Mvc;
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

        public SaleBuilder([FromServices] IEstablishmentRepository itemRepository)
        {
            this.itemRepository = itemRepository;
        }
        public ISaleBuilder WithTimestampArrival(DateTime? timestampArrival)
        {
            Entity.TimestampArrival = timestampArrival;
            return this;
        }

        public ISaleBuilder WithTimestampPayment(DateTime timestampPayment)
        {
            Entity.TimestampPayment = timestampPayment;
            return this;
        }

        public ISaleBuilder WithTable(Table table)
        {
            Entity.Table = table;
            return this;
        }

        public ISaleBuilder WithSoldItems(List<(Item item, int quantity)> itemsAndQuantities)
        {
            var projectedToSalesItems = itemsAndQuantities.Select(x => new SalesItemsBuilder().WithSale(this.Entity).WithItem(x.item).WithQuantity(x.quantity).Build());
            Entity.SalesItems = projectedToSalesItems.ToList();
            return this;
        }

        public ISaleBuilder WithSoldItem((Item item, int quantity) itemAndQuantity)
        {
            var projectedToSalesItems = new SalesItemsBuilder().WithSale(this.Entity).WithItem(itemAndQuantity.item).WithQuantity(itemAndQuantity.quantity).Build();
            Entity.SalesItems.Add(projectedToSalesItems);
            return this;
        }

        public override ISaleBuilder UseExistingEntity(Sale entity)
        {
            Entity = entity;
            return this;
        }

        //public override ISaleBuilder CreateNewEntity()
        //{
        //    Entity = new Sale();
        //    return this;
        //}
    }
}
