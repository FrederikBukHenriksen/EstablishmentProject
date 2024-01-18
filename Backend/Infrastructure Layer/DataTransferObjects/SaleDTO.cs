﻿using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Infrastructure_Layer.DataTransferObjects
{
    public class SaleDTO
    {
        public Guid id { get; set; }
        public SaleType? SaleType { get; set; } = null;
        public PaymentType? PaymentType { get; set; } = null;
        public DateTime? TimestampArrival { get; set; } = null;
        public DateTime TimestampPayment { get; set; }
        public List<Guid> SalesItems { get; set; } = new List<Guid>();
        public Guid? Table { get; set; } = null;
        public Guid? Employee { get; set; } = null;

        public SaleDTO(Sale sale)
        {
            this.id = sale.Id;
            this.SaleType = sale.SaleType;
            this.PaymentType = sale.PaymentType;
            this.TimestampArrival = sale.TimestampArrival;
            this.TimestampPayment = sale.TimestampPayment;
            this.SalesItems = sale.SalesItems.Select(salesItem => salesItem.Id).ToList();
            this.Table = sale.Table?.Id;
            this.Employee = sale.Employee?.Id;
        }
    }
}