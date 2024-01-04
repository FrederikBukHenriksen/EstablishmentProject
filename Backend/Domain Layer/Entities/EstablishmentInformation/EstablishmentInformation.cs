namespace WebApplication1.Domain_Layer.Entities
{
    public class EstablishmentInformation : EntityBase
    {
        public virtual Location? Location { get; set; }
        public virtual ICollection<OpeningHours> OpeningHours { get; set; } = new List<OpeningHours>();
    }
}
