namespace WebApplication1.Domain_Layer.Entities
{
    public enum TimeOfSale
    {
        Payment,
        Ordering,
        Booking,
        Leaving,
    }



    public class Preferences : EntityBase
    {
        public TimeOfSale timeOfSale { get; set; } = TimeOfSale.Payment;
        public Currency currency { get; set; }
    }


}
