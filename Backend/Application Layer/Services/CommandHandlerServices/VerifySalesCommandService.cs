using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Domain_Layer.Exceptions;

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
                IEnumerable<Guid> allSales = this.unitOfWork.salesRepository.GetAllSalesFromEstablishment(establishmentId).Select(x => x.Id);
                if (!salesIds.All(guid => allSales.Contains(guid)))
                {
                    throw new Unauthorised();
                }
            }

        }

    }
}
