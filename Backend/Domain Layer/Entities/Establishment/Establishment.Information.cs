using NodaTime;

namespace WebApplication1.Domain_Layer.Entities
{
    public interface IEstablishment_Information
    {
        List<OpeningHours> SetOpeningHours(DayOfWeek dayOfWeek, LocalTime open, LocalTime close);
        List<OpeningHours> GetAllOpeningHours();
        OpeningHours GetOpeningHours(DayOfWeek dayOfWeek);
    }

    public partial class Establishment : EntityBase, IEstablishment_Information
    {
        public List<OpeningHours> SetOpeningHours(DayOfWeek dayOfWeek, LocalTime open, LocalTime close)
        {
            OpeningHours openingHours = new OpeningHours(dayOfWeek, open, close);
            this.Information.setOpeningHour(openingHours);
            return this.GetAllOpeningHours();
        }

        public List<OpeningHours> GetAllOpeningHours()
        {
            return this.Information.OpeningHours.ToList();
        }

        public OpeningHours GetOpeningHours(DayOfWeek dayOfWeek)
        {
            return this.Information.OpeningHours.FirstOrDefault(x => x.dayOfWeek == dayOfWeek);
        }
    }
}
