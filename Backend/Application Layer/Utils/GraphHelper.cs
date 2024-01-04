using WebApplication1.Domain_Layer.Entities;
using MathNet.Numerics.Statistics;
using System;


namespace WebApplication1.Utils
{

    public static class GraphHelper
    {
        public static List<(DateTime,List<T>)> TimelineBasedOnTimeResolution<T>(this IEnumerable<T> input, TimeResolution timeResolution, Func<T, DateTime> dateTimeSelector)
        {
            List<IGrouping<DateTime, T>> group = input.GroupBy(x => dateTimeSelector(x).TimeResolutionUniqueRounder(timeResolution)).ToList();
            return group.Select(x => (x.Key, x.ToList())).ToList();
        }

        //Groups data based on the time resolution
        public static List<(int, List<List<T>>)> TimeResolutionGroup<T>(this IEnumerable<T> input, TimeResolution timeResolution, Func<T, DateTime> dateTimeSelector)
        {
            var grouPair = input.GroupBy(x => TimeHelper.GroupForAverage(dateTimeSelector(x),timeResolution)); //[month, year]
            var groupPairToList = grouPair.Select(x => (x.Key, x.ToList())).ToList();

            //Collect and group all entries from monthYearList by their month
            var group = groupPairToList.GroupBy(x => TimeHelper.PlainIdentifierBasedOnTimeResolution(x.Key, timeResolution)); //[month]
            var groupRemoveUnnesecaryDateTime = group.Select(x => (x.Key, x.Select(x => x.Item2).ToList())).ToList();
            return groupRemoveUnnesecaryDateTime;
        }

        public static List<(int, double)> AveragePerDateTime<T>(this List<(int, List<List<T>>)> input)
        {
            return input.Select(x => (x.Item1, x.Item2.Average(y => y.Count))).ToList();
        }

        public static double average(IEnumerable<double> input)
        {
            return input.Average();
        }


        public static double MedianPerDateTime(IEnumerable<double> input)
        {
            return Statistics.Median(input);
        }
    }
}
