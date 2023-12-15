using DMIOpenData;
using System.Collections.Generic;
using System.Runtime.Serialization;
using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Data.DataModels;
using WebApplication1.Domain.Entities;
using WebApplication1.Domain.Services.Repositories;
using WebApplication1.Services;
using WebApplication1.Services.Analysis;
using WebApplication1.Utils;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WebApplication1.CommandHandlers
{
    [KnownType(typeof(MSC_Sales_TimeOfVisit_LengthOfVisit))]
    [KnownType(typeof(MSC_Sales_TimeOfVisit_TotalPrice))]
    public abstract class MeanShiftClusteringCommand : CommandBase
    {
        public SalesSortingParameters? salesSortingParameters { get; set; }
        public abstract List<(Sale, List<double>)> GetData();
        public abstract List<double> GetBandwidths();
    }

    public class MeanShiftClusteringReturn : ReturnBase
    {
        public List<List<Sale>> clusters { get; set; }
        public Dictionary<Sale,List<double>> calculations { get; set; }
    }

    public class salesClustering : HandlerBase<MeanShiftClusteringCommand, MeanShiftClusteringReturn>
    {
        public salesClustering (IUserContextService userContextService, IEstablishmentRepository establishmentRepository, ISalesRepository salesRepository)
        {
        }

        public override MeanShiftClusteringReturn Handle(MeanShiftClusteringCommand command)
        {
            //Arrange
            List<(Sale, List<double>)> data = command.GetData();

            //Act
            List<List<Sale>> clusteredSales = MeanShiftClustering.Cluster(command.GetData(), command.GetBandwidths());

            //Return

            return new MeanShiftClusteringReturn { clusters = clusteredSales, calculations = data.ToDictionary(x => x.Item1,x => x.Item2) };
        }
    }
}
