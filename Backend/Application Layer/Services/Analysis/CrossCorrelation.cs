using MathNet.Numerics.Statistics;

namespace WebApplication1.Services.Analysis
{
    public class CrossCorrelation
    {
        public static List<(TimeSpan, double)> DoAnalysis(List<(DateTime, double)> list1WithTime, List<(DateTime, double)> list2WithTime)
        {
            int times = (list2WithTime.Count - list1WithTime.Count) + 1;

            List<(TimeSpan, double)> LagAndCorrelation = new List<(TimeSpan, double)>();

            for (int i = 0; i < times; i++)
            {
                var trimmedList2WithTime = list2WithTime.GetRange(i, list1WithTime.Count).ToArray();
                var lag = trimmedList2WithTime.First().Item1 - list1WithTime.First().Item1;

                var list1Value = list1WithTime.Select(x => x.Item2).ToList();
                var list2Value = trimmedList2WithTime.Select(x => x.Item2).ToList();

                var correlation = Correlation.Spearman(list1Value, list2Value);

                LagAndCorrelation.Add((lag, correlation));
            }
            return LagAndCorrelation;
        }
        //public static List<(TimeSpan, double)> DoAnalysis(List<(DateTime, double)> list1WithTime, List<(DateTime, double)> list2WithTime)
        //{
        //    int times = Math.Max(0, list2WithTime.Count - list1WithTime.Count) + 1;

        //    List<(TimeSpan, double)> LagAndCorrelation = new List<(TimeSpan, double)>();

        //    for (int i = 0; i < times; i++)
        //    {
        //        var endIndex = i + list1WithTime.Count;
        //        if (endIndex > list2WithTime.Count)
        //        {
        //            // If endIndex goes beyond the length of list2WithTime, adjust it
        //            endIndex = list2WithTime.Count;
        //        }

        //        var trimmedList2WithTime = list2WithTime.Skip(i).Take(list1WithTime.Count).ToArray();

        //        var lag = trimmedList2WithTime.First().Item1 - list1WithTime.First().Item1;

        //        var list1Value = list1WithTime.Select(x => x.Item2).ToList();
        //        var list2Value = trimmedList2WithTime.Select(x => x.Item2).ToList();

        //        var correlation = Correlation.Spearman(list1Value, list2Value);

        //        LagAndCorrelation.Add((lag, correlation));
        //    }
        //    return LagAndCorrelation;
        //}
    }
}
