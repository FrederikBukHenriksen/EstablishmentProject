using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Services;

namespace WebApplication1.Application_Layer.Services
{
    public interface IVerifyEstablishmentCommandService
    {
        void VerifyEstablishment(ICommand command);
    }

    public class VerifyEstablishmentCommandService : IVerifyEstablishmentCommandService
    {
        private IUserContextService userContextService;

        public VerifyEstablishmentCommandService(IUserContextService userContextService)
        {
            this.userContextService = userContextService;
        }

        public void VerifyEstablishment(ICommand command)
        {
            var accesibleEstablishmentsIds = this.userContextService.GetUser().UserRoles.Select(x => x.Establishment.Id);
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
                List<Guid> establishmentId = (command as ICmdField_EstablishmentIds).EstablishmentIds;
                if (!establishmentId.All(x => accesibleEstablishmentsIds.Contains(x)))
                {
                    throw new UnauthorizedAccessException();
                }
            }
        }
    }
}
