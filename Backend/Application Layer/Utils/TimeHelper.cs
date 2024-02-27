namespace WebApplication1.Utils
{
    public enum TimeResolution
    {
        Hour,
        Date,
        Month,
        Year,
    }



    public class DateTimePeriod
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
        public static int FromTimeSpanToHours(TimeSpan timespan)
        {
            return (int)timespan.TotalHours;
        }
        public static Func<DateTime, int> DateTimeExtractorFunction(TimeResolution timeResolution)
        {
            switch (timeResolution)
            {
                case TimeResolution.Hour:
                    return x => x.Hour;
                case TimeResolution.Date:
                    return x => x.Day;
                case TimeResolution.Month:
                    return x => x.Month;
                case TimeResolution.Year:
                    return x => x.Year;
                default:
                    throw new ArgumentException();
            }
        }

        public static DateTime AddToDateTime(this DateTime datetime, int amount, TimeResolution timeResolution)
        {
            switch (timeResolution)
            {
                case TimeResolution.Hour:
                    return datetime.AddHours(amount);
                case TimeResolution.Date:
                    return datetime.AddDays(amount);
                case TimeResolution.Month:
                    return datetime.AddMonths(amount);
                case TimeResolution.Year:
                    return datetime.AddYears(amount);
                default:
                    throw new ArgumentException();
            }
        }

        public static List<DateTime> CreateTimelineAsList(DateTime start, DateTime end, TimeResolution resolution)
        {
            if (end < start)
            {
                throw new ArgumentException("End must be equal or later than start");
            }

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

            for (DateTime date = TimeResolutionUniqueRounder(start, resolution); date < end; date = res(date))
            {
                timeline.Add(date);
            }

            return timeline;
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


        //v2
        public static List<DateTime> CreateTimelineAsListV2(DateTime start, DateTime end, TimeResolution resolution)
        {
            List<DateTime> timeline = new List<DateTime>();

            DateTime current = start;

            while (current < end)
            {
                timeline.Add(current);

                switch (resolution)
                {
                    case TimeResolution.Hour:
                        current = current.AddHours(1);
                        break;
                    case TimeResolution.Date:
                        current = current.AddDays(1);
                        break;
                    case TimeResolution.Month:
                        current = current.AddMonths(1);
                        break;
                    case TimeResolution.Year:
                        current = current.AddYears(1);
                        break;
                    default:
                        throw new ArgumentException("Invalid time resolution.");
                }
            }

            return timeline;
        }

        public static Dictionary<DateTime, List<T>> MapObjectsToTimelineV2<T>(IEnumerable<T> objects, Func<T, DateTime> extractor, List<DateTime> timeline, TimeResolution timeResolution)
        {
            Dictionary<DateTime, List<T>> mappedTimeline = new Dictionary<DateTime, List<T>>();

            foreach (var obj in objects)
            {
                DateTime objDateTime = extractor(obj);
                DateTime mappedDateTime = MapDateTime(objDateTime, timeResolution);

                if (!mappedTimeline.ContainsKey(mappedDateTime))
                    mappedTimeline[mappedDateTime] = new List<T>();

                mappedTimeline[mappedDateTime].Add(obj);
            }

            // Fill in missing timeline points with empty lists
            foreach (var timelinePoint in timeline)
            {
                if (!mappedTimeline.ContainsKey(timelinePoint))
                    mappedTimeline[timelinePoint] = new List<T>();
            }

            return mappedTimeline;
        }

        private static DateTime MapDateTime(DateTime dateTime, TimeResolution timeResolution)
        {
            switch (timeResolution)
            {
                case TimeResolution.Hour:
                    return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0);
                case TimeResolution.Date:
                    return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
                case TimeResolution.Month:
                    return new DateTime(dateTime.Year, dateTime.Month, 1, 0, 0, 0);
                case TimeResolution.Year:
                    return new DateTime(dateTime.Year, 1, 1, 0, 0, 0);
                default:
                    throw new ArgumentException("Invalid time resolution specified.");
            }
        }

        public static List<(DateTime, List<T>)> MapObjectsToTimelineV3<T>(IEnumerable<T> objects, Func<T, DateTime> extractor, List<DateTime> timeline, TimeResolution timeResolution)
        {
            Dictionary<DateTime, List<T>> mappedTimeline = new Dictionary<DateTime, List<T>>();

            foreach (var obj in objects)
            {
                DateTime objDateTime = extractor(obj);
                DateTime mappedDateTime = MapDateTime(objDateTime, timeResolution);

                if (!mappedTimeline.ContainsKey(mappedDateTime))
                    mappedTimeline[mappedDateTime] = new List<T>();

                mappedTimeline[mappedDateTime].Add(obj);
            }

            // Fill in missing timeline points with empty lists
            foreach (var timelinePoint in timeline)
            {
                if (!mappedTimeline.ContainsKey(timelinePoint))
                    mappedTimeline[timelinePoint] = new List<T>();
            }

            return mappedTimeline.Select(kv => (kv.Key, kv.Value)).ToList();
        }

        public static List<(DateTime, List<T>)> MapObjectsToTimelineV4<T>(IEnumerable<T> objects, Func<T, DateTime> extractor, DateTime start, DateTime end, TimeResolution timeResolution)
        {
            // Create the timeline based on the specified start, end, and time resolution
            List<DateTime> timeline = CreateTimeline(start, end, timeResolution);

            List<(DateTime, List<T>)> mappedTimeline = new List<(DateTime, List<T>)>();
            foreach (var time in timeline)
            {
                var iterationEntry = (time, new List<T>());
                foreach (var obj in objects)
                {
                    DateTime objDateTime = extractor(obj);
                    DateTime mappedDateTime = MapDateTime(objDateTime, timeResolution);
                    if (mappedDateTime == time)
                    {
                        iterationEntry.Item2.Add(obj);
                    }
                }
                mappedTimeline.Add(iterationEntry);
            }
            return mappedTimeline;
        }

        private static List<DateTime> CreateTimeline(DateTime start, DateTime end, TimeResolution timeResolution)
        {
            List<DateTime> timeline = new List<DateTime>();

            DateTime current = start;
            while (current < end)
            {
                timeline.Add(current);
                current = current.AddToDateTime(1, timeResolution);
            }

            return timeline;
        }
    }
}
