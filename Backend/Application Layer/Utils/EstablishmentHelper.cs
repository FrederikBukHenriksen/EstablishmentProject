using NodaTime;
using WebApplication1.Domain.Entities;

namespace WebApplication1.Application_Layer.Utils
{
    public static class EstablishmentHelper
    {
        public static bool IsOpen(this Establishment establishment, DateTime dateTime)
        {
            var openingDay = establishment.EstablishmentInformation.OpeningHours.Where(x => x.dayOfWeek == dateTime.DayOfWeek).FirstOrDefault();
            if (openingDay != null)
            {
                var dateTimeAsLocalTime = new LocalTime(dateTime.Hour, dateTime.Minute, dateTime.Second);
                return openingDay.open <= dateTimeAsLocalTime && openingDay.close >= dateTimeAsLocalTime;
            }
            return false;
        }

    }
}
