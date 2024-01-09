using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Application_Layer.Services.DataFetching
{
    public interface IDataFetching
    {
        Task<ICollection<Item>> FetchItems();
        Task<ICollection<Table>> FetchTables();
        Task<ICollection<Sale>> FetchSales();
        Task<ICollection<Employee>> FetchEmployees();
    }
}
