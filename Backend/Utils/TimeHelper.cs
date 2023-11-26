using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using WebApplication1.CommandHandlers;

namespace WebApplication1.Utils
{
    public static class TimeHelper
    {
        public static bool IsEntityWithinTimeframe<Entity>(
               this Entity entity,
               DateTime start,
               DateTime end,
               Func<Entity, DateTime> timestampSelector)
               where Entity : class
        {
            DateTime entityTimestamp = timestampSelector(entity);
            return entityTimestamp >= start && entityTimestamp <= end;
        }

        public static ICollection<Entity> GetEntitiesWithinTimeframe<Entity>(
            this ICollection<Entity> entityList,
            DateTime start,
            DateTime end,
            Func<Entity, DateTime> timestampSelector)
            where Entity : class
        {
            ICollection<Entity> entitiesWithinTimeframe = entityList
                .Where(entity => entity.IsEntityWithinTimeframe(start, end, timestampSelector))
                .ToList();

            return entitiesWithinTimeframe;
        }

        public static List<DateTime> CreateTimeline(DateTime start, DateTime end, TimeResolution resolution)
        {

            Func<DateTime, DateTime> res = x => {
                switch (resolution)
                {
                    case TimeResolution.Hour:
                        return x.AddHours(1);
                    case TimeResolution.Date:
                        return x.AddDays(1);
                    case TimeResolution.Month:
                        return x.AddMonths(1);
                    case TimeResolution.Year:
                        return x.AddYears(1);
                    default:
                        return x;
                }
            };

            List<DateTime> timeline = new List<DateTime>();

            for (DateTime date = start; date <= end; date = res(date))
            {
                timeline.Add(date);
            }

            return timeline;
        }

        public static int UseTimeResolution(this DateTime dateTime, TimeResolution timeResolution)
        {
            switch (timeResolution)
            {
                case TimeResolution.Hour:
                    return dateTime.Hour;
                case TimeResolution.Week:
                    return (int)dateTime.DayOfWeek;
                case TimeResolution.Date:
                    return dateTime.Day;

                case TimeResolution.Month:
                    return dateTime.Month;
                case TimeResolution.Year:
                    return dateTime.Year;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
