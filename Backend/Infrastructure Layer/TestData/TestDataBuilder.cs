using WebApplication1.Utils;

namespace WebApplication1.Infrastructure.Data
{

    public interface ITestDataBuilder
    {
        Dictionary<DateTime, int> FINALAggregateDistributions(List<Dictionary<DateTime, int>> listOfDistributions);
        Dictionary<DateTime, int> FINALgenerateDistrubution(DateTime start, DateTime end, Func<double, double> distributionFunction, TimeResolution timeResolution);
        Dictionary<DateTime, int> FINALFilterOnOpeningHours(int openHour, int closeHour, Dictionary<DateTime, int> distribution);
    }

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

        public Dictionary<DateTime, int> FINALgenerateDistrubution(DateTime start, DateTime end, Func<double, double> distributionFunction, TimeResolution timeResolution)
        {
            Dictionary<DateTime, int> dictionary = new Dictionary<DateTime, int>();

            Func<DateTime, int> valueExtractor = TimeHelper.DateTimeExtractorFunction(timeResolution);

            List<DateTime> timeline = TimeHelper.CreateTimeline(start: start, end: end, TimeResolution.Hour);

            // Generate distribution
            foreach (DateTime date in timeline)
            {
                int time = valueExtractor(date);
                double value = distributionFunction(time);
                dictionary.Add(date, (int)value);
            }

            return dictionary;
        }

        public Dictionary<DateTime, int> FINALFilterOnOpeningHours(int openHour, int closeHour, Dictionary<DateTime, int> distribution)
        {
            return distribution.Where(x => x.Key.Hour >= openHour && x.Key.Hour < closeHour).ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
