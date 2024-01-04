using NodaTime;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Domain_Layer.Services.Entity_builders;

namespace WebApplication1.Infrastructure.Data
{

    public interface ITestDataCreatorService
    {
        List<OpeningHours> CreateSimpleOpeningHoursForWeek(LocalTime open, LocalTime close);
        List<DateTime> FilterDistrubutionBasedOnOpeningHours(List<DateTime> timeline, List<OpeningHours> openingHours);
        List<Sale> SaleGenerator(List<(Item item, int quanity)> items, Dictionary<DateTime, int> DistributionOverTime);
        Dictionary<DateTime, int> AggregateDistributions(List<Dictionary<DateTime, int>> listOfDistributions);
        Dictionary<DateTime, int> GenerateDistributionFromTimeline(List<DateTime> dateTimePoints, Func<DateTime, int> dateExtractor, Func<double, double> distributionFunction);
    }

    public class TestDataCreatorService : ITestDataCreatorService
    {

        public static Func<double, double> GetSinusFunction(double amplitude = 1.0, double frequency = 2 * double.Pi, double phaseShift = 0, double verticalShift = 0)
        {
            return x => amplitude * Math.Sin(frequency * (x + phaseShift)) + verticalShift;
        }

        public static Func<double, double> GetNormalFunction(double mean, double stdDev)
        {
            return x => (1 / (stdDev * Math.Sqrt(2 * Math.PI))) * Math.Exp(-Math.Pow(x - mean, 2) / (2 * Math.Pow(stdDev, 2)));
        }

        public static Func<double, double> GetLinearFuncition(double a, double b)
        {
            return x => a * x + b;
        }

        private IFactoryServiceBuilder factoryServiceBuilder;

        public TestDataCreatorService(IFactoryServiceBuilder factoryServiceBuilder)
        {
            this.factoryServiceBuilder = factoryServiceBuilder;
        }

        public Dictionary<DateTime, int> GenerateDistributionFromTimeline(List<DateTime> dateTimePoints, Func<DateTime, int> dateExtractor, Func<double, double> distributionFunction)
        {
            Dictionary<DateTime, int> dictionary = new Dictionary<DateTime, int>();

            foreach (DateTime date in dateTimePoints)
            {
                int time = dateExtractor(date);
                double value = distributionFunction(time);
                dictionary.Add(date, (int)value);
            }

            return dictionary;
        }

        public Dictionary<DateTime, int> GenerateHourlyDistibutionFromTimeline(List<DateTime> dateTimePoints, Func<double, double> hourlyDistribution)
        {
            Dictionary<DateTime, int> dictionary = new Dictionary<DateTime, int>();

            foreach (DateTime date in dateTimePoints)
            {
                double time = date.Hour + (date.Minute / 60.0);

                double value = hourlyDistribution(time);

                dictionary.Add(date, (int)value);
            }

            return dictionary;
        }

        public Dictionary<DateTime, int> GenerateDatelyDistributionFromTimeline(List<DateTime> dateTimePoints, Func<double, double> dailyDistribution)
        {
            Dictionary<DateTime, int> dictionary = new Dictionary<DateTime, int>();

            foreach (DateTime date in dateTimePoints)
            {
                double time = date.Day;

                double value = dailyDistribution(time);

                dictionary.Add(date, (int)value);
            }

            return dictionary;
        }

        public Dictionary<DateTime, int> GenerateDayOfYearDistributionFromTimeline(List<DateTime> dateTimePoints, Func<double, double> dailyDistribution)
        {
            Dictionary<DateTime, int> dictionary = new Dictionary<DateTime, int>();

            foreach (DateTime date in dateTimePoints)
            {
                double time = date.DayOfYear;

                double value = dailyDistribution(time);

                dictionary.Add(date, (int)value);
            }

            return dictionary;
        }

        public Dictionary<DateTime, int> GenerateMonthlyDistributionFromTimeline(List<DateTime> dateTimePoints, Func<double, double> monthlyDistribution)
        {
            Dictionary<DateTime, int> dictionary = new Dictionary<DateTime, int>();

            foreach (DateTime date in dateTimePoints)
            {
                double time = date.Month;

                double value = monthlyDistribution(time);

                dictionary.Add(date, (int)value);
            }

            return dictionary;
        }

        public Dictionary<DateTime, int> GenerateYearlyDistributionFromTimeline(List<DateTime> dateTimePoints, Func<double, double> yearlyDistribution)
        {
            Dictionary<DateTime, int> dictionary = new Dictionary<DateTime, int>();

            foreach (DateTime date in dateTimePoints)
            {
                double time = date.Year;

                double value = yearlyDistribution(time);

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

        public List<DateTime> FilterDistrubutionBasedOnOpeningHours(List<DateTime> timeline, List<OpeningHours> openingHours)
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

        public List<Sale> SaleGenerator(List<(Item item, int quanity)> items, Dictionary<DateTime, int> DistributionOverTime)
        {
            List<Sale> sales = new List<Sale>();

            //Iterate over the distribution
            foreach (KeyValuePair<DateTime, int> entry in DistributionOverTime)
            {
                var date = entry.Key;
                var value = entry.Value;

                Sale sale = this.factoryServiceBuilder.SaleBuilder().WithTimestampPayment(date).WithSoldItems(items).Build();

                for (int i = 0; i < value; i++)
                {
                    sales.Add(sale);
                }




            }
            return sales;
        }
    }
}
