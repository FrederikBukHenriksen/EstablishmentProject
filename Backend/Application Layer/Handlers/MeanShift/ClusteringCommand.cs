using WebApplication1.CommandsHandlersReturns;

namespace WebApplication1.Application_Layer.Handlers.MeanShift
{
    public abstract class ClusteringCommand : CommandBase, ICmdField_SalesIds
    {
        public ClusteringCommand(
            Guid establishmentId,
            List<Guid> salesIds
            )
        {
            this.EstablishmentId = establishmentId;
            this.SalesIds = salesIds;

        }
        public Guid EstablishmentId { get; set; }
        public List<Guid> SalesIds { get; set; }

    }

    public class ClusteringReturn : ReturnBase
    {
        public ClusteringReturn(
            List<List<Guid>> clusters,
            List<(Guid id, List<double> values)> calculationValues)
        {
            this.clusters = clusters;
            this.calculationValues = calculationValues;
        }
        public List<List<Guid>> clusters { get; }
        public List<(Guid id, List<double> values)> calculationValues { get; }
    }
}
