using WebApplication1.Commands;
using WebApplication1.Repositories;

namespace WebApplication1.CommandHandlers
{
    public class GetProductSalesQuery
    {
        public List<Guid> ProductId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class GetProductSalesQueryHandlerDTO
    {
        public string ProductName { get; set; }
    }

    public class GetProductSalesQueryHandler : CommandHandlerBase<LoginCommand, string>
    {
        private readonly ISalesRepository _salesRepository;
        public GetProductSalesQueryHandler(ISalesRepository salesRepository)
        {
            _salesRepository = salesRepository;
        }

        public override Task<string> ExecuteAsync(LoginCommand command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
