namespace WebApplication1.Services.Analysis
{
    public static class MeanShiftClustering //det her er V2
    {
        public static List<List<T>> Clusterv2<T>(List<(T, List<double>)> data, List<double> bandwidth)
        {
            double toleranceForIteration = 0.001;
            double toleranceForClustering = 0.01;
            double maxIteration = 1000;

            List<(T, List<double>)> endpointsMeanValue = new List<(T, List<double>)>();

            for (int dataIndex = 0; dataIndex < data.Count; dataIndex++)
            {
                List<double> mean = new List<double>(data[dataIndex].Item2);

                bool isWithinTolerance = false;
                int numberOfIterations = 0;
                while (!isWithinTolerance && numberOfIterations < maxIteration)
                {
                    var neighbouringPoints = new List<(T, List<double>)>();
                    // Find the neighbour datapoints
                    for (int i = dataIndex + 1; i < data.Count; i++)
                    {
                        var item = data[i];
                        if (isWithinBandwith(mean, item.Item2, bandwidth) && dataIndex != i)
                        {
                            neighbouringPoints.Add(item);
                        }
                    }

                    //Just add it as a new cluster if there are no neighbouring points
                    if (!(neighbouringPoints.Count > 0))
                    {
                        break;
                    }

                    List<double> shift = CalculateShift(mean, neighbouringPoints.Select(x => x.Item2).ToList(), bandwidth);
                    List<double> newMean = mean.Zip(shift, (m, s) => m + s).ToList();
                    isWithinTolerance = CheckElementwiseDifference(mean, newMean, toleranceForIteration);

                    //Prepare for next iteration
                    mean = newMean;
                    numberOfIterations++;
                }
                endpointsMeanValue.Add((data[dataIndex].Item1, mean));
            }
            return GroupPoints(endpointsMeanValue, toleranceForClustering);
        }






        public static List<List<T>> Cluster<T>(List<(T, List<double>)> data, List<double> bandwidth) //Det her et mit forsøg med´en af gangen.
        {
            double toleranceForClustering = 0.01;
            double learningFactor = 0.5;

            for (int q = 0; q < 100; q++)
            {

                for (int dataIndex = 0; dataIndex < data.Count; dataIndex++)
                {
                    List<double> mean = new List<double>(data[dataIndex].Item2);

                    bool isWithinTolerance = false;
                    //WHILE, tidligere
                    var neighbouringPoints = new List<(T, List<double>)>();
                    // Find the neighbour datapoints
                    for (int i = 0; i < data.Count; i++)
                    {
                        var item = data[i];
                        if (isWithinBandwith(mean, item.Item2, bandwidth) && dataIndex != i)
                        {
                            neighbouringPoints.Add(item);
                        }
                    }

                    if (!(neighbouringPoints.Count > 0))
                    {
                        break;
                    }

                    List<double> shift = CalculateShift(mean, neighbouringPoints.Select(x => x.Item2).ToList(), bandwidth);
                    shift = shift.Select(x => x * learningFactor).ToList();
                    List<double> newMean = mean.Zip(shift, (m, s) => m + s).ToList();

                    //Update location
                    data[dataIndex] = (data[dataIndex].Item1, newMean);

                }
            }
            var res = GroupPoints(data, toleranceForClustering);
            return res;
        }


        private static List<double> CalculateShiftBackupv1(List<double> dataPoint, List<List<double>> neighouringDataPoints, List<double> bandwidth)
        {
            List<List<double>> shifts = new List<List<double>>();
            foreach (var neighbourDataPoint in neighouringDataPoints)
            {
                List<double> difference = ABVector(dataPoint, neighbourDataPoint);
                List<double> absDifference = difference.Select(x => Math.Abs(x)).ToList();

                List<double> normalizedDifference = absDifference.Zip(bandwidth, (x, y) => x / y).ToList();
                double lengthOfNormalizedDataPoint = Math.Sqrt(normalizedDifference.Select(x => Math.Pow(x, 2)).Sum());
                double weight = normalFunction(0, 0.5)(lengthOfNormalizedDataPoint);
                List<double> ABvector = dataPoint.Zip(neighbourDataPoint, (a, b) => b - a).ToList();
                shifts.Add(ABvector.Select(x => x * weight).ToList());


            }
            List<double> shift = CalculateAverage(shifts);
            return shift;
        }

        private static List<double> CalculateShift(List<double> dataPoint, List<List<double>> neighouringDataPoints, List<double> bandwidth)
        {
            List<List<double>> shifts = new List<List<double>>();
            foreach (var neighbourDataPoint in neighouringDataPoints)
            {
                List<double> difference = ABVector(dataPoint, neighbourDataPoint);
                List<double> absDifference = difference.Select(x => Math.Abs(x)).ToList();
                double weight = absDifference.Zip(bandwidth, (dif, band) => 1 - (dif / band)).ToList().Average();

                //List<double> normalizedDifference = absDifference.Zip(bandwidth, (x, y) => x / y).ToList();
                //double lengthOfNormalizedDataPoint = Math.Sqrt(normalizedDifference.Select(x => Math.Pow(x, 2)).Sum());
                List<double> ABvector = dataPoint.Zip(neighbourDataPoint, (a, b) => b - a).ToList();
                shifts.Add(ABvector.Select(x => x * weight).ToList());


            }
            List<double> shift = CalculateAverage(shifts);
            return shift;
        }


        private static List<double> ABVector(List<double> point1, List<double> point2)
        {
            List<double> ABvector = point1.Zip(point2, (a, b) => b - a).ToList();
            return ABvector;
        }


        private static List<double> CalculateAverage(List<List<double>> shifts)
        {
            int count = shifts.Count;
            int length = shifts.First().Count;

            // Calculate the sum of corresponding elements across all lists
            var sums = shifts
                .SelectMany(innerList => innerList.Select((value, index) => new { value, index }))
                .GroupBy(pair => pair.index)
                .Select(group => group.Sum(pair => pair.value));

            // Calculate the average by dividing each sum by the total count of lists
            List<double> average = sums.Select(sum => sum / (double)count).ToList();

            return average;
        }




























        private static bool CheckElementwiseDifference(List<double> list1, List<double> list2, double threshold)
        {
            for (int i = 0; i < list1.Count; i++)
            {
                if (Math.Abs(list1[i] - list2[i]) >= threshold)
                    return false; //Difference exceeds threshold
            }
            return true; //All differences are within threshold
        }

        public static List<List<T>> GroupPoints<T>(List<(T, List<double>)> points, double tolerance)
        {
            List<List<(T, List<double>)>> groupedPoints = new List<List<(T, List<double>)>>();

            foreach (var point in points)
            {
                bool added = false;

                foreach (var group in groupedPoints)
                {
                    if (group.Any(p => CalculateDistance(p.Item2, point.Item2) <= tolerance))
                    {
                        group.Add(point);
                        added = true;
                        break;
                    }
                }

                if (!added)
                {
                    groupedPoints.Add(new List<(T, List<double>)> { point });
                }
            }

            return groupedPoints.Select(group => group.Select(p => p.Item1).ToList()).ToList();
        }


        public static double CalculateDistance(List<double> point1, List<double> point2)
        {
            double sumOfSquares = 0.0;
            for (int i = 0; i < point1.Count; i++)
            {
                double diff = point1[i] - point2[i];
                sumOfSquares += diff * diff;
            }

            return Math.Sqrt(sumOfSquares);
        }


        private static List<double> CalculateMean(List<List<double>> data)
        {
            List<double> mean = new List<double>();

            for (int i = 0; i < data[0].Count; i++)
            {
                double sum = 0;

                for (int j = 0; j < data.Count; j++)
                {
                    sum += data[j][i];
                }

                mean.Add(sum / data.Count);
            }

            return mean;
        }



        private static bool isWithinBandwith(List<double> a, List<double> b, List<double> bandwidth)
        {
            double distanceSquared = 0;

            for (int i = 0; i < a.Count; i++)
            {
                double normalizedDistance = (a[i] - b[i]) / bandwidth[i];

                distanceSquared += Math.Pow(normalizedDistance, 2);
            }
            return Math.Sqrt(distanceSquared) < 1.0;
        }

        private static List<double> GaussianKernel(List<double> distances)
        {
            Func<double, double> gaussianFunction = normalFunction(0, 0.5);
            return distances.Select(gaussianFunction).ToList();
        }

        private static Func<double, double> normalFunction(double mean, double stdDev)
        {
            return x => (1 / (stdDev * Math.Sqrt(2 * Math.PI))) * Math.Exp(-Math.Pow(x - mean, 2) / (2 * Math.Pow(stdDev, 2)));
        }

        private static List<double> ElementwiseAbsoluteDistance(List<double> point1, List<double> point2)
        {

            return point1.Zip(point2, (x, y) => Math.Abs(x - y)).ToList();

        }
    }
}
