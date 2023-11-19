using YamlDotNet.Core.Tokens;
using MathNet.Numerics.Statistics;

namespace WebApplication1.Services.Analysis
{
    public class SpearmanCrossCorrelation
    {

        public static double[] DoAnalysis(List<double> list1, List<double> list2)
        {
            //double[] list1 = { 18, 20, 22, 25, 28, 30, 27, 23}; //8 hours open

            //double[] list2 = { 18, 20, 22, 25, 28, 30, 31, 29, 27, 26, 24, 22, 20, 19, 18, 17, 18, 19, 21, 23, 25, 26, 27, 25 };

            int times = (list2.Count - list1.Count) + 1;

            double[] correlations = new double[times];

            for (int i = 0; i < times; i++)
            {
                var trimmedList2 = list2.GetRange(i, list1.Count).ToArray();

                var correlation = Correlation.Spearman(list1, trimmedList2);
                correlations[i] = correlation;
            }
            Console.WriteLine("Correlations:" + correlations.ToString()); ;
            return correlations;
        }
    }
}

