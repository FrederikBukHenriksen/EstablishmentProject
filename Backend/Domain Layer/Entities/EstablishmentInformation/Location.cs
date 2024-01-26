using WebApplication1.Application_Layer.Objects;

namespace WebApplication1.Domain_Layer.Entities
{
    public class Location : EntityBase
    {

        private double Latitude = 55.676098;
        private double Longitude = 12.568337;
        public Location()
        {

        }

        public Coordinates getCoordinates()
        {
            return new Coordinates(this.Latitude, this.Longitude);
        }
    }
}