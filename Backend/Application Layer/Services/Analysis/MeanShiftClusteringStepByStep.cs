namespace WebApplication1.Services.Analysis
{
    public interface IMeanShiftClustering
    {
        List<List<T>> Cluster<T>(List<(T, List<double>)> data, List<double> bandwidth);
    }

    public class MeanShiftDataPoint<T>
    {
        public T Object { get; }
        public List<double> Location { get; set; }
        public bool HasConverged { get; set; } = false;
        public bool IsGrouped { get; set; } = false;
        public MeanShiftDataPoint(T obj, List<double> loc)
        {
            this.Object = obj;
            this.Location = loc;
        }
    }

    public class MeanShiftClusteringStepByStep : IMeanShiftClustering
    {
        private double toleranceForCovergence = 0.01;
        private double toleranceForGrouping = 0.1;

        public List<List<T>> Cluster<T>(List<(T, List<double>)> data, List<double> bandwidth)
        {
            List<MeanShiftDataPoint<T>> dataWithConvergence = data.Select(x => new MeanShiftDataPoint<T>(x.Item1, x.Item2)).ToList();

            while (!dataWithConvergence.All(x => x.HasConverged))
            {
                foreach (var dataPoint in dataWithConvergence)
                {
                    if (dataPoint.HasConverged)
                    {
                        continue;
                    }

                    //Identify neighbouring data points
                    var neighbouringPoints = new List<MeanShiftDataPoint<T>>();
                    foreach (var neighbourDataPoint in dataWithConvergence)
                    {
                        if (ClusterHelper.isWithinBandwith(dataPoint.Location, neighbourDataPoint.Location, bandwidth) && dataPoint != neighbourDataPoint)
                        {
                            neighbouringPoints.Add(neighbourDataPoint);
                        }
                    }

                    //Calculate and perform shift
                    List<double> shift = ClusterHelper.CalculateShift(dataPoint.Location, neighbouringPoints.Select(x => x.Location).ToList(), bandwidth);
                    List<double> newLocation = dataPoint.Location.Zip(shift, (m, s) => m + s).ToList();

                    //Check for convergence
                    var hasPointConverged = ClusterHelper.HasPointsConverged(dataPoint.Location, newLocation, this.toleranceForCovergence);
                    if (hasPointConverged)
                    {
                        dataPoint.HasConverged = true;
                    }

                    //Update location
                    dataPoint.Location = newLocation;
                }
            }
            //Send the all datapoints to grouping after convergenve
            return ClusterHelper.GroupPoints(dataWithConvergence, this.toleranceForGrouping);
        }
    }

    public class MeanShiftClusteringDirectly : IMeanShiftClustering
    {
        private double toleranceForCovergence = 0.01;
        private double toleranceForGrouping = 0.1;

        public List<List<T>> Cluster<T>(List<(T, List<double>)> data, List<double> bandwidth)
        {
            List<MeanShiftDataPoint<T>> dataWithConvergence = data.Select(x => new MeanShiftDataPoint<T>(x.Item1, x.Item2)).ToList();

            foreach (var dataPoint in dataWithConvergence)
            {
                while (!dataPoint.HasConverged)
                {
                    //Identify neighbouring data points
                    var neighbouringPoints = new List<MeanShiftDataPoint<T>>();
                    foreach (var neighbourDataPoint in dataWithConvergence)
                    {
                        if (ClusterHelper.isWithinBandwith(dataPoint.Location, neighbourDataPoint.Location, bandwidth) && dataPoint != neighbourDataPoint)
                        {
                            neighbouringPoints.Add(neighbourDataPoint);
                        }
                    }

                    //Calculate and perform shift
                    List<double> shift = ClusterHelper.CalculateShift(dataPoint.Location, neighbouringPoints.Select(x => x.Location).ToList(), bandwidth);
                    List<double> newLocation = dataPoint.Location.Zip(shift, (m, s) => m + s).ToList();

                    //Check for convergence
                    var hasPointConverged = ClusterHelper.HasPointsConverged(dataPoint.Location, newLocation, this.toleranceForCovergence);
                    if (hasPointConverged)
                    {
                        dataPoint.HasConverged = true;
                    }

                    //Update location
                    dataPoint.Location = newLocation;
                }
            }
            //Send the all datapoints to grouping after convergenve
            return ClusterHelper.GroupPoints(dataWithConvergence, this.toleranceForGrouping);
        }
    }

    public static class ClusterHelper
    {

        public static bool isWithinBandwith(List<double> point1, List<double> point2, List<double> bandwidth)
        {
            double distanceSquared = 0;
            for (int i = 0; i < point1.Count; i++)
            {
                double normalizedDistance = (point1[i] - point2[i]) / bandwidth[i];

                distanceSquared += Math.Pow(normalizedDistance, 2);
            }
            return Math.Sqrt(distanceSquared) <= 1.0;
        }

        public static List<double> CalculateShift(List<double> dataPoint, List<List<double>> neighouringDataPoints, List<double> bandwidth)
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

        public static List<double> CalculateAverage(List<List<double>> shifts)
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

        public static List<List<T>> GroupPoints<T>(List<MeanShiftDataPoint<T>> AllPointsConverged, double tolerance)
        {
            List<List<MeanShiftDataPoint<T>>> Clusters = new List<List<MeanShiftDataPoint<T>>>();

            foreach (var dataPoint in AllPointsConverged)
            {
                foreach (var cluster in Clusters)
                {
                    if (cluster.Any(dataPointInCluster => CalculateDistance(dataPoint.Location, dataPointInCluster.Location) <= tolerance))
                    {
                        cluster.Add(dataPoint);
                        dataPoint.IsGrouped = true;
                        break;
                    }
                }
                if (!dataPoint.IsGrouped)
                {
                    Clusters.Add(new List<MeanShiftDataPoint<T>> { dataPoint });
                }
            }

            return Clusters.Select(cluster => cluster.Select(dataPoint => dataPoint.Object).ToList()).ToList();
        }

        public static bool HasPointsConverged(List<double> list1, List<double> list2, double threshold)
        {
            var vetor = VectorFromTo(list1, list2);
            var absVectorValues = vetor.Select(x => Math.Abs(x)).ToList();
            return absVectorValues.All(x => x <= threshold);
        }

        public static List<double> VectorFromTo(List<double> point1, List<double> point2)
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
