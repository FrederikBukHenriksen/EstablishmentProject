using WebApplication1.Application_Layer.Objects;
using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Domain.Entities;
using WebApplication1.Domain.Services.Repositories;
using WebApplication1.Services;
using WebApplication1.Utils;

namespace WebApplication1.CommandHandlers
{
    public class MeanSales
    {
        public class MeanItemSalesCommand: CommandBase
        {
            public List<Guid> MustContaiedItems { get; set; } = new List<Guid>();
            public List<TimePeriod> UseDataFromTimeframePeriods { get; set; } = new List<TimePeriod>();
            public TimeResolution TimeResolution { get; set; }
            public TimePeriod Timeline { get; set; }
        }

        public class MeanItemSalesReturn : ReturnBase
        {
            public List<TimeAndValue<double?>> Data { get; set; } = new List<TimeAndValue<double?>>();
        }

        public class MeanItemSalesHandler : HandlerBase<MeanItemSalesCommand, MeanItemSalesReturn>
        {
            private IUserContextService userContextService;
            private IEstablishmentRepository establishmentRepository;

            public MeanItemSalesHandler(IUserContextService userContextService, IEstablishmentRepository establishmentRepository)
            {
                this.userContextService = userContextService;
                this.establishmentRepository = establishmentRepository;
            }

            public override MeanItemSalesReturn Handle(MeanItemSalesCommand command)
            {
                Establishment activeEstablishment = userContextService.GetActiveEstablishment();

                List<Sale> sales = establishmentRepository.GetEstablishmentSales(userContextService.GetActiveEstablishment().Id).ToList();

                //Sort by time periods
                sales.SortSalesByTimePeriods(command.UseDataFromTimeframePeriods);

                //Sort by items
                var establishmentItems = establishmentRepository.GetEstablishmentItems(activeEstablishment.Id).ToList();
                var mustBeConatineditems = establishmentItems.Where(x => command.MustContaiedItems.Contains(x.Id)).ToList();
                sales.SortSalesByRequiredConatinedItems(mustBeConatineditems);

                //Group by time resolution
                List<(int dateTimeIdentifier, List<List<Sale>> listsOfSales)> groupedByDay = GraphHelper.TimeResolutionGroup(sales,command.TimeResolution, x => x.TimestampPayment);

                //Calculate average per day
                //List<(int dateTimeIdentifier, double average)> DateTimeIdentifierAndAverage = groupedByDay.Select(x => (x.dateTimeIdentifier, x.listsOfSales.Average(y => y.Count))).ToList();
                List<(int dateTimeIdentifier, double average)> DateTimeIdentifierAndAverage = groupedByDay.Select(x => (x.dateTimeIdentifier, GraphHelper.average(x.listsOfSales.Select(y => (double) y.Count)))).ToList();


                //Create timeline
                List<DateTime> timeline = TimeHelper.CreateTimeline(command.Timeline.Start, command.Timeline.End, command.TimeResolution);

                //Map the average results onto the timeline
                List<TimeAndValue<double?>> res = new List<TimeAndValue<double?>>();

                foreach (var time in timeline)
                {
                    var timelineDateTimeIdentifier = TimeHelper.PlainIdentifierBasedOnTimeResolution(time, command.TimeResolution);

                    foreach (var (dateTimeIdentifier, average) in DateTimeIdentifierAndAverage)
                    {
                        if (dateTimeIdentifier == timelineDateTimeIdentifier)
                        {
                            res.Add(new TimeAndValue<double?> { DateTime = time, Value = average });
                        }
                        else
                        {
                            res.Add(new TimeAndValue<double?> { DateTime = time, Value = null });
                        }
                    }
                }
                return new MeanItemSalesReturn { Data = res };
            }
        }
    }
}
