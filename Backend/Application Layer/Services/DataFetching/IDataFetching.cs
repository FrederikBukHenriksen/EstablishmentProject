using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Application_Layer.Services.DataFetching
{
    public interface IRetrieveItemsData
    {
        Task<List<(Func<Establishment, Item>, RetrivingMetadata)>> RetrieveItems();
    }

    public interface IRetrieveTablesData
    {
        Task<List<(Func<Establishment, Table>, RetrivingMetadata)>> RetrieveTables();
    }

    public interface IRetrieveSalesData
    {
        Task<List<(Func<Establishment, List<EntityIdAndForeignId>, Sale>, RetrivingMetadata)>> RetrieveSales();
    }



    public class EntityIdAndForeignId
    {
        public Guid entityId;
        public string ForeingId;

        public EntityIdAndForeignId(Guid entityId, string foreingId)
        {
            this.entityId = entityId;
            this.ForeingId = foreingId;
        }
    }
}
