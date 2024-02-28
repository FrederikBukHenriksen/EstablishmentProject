using Microsoft.IdentityModel.Tokens;
using WebApplication1.Application_Layer.Services.CommandHandlerServices;
using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Services;

namespace WebApplication1.Application_Layer.Services
{
    public class VerifyEstablishmentCommandService : IVerifyCommand
    {
        private IUserContextService userContextService;

        public VerifyEstablishmentCommandService(IUserContextService userContextService)
        {
            this.userContextService = userContextService;
        }

        public void Verify(ICommand command)
        {
            List<Guid> accesibleEstablishmentsIds = this.userContextService.GetUser().UserRoles.Select(x => x.Establishment.Id).ToList();
            if (command is ICmdField_EstablishmentId)
            {
                Guid establishmentId = (command as ICmdField_EstablishmentId).EstablishmentId;
                if (!accesibleEstablishmentsIds.Contains(establishmentId))
                {
                    throw new UnauthorizedAccessException();
                }
            }
            if (command is ICmdField_EstablishmentIds)
            {
                List<Guid> establishmentIds = (command as ICmdField_EstablishmentIds).EstablishmentIds;
                if (!establishmentIds.IsNullOrEmpty() && !establishmentIds.All(x => accesibleEstablishmentsIds.Contains(x)))
                {
                    throw new UnauthorizedAccessException();
                }
            }
        }
    }
}
