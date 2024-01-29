using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Application_Layer.Services.DataFetching
{
    public interface IDataFetching
    {
        Task<List<(Func<IEntity>, RetrivingMetadata)>> FetchItems(Establishment establishment);
        Task<List<(Func<IEntity>, RetrivingMetadata)>> FetchTables(Establishment establishment);
        Task<List<(Func<IEntity>, RetrivingMetadata)>> FetchSales(Establishment establishment);
    }
}
