using Microsoft.AspNetCore.Mvc;
using WebApplication1.Application_Layer.CommandsQueriesHandlersReturns.Sales;
using WebApplication1.Domain_Layer.Services.Repositories;
using WebApplication1.Infrastructure_Layer.DataTransferObjects;
using WebApplication1.Services;
using WebApplication1.Utils;

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
            if (sale.Establishment.Id == activeEstablishment.Id)
            {
                return new SaleDTO(sale);
            }
            return this.Unauthorized();
        }

        [HttpPost("get-sales")]
        public ActionResult<GetSalesReturn> GetSales([FromBody] GetSalesCommand command)
        {
            var activeEstablishment = this.userContextService.GetActiveEstablishment();
            var sales = this.salesRepository.GetAll().ToList();
            sales = SalesSortingParametersExecute.SortSales(sales, command.SortingParameters);
            List<SaleDTO> salesDTO = new List<SaleDTO>();

            foreach (Guid saleId in command.SalesIds)
            {
                var sale = sales.Find(x => x.Id == saleId);
                if (sale != null)
                {

                    if (sale.Establishment.Id == activeEstablishment.Id)
                    {
                        salesDTO.Add(new SaleDTO(sale));
                    }
                    else
                    {
                        return this.Unauthorized();
                    }
                }

            }
            return new GetSalesReturn { Sales = salesDTO };
        }

        [HttpPost]
        public ActionResult<bool> GetAverageSales([FromBody] GetSalesCommand command)
        {
            return true;
        }


    }
}