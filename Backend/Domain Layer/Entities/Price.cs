using WebApplication1.Domain.Entities;

namespace WebApplication1.Domain_Layer.Entities
{
    public class Price : EntityBase
    {
        public double Value { get; set; }
        public Currency Currency { get; set; }
    }

    public enum Currency
    {
        DKK,
        EUR,
    }
}
