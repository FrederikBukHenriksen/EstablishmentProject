using NJsonSchema.NewtonsoftJson.Converters;
using System.Runtime.Serialization;
using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Domain_Layer.Services.Repositories;
using WebApplication1.Services;
using WebApplication1.Services.Analysis;
using WebApplication1.Utils;

namespace WebApplication1.CommandHandlers
{
    [Newtonsoft.Json.JsonConverter(typeof(JsonInheritanceConverter), "$type")]
    [KnownType(typeof(MSC_Sales_TimeOfVisit_TotalPrice))]
    [KnownType(typeof(MSC_Sales_TimeOfVisit_LengthOfVisit))]

    public abstract class MeanShiftClusteringCommand : CommandBase
    {
        protected IServiceProvider serviceProvider;
        public SalesSortingParameters? salesSortingParameters { get; set; }
        public abstract List<(Sale, List<double>)> GetData();
        public abstract List<double> GetBandwidths();
        public void setServiceProvider(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
    }
    public class MSC_Sales_TimeOfVisit_LengthOfVisit : MeanShiftClusteringCommand
    {
        //private List<Sale> sales;

        public override List<(Sale, List<double>)> GetData()
        {
            List<Sale> sales = this.serviceProvider.GetService<ISalesRepository>().GetSalesFromEstablishment(this.serviceProvider.GetService<IUserContextService>().GetActiveEstablishment());
            List<(Sale sale, List<double> values)> SalesWithValues = sales
                .Select(sale => (
                    entity: sale,
                    values: new List<double> { sale.GetTimeOfSale().Second, sale.GetTotalPrice() }
                    ))
                .ToList();

            return SalesWithValues;
        }

        public override List<double> GetBandwidths()
        {
            return new List<double> { 1, 1 };
        }
    }

    public class MSC_Sales_TimeOfVisit_TotalPrice : MeanShiftClusteringCommand
    {
        private ISalesRepository salesRepository;
        private List<Sale> sales;

        public override List<(Sale, List<double>)> GetData()
        {
            List<Sale> sales = this.serviceProvider.GetService<ISalesRepository>().GetSalesFromEstablishment(this.serviceProvider.GetService<IUserContextService>().GetActiveEstablishment());
            List<(Sale sale, List<double> values)> SalesWithValues = sales
                 .Select(sale => (
                     entity: sale,
                     values: new List<double> { sale.GetTimeOfSale().Second, sale.GetTotalPrice() }
                     ))
                 .ToList();

            return SalesWithValues;
        }

        public override List<double> GetBandwidths()
        {
            return new List<double> { 1, 1 };
        }
    }

    public class MeanShiftClusteringReturn : ReturnBase
    {
        public List<List<Sale>> clusters { get; set; }
        public Dictionary<Sale, List<double>> calculations { get; set; }
    }


    public class salesClustering : HandlerBase<MeanShiftClusteringCommand, MeanShiftClusteringReturn>
    {
        private IServiceProvider serviceProvider;

        public salesClustering(IServiceProvider serviceProvider, IUserContextService userContextService, IEstablishmentRepository establishmentRepository, ISalesRepository salesRepository)
        {
            this.serviceProvider = serviceProvider;
        }

        public override MeanShiftClusteringReturn Handle(MeanShiftClusteringCommand command)
        {
            //Arrange
            command.setServiceProvider(this.serviceProvider);
            List<(Sale, List<double>)> data = command.GetData();

            //Act
            List<List<Sale>> clusteredSales = MeanShiftClustering.Cluster(data, command.GetBandwidths());

            //Return

            return new MeanShiftClusteringReturn { clusters = clusteredSales, calculations = data.ToDictionary(x => x.Item1, x => x.Item2) };
        }
    }
}
