using Microsoft.IdentityModel.Tokens;
using WebApplication1.Application_Layer.Services;
using WebApplication1.CommandHandlers;
using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Infrastructure_Layer.DataTransferObjects;

namespace WebApplication1.Application_Layer.Handlers.ItemHandler
{
    public class GetItemsCommand : CommandBase, ICmdField_EstablishmentId
    {
        public Guid EstablishmentId { get; set; }
        public List<Guid> ItemIds { get; set; } = new List<Guid>();

    }

    public interface IItemReturn : IReturn
    {
        IItemReturn Create(List<Item> items);
    }

    public class GetItemsIdReturn : ReturnBase, IItemReturn
    {
        public List<Guid> id { get; set; }

        public IItemReturn Create(List<Item> items)
        {
            this.id = items.Select(x => x.Id).ToList();
            return this;
        }
    }

    public class GetItemsEntityReturn : ReturnBase, IItemReturn
    {
        public List<Item> entity { get; set; }

        public IItemReturn Create(List<Item> items)
        {
            this.entity = items;
            return this;
        }
    }

    public class GetItemsDTOReturn : ReturnBase, IItemReturn
    {
        public List<ItemDTO> dto { get; set; }

        public IItemReturn Create(List<Item> items)
        {
            this.dto = items.Select(x => new ItemDTO(x)).ToList();
            return this;
        }
    }

    public class GetItemsHandler<T> : HandlerBase<GetItemsCommand, T> where T : IItemReturn, new()
    {
        private IUnitOfWork unitOfWork;

        public GetItemsHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async override Task<T> Handle(GetItemsCommand command)
        {
            Establishment establishment = this.unitOfWork.establishmentRepository.IncludeItems().GetById(command.EstablishmentId);
            List<Item> items = establishment.GetItems();

            if (!command.ItemIds.IsNullOrEmpty())
            {
                items = items.Where(x => command.ItemIds.Any(y => y == x.Id)).ToList();
            }

            return (T)new T().Create(items);
        }
    }
}
