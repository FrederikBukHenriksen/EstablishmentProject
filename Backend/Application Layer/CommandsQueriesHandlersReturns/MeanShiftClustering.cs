using DMIOpenData;
using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Domain.Entities;
using WebApplication1.Domain.Services.Repositories;
using WebApplication1.Services;
using WebApplication1.Services.Analysis;
using WebApplication1.Utils;

namespace WebApplication1.CommandHandlers
{

    public class MeanShiftClusteringCommand : CommandBase
    {
        public Guid ItemId { get; set; }
        public TimeResolution Resolution { get; set; }
        public TimePeriod timePeriod { get; set; }
    }

    public class MeanShiftClusteringReturn : ReturnBase
    {
        public List<List<(double, double)>> clusters { get; set; }
    }


    public class MeanShiftClusteringHandler : HandlerBase<MeanShiftClusteringCommand, MeanShiftClusteringReturn>
    {
        private IWeatherApi weatherApi;
        private IUserContextService userContextService;
        private ISalesRepository salesRepository;

        public MeanShiftClusteringHandler (IUserContextService userContextService, ISalesRepository salesRepository)
        {
            this.weatherApi = new DmiWeatherApi();
            this.userContextService = userContextService;
            this.salesRepository = salesRepository;
        }

        public override MeanShiftClusteringReturn Handle(MeanShiftClusteringCommand command)
        {
            //Prep data
            Establishment activeEstablishment = userContextService.GetActiveEstablishment();
            List<Sale> sales = salesRepository.GetSalesFromEstablishment(activeEstablishment);


            //Kør gennem algo

            //Returner resultat



            return new MeanShiftClusteringReturn();
        }
    }
}
