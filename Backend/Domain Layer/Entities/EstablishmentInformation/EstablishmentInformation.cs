namespace WebApplication1.Domain.Entities
{
    public class EstablishmentInformation : EntityBase
    {
        public virtual Location? Location { get; set; }
        public virtual ICollection<OpeningHours> OpeningHours { get; set; } = new List<OpeningHours>();
    }
}
