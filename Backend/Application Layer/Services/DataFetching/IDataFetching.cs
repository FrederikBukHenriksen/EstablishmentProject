using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Application_Layer.Services.DataFetching
{
    public interface IDataFetching
    {
        Task<List<(Func<Item>, RetrivingMetadata)>> FetchItems(Establishment establishment);
        Task<List<(Func<Table>, RetrivingMetadata)>> FetchTables(Establishment establishment);
        Task<List<(Func<Sale>, RetrivingMetadata)>> FetchSales(Establishment establishment);
    }

    public interface IRetrieveItems
    {
        Task<List<(Func<Item>, RetrivingMetadata)>> FetchItems(Establishment establishment);
    }
}
