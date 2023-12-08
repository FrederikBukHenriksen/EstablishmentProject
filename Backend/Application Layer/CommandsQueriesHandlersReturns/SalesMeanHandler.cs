﻿using System.Runtime.Serialization;
using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Domain.Entities;
using WebApplication1.Domain.Services.Repositories;
using WebApplication1.Services;
using WebApplication1.Utils;

namespace WebApplication1.CommandHandlers
{


    [KnownType(typeof(SalesMeanOverTimeAverageSpend))]
    [KnownType(typeof(SalesMeanOverTimeAverageNumberOfSales))]

    public class SalesMeanOverTime : CommandBase
    {
        public SalesSortingParameters? salesSortingParameters { get; set; }
        public TimeResolution TimeResolution { get; set; }
        public List<(int timeResolutionIdentifer, double averageValue)> CalcuateAverageOverTime(List<Sale> sales)
        { return new List<(int timeResolutionIdentifer, double averageValue)> { };}
    }

    public class SalesMeanOverTimeAverageSpend : SalesMeanOverTime
    {
        public new List<(int timeResolutionIdentifer, double averageValue)> CalcuateAverageOverTime(List<Sale> sales)
        {
            IEnumerable<IGrouping<int, Sale>> groupedSales = sales.GroupBy(x => TimeHelper.PlainIdentifierBasedOnTimeResolution(x.TimestampPayment, TimeResolution));
            return groupedSales.Select(x => (x.Key, x.Average(x => x.GetTotalPrice()))).ToList();
        }
    }

    public class SalesMeanOverTimeAverageNumberOfSales : SalesMeanOverTime
    {
        public new List<(int timeResolutionIdentifer, double averageValue)> CalcuateAverageOverTime(List<Sale> sales)
        {
            IEnumerable<IGrouping<DateTime, Sale>> GroupedOnTimeResolutionUnique = sales.GroupBy(x => TimeHelper.GroupForAverage(x.TimestampPayment, TimeResolution));
            List<(DateTime, double)> ok = GroupedOnTimeResolutionUnique.Select(x => (x.Key, (double) x.Count())).ToList();
            IEnumerable<IGrouping<int, (DateTime, double)>> secondGrouping = ok.GroupBy(x => TimeHelper.PlainIdentifierBasedOnTimeResolution(x.Item1, TimeResolution));
            List<(int Key, double)> ok2 = secondGrouping.Select(x => (x.Key, x.Average(x => x.Item2))).ToList();
            return ok2;
        }
    }

    public class SalesMeanQueryReturn : ReturnBase
    {
        public List<(int,double?)> Data { get; set; }
    }

    public class SalesMeanOverTimeQueryHandler : HandlerBase<SalesMeanOverTime, SalesMeanQueryReturn>
    {
        private IEstablishmentRepository establishmentRepository;
        private ISalesRepository salesRepository;
        private IUserContextService userContextService;

        public SalesMeanOverTimeQueryHandler(IEstablishmentRepository establishmentRepository, IUserContextService userContextService, ISalesRepository salesRepository)
        {
            this.establishmentRepository = establishmentRepository;
            this.salesRepository = salesRepository;
            this.userContextService = userContextService;
        }   

        public override SalesMeanQueryReturn Handle(SalesMeanOverTime command)
        {
            //Test
            var testFunction = command.CalcuateAverageOverTime;


            //Fetch
            Establishment activeEstablishment = userContextService.GetActiveEstablishment();

            List<Sale> sales = establishmentRepository.GetEstablishmentSales(activeEstablishment.Id).ToList();
            sales = salesRepository.IncludeSalesItems(sales);

            if (command.salesSortingParameters != null) sales = SalesSortingParametersExecute.SortSales(sales, command.salesSortingParameters);

            //Act
            List<(int timeResolutionIdentifer, double averageValue)> avergageData = command.CalcuateAverageOverTime(sales);

            //Return
            List<(int Key, double? Value)> dictionaryToList = TimeHelper.MapValuesWithDateToTimeResolutionTimeline(avergageData, command.TimeResolution);

            return new SalesMeanQueryReturn { Data = dictionaryToList };
        }


    }
}