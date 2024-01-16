using Microsoft.AspNetCore.Mvc;
using WebApplication1.Domain_Layer.Services.Repositories;
using WebApplication1.Infrastructure_Layer.DataTransferObjects;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/establishment/sales")]
    public class ItemController : ControllerBase
    {
        private ISalesRepository salesRepository;
        private IUserContextService userContextService;
        private IItemRepository itemRepository;

        public ItemController(
            ISalesRepository establishmentRepository,
            IUserContextService userContextService,
            IItemRepository itemRepository
            )
        {
            this.salesRepository = establishmentRepository;
            this.userContextService = userContextService;
            this.itemRepository = itemRepository;
        }

        [HttpPost("get-items")]
        public ActionResult<List<ItemDTO>> GetItems(List<Guid> itemsId)
        {
            var activeEstablishment = this.userContextService.GetActiveEstablishment();
            var items = this.itemRepository.GetAll().ToList();
            List<ItemDTO> salesDTO = new List<ItemDTO>();

            foreach (Guid itemId in itemsId)
            {
                var item = items.FirstOrDefault(item => item.Id == itemId);
                if (item.EstablishmentId == activeEstablishment.Id)
                {
                    salesDTO.Add(new ItemDTO(item));
                }
                else
                {
                    return this.Unauthorized();
                }
            }
            return salesDTO;
        }

    }
}