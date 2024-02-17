using WebApplication1.CommandsHandlersReturns;

namespace WebApplication1.Application_Layer.Services.CommandHandlerServices
{
    public interface IVerifyItemsCommandService
    {
        void VerifyItems(ICommand command);
    }
    public class VerifyItemsCommandService : IVerifyItemsCommandService
    {
        private IUnitOfWork unitOfWork;

        public VerifyItemsCommandService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void VerifyItems(ICommand command)
        {
            if (command is ICmdField_ItemsId)
            {
                Guid establishmentId = (command as ICmdField_ItemsId).EstablishmentId;
                List<Guid> itemsIds = (command as ICmdField_ItemsId).ItemsIds;
                IEnumerable<Guid> allItems = this.unitOfWork.establishmentRepository.GetById(establishmentId).Items.Select(x => x.Id);

                if (!itemsIds.All(guid => allItems.Contains(guid)))
                {
                    throw new UnauthorizedAccessException();
                }
            }

        }

    }
}
