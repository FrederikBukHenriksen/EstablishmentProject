namespace WebApplication1.Domain_Layer.Entities
{
    public class EstablishmentInformation : EntityBase
    {
        public EstablishmentInformation()
        {

        }
        public Location Location { get; set; } = new Location();
    }

}
