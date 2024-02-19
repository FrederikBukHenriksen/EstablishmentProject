using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Application_Layer.Services.DataFetching
{
    public interface IRetrieveItemsData
    {
        Task<List<(Func<Establishment, List<ForeingIDAnEntityID>, Item>, RetrivingMetadata)>> RetrieveItems();
    }

    public interface IRetrieveTablesData
    {
        Task<List<(Func<Establishment, List<ForeingIDAnEntityID>, Table>, RetrivingMetadata)>> RetrieveTables();
    }



    public interface IRetrieveSalesData
    {
        Task<List<(Func<Establishment, List<ForeingIDAnEntityID>, Sale>, RetrivingMetadata)>> RetrieveSales();
    }



    public class ForeingIDAnEntityID
    {
        public Guid entityId;
        public string ForeingId;

        public ForeingIDAnEntityID(Guid entityId, string foreingId)
        {
            this.entityId = entityId;
            this.ForeingId = foreingId;
        }
    }
}
