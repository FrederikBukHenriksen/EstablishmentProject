namespace WebApplication1.Services.Analysis
{
    public class ModifiedZScore
    {
        static void run()
        {
            // Example data
            List<double> data = new List<double> { 1, 2, 3, 4, 5, 100 };

            // Detect outliers using modified Z-score
            List<int> outliers = DetectOutliersModifiedZScore(data);

            // Display the results
            Console.WriteLine("Outliers detected at indices:");
            foreach (var index in outliers)
            {
                Console.WriteLine(index);
            }
        }

        static List<int> DetectOutliersModifiedZScore(List<double> data)
        {
            double median = CalculateMedian(data);
            double mad = CalculateMedianAbsoluteDeviation(data, median);

            // Set a threshold (e.g., 3.5) based on the desired level of sensitivity
            double threshold = 3.5;

            List<int> outliers = new List<int>();
            for (int i = 0; i < data.Count; i++)
            {
                double modifiedZScore = 0.6745 * Math.Abs((data[i] - median) / mad); // 0.6745 is a constant factor for consistency
                if (modifiedZScore > threshold)
                {
                    outliers.Add(i);
                }
            }

            return outliers;
        }

        static double CalculateMedian(List<double> data)
        {
            List<double> sortedData = data.OrderBy(x => x).ToList();
            int n = sortedData.Count;

            if (n % 2 == 0)
            {
                // If even number of elements, take the average of the middle two
                return (sortedData[n / 2 - 1] + sortedData[n / 2]) / 2.0;
            }
            else
            {
                // If odd number of elements, take the middle element
                return sortedData[n / 2];
            }
        }

        static double CalculateMedianAbsoluteDeviation(List<double> data, double median)
        {
            List<double> absoluteDeviations = data.Select(x => Math.Abs(x - median)).ToList();
            return CalculateMedian(absoluteDeviations);
        }

    }
}
