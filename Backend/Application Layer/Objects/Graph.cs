namespace WebApplication1.Application_Layer.Objects
{
    public class TimeAndValue<Type>
    {
        public DateTime dateTime { get; set; }
        public Type value { get; set; }
    }

    public class Coordinates
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Coordinates(double x, double y)
        {
            this.Latitude = x;
            this.Longitude = y;
        }
    }
}
