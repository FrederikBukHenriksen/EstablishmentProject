using WebApplication1.Domain.Entities;
using WebApplication1.Domain_Layer.Entities.Establishment;

namespace WebApplication1.Domain_Layer.Entities
{
    public class Information : EntityBase
    {
        public Location? Location { get; set; }
        public ICollection<OpeningHours> OpeningHours { get; set; } = new List<OpeningHours>();
    }
}
