using Microsoft.AspNetCore.Mvc;
using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Application_Layer.Services
{


    public enum DataFetchingServiceSystem
    {
        Upserve,
    }

    public enum DataFetchingServiceType
    {
        Items,
        Sales,
        Tables,
    }

    public interface IDataFetcingAndStoringService
    {
        void FetchAndLoadItems(Establishment establishment);
        void FetchAndLoadTables(Establishment establishment);
        void FetchAndLoadSales(Establishment establishment);
    }

    public class DataFetcingAndStoringService : IDataFetcingAndStoringService
    {
        private IUnitOfWork unitOfWork;

        public DataFetcingAndStoringService([FromServices] IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async void FetchAndLoadItems(Establishment establishment)
        {
            //Fetch establishments settings for data retrival
            //DataRetrivalIntegration? foundDataRetrival = this.unitOfWork.establishmentRepository.EnableLazyLoading().GetById(establishment.Id)!.Settings.ItemDataRetrivalIntegration;
            //if (foundDataRetrival == null)
            //{
            //    throw new Exception("No retriving integration was found for this");
            //}

            //DataFetchingServiceSystem thirdPartySystem = (DataFetchingServiceSystem)foundDataRetrival.TypeOfService;
            //string credentials = foundDataRetrival.credentials;

            //IRetrieveItems adapterRetrieveItems;
            //switch (thirdPartySystem)
            //{
            //    case DataFetchingServiceSystem.Upserve:
            //        adapterRetrieveItems = new UpserveAdapter(new UpserveCredentials(credentials));
            //        break;
            //    default:
            //        throw new Exception("Integration type was not found");
            //}

            //List<(Func<Item>, RetrivingMetadata)> retrivedItems = await adapterRetrieveItems.FetchItems(establishment);

            //List<Item> existingItems = this.unitOfWork.establishmentRepository.GetById(establishment.Id)!.GetItems();
            //List<(Guid entityId, string foreignId)> joingingTable = new List<(Guid, string)>();

            //foreach (var retrivedItem in retrivedItems)
            //{
            //    Item item = retrivedItem.Item1();
            //    if (joingingTable.Any(x => x.foreignId == retrivedItem.Item2.ThirdPartyId))
            //    {
            //        //establishment.UpdateItem(item);
            //    }
            //    else
            //    {
            //        establishment.AddItem(item);
            //    }
            //}
            //using (var uow = this.unitOfWork)
            //{
            //    uow.establishmentRepository.Update(establishment);
            //}
        }

        public async void FetchAndLoadTables(Establishment establishment)
        {
        }

        public async void FetchAndLoadSales(Establishment establishment)
        {
        }
    }
}




