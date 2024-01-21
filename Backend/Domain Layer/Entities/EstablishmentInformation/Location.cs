namespace WebApplication1.Domain_Layer.Entities
{
    public class Location : EntityBase
    {
        //public Coordinates Coordinates { get; set; } = new Coordinates { Latitude = 55.676098, Longitude = 12.568337 };
    }

    public class Coordinates
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}