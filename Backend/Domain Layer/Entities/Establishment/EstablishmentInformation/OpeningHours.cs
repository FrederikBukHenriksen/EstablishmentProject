using NodaTime;
using WebApplication1.Domain.Entities;
using WebApplication1.Utils;

namespace WebApplication1.Domain_Layer.Entities.Establishment
{
    public class OpeningHours : EntityBase
    {
        public DayOfWeek dayOfWeek { get; set; }
        public LocalTime open { get; set; }
        public LocalTime close { get; set; }

        public OpeningHours(DayOfWeek dayOfWeek, LocalTime open, LocalTime close)
        {
            this.dayOfWeek = dayOfWeek;
            this.open = open;
            this.close = close;
        }
    }
}
