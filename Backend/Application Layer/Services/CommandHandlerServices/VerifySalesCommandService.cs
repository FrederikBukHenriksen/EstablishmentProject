using Microsoft.IdentityModel.Tokens;
using WebApplication1.CommandsHandlersReturns;

namespace WebApplication1.Application_Layer.Services.CommandHandlerServices
{
    public class VerifySalesCommandService : IVerifyCommand
    {
        private IUnitOfWork unitOfWork;

        public VerifySalesCommandService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void Verify(ICommand command)
        {
            if (command is ICmdField_SalesIds)
            {
                Guid establishmentId = (command as ICmdField_SalesIds).EstablishmentId;
                List<Guid> salesIds = (command as ICmdField_SalesIds).SalesIds;
                List<Guid> allSalesOfEstablishment = this.unitOfWork.establishmentRepository.IncludeSales().GetById(establishmentId).Sales.Select(x => x.Id).ToList();
                if (!salesIds.IsNullOrEmpty() && !salesIds.All(guid => allSalesOfEstablishment.Contains(guid)))
                {
                    throw new UnauthorizedAccessException();
                }
            }

        }

    }
}
