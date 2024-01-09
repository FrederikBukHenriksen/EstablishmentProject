using Microsoft.AspNetCore.Mvc;
using WebApplication1.Domain_Layer.Services.Repositories;
using WebApplication1.Infrastructure_Layer.DataTransferObjects;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/establishment/sales")]
    public class SaleController : ControllerBase
    {
        private ISalesRepository salesRepository;
        private IUserContextService userContextService;

        public SaleController(
            ISalesRepository establishmentRepository,
            IUserContextService userContextService
            )
        {
            this.salesRepository = establishmentRepository;
            this.userContextService = userContextService;
        }

        [HttpGet("get-sale")]
        public ActionResult<SaleDTO> GetSale(Guid saleId)
        {
            var activeEstablishment = this.userContextService.GetActiveEstablishment();
            var sale = this.salesRepository.GetById(saleId);
            if (sale.Establishment == activeEstablishment)
            {
                return new SaleDTO(sale);
            }
            return this.Unauthorized();
        }

        [HttpPost("get-sales")]
        public ActionResult<List<SaleDTO>> GetSales(List<Guid> saleIds)
        {
            var activeEstablishment = this.userContextService.GetActiveEstablishment();
            var sales = this.salesRepository.GetAll().ToList();
            List<SaleDTO> salesDTO = new List<SaleDTO>();

            foreach (Guid saleId in saleIds)
            {
                var sale = sales.FirstOrDefault(sale => sale.Id == saleId);
                if (sale.Establishment == activeEstablishment)
                {
                    salesDTO.Add(new SaleDTO(sale));
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