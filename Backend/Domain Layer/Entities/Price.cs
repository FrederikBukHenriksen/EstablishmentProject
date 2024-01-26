namespace WebApplication1.Domain_Layer.Entities
{
    public enum Currency
    {
        UNKNOWN,
        DKK,
        EUR,
    }

    public class Price : EntityBase
    {
        public Price(double value, Currency currency)
        {
            this.Value = value;
            this.Currency = currency;
        }

        public Price()
        {
        }

        public double Value { get; set; }
        public Currency Currency { get; set; }

        //Validators
        public bool IsPriceValid(double price)
        {
            return price >= 0;
        }
    }
}