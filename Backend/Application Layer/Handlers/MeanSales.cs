﻿using WebApplication1.Application_Layer.Objects;
using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Domain_Layer.Services.Repositories;
using WebApplication1.Services;
using WebApplication1.Utils;

namespace WebApplication1.CommandHandlers
{
    public class MeanSales
    {
        public class MeanSalesCommand : CommandBase
        {
            public SalesSortingParameters Sorter { get; set; } = new SalesSortingParameters();
            //public List<Guid> MustContaiedItems { get; set; } = new List<Guid>();
            //public List<TimePeriod> UseDataFromTimeframePeriods { get; set; } = new List<TimePeriod>();
            public TimeResolution TimeResolution { get; set; }
            public DateTimePeriod Timeline { get; set; }
            //public MeanShiftClusteringAttributes MeanAttributes { get; set; }
        }

        public class MeanSalesReturn : ReturnBase
        {
            public List<TimeAndValue<double?>> Data { get; set; } = new List<TimeAndValue<double?>>();
        }

        public class MeanSalesHandler : HandlerBase<MeanSalesCommand, MeanSalesReturn>
        {
            private IUserContextService userContextService;
            private IEstablishmentRepository establishmentRepository;
            private ISalesRepository salesRepository;

            public enum MeanAttributes
            {
                AverageNumberOfSales,
                AveragePriceOfSale,
            }

            private Dictionary<MeanAttributes, Func<List<List<Sale>>, double>> meanAtributeDictionary = new Dictionary<MeanAttributes, Func<List<List<Sale>>, double>> {
                { MeanAttributes.AverageNumberOfSales,
                    sales => (double) sales.Select(x => x.Count()).Average()
                },
                { MeanAttributes.AveragePriceOfSale,
                    sales =>  sales.SelectMany(sublist => sublist.Select(sale => sale.GetTotalPrice())).Average()
                },
            };


            public MeanSalesHandler(IUserContextService userContextService, IEstablishmentRepository establishmentRepository, ISalesRepository salesRepository)
            {
                this.userContextService = userContextService;
                this.establishmentRepository = establishmentRepository;
                this.salesRepository = salesRepository;
            }

            public override async Task<MeanSalesReturn> Handle(MeanSalesCommand command)
            {
                Establishment activeEstablishment = this.userContextService.GetActiveEstablishment();

                List<Sale> sales = this.establishmentRepository.GetEstablishmentSales(this.userContextService.GetActiveEstablishment().Id).ToList();
                sales = this.salesRepository.IncludeSalesItems(sales);
                //sales.Add(TestDataFactory.CreateSale(timestampEnd: DateTime.Now.AddYears(-1).AddDays(-1)));
                //sales.Add(TestDataFactory.CreateSale(timestampEnd: DateTime.Now.AddYears(-1).AddDays(-1)));
                //sales.Add(TestDataFactory.CreateSale(timestampEnd: DateTime.Now.AddYears(-1).AddDays(-1)));
                //sales.Add(TestDataFactory.CreateSale(timestampEnd: DateTime.Now.AddYears(-1).AddDays(-1)));

                //Sort by items
                if (command.Sorter != null)
                {
                    sales = SalesSortingParametersExecute.SortSales(sales, command.Sorter);
                }

                //Group by time resolution
                List<(int dateTimeIdentifier, List<List<Sale>> listsOfSales)> groupedByDay = GraphHelper.TimeResolutionGroup(sales, command.TimeResolution, x => x.GetTimeOfSale());

                Func<List<List<Sale>>, double> meanMethod = sales => (double)sales.Select(x => x.Count()).Average();

                List<(int dateTimeIdentifier, double values)> averageApplied = groupedByDay
                    .Select(x =>
                    (x.dateTimeIdentifier,
                    meanMethod(x.listsOfSales)))
                    .ToList();

                //Create timeline
                List<DateTime> timeline = TimeHelper.CreateTimelineAsList(command.Timeline, command.TimeResolution);

                //Map the average results onto the timeline
                List<TimeAndValue<double?>> res = new List<TimeAndValue<double?>>();

                foreach (var time in timeline)
                {
                    var timelineDateTimeIdentifier = TimeHelper.PlainIdentifierBasedOnTimeResolution(time, command.TimeResolution);

                    var find = averageApplied.Any(x => x.dateTimeIdentifier == timelineDateTimeIdentifier);
                    //foreach (var (dateTimeIdentifier, average) in averageApplied)
                    //{
                    if (find)
                    //if (dateTimeIdentifier == timelineDateTimeIdentifier)
                    {
                        res.Add(new TimeAndValue<double?> { dateTime = time, value = (double?)averageApplied.Find(x => x.dateTimeIdentifier == timelineDateTimeIdentifier).values });
                    }
                    else
                    {
                        res.Add(new TimeAndValue<double?> { dateTime = time, value = null });
                    }
                    //}
                }
                return new MeanSalesReturn { Data = res };
            }



        }
    }
}
