using Microsoft.IdentityModel.Tokens;
using WebApplication1.Application_Layer.Objects;
using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Domain.Entities;
using WebApplication1.Domain.Services.Repositories;
using WebApplication1.Services;
using WebApplication1.Utils;

namespace WebApplication1.CommandHandlers
{
    public class SalesQuery : CommandBase
    {
        public List<Guid>? MustContaiedItems { get; set; }
        //public List<Guid>? PossibleTables { get; set; }
        public TimeResolution TimeResolution { get; set; }
        public TimePeriod TimePeriod { get; set; }
    }

    public class SalesQueryReturn : ReturnBase
    {
        public List<TimeAndValue<int>> Data { get; set; } = new List<TimeAndValue<int>>();
    }

    public class SalesQueryHandler : HandlerBase<SalesQuery, SalesQueryReturn>
    {
        private readonly IEstablishmentRepository establishmentRepository;
        private readonly IUserContextService userContextService;

        public SalesQueryHandler(IEstablishmentRepository establishmentRepository, IUserContextService userContextService)
        {
            this.establishmentRepository = establishmentRepository;
            this.userContextService = userContextService;
        }   

        public override SalesQueryReturn Handle(SalesQuery command)
        {
            Establishment activeEstablishment = userContextService.GetActiveEstablishment();

            List<Sale> sales = establishmentRepository.GetEstablishmentSales(userContextService.GetActiveEstablishment().Id).ToList();

            //Sort by time period
            sales.SortSalesByTimePeriod(command.TimePeriod);

            //Sort by items
            if (!command.MustContaiedItems.IsNullOrEmpty()){
                var establishmentItems = establishmentRepository.GetEstablishmentItems(activeEstablishment.Id).ToList();
                var mustBeConatineditems = establishmentItems.Where(x => command.MustContaiedItems.Contains(x.Id)).ToList();
                sales.SortSalesByRequiredConatinedItems(mustBeConatineditems);
            }

            ////Sort by tables
            //if (!command.PossibleTables.IsNullOrEmpty()){
            //    var establishmentTables = establishmentRepository.GetEstablishmentTables(activeEstablishment.Id).ToList();
            //    var possibleTables = establishmentTables.Where(x => command.PossibleTables.Contains(x.Id)).ToList();
            //    sales.SortSalesByTables(possibleTables);
            //}

            //Group by time resolution
            var SalesGroupedByTime = sales.GroupBy(x => x.TimestampPayment.TimeResolutionUniqueRounder(command.TimeResolution));
            List<TimeAndValue<int>> res = SalesGroupedByTime.Select(x => new TimeAndValue<int> { DateTime = x.Key, Value = x.Count() }).ToList();

            return new SalesQueryReturn { Data = res };
        }
    }
}