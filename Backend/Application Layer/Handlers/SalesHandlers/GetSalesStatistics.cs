﻿using Microsoft.IdentityModel.Tokens;
using NJsonSchema.NewtonsoftJson.Converters;
using System.Runtime.Serialization;
using WebApplication1.Application_Layer.Services;
using WebApplication1.CommandHandlers;
using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Utils;

namespace WebApplication1.Application_Layer.Handlers.SalesHandlers
{
    [Newtonsoft.Json.JsonConverter(typeof(JsonInheritanceConverter), "$type")]
    [KnownType(typeof(GetSalesAverageSpend))]
    [KnownType(typeof(GetSalesAverageNumberOfItems))]
    [KnownType(typeof(GetSalesAverageTimeOfPayment))]
    [KnownType(typeof(GetSalesAverageTimeOfArrival))]
    [KnownType(typeof(GetSalesAverageSeatTime))]

    public abstract class GetSalesStatisticsCommand : CommandBase, ICmdField_SalesIds
    {
        public Guid EstablishmentId { get; set; }
        public List<Guid> SalesIds { get; set; } = new List<Guid>();

        public abstract double Caldulate(List<Sale> sales);
    }

    public class GetSalesAverageSpend : GetSalesStatisticsCommand
    {
        public override double Caldulate(List<Sale> sales)
        {
            return sales.Average(x => x.GetTotalPrice());
        }
    }

    public class GetSalesAverageNumberOfItems : GetSalesStatisticsCommand
    {
        public override double Caldulate(List<Sale> sales)
        {
            return sales.Average(x => x.GetNumberOfSoldItems());
        }
    }

    public class GetSalesAverageTimeOfPayment : GetSalesStatisticsCommand
    {
        public override double Caldulate(List<Sale> sales)
        {
            return sales.Average(x => (double)x.GetTimeOfPayment().TimeOfDay.Minutes);
        }
    }

    public class GetSalesAverageTimeOfArrival : GetSalesStatisticsCommand
    {
        public override double Caldulate(List<Sale> sales)
        {
            var SalesSorting = new SalesSorting(mustContainAllAttributes: new List<SaleAttributes> { SaleAttributes.TimestampArrival });
            List<Sale> SalesWithTimeOfArrival = SalesSortingParametersExecute.SortSales(sales, SalesSorting);
            if (SalesWithTimeOfArrival.IsNullOrEmpty())
            {
                throw new Exception("No sales with time of arrival");
            }
            return SalesWithTimeOfArrival.Average(x => (double)x.GetTimeOfArrival()?.TimeOfDay.Minutes);
        }
    }

    public class GetSalesAverageSeatTime : GetSalesStatisticsCommand
    {
        public override double Caldulate(List<Sale> sales)
        {
            var SalesSorting = new SalesSorting(mustContainAllAttributes: new List<SaleAttributes> { SaleAttributes.TimestampArrival, SaleAttributes.TimestampPayment });
            List<Sale> SalesWithTimeOfArrival = SalesSortingParametersExecute.SortSales(sales, SalesSorting);
            if (SalesWithTimeOfArrival.IsNullOrEmpty())
            {
                throw new Exception("No sales with a seattime");
            }
            return SalesWithTimeOfArrival.Average(x => (double)(x.GetTimeOfPayment().TimeOfDay.Minutes - (int)x.GetTimeOfArrival()?.TimeOfDay.Minutes));
        }
    }

    public class GetSalesStatisticsReturn : ReturnBase
    {
        public double metric;
    }



    public class GetSalesStatistics : HandlerBase<GetSalesStatisticsCommand, GetSalesStatisticsReturn>
    {
        private IUnitOfWork unitOfWork;

        public GetSalesStatistics(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async override Task<GetSalesStatisticsReturn> Handle(GetSalesStatisticsCommand command)
        {
            Establishment establishment = this.unitOfWork.establishmentRepository.IncludeSales().IncludeSalesItems().GetById(command.EstablishmentId)!;
            List<Sale> sales = establishment.GetSales();
            var metric = command.Caldulate(sales);
            return new GetSalesStatisticsReturn { metric = metric };
        }
    }
}