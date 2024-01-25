using NodaTime;

namespace WebApplication1.Utils
{
    public enum TimeResolution
    {
        Hour,
        Date,
        Month,
        Year,
    }

    public interface ITimePeriod<T>
    {
        T Start { get; set; }
        T End { get; set; }
    }

    public class localDatePeriod : ITimePeriod<LocalDate>
    {
        public LocalDate Start { get; set; }
        public LocalDate End { get; set; }

        public localDatePeriod(LocalDate start, LocalDate end)
        {
            this.Start = start;
            this.End = end;
        }
    }

    public class LocalTimePeriod : ITimePeriod<LocalTime>
    {
        public LocalTime Start { get; set; }
        public LocalTime End { get; set; }

        public LocalTimePeriod(LocalTime start, LocalTime end)
        {
            this.Start = start;
            this.End = end;
        }
    }

    public class LocalDateTime : ITimePeriod<LocalDateTime>
    {
        public LocalDateTime Start { get; set; }
        public LocalDateTime End { get; set; }

        public LocalDateTime(LocalDateTime start, LocalDateTime end)
        {
            this.Start = start;
            this.End = end;
        }
    }

    public class DateTimePeriod : ITimePeriod<DateTime>
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public DateTimePeriod(DateTime start, DateTime end)
        {
            this.Start = start;
            this.End = end;
        }
    }



    public static class TimeHelper
    {
        public static int FromTimeSpanToHours(TimeSpan timespan, TimeResolution timeResolution)
        {
            return (int)timespan.TotalHours;
        }

        public static LocalTime DateTimeToLocalTime(DateTime dateTime)
        {
            return new LocalTime(dateTime.Hour, dateTime.Minute, dateTime.Second);
        }

        public static DateTime LocalDateAndLocalTimeToDateTime(LocalDate localDate, LocalTime localTime)
        {
            return new DateTime(localDate.Year, localDate.Month, localDate.Day, localTime.Hour, localTime.Minute, localTime.Second);
        }

        public static bool IsWithinPeriod_StartAndEndIndluded<T>(T timestamp, T start, T end) where T : IComparable<T>
        {
            return timestamp.CompareTo(start) >= 0 && timestamp.CompareTo(end) <= 0;
        }

        public static bool IsTimeWithinPeriod_StartNotIncluded<T>(T timestamp, T start, T end) where T : IComparable<T>
        {
            return timestamp.CompareTo(start) > 0 && timestamp.CompareTo(end) <= 0;
        }

        public static bool IsTimeWithinPeriod_EndNotIncluded<T>(T timestamp, T start, T end) where T : IComparable<T>
        {
            return timestamp.CompareTo(start) >= 0 && timestamp.CompareTo(end) < 0;
        }

        public static bool IsTimeWithinPeriod_StartAndEndNotIncluded<T>(T timestamp, T start, T end) where T : IComparable<T>
        {
            return timestamp.CompareTo(start) > 0 && timestamp.CompareTo(end) < 0;
        }

        public static bool IsEntityWithinTimeframe<Entity>(
            this Entity entity,
            DateTimePeriod timePeriod,
            Func<Entity, DateTime> timestampSelector)
            where Entity : class
        {
            DateTime entityTimestamp = timestampSelector(entity);
            return entityTimestamp >= timePeriod.Start && entityTimestamp <= timePeriod.End;
        }

        public static List<Entity> SortEntitiesWithinDateTimePeriod<Entity>(
            this List<Entity> entityList,
            DateTimePeriod timePeriod,
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
            List<DateTimePeriod> timePeriods,
            Func<Entity, DateTime> timestampSelector)
            where Entity : class
        {
            List<Entity> entitiesWithinTimeframe = new List<Entity>();
            foreach (var period in timePeriods)
            {
                List<Entity> entitiesFromPeriod = entityList.SortEntitiesWithinDateTimePeriod(period, timestampSelector).ToList();
                entitiesWithinTimeframe.AddRange(entitiesFromPeriod);
            }
            return entitiesWithinTimeframe;
        }

        public static List<DateTime> CreateTimelineAsList(DateTimePeriod timePeriod, TimeResolution resolution)
        {
            Func<DateTime, DateTime> res = x =>
            {
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

        public static List<DateTime> CreateTimelineAsList(DateTime start, DateTime end, TimeResolution resolution)
        {
            Func<DateTime, DateTime> res = x =>
            {
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

            for (DateTime date = TimeResolutionUniqueRounder(start, resolution); date <= end; date = res(date))
            {
                timeline.Add(date);
            }

            return timeline;
        }

        public static LocalDate GetLocalDateFromDateTime(DateTime datetime)
        {
            return new LocalDate(datetime.Year, datetime.Month, datetime.Day);
        }

        public static LocalTime GetLocalTimeFromDateTime(DateTime dateTime)
        {
            return new LocalTime(dateTime.Hour, dateTime.Minute, dateTime.Second);
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
                    return new DateTime(1, 1, dateTime.Day, 0, 0, 0).AddHours(dateTime.Hour);
                case TimeResolution.Date:
                    return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
                case TimeResolution.Month:
                    return new DateTime(dateTime.Year, dateTime.Month, 1, 0, 0, 0);
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
            switch (timeResolution)
            {
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

        public static Dictionary<DateTime, List<T>> MapObjectsToTimeline<T>(IEnumerable<T> objects, Func<T, DateTime> extractor, List<DateTime> timeline, TimeResolution timeResolution)
        {
            // Create a dictionary with the provided timeline as keys and empty lists as values
            Dictionary<DateTime, List<T>> timelineAsDictionary = timeline.ToDictionary(x => x, x => new List<T>());

            // Map objects to the timeline
            foreach (var obj in objects)
            {
                var objTime = TimeHelper.TimeResolutionUniqueRounder(extractor(obj), timeResolution);

                if (timelineAsDictionary.ContainsKey(objTime))
                {
                    timelineAsDictionary[objTime].Add(obj);
                }
            }
            return timelineAsDictionary;
        }





    }
}
