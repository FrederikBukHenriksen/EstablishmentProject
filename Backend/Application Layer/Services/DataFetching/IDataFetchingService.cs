using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using WebApplication1.Application_Layer.Services.DataFetching;
using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Application_Layer.Services
{

    public enum DataRetrivalSystem
    {
        Lightspeed,
    }

    public interface IDataFetcingAndStoringService
    {
        Task RetrieveAllData(Establishment establishment, string credentials, DataRetrivalSystem system);
    }
    [ExcludeFromCodeCoverage]
    public class DataFetcingAndStoringService : IDataFetcingAndStoringService
    {
        private IUnitOfWork unitOfWork;
        private List<EntityIdAndForeignId> foreingIDAnEntityID = new List<EntityIdAndForeignId>();

        public DataFetcingAndStoringService([FromServices] IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task RetrieveAllData(Establishment establishment, string credentials, DataRetrivalSystem system)
        {
            this.RetrieveItems(establishment, credentials, system);
            this.RetrieveTables(establishment, credentials, system);
            this.RetrieveSales(establishment, credentials, system);

            using (var uow = this.unitOfWork)
            {
                uow.establishmentRepository.Update(establishment);
            }
        }

        public async Task RetrieveItems(Establishment establishment, string credentials, DataRetrivalSystem system)
        {
            IRetrieveItemsData adapterRetrieveItems;

            switch (system)
            {
                case DataRetrivalSystem.Lightspeed:
                    adapterRetrieveItems = new UpserveAdapter(new LighspeedCredentials(credentials));
                    break;
                default:
                    throw new NotSupportedException($"Data fetching service system '{system}' is not supported.");
            }

            var loadItems = await adapterRetrieveItems.RetrieveItems();

            foreach (var loadItem in loadItems)
            {
                var item = loadItem.Item1(establishment);
                establishment.AddItem(item);
                this.foreingIDAnEntityID.Add(new EntityIdAndForeignId(item.Id, loadItem.Item2.ThirdPartyId));
            }
        }

        public async Task RetrieveTables(Establishment establishment, string credentials, DataRetrivalSystem system)
        {
            IRetrieveTablesData adapterRetrieveTables;

            switch (system)
            {
                case DataRetrivalSystem.Lightspeed:
                    adapterRetrieveTables = new UpserveAdapter(new LighspeedCredentials(credentials));
                    break;
                default:
                    throw new NotSupportedException($"Data fetching service system '{system}' is not supported.");
            }

            var loadTables = await adapterRetrieveTables.RetrieveTables();

            foreach (var loadTable in loadTables)
            {
                var table = loadTable.Item1(establishment);
                establishment.AddTable(table);
                this.foreingIDAnEntityID.Add(new EntityIdAndForeignId(table.Id, loadTable.Item2.ThirdPartyId));
            }
        }

        public async Task RetrieveSales(Establishment establishment, string credentials, DataRetrivalSystem system)
        {
            IRetrieveSalesData adapterRetrieveSales;

            switch (system)
            {
                case DataRetrivalSystem.Lightspeed:
                    adapterRetrieveSales = new UpserveAdapter(new LighspeedCredentials(credentials));
                    break;
                default:
                    throw new NotSupportedException($"Data fetching service system '{system}' is not supported.");
            }

            var loadSales = await adapterRetrieveSales.RetrieveSales();

            foreach (var loadSale in loadSales)
            {
                var sale = loadSale.Item1(establishment, this.foreingIDAnEntityID);
                establishment.AddSale(sale);
                this.foreingIDAnEntityID.Add(new EntityIdAndForeignId(sale.Id, loadSale.Item2.ThirdPartyId));
            }
        }
    }
}




