using MathNet.Numerics.Statistics;

namespace WebApplication1.Services.Analysis
{
    public class CrossCorrelation
    {
        public static List<(TimeSpan, double)> DoAnalysis(List<(DateTime, double)> ReferenceData, List<(DateTime, double)> ShiftingData)
        {
            ListMustNotBeEmpty(ReferenceData);
            ListMustNotBeEmpty(ShiftingData);
            ShiftingDataMustBeLongerThanRefernece(reference: ReferenceData, shifting: ShiftingData);
            DataElementsInListMustNotAllBeTheSame(ReferenceData);
            DataElementsInListMustNotAllBeTheSame(ShiftingData);

            int NumberPossibleOfCorrelations = (ShiftingData.Count - ReferenceData.Count) + 1;

            List<(TimeSpan, double)> LagAndCorrelation = new List<(TimeSpan, double)>();

            for (int i = 0; i < NumberPossibleOfCorrelations; i++)
            {
                var trimmedList2WithTime = ShiftingData.GetRange(i, ReferenceData.Count).ToArray();

                var list1Value = ReferenceData.Select(x => x.Item2).ToList();
                var list2Value = trimmedList2WithTime.Select(x => x.Item2).ToList();

                var correlation = Correlation.Spearman(list1Value, list2Value);

                var lag = trimmedList2WithTime.First().Item1 - ReferenceData.First().Item1;

                LagAndCorrelation.Add((lag, correlation));
            }
            return LagAndCorrelation;
        }

        private static void ShiftingDataMustBeLongerThanRefernece<T>(List<T> reference, List<T> shifting)
        {
            if (reference.Count <= shifting.Count)
            {
                throw new ArgumentException("Reference data cannot be longer than shifting data");
            }

        }

        private static void DataElementsInListMustNotAllBeTheSame(List<(DateTime, double)> list)
        {
            if (list.Select(x => x.Item2).Distinct().Count() == 1)
            {
                throw new ArgumentException("Data in list must have variation");
            }
        }

        private static void ListMustNotBeEmpty<T>(List<T> list)
        {
            if (list.Count == 0)
            {
                throw new ArgumentException("List cannot be empty");
            }
        }

    }
}
