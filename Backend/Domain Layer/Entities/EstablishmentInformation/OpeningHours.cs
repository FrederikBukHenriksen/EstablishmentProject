using NodaTime;

namespace WebApplication1.Domain.Entities
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
