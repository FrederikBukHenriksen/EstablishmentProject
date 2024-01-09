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
    [KnownType(typeof(MSC_Sales_TimeOfVisit_LengthOfVisit))]
    //[KnownType(typeof(MSC_Sales_TimeOfVisit_TotalPrice))]


    public abstract class MeanShiftClusteringCommand : CommandBase
    {
        protected IServiceProvider serviceProvider;
        public SalesSortingParameters? salesSortingParameters { get; set; }
        public abstract List<(Sale, Dictionary<string, double>)> GetData();
        public abstract List<double> GetBandwidths();
        public void setServiceProvider(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
    }
    public class MSC_Sales_TimeOfVisit_LengthOfVisit : MeanShiftClusteringCommand
    {
        //private List<Sale> sales;

        public override List<(Sale, Dictionary<string, double>)> GetData()
        {
            List<Sale> sales = this.serviceProvider.GetService<ISalesRepository>().GetSalesFromEstablishment(this.serviceProvider.GetService<IUserContextService>().GetActiveEstablishment());
            List<(Sale sale, Dictionary<string, double>)> SalesWithValues = sales
                .Select(sale => (
                    entity: sale,
                    values: new Dictionary<string, double> { { "TimeOfSaleSecond", sale.GetTimeOfSale().Second }, { "TotalPrice", sale.GetTotalPrice() } }
                    ))
                .ToList();

            return SalesWithValues;
        }

        public override List<double> GetBandwidths()
        {
            return new List<double> { 1, 1 };
        }
    }

    //public class MSC_Sales_TimeOfVisit_TotalPrice : MeanShiftClusteringCommand
    //{
    //    private ISalesRepository salesRepository;
    //    private List<Sale> sales;

    //    public override List<(Sale, Dictionary<string, double>)> GetData()
    //    {
    //        List<Sale> sales = this.serviceProvider.GetService<ISalesRepository>().GetSalesFromEstablishment(this.serviceProvider.GetService<IUserContextService>().GetActiveEstablishment());
    //        List<(Sale sale, List<double> values)> SalesWithValues = sales
    //             .Select(sale => (
    //                 entity: sale,
    //                 values: new List<double> { sale.GetTimeOfSale().Second, sale.GetTotalPrice() }
    //                 ))
    //             .ToList();

    //        return SalesWithValues;
    //    }

    //    public override List<double> GetBandwidths()
    //    {
    //        return new List<double> { 1, 1 };
    //    }
    //}

    public class MeanShiftClusteringReturn : ReturnBase
    {
        public List<List<Guid>> clusters { get; set; }
        public Dictionary<Guid, Dictionary<string, double>> calculations { get; set; }
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
            List<(Sale, Dictionary<string, double>)> saleDataAttributes = command.GetData();
            List<(Sale, List<double>)> saleData = saleDataAttributes.Select(x => (x.Item1, x.Item2.Select(y => y.Value).ToList())).ToList();

            //Act
            List<List<Sale>> clusteredSales = MeanShiftClustering.Cluster(saleData, command.GetBandwidths());

            //Return

            return new MeanShiftClusteringReturn
            {
                clusters = clusteredSales.Select(innerList => innerList.Select(sale => sale.Id).ToList()).ToList(),
                calculations = saleDataAttributes.ToDictionary(x => x.Item1.Id, x => x.Item2)
            };
        }
    }
}
