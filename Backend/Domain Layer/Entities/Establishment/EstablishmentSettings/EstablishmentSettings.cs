namespace WebApplication1.Domain_Layer.Entities
{
    public class EstablishmentSettings : EntityBase
    {
        public DataRetrivalIntegration? ItemDataRetrivalIntegration { get; set; } = null;
        public DataRetrivalIntegration? TableDataRetrivalIntegration { get; set; } = null;
        public DataRetrivalIntegration? SaleDataRetrivalIntegration { get; set; } = null;
    }
}
