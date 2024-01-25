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
        void FetchAndLoadAll();
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

        public async void FetchAndLoadAll()
        {
            this.FetchAndLoadItems();
            this.FetchAndLoadTables();
            this.FetchAndLoadSales();
        }

        public async void FetchAndLoadItems()
        {
            var fetchedItems = await this.dataFetchingService.FetchItems();

            foreach (var item in fetchedItems)
            {
                this.establishment.AddItem(item);
            }
        }

        public async void FetchAndLoadTables()
        {
            var fetchedTables = await this.dataFetchingService.FetchTables();

            foreach (var table in fetchedTables)
            {
                this.establishment.AddTable(table);
            }
        }

        public async void FetchAndLoadSales()
        {
            var fetchedSales = await this.dataFetchingService.FetchSales();

            foreach (var sale in fetchedSales)
            {
                this.establishment.AddSale(sale);
            }
        }
    }
}




