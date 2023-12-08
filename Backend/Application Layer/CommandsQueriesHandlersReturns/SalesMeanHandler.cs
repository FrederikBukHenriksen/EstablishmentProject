using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Domain.Entities;
using WebApplication1.Domain.Services.Repositories;
using WebApplication1.Services;
using WebApplication1.Utils;

namespace WebApplication1.CommandHandlers
{
    public  class SalesMeanOverTime : CommandBase
    {
        public SalesSortingParameters? salesSortingParameters { get; set; }
        public TimeResolution TimeResolution { get; set; }
        public List<(int timeResolutionIdentifer, double averageValue)> CalcuateAverageOverTime(List<Sale> sales)
        {
            IEnumerable<IGrouping<int, Sale>> groupedSales = sales.GroupBy(x => TimeHelper.PlainIdentifierBasedOnTimeResolution(x.TimestampPayment, TimeResolution));
            return groupedSales.Select(x => ( x.Key, x.Average(x => 1)) ).ToList();
        }
    }

    public class SalesMeanQueryReturn : ReturnBase
    {
        public List<(int,double?)> Data { get; set; }
    }

    public class SalesMeanQueryHandler : HandlerBase<SalesMeanOverTime, SalesMeanQueryReturn>
    {
        private IEstablishmentRepository establishmentRepository;
        private ISalesRepository salesRepository;
        private IUserContextService userContextService;

        public SalesMeanQueryHandler(IEstablishmentRepository establishmentRepository, IUserContextService userContextService, ISalesRepository salesRepository)
        {
            this.establishmentRepository = establishmentRepository;
            this.salesRepository = salesRepository;
            this.userContextService = userContextService;
        }   

        public override SalesMeanQueryReturn Handle(SalesMeanOverTime command)
        {
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