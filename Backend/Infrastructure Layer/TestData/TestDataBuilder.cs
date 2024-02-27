using WebApplication1.Utils;

namespace WebApplication1.Infrastructure.Data
{

    public interface ITestDataBuilder
    {
        //List<OpeningHours> CreateSimpleOpeningHoursForWeek(LocalTime open, LocalTime close);
        //List<DateTime> SLETTES_DistrubutionBasedOnTimlineAndOpeningHours(List<DateTime> timeline, List<OpeningHours> openingHours);
        //List<DateTime> OpenHoursCalendar(DateTime datetimeStart, DateTime datetimeEnd, TimeResolution timeResolution, List<OpeningHours> openingHours);
        //Dictionary<DateTime, int> GenerateDistributionFromTimeline(List<DateTime> dateTimePoints, Func<DateTime, int> dateExtractor, Func<double, double> distributionFunction);
        //Dictionary<DateTime, int> DistributionByTimeresolution(List<DateTime> dateTimePoints, Func<double, double> distributionFunction, TimeResolution timeResolution);
        Dictionary<DateTime, int> FINALAggregateDistributions(List<Dictionary<DateTime, int>> listOfDistributions);
        Dictionary<DateTime, int> FINALgenerateDistrubution(DateTime start, DateTime end, Func<double, double> distributionFunction, TimeResolution timeResolution);
        Dictionary<DateTime, int> FINALFilterOnOpeningHours(int openHour, int closeHour, Dictionary<DateTime, int> distribution);
    }

    //public class OpeningHours
    //{
    //    public DayOfWeek dayOfWeek { get; set; }
    //    public LocalTime open { get; set; }
    //    public LocalTime close { get; set; }

    //    public OpeningHours(DayOfWeek dayOfWeek, LocalTime open, LocalTime close)
    //    {
    //        this.dayOfWeek = dayOfWeek;
    //        this.open = open;
    //        this.close = close;
    //    }
    //}

    public class TestDataBuilder : ITestDataBuilder
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

        public Dictionary<DateTime, int> FINALAggregateDistributions(List<Dictionary<DateTime, int>> listOfDistributions)
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

        public Dictionary<DateTime, int> DistributionByTimeresolution(List<DateTime> dateTimePoints, Func<double, double> distributionFunction, TimeResolution timeResolution)
        {
            return this.GenerateDistributionFromTimeline(dateTimePoints, TimeHelper.DateTimeExtractorFunction(timeResolution), distributionFunction);

        }

        public Dictionary<DateTime, int> FINALgenerateDistrubution(DateTime start, DateTime end, Func<double, double> distributionFunction, TimeResolution timeResolution)
        {
            List<DateTime> timeline = TimeHelper.CreateTimelineAsList(start: start, end: end, resolution: TimeResolution.Hour);
            return this.DistributionByTimeresolution(timeline, distributionFunction, timeResolution);

        }

        public Dictionary<DateTime, int> FINALFilterOnOpeningHours(int openHour, int closeHour, Dictionary<DateTime, int> distribution)
        {
            return distribution.Where(x => x.Key.Hour >= openHour && x.Key.Hour < closeHour).ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
