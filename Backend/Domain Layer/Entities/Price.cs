namespace WebApplication1.Domain_Layer.Entities
{
    public enum Currency
    {
        DKK,
        EUR,
    }

    public class Price : EntityBase
    {
        public Price(double value, Currency currency)
        {
            if (!this.IsPriceValid(value))
            {
                throw new Exception("Price must be a positive number");
            }
            this.Value = value;
            this.Currency = currency;
        }

        public Price(double price, Currency? currency)
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