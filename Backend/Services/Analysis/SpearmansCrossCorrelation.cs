using YamlDotNet.Core.Tokens;
using MathNet.Numerics.Statistics;

namespace WebApplication1.Services.Analysis
{
    public class SpearmansCrossCorrelation
    {
        public static void DoAnalysis()
        {
            //// Youtube example
            //double[] signal1 = { 1, 5, -8 };
            //double[] signal2 = { 3, 2, 1, 6, -7 };

            //my test data:
            double[] coffeesSold = { 18, 20, 22, 25, 28, 30, 27, 23}; //8 hours open

            double[] Temperature = { 18, 20, 22, 25, 28, 30, 31, 29, 27, 26, 24, 22, 20, 19, 18, 17, 18, 19, 21, 23, 25, 26, 27, 25 };

            int times = (Temperature.Length - coffeesSold.Length) + 1;

            double[] correlations = new double[times];

            for (int i = 0; i < times; i++)
            {
                var trimmedTemperature = new ArraySegment<double>(Temperature, i, coffeesSold.Length).ToArray();
                var lol = Correlation.Spearman(coffeesSold, trimmedTemperature);
                correlations[i] = lol;
            }
            Console.WriteLine("Correlations:" + correlations.ToString()); ;
        }
    }
}

