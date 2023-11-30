using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using WebApplication1.CommandHandlers;
using System;

namespace WebApplication1.Utils
{
    public enum TimeResolution
    {
        Hour,
        Date,
        Month,
        Year,
    }

    public class TimePeriod
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public TimePeriod(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }
    }

    public static class TimeHelper
    {
        public static bool IsEntityWithinTimeframe<Entity>(
            this Entity entity,
            TimePeriod timePeriod,
            Func<Entity, DateTime> timestampSelector)
            where Entity : class
        {
            DateTime entityTimestamp = timestampSelector(entity);
            return entityTimestamp >= timePeriod.Start && entityTimestamp <= timePeriod.End;
        }

        public static List<Entity> SortEntitiesWithinTimePeriod<Entity>(
            this List<Entity> entityList,
            TimePeriod timePeriod,
            Func<Entity, DateTime> timestampSelector)
            where Entity : class
        {
            List<Entity> entitiesWithinTimeframe = entityList
                .Where(entity => entity.IsEntityWithinTimeframe(timePeriod, timestampSelector))
                .ToList();

            return entitiesWithinTimeframe;
        }

        public static List<Entity> SortEntitiesWithinTimePeriods<Entity>(
            this List<Entity> entityList,
            List<TimePeriod> timePeriods,
            Func<Entity, DateTime> timestampSelector)
            where Entity : class
        {
            List<Entity> entitiesWithinTimeframe = new List<Entity>();
            foreach (var period in timePeriods)
            {
                List<Entity> entitiesFromPeriod = entityList.SortEntitiesWithinTimePeriod(period, timestampSelector).ToList();
                entitiesWithinTimeframe.AddRange(entitiesFromPeriod);
            }
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

        public static int PlainIdentifierBasedOnTimeResolution(this DateTime dateTime, TimeResolution timeResolution)
        {
            switch (timeResolution)
            {
                case TimeResolution.Hour:
                    return dateTime.Hour;
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

        public static DateTime GroupForAverage(this DateTime dateTime, TimeResolution timeResolution)
        {
            switch (timeResolution)
            {
                case TimeResolution.Hour:
                    return new DateTime(0,0,dateTime.Day).AddHours(dateTime.Hour);
                case TimeResolution.Date:
                    return new DateTime(0, dateTime.Month, dateTime.Day);
                case TimeResolution.Month:
                    return new DateTime(dateTime.Year, dateTime.Month, 0);
                case TimeResolution.Year:
                    throw new ArgumentException("Year is not supported for this method");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static DateTime TimeResolutionUniqueRounder(this DateTime dateTime, TimeResolution timeResolution)
        {
            switch (timeResolution)
            {
                case TimeResolution.Hour:
                    return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day).AddHours(dateTime.Hour);
                case TimeResolution.Date:
                    return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);   
                case TimeResolution.Month:
                    return new DateTime(dateTime.Year, dateTime.Month, 0);
                case TimeResolution.Year:
                    return new DateTime(dateTime.Year);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
