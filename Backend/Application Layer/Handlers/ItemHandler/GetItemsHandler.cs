﻿using WebApplication1.Application_Layer.Services;
using WebApplication1.CommandHandlers;
using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Application_Layer.Handlers.ItemHandler
{
    public class GetItemsCommand : CommandBase
    {
        public Guid EstablishmentId { get; set; }
    }

    public class GetItemsReturn : ReturnBase
    {
        public List<Guid> Items { get; set; }
        public GetItemsReturn(List<Item> items)
        {
            this.Items = items.Select(x => x.Id).ToList();
        }
    }

    public class GetItemsHandler : HandlerBase<GetItemsCommand, GetItemsReturn>
    {
        private IUnitOfWork unitOfWork;

        public GetItemsHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async override Task<GetItemsReturn> Handle(GetItemsCommand command)
        {
            Establishment establishment = this.unitOfWork.establishmentRepository.IncludeItems().GetById(command.EstablishmentId);
            List<Item> items = establishment.GetItems();

            return new GetItemsReturn(items);
        }
    }
}