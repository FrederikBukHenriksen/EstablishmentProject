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
            if (command is IEstablishmentCommandField)
            {
                Guid establishmentId = (command as IEstablishmentCommandField).EstablishmentId;
                var test = this.userContextService.GetAccessibleEstablishmentsIds();
                var testUser = this.userContextService.GetUser();
                if (!this.userContextService.GetAccessibleEstablishmentsIds().Any(x => x == establishmentId))
                {
                    throw new Unauthorised();
                }
            }

        }
    }
}
