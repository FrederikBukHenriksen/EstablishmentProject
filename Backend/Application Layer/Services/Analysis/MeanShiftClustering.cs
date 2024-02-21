namespace WebApplication1.Services.Analysis
{
    public static class MeanShiftClustering
    {
        public class MeanShiftDataPoint<T>
        {
            public T Object { get; set; }
            public List<double> Location { get; set; }
            public bool HasConverged { get; set; }

        }


        public static List<List<T>> Cluster<T>(List<(T, List<double>)> data, List<double> bandwidth)
        {

            double toleranceForCovergence = 0.01;

            double toleranceForGrouping = 0.01;

            double learningFactor = 0.5;

            List<(T, List<double>, bool)> dataWithConvergence = data.Select(x => (x.Item1, x.Item2, false)).ToList();


            //for (int q = 0; q < 100; q++)
            while (!dataWithConvergence.All(x => x.Item3 == true))
            {
                for (int dataIndex = 0; dataIndex < dataWithConvergence.Count; dataIndex++)
                {
                    if (dataWithConvergence[dataIndex].Item3 == true)
                    {
                        break;
                    }

                    List<double> location = new List<double>(dataWithConvergence[dataIndex].Item2);

                    bool isWithinTolerance = false;

                    var neighbouringPoints = new List<(T, List<double>)>();
                    for (int i = 0; i < dataWithConvergence.Count; i++)
                    {
                        var item = dataWithConvergence[i];
                        if (isWithinBandwith(location, item.Item2, bandwidth) && dataIndex != i)
                        {
                            neighbouringPoints.Add((item.Item1, item.Item2));
                        }
                    }

                    //If no neighbouring points, convergence is reached
                    if (!(neighbouringPoints.Count > 0))
                    {
                        dataWithConvergence[dataIndex] = (dataWithConvergence[dataIndex].Item1, dataWithConvergence[dataIndex].Item2, true);
                        break;
                    }
                    //Calculate and perform shift
                    List<double> shift = CalculateShift(location, neighbouringPoints.Select(x => x.Item2).ToList(), bandwidth);
                    shift = shift.Select(x => x * learningFactor).ToList();
                    List<double> newLocation = location.Zip(shift, (m, s) => m + s).ToList();

                    //Check for convergence
                    var hasPointConverged = HasPointsConverged(dataWithConvergence[dataIndex].Item2, newLocation, toleranceForCovergence);
                    if (hasPointConverged)
                    {
                        dataWithConvergence[dataIndex] = (dataWithConvergence[dataIndex].Item1, dataWithConvergence[dataIndex].Item2, true);
                    }

                    //Update location
                    dataWithConvergence[dataIndex] = (dataWithConvergence[dataIndex].Item1, newLocation, dataWithConvergence[dataIndex].Item3);
                }
            }
            var res = GroupPoints(dataWithConvergence.Select(x => (x.Item1, x.Item2)).ToList(), toleranceForGrouping);
            return res;
        }


        private static bool isWithinBandwith(List<double> point1, List<double> point2, List<double> bandwidth)
        {
            double distanceSquared = 0;
            for (int i = 0; i < point1.Count; i++)
            {
                double normalizedDistance = (point1[i] - point2[i]) / bandwidth[i];

                distanceSquared += Math.Pow(normalizedDistance, 2);
            }
            return Math.Sqrt(distanceSquared) <= 1.0;
        }

        private static List<double> CalculateShift(List<double> dataPoint, List<List<double>> neighouringDataPoints, List<double> bandwidth)
        {
            List<List<double>> shifts = new List<List<double>>();
            foreach (var neiDataPoint in neighouringDataPoints)
            {
                List<double> vector = VectorFromTo(dataPoint, neiDataPoint);
                List<double> absVectorValues = vector.Select(x => Math.Abs(x)).ToList();
                double weight = absVectorValues.Zip(bandwidth, (dif, band) => 1 - (dif / band)).ToList().Average();
                shifts.Add(vector.Select(x => x * weight).ToList());
            }
            List<double> shift = CalculateAverage(shifts);
            return shift;
        }

        private static List<double> CalculateAverage(List<List<double>> shifts)
        {
            int count = shifts.Count;
            int length = shifts.First().Count;

            var sums = shifts
                .SelectMany(innerList => innerList.Select((value, index) => new { value, index }))
                .GroupBy(pair => pair.index)
                .Select(group => group.Sum(pair => pair.value));

            List<double> average = sums.Select(sum => sum / (double)count).ToList();

            return average;
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


        private static bool HasPointsConverged(List<double> list1, List<double> list2, double threshold)
        {
            var vetor = VectorFromTo(list1, list2);
            var absVectorValues = vetor.Select(x => Math.Abs(x)).ToList();
            return absVectorValues.All(x => x <= threshold);
        }

        private static List<double> VectorFromTo(List<double> point1, List<double> point2)
        {
            List<double> ABvector = point1.Zip(point2, (a, b) => b - a).ToList();
            return ABvector;
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



    }
}
