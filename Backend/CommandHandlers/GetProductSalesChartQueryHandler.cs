using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebApplication1.Commands;
using WebApplication1.Repositories;
using WebApplication1.Services;

namespace WebApplication1.CommandHandlers
{
    public class GetProductSalesPerDayQuery : ACommand
    {
        public Guid ItemId { get; set; }
        public TimeResolution Resolution { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
    public class ProductSalesPerDayDTO
    {
        public ICollection<SalesAndTimeSlotDTO>? values { get; set; }
    }

    public class SalesAndTimeSlotDTO
    {
        public DateTime Date { get; set; }
        public int SalesCount { get; set; }
    }

    public enum TimeResolution
    {
        quarterHour,
        halfHour,
        hour,
        day,
        month,
        year,
    }

    public class GetProductSalesChartQueryHandler : CommandHandlerBase<GetProductSalesPerDayQuery, ProductSalesPerDayDTO>
    {
        private readonly ISalesRepository salesRepository;
        private readonly IUserContextService userContextService;

        public GetProductSalesChartQueryHandler(ISalesRepository salesRepository, IUserContextService userContextService)
        {
            this.salesRepository = salesRepository;
            this.userContextService = userContextService;
        }   

        public override ProductSalesPerDayDTO Execute(GetProductSalesPerDayQuery command)
        {
            Establishment? Establishment = this.userContextService.GetActiveEstablishment();

            IEnumerable<Sale> sales = this.salesRepository.GetAll().Where(x => x.Establishment.Id == Establishment.Id);

            IEnumerable<Sale> salesWithinTimePeriod = sales.Where(x => x.TimestampStart >= command.StartDate && x.TimestampStart <= command.EndDate);

            IEnumerable<IGrouping<DateTime, Sale>> salesGroupedByTimeSlots = salesWithinTimePeriod.GroupBy(x => x.TimestampEnd.Date);


            //Create timeline
            List<DateTime> timeline = new List<DateTime>();

            Func<DateTime,DateTime> res = x => {
                switch (command.Resolution)
                {
                    case TimeResolution.quarterHour:
                        return x.AddMinutes(15);
                    case TimeResolution.halfHour:
                        return x.AddMinutes(30);
                    case TimeResolution.hour:
                        return x.AddHours(1);
                    case TimeResolution.day:
                        return x.AddDays(1);
                    case TimeResolution.month:
                        return x.AddMonths(1);
                    case TimeResolution.year:
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
            List<SalesAndTimeSlotDTO> salesPerDay = new List<SalesAndTimeSlotDTO>();
            foreach (DateTime date in timeline)
            {
                int salesOnDate = salesGroupedByTimeSlots.Where(x => x.Key == date).Count();
                salesPerDay.Add(new SalesAndTimeSlotDTO { Date = date, SalesCount = salesOnDate});
            }
            return new ProductSalesPerDayDTO() { values = salesPerDay };
        }
    }
}