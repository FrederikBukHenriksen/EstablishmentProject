using Microsoft.AspNetCore.Mvc;
using WebApplication1.Application_Layer.Services.DataFetching;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Domain_Layer.Services.Repositories;

namespace WebApplication1.Application_Layer.Services
{


    public enum DataFetchingServiceType
    {
        Upserve,
    }

    public interface IDataFetcingAndStoringService
    {
        void FetchAndLoadItems();
        void FetchAndLoadTables();
        void FetchAndLoadSales();
    }

    public class DataFetcingAndStoringService : IDataFetcingAndStoringService
    {
        private Establishment establishment;
        private IEstablishmentRepository establishmentRepository;
        private IDataFetching dataFetchingService;

        public DataFetcingAndStoringService(Establishment establishment, [FromServices] IEstablishmentRepository establishmentRepository)
        {
            this.establishment = establishment;
            this.establishmentRepository = establishmentRepository;
        }

        public async void FetchAndLoadItems()
        {
        }

        public async void FetchAndLoadTables()
        {
        }

        public async void FetchAndLoadSales()
        {
        }
    }
}




