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
        public ICollection<(DateTime, int)>? values = null;
    }

    public enum TimeResolution
    {
        quarter,
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

        public override async Task<ProductSalesPerDayDTO> ExecuteAsync(GetProductSalesPerDayQuery command, CancellationToken cancellationToken)
        {
            Establishment? Establishment = this.userContextService.GetActiveEstablishment();

            IEnumerable<Sale> sales = this.salesRepository.GetAll().Where(x => x.Establishment.Id == Establishment.Id);
            IEnumerable<Sale> saleTimeslot = sales.Where(x => x.TimeStamp >= command.StartDate && x.TimeStamp <= command.EndDate);
            IEnumerable<IGrouping<DateTime, Sale>> grouped = saleTimeslot.GroupBy(x => x.TimeStamp.Date);

            //Create timeline
            List<DateTime> timeline = new List<DateTime>();

            Func<DateTime,DateTime> res = x => {
                switch (command.Resolution)
                {
                    case TimeResolution.quarter:
                        return x.AddMinutes(15);
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
            List<(DateTime, int)> salesPerDay = new List<(DateTime, int)>();
            foreach (DateTime date in timeline)
            {
                int salesOnDate = grouped.Where(x => x.Key == date).Count();
                salesPerDay.Add((date, salesOnDate));
            }

            return new ProductSalesPerDayDTO() { values = salesPerDay };
        }
    }
}


//Liste, med lister over alle transactions

//Udregn frekvensen for hver item. Hvor mange fremgår dét item fra transactions

//Ud fra frekvensen, så tildel en prioritet. Hvis lige høj frekvens, få FCFS    

//reorder transactionerne ift prioriteten

//Take the first transaction, and make at tree from it out of null. Make sure all transactions have a path from null.