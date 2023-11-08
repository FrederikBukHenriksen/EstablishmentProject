using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebApplication1.Commands;
using WebApplication1.Repositories;
using WebApplication1.Services;

namespace WebApplication1.CommandHandlers
{
    public class GetProductSalesChartQuery : ACommand
    {
        public Guid ItemId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
        public class LineChartData  : Chart
    {
        public string? xLegend = null;
        public string? yLegend = null;
        public ICollection<(DateTime, int)> values = null;
    }

    public abstract class Chart
    {
        public required ChartType type;
    }

    public enum ChartType
    {
        LineChart,
        BarChart,
        PieChart
    }

    public class GetProductSalesChartQueryHandler : CommandHandlerBase<GetProductSalesChartQuery, LineChartData>
    {
        private readonly ISalesRepository salesRepository;
        private readonly IUserContextService userContextService;

        public GetProductSalesChartQueryHandler(ISalesRepository salesRepository, IUserContextService userContextService)
        {
            this.salesRepository = salesRepository;
            this.userContextService = userContextService;
        }

        public override async Task<LineChartData> ExecuteAsync(GetProductSalesChartQuery command, CancellationToken cancellationToken)
        {
            Establishment? Establishment = this.userContextService.GetActiveEstablishment();

            //create timeline
            List<DateTime> datesInRange = new List<DateTime>();
            for (DateTime date = command.StartDate; date <= command.EndDate; date = date.AddDays(1))
            {
                datesInRange.Add(date);
            }

            IEnumerable<Sale> sales = this.salesRepository.GetAll().Where(x => x.Establishment.Id == Establishment.Id);
            IEnumerable<Sale> saleTimeslot = sales.Where(x => x.TimeStamp >= command.StartDate && x.TimeStamp <= command.EndDate);
            IEnumerable<IGrouping<DateTime, Sale>> grouped = saleTimeslot.GroupBy(x => x.TimeStamp.Date);

            //Map every sale onto dateInRange
            List<(DateTime, int)> salesPerDay = new List<(DateTime, int)>();
            foreach (DateTime date in datesInRange)
            {
                int salesOnDate = grouped.Where(x => x.Key == date).Count();
                salesPerDay.Add((date, salesOnDate));
            }

            return new LineChartData() { type = ChartType.LineChart, values = salesPerDay };
        }
    }
}


//Liste, med lister over alle transactions

//Udregn frekvensen for hver item. Hvor mange fremgår dét item fra transactions

//Ud fra frekvensen, så tildel en prioritet. Hvis lige høj frekvens, få FCFS    

//reorder transactionerne ift prioriteten

//Take the first transaction, and make at tree from it out of null. Make sure all transactions have a path from null.