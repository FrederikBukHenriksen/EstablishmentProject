using WebApplication1.Application_Layer.Services;
using WebApplication1.CommandHandlers;
using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Infrastructure_Layer.DataTransferObjects;

namespace WebApplication1.Application_Layer.Handlers.ItemHandler
{
    public class GetItemDTOCommand : CommandBase, ICmdField_ItemsId
    {
        public Guid EstablishmentId { get; set; }
        public List<Guid> ItemsIds { get; set; }
    }

    public class GetItemDTOReturn : ReturnBase
    {
        public List<ItemDTO> Items { get; set; }
    }

    public class GetItemDTOHandler : HandlerBase<GetItemDTOCommand, GetItemDTOReturn>
    {
        private IUnitOfWork unitOfWork;

        public GetItemDTOHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async override Task<GetItemDTOReturn> Handle(GetItemDTOCommand command)
        {
            Establishment establishment = this.unitOfWork.establishmentRepository.IncludeItems().GetById(command.EstablishmentId);
            IEnumerable<Item> items = establishment.Items.Where(x => command.ItemsIds.Any(y => y == x.Id));
            items = items.Where(x => command.ItemsIds.Contains(x.Id));
            List<ItemDTO> itemsDTO = items.Select(x => new ItemDTO(x)).ToList();

            return new GetItemDTOReturn { Items = itemsDTO };
        }
    }


}
