using Microsoft.IdentityModel.Tokens;
using WebApplication1.CommandsHandlersReturns;

namespace WebApplication1.Application_Layer.Services.CommandHandlerServices
{
    public class VerifyTablesCommandService : IVerifyCommand
    {
        private IUnitOfWork unitOfWork;

        public VerifyTablesCommandService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void Verify(ICommand command)
        {
            if (command is ICmdField_TablesIds)
            {
                Guid establishmentId = (command as ICmdField_TablesIds).EstablishmentId;
                List<Guid> tablesIds = (command as ICmdField_TablesIds).TablesIds;
                List<Guid> allTables = this.unitOfWork.establishmentRepository.IncludeTables().GetById(establishmentId).Tables.Select(x => x.Id).ToList();

                if (!allTables.IsNullOrEmpty() && !tablesIds.All(guid => allTables.Contains(guid)))
                {
                    throw new UnauthorizedAccessException();
                }
            }

        }

    }
}
