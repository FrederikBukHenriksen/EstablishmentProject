﻿using NodaTime;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Utils;

namespace WebApplication1.Infrastructure.Data
{

    public interface ITestDataCreatorService
    {
        List<OpeningHours> CreateSimpleOpeningHoursForWeek(LocalTime open, LocalTime close);
        List<DateTime> SLETTES_DistrubutionBasedOnTimlineAndOpeningHours(List<DateTime> timeline, List<OpeningHours> openingHours);
        List<DateTime> OpenHoursCalendar(DateTime datetimeStart, DateTime datetimeEnd, TimeResolution timeResolution, List<OpeningHours> openingHours);
        Dictionary<DateTime, int> AggregateDistributions(List<Dictionary<DateTime, int>> listOfDistributions);
        Dictionary<DateTime, int> GenerateDistributionFromTimeline(List<DateTime> dateTimePoints, Func<DateTime, int> dateExtractor, Func<double, double> distributionFunction);
        Dictionary<DateTime, int> DistributionOnTimeres(List<DateTime> dateTimePoints, Func<double, double> distributionFunction, TimeResolution timeResolution);


    }

    public class TestDataCreatorService : ITestDataCreatorService
    {


        public static Func<double, double> GetCosineFunction(double amplitude = 1.0, double period = 2.0 * Math.PI, double horizontalShift = 0.0, double verticalShift = 0.0)
        {
            if (period == 0)
            {
                throw new ArgumentException("Parameter 'period' cannot be zero.");
            }

            return x =>
            {
                double argument = (x - horizontalShift) / period;
                double cosineResult = Math.Cos(argument);
                double y = amplitude * cosineResult + verticalShift;
                return y;
            };
        }

        public static Func<double, double> GetNormalFunction(double mean, double stdDev)
        {
            return x => (1 / (stdDev * Math.Sqrt(2 * Math.PI))) * Math.Exp(-Math.Pow(x - mean, 2) / (2 * Math.Pow(stdDev, 2)));
        }

        public static Func<double, double> GetLinearFuncition(double a, double b)
        {
            return x => a * x + b;
        }

        public Dictionary<DateTime, int> GenerateDistributionFromTimeline(List<DateTime> dateTimePoints, Func<DateTime, int> valueExtractor, Func<double, double> distributionFunction)
        {
            Dictionary<DateTime, int> dictionary = new Dictionary<DateTime, int>();

            foreach (DateTime date in dateTimePoints)
            {
                int time = valueExtractor(date);
                double value = distributionFunction(time);
                dictionary.Add(date, (int)value);
            }

            return dictionary;
        }

        public List<OpeningHours> CreateSimpleOpeningHoursForWeek(LocalTime open, LocalTime close)
        {
            var openingHoursList = new List<OpeningHours>();
            for (int i = 0; i < 7; i++)
            {
                openingHoursList.Add(new OpeningHours((DayOfWeek)i, open, close));
            }
            return openingHoursList;
        }

        public List<DateTime> SLETTES_DistrubutionBasedOnTimlineAndOpeningHours(List<DateTime> timeline, List<OpeningHours> openingHours)
        {
            foreach (OpeningHours openingHour in openingHours)
            {
                timeline.RemoveAll((x => x.DayOfWeek != openingHour.dayOfWeek && !(new LocalTime(x.Hour, x.Minute, x.Second) >= openingHour.open && new LocalTime(x.Hour, x.Minute, x.Second) < openingHour.close)));
            }
            return timeline;
        }

        public Dictionary<DateTime, int> AggregateDistributions(List<Dictionary<DateTime, int>> listOfDistributions)
        {
            Dictionary<DateTime, int> dictionary = new Dictionary<DateTime, int>();

            foreach (Dictionary<DateTime, int> distribution in listOfDistributions)
            {
                foreach (KeyValuePair<DateTime, int> entry in distribution)
                {
                    var date = entry.Key;
                    var value = entry.Value;

                    if (dictionary.ContainsKey(date))
                    {
                        dictionary[date] += value;
                    }
                    else
                    {
                        dictionary.Add(date, value);
                    }
                }
            }

            return dictionary;
        }

        public List<DateTime> OpenHoursCalendar(DateTime start, DateTime end, TimeResolution timeResolution, List<OpeningHours> openingHours)
        {
            var timeline = TimeHelper.CreateTimelineAsList(start, end, TimeResolution.Hour);
            return this.SLETTES_DistrubutionBasedOnTimlineAndOpeningHours(timeline, openingHours);
        }

        public Dictionary<DateTime, int> DistributionOnTimeres(List<DateTime> dateTimePoints, Func<double, double> distributionFunction, TimeResolution timeResolution)
        {
            return this.GenerateDistributionFromTimeline(dateTimePoints, TimeHelper.DateTimeExtractorFunction(timeResolution), distributionFunction);

        }
    }
}
