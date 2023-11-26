using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Repositories;
using WebApplication1.Services;
using WebApplication1.Utils;

namespace WebApplication1.CommandHandlers
{
    public class MeanSales
    {
        public class MeanItemSalesCommand: CommandBase
        {
            public Item Item { get; set; }
            public List<(DateTime, DateTime)> UseDataFromTimeframePeriods { get; set; } = new List<(DateTime, DateTime)>();
            public TimeResolution TimeResolution { get; set; }
        }

        public class MeanItemSalesReturn : Return
        {
            public List<(int, double)> MeanSales { get; set; }
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

            public override MeanItemSalesReturn Execute(MeanItemSalesCommand command)
            {
                Establishment activeEstablishment = userContextService.GetActiveEstablishment();

                ICollection<Sale> sales = establishmentRepository.GetSales(userContextService.GetActiveEstablishment().Id);

                //Filter sales by timeframe periods
                List<Sale> salesWithinTimeframe = new List<Sale>();
                foreach (var period in command.UseDataFromTimeframePeriods)
                {
                    ICollection<Sale> salesFromPeriod = TimeHelper.GetEntitiesWithinTimeframe(sales, period.Item1, period.Item2, x => x.TimestampEnd);
                    salesWithinTimeframe.AddRange(salesFromPeriod);
                }

                var groupedByDay = salesWithinTimeframe.GroupBy(x => TimeHelper.UseTimeResolution(x.TimestampEnd, command.TimeResolution));

                //Average sales per day
                List<(int,double)> res = new List<(int, double)>();
                foreach (var group in groupedByDay)
                {
                    double meanValue = group.Average(x => 1);
                    res.Add((group.Key, meanValue));
                }

                return new MeanItemSalesReturn { MeanSales = res };
            }
        }
    }
}
