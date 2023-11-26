using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Repositories;
using WebApplication1.Services;

namespace WebApplication1.CommandHandlers
{
    public class GetProductSalesPerDayQuery : CommandBase
    {
        public Guid ItemId { get; set; }
        public TimeResolution Resolution { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
    public class GraphDTO
    {
        public ICollection<TimeAndValue>? values { get; set; }
    }

    public class TimeAndValue
    {
        public DateTime Date { get; set; }
        public int SalesCount { get; set; }
    }

    public enum TimeResolution
    {
        Hour,
        Date,
        Week,
        Month,
        Year,
    }

    public class GetProductSalesChartQueryHandler : HandlerBase<GetProductSalesPerDayQuery, GraphDTO>
    {
        private readonly IEstablishmentRepository establishmentRepository;
        private readonly ISalesRepository salesRepository;
        private readonly IUserContextService userContextService;

        public GetProductSalesChartQueryHandler(IEstablishmentRepository establishmentRepository,ISalesRepository salesRepository, IUserContextService userContextService)
        {
            this.establishmentRepository = establishmentRepository;
            this.salesRepository = salesRepository;
            this.userContextService = userContextService;
        }   

        public override GraphDTO Execute(GetProductSalesPerDayQuery command)
        {
            Establishment? activeEstablishment = this.userContextService.GetActiveEstablishment();

            var sales = establishmentRepository.GetSales(activeEstablishment.Id) ;

            IEnumerable<Sale> salesWithinTimePeriod = sales.Where(x => x.TimestampStart >= command.EndDate);

            IEnumerable<IGrouping<DateTime, Sale>> salesGroupedByTimeSlots = salesWithinTimePeriod.GroupBy(x => x.TimestampEnd.Date);

            //Create timeline
            List<DateTime> timeline = new List<DateTime>();

            Func<DateTime,DateTime> res = x => {
                switch (command.Resolution)
                {
                    case TimeResolution.Hour:
                        return x.AddHours(1);
                    case TimeResolution.Date:
                        return x.AddDays(1);
                    case TimeResolution.Month:
                        return x.AddMonths(1);
                    case TimeResolution.Year:
                        return x.AddYears(1);
                    default:
                        return x;
                }
            };

            for (DateTime date = command.StartDate; date <= command.EndDate; date = res(date))
            {
                timeline.Add(date);
            }

            //Map every sale of item onto dateInRange
            List<TimeAndValue> salesPerDay = new List<TimeAndValue>();
            foreach (DateTime date in timeline)
            {
                int salesOnDate = salesGroupedByTimeSlots.Where(x => x.Key == date).Count();
                salesPerDay.Add(new TimeAndValue { Date = date, SalesCount = salesOnDate});
            }
            return new GraphDTO() { values = salesPerDay };
        }
    }
}