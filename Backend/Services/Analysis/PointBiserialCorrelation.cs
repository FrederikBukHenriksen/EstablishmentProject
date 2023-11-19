namespace WebApplication1.Services.Analysis
{
    public class PointBiserialCorrelation
    {

        public static double CalculatePointBiserialCorrelation(List<double> numericalData, List<int> binarySequence)
        {
            if (numericalData.Count != binarySequence.Count)
            {
                throw new ArgumentException("Input sequences must have the same length.");
            }

            int n = numericalData.Count;

            double meanNumerical = numericalData.Average();
            double sdNumerical = Math.Sqrt(numericalData.Sum(x => Math.Pow(x - meanNumerical, 2)) / (n - 1));

            double sumPositive = 0;
            double sumNegative = 0;

            for (int i = 0; i < n; i++)
            {
                if (binarySequence[i] == 1)
                {
                    sumPositive += Math.Pow((numericalData[i] - meanNumerical) / sdNumerical, 2);
                }
                else
                {
                    sumNegative += Math.Pow((numericalData[i] - meanNumerical) / sdNumerical, 2);
                }
            }

            double correlation = Math.Sqrt(sumPositive / (sumPositive + sumNegative));

            return correlation;
        }
    }
}
