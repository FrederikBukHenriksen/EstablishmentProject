﻿using Microsoft.IdentityModel.Tokens;
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
            if (command is ICmdField_ItemsIds)
            {
                Guid establishmentId = (command as ICmdField_ItemsIds).EstablishmentId;
                List<Guid> itemsIds = (command as ICmdField_ItemsIds).ItemsIds;
                List<Guid> allItems = this.unitOfWork.establishmentRepository.IncludeItems().GetById(establishmentId).Items.Select(x => x.Id).ToList();

                if (!itemsIds.IsNullOrEmpty() && !itemsIds.All(guid => allItems.Contains(guid)))
                {
                    throw new UnauthorizedAccessException();
                }
            }

        }

    }
}
