using WebApplication1.CommandsHandlersReturns;

namespace WebApplication1.Application_Layer.Services.CommandHandlerServices
{

    public interface IVerifySalesCommandService
    {
        void VerifySales(ICommand command);
    }
    public class VerifySalesCommandService : IVerifySalesCommandService
    {
        private IUnitOfWork unitOfWork;

        public VerifySalesCommandService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void VerifySales(ICommand command)
        {
            if (command is ICmdField_SalesIds)
            {
                Guid establishmentId = (command as ICmdField_SalesIds).EstablishmentId;
                List<Guid> salesIds = (command as ICmdField_SalesIds).SalesIds;
                List<Guid> allSales = this.unitOfWork.establishmentRepository.IncludeSales().GetById(establishmentId).Sales.Select(x => x.Id).ToList();
                if (!salesIds.All(guid => allSales.Contains(guid)))
                {
                    throw new UnauthorizedAccessException();
                }
            }

        }

    }
}
