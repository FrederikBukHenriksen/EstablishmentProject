using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using WebApplication1.CommandHandlers;
using System;
using WebApplication1.Application_Layer.Objects;

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

        public static List<DateTime> CreateTimelineAsList(TimePeriod timePeriod, TimeResolution resolution)
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

            for (DateTime date = TimeResolutionUniqueRounder(timePeriod.Start, resolution); date <= timePeriod.End; date = res(date))
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
                    return new DateTime(1,1,dateTime.Day,0,0,0).AddHours(dateTime.Hour);
                case TimeResolution.Date:
                    return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day,0,0,0);
                case TimeResolution.Month:
                    return new DateTime(dateTime.Year, dateTime.Month, 1,0,0,0);
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
                    return new DateTime(dateTime.Year, dateTime.Month, 1);
                case TimeResolution.Year:
                    return new DateTime(dateTime.Year);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        //public static IEnumerable<(DateTime, IEnumerable<T?>)> mapToATimeline<T>(
        //    IEnumerable<T> values,
        //    Func<T, DateTime> datetimeSelector,
        //    TimePeriod timePeriod,
        //    TimeResolution timeResolution)
        //{
        //    IEnumerable<DateTime> timeline = TimeHelper.CreateTimelineAsList(timePeriod, timeResolution);
        //    IDictionary<DateTime, IEnumerable<T?>> timelineDictionary = timeline.ToDictionary(x => x, x => (IEnumerable<T?>)new List<T?>());

        //    foreach (var value in values)
        //    {
        //        DateTime dateTime = datetimeSelector(value).TimeResolutionUniqueRounder(timeResolution);

        //        // Check if the key exists in the dictionary
        //        if (timelineDictionary.TryGetValue(dateTime, out IEnumerable<T?> output))
        //        {
        //            // Key exists, add the value to the existing list
        //            var updatedList = output.Concat(new List<T?> { value });
        //            timelineDictionary[dateTime] = updatedList;
        //        }
        //    }

        //    IEnumerable<(DateTime, IEnumerable<T?>)> res = timelineDictionary.Select(dic => (dic.Key, dic.Value)).ToList();
        //    return res;
        //}


        public static IEnumerable<int> GetTimelineForTimeResolution(TimeResolution timeResolution)
        {
            switch (timeResolution) {
                case TimeResolution.Hour:
                    return Enumerable.Range(0, 24);
                case TimeResolution.Date:
                    return Enumerable.Range(1, 31);
                case TimeResolution.Month:
                    return Enumerable.Range(1, 12);
                case TimeResolution.Year:
                    throw new ArgumentException("Year is not supported for this method");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static List<(int Key, double? Value)> MapValuesWithDateToTimeResolutionTimeline(List<(int timeResolutionIdentifer, double value)> averageData, TimeResolution timeResolution)
        {
            IEnumerable<int> timeResolutionTimeline = GetTimelineForTimeResolution(timeResolution);
            IEnumerable<(int timeResolutionIdentifier, double? value)> timelineWithSpaceForData = timeResolutionTimeline.Select(x => (x, (double?)null));
            Dictionary<int, double?> timelineAsDictionary = timelineWithSpaceForData.ToDictionary(x => x.timeResolutionIdentifier, x => x.value);

            foreach (var average in averageData)
            {
                var averageTimeIdentifier = average.timeResolutionIdentifer;
                if (timelineAsDictionary.ContainsKey(averageTimeIdentifier))
                {
                    timelineAsDictionary[averageTimeIdentifier] = average.value;
                }
            }

            List<(int Key, double? Value)> dictionaryToList = timelineAsDictionary.Select(x => (x.Key, x.Value)).ToList();
            return dictionaryToList;
        }


    }
}
