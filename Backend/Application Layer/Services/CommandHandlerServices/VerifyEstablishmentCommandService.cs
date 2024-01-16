using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Domain_Layer.Exceptions;
using WebApplication1.Services;

namespace WebApplication1.Application_Layer.Services
{
    public class VerifyEstablishmentCommandService
    {
        private IUserContextService userContextService;

        public VerifyEstablishmentCommandService(IUserContextService userContextService)
        {
            this.userContextService = userContextService;
        }

        public void VerifyEstablishment(ICommand command)
        {
            var accesibleEstablishmentsIds = this.userContextService.GetAccessibleEstablishmentsIds();
            if (command is ICmdField_EstablishmentId)
            {
                Guid establishmentId = (command as ICmdField_EstablishmentId).EstablishmentId;
                if (!accesibleEstablishmentsIds.Contains(establishmentId))
                {
                    throw new Unauthorised();
                }
            }
            if (command is ICmdField_EstablishmentIds)
            {
                List<Guid> establishmentId = (command as ICmdField_EstablishmentIds).EstablishmentIds;
                if (!establishmentId.All(x => accesibleEstablishmentsIds.Contains(x)))
                {
                    throw new Unauthorised();
                }
            }
        }
    }
}
