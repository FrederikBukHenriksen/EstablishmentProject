using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Domain_Layer.Exceptions;

namespace WebApplication1.Application_Layer.Services.CommandHandlerServices
{
    public class VerifyItemsCommandService
    {
        private IUnitOfWork unitOfWork;

        public VerifyItemsCommandService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void VerifyItems(ICommand command)
        {
            if (command is ICmdField_SalesIds)
            {
                Guid establishmentId = (command as ICmdField_ItemsId).EstablishmentId;
                List<Guid> itemsIds = (command as ICmdField_ItemsId).ItemsIds;
                IEnumerable<Guid> allItems = this.unitOfWork.itemRepository.GetAllItemsFromEstablishment(establishmentId).Select(x => x.Id);
                if (!itemsIds.All(guid => allItems.Contains(guid)))
                {
                    throw new Unauthorised();
                }
            }

        }

    }
}
