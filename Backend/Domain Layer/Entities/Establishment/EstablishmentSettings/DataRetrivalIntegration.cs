using WebApplication1.Application_Layer.Services;

namespace WebApplication1.Domain_Layer.Entities
{
    public class DataRetrivalIntegration : EntityBase
    {
        public DataFetchingServiceType TypeOfService { get; set; }
        public string credentials { get; set; }
    }
}
