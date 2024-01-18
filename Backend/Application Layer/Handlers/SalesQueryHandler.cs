﻿//using WebApplication1.CommandsHandlersReturns;
//using WebApplication1.Domain_Layer.Entities;
//using WebApplication1.Domain_Layer.Services.Repositories;
//using WebApplication1.Services;
//using WebApplication1.Utils;

//namespace WebApplication1.CommandHandlers
//{
//    public class SalesQuery : CommandBase
//    {
//        public SalesSorting? salesSortingParameters { get; set; }
//        public TimeResolution TimeResolution { get; set; }
//    }

//    public class SalesQueryReturn : ReturnBase
//    {
//        public Dictionary<DateTime, int> data { get; set; }
//    }

//    public class SalesQueryHandler : HandlerBase<SalesQuery, SalesQueryReturn>
//    {
//        private IEstablishmentRepository establishmentRepository;
//        private ISalesRepository salesRepository;
//        private IUserContextService userContextService;

//        public SalesQueryHandler(IEstablishmentRepository establishmentRepository, IUserContextService userContextService, ISalesRepository salesRepository)
//        {
//            this.establishmentRepository = establishmentRepository;
//            this.salesRepository = salesRepository;
//            this.userContextService = userContextService;
//        }

//        public async override Task<SalesQueryReturn> Handle(SalesQuery command)
//        {
//            Establishment activeEstablishment = this.userContextService.GetActiveEstablishment();

//            List<Sale> sales = this.establishmentRepository.GetEstablishmentSales(this.userContextService.GetActiveEstablishment().Id).ToList();
//            sales = this.salesRepository.IncludeSalesItems(sales);

//            //Sort by items
//            if (command.salesSortingParameters != null) sales = SalesSortingParametersExecute.SortSales(sales, command.salesSortingParameters);

//            var salesGrouped = sales.GroupBy(x => TimeHelper.TimeResolutionUniqueRounder(x.TimestampPayment, command.TimeResolution));
//            var salesPerTimeResolution = salesGrouped.Select(x => (x.Key, x.Count())).ToList();


//            //List<(DateTime, IEnumerable<Sale?>)> timelineWithSales = TimeHelper.mapToATimeline(sales, x => x.GetTimeOfSale(), command.TimePeriods, command.TimeResolution).ToList();
//            //List<TimeAndValue<int>> res = timelineWithSales.Select(x => new TimeAndValue<int> { dateTime = x.Item1, value = x.Item2.Count() }).ToList();
//            return new SalesQueryReturn { data = salesPerTimeResolution.ToDictionary(x => x.Key, x => x.Item2) };

//            //return new SalesQueryReturn { Sales = sales.Select(x => new Sale()).ToList() };
//        }
//    }
//}