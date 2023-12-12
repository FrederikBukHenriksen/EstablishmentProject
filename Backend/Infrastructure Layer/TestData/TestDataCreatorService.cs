using MathNet.Numerics;
using Microsoft.IdentityModel.Tokens;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Application_Layer.Utils;
using WebApplication1.Domain.Entities;
using WebApplication1.Domain_Layer.Entities.Establishment;
using WebApplication1.Domain_Layer.Services.Entity_builders;
using WebApplication1.Utils;

namespace WebApplication1.Infrastructure.Data
{

    public class TestDataCreatorService
    {
        private FactoryServiceBuilder factoryServiceBuilder;

        public static Func<double, double> GetSinusFunction(double amplitude = 1.0, double frequency = 2* double.Pi, double phaseShift = 0, double verticalShift = 0)
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


        public TestDataCreatorService()
        {
        }


        public Dictionary<DateTime, int> GenerateDailyDistibutionFromTimeline(List<DateTime> dateTimePoints, Func<double, double> dailyDistribution)
        {
            Dictionary<DateTime, int> dictionary = new Dictionary<DateTime, int>();

            foreach (DateTime date in dateTimePoints)
            {
                double time = date.Hour + (date.Minute / 60.0);

                double value = dailyDistribution(time);

                int rounded = (int)Math.Round(value, 0);

                dictionary.Add(date, rounded);
            }

            return dictionary;
        }

        public List<DateTime> FilterDistrubutionBasedOnOpeningHours(List<DateTime> timeline, List<OpeningHours> openingHours)
        {
            foreach (OpeningHours openingHour in openingHours)
            {
                timeline.RemoveAll((x => x.DayOfWeek != openingHour.dayOfWeek && !(new LocalTime(x.Hour,x.Minute,x.Second) >= openingHour.open && new LocalTime(x.Hour, x.Minute, x.Second) < openingHour.close)));
            }
            return timeline;
        }



        public List<Sale> SaleGenerator(List<(Item item,int quanity)> items, Dictionary<DateTime, int> DistributionOverTime)
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
