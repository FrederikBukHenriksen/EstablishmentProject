namespace WebApplication1.Domain_Layer.Entities
{
    public enum Currency
    {
        UNKNOWN,
        DKK,
        EUR,
        GBP,
        USD
    }

    public class Price : EntityBase
    {
        public Price(double value, Currency currency)
        {
            this.Amount = value;
            this.Currency = currency;
        }

        public Price()
        {
        }

        public double Amount { get; set; }
        public Currency Currency { get; set; }

        //Validators
        public bool IsPriceValid(double price)
        {
            return price >= 0;
        }
    }
}