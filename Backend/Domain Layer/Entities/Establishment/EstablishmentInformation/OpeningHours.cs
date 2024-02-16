using NodaTime;

namespace WebApplication1.Domain_Layer.Entities
{
    public interface IOpeningHours
    {
        public DayOfWeek GetDayOfWeek();
        public LocalTime GetTimeOfOpening();
        public LocalTime GetTimeOfClosing();
    }

    public class OpeningHours : EntityBase, IOpeningHours
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

        public DayOfWeek GetDayOfWeek()
        {
            return this.dayOfWeek;
        }

        public LocalTime GetTimeOfOpening()
        {
            return this.open;
        }

        public LocalTime GetTimeOfClosing()
        {
            return this.close;
        }
    }
}
