using Microsoft.IdentityModel.Tokens;

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
        public bool IsClustered { get; set; } = false;
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
            ClusterHelper.DimensionWithinTheDataMustBeTheSame(data.Select(x => x.Item2).ToList());
            ClusterHelper.BandwidthMustBePositive(bandwidth);
            ClusterHelper.DataAndBandwidthDimensionMustMatch(data.Select(x => x.Item2).ToList(), bandwidth);
            List<MeanShiftDataPoint<T>> dataWithConvergence = data.Select(x => new MeanShiftDataPoint<T>(x.Item1, x.Item2)).ToList();


            while (!dataWithConvergence.All(x => x.HasConverged))
            {
                var res = this.SelectNextDataPointAndReevaluteAllDataPointsConvergence(dataWithConvergence, bandwidth, this.toleranceForCovergence);
                var dataPoint = res.Object;
                var shift = res.Shift;

                //Check for convergence
                dataPoint.Location = dataPoint.Location.Zip(shift, (m, s) => m + s).ToList();
                if (ClusterHelper.HasPointConverged(shift, this.toleranceForCovergence))
                {
                    dataPoint.HasConverged = true;
                }
            }
            //Send the all datapoints to grouping after convergenve
            return ClusterHelper.GroupPoints(dataWithConvergence, this.toleranceForGrouping);
        }

        private (MeanShiftDataPoint<T> Object, List<double> Shift) SelectNextDataPointAndReevaluteAllDataPointsConvergence<T>(List<MeanShiftDataPoint<T>> dataWithConvergence, List<double> bandwidth, double convergenceTolerance)
        {
            List<(MeanShiftDataPoint<T> Object, List<double> Shift, int Neighbours)> shiftingMagnitudeAndNeighbours = new List<(MeanShiftDataPoint<T>, List<double>, int)>();
            foreach (var dataPoint in dataWithConvergence)
            {
                //Calculate number of neighbours and shift
                var neighbouringPoints = ClusterHelper.FindNeighbours(dataPoint, dataWithConvergence, bandwidth);
                List<double> shift = ClusterHelper.CalculateShift(dataPoint.Location, neighbouringPoints.Select(x => x.Location).ToList(), bandwidth);
                shiftingMagnitudeAndNeighbours.Add((dataPoint, shift, neighbouringPoints.Count));

                //Reevaluate convergence
                if (!ClusterHelper.HasPointConverged(shift, convergenceTolerance))
                {
                    dataPoint.HasConverged = false;
                }
            }
            var ordered = shiftingMagnitudeAndNeighbours.Where(x => x.Object.HasConverged == false).OrderBy(x => x.Neighbours).ThenBy(x => ClusterHelper.VectorLength(x.Shift)).ToList();
            return (ordered.First().Object, ordered.First().Shift);
        }
    }

    public class MeanShiftClusteringStationary : IMeanShiftClustering
    {
        private double toleranceForCovergence = 0.01;
        private double toleranceForGrouping = 0.1;

        public List<List<T>> Cluster<T>(List<(T, List<double>)> data, List<double> bandwidth)
        {
            ClusterHelper.DimensionWithinTheDataMustBeTheSame(data.Select(x => x.Item2).ToList());
            ClusterHelper.BandwidthMustBePositive(bandwidth);
            ClusterHelper.DataAndBandwidthDimensionMustMatch(data.Select(x => x.Item2).ToList(), bandwidth);
            List<MeanShiftDataPoint<T>> dataWithConvergence = data.Select(x => new MeanShiftDataPoint<T>(x.Item1, x.Item2)).ToList();
            List<MeanShiftDataPoint<T>> dataWithConvergenceRefernece = data.Select(x => new MeanShiftDataPoint<T>(x.Item1, x.Item2)).ToList();


            foreach (var dataPoint in dataWithConvergence)
            {
                while (!dataPoint.HasConverged)
                {
                    //Identify neighbouring data points
                    var neighbouringPoints = ClusterHelper.FindNeighbours(dataPoint, dataWithConvergenceRefernece, bandwidth);

                    //Calculate and perform shift
                    List<double> shift = ClusterHelper.CalculateShift(dataPoint.Location, neighbouringPoints.Select(x => x.Location).ToList(), bandwidth);

                    //Check for convergence
                    if (ClusterHelper.HasPointConverged(shift, this.toleranceForCovergence))
                    {
                        var test = neighbouringPoints.Average(x => x.Location[0]);
                        dataPoint.HasConverged = true;
                    }
                    else
                    {
                        dataPoint.Location = dataPoint.Location.Zip(shift, (m, s) => m + s).ToList();
                    }
                }
            }
            //Send the all datapoints to grouping after convergenve
            return ClusterHelper.GroupPoints(dataWithConvergence, this.toleranceForGrouping);
        }
    }

    public static class ClusterHelper
    {

        public static void DataAndBandwidthDimensionMustMatch(List<List<double>> data, List<double> bandwidth)
        {
            if (!data.All(x => x.Count == bandwidth.Count))
            {
                throw new ArgumentException("Data and bandwidth dimension must match");
            }
        }

        public static void BandwidthMustBePositive(List<double> bandwidth)
        {
            if (bandwidth.Any(x => x <= 0))
            {
                throw new ArgumentException("Bandwidth must be positive");
            }
        }

        public static void DimensionWithinTheDataMustBeTheSame(List<List<double>> data)
        {
            if (!data.All(x => x.Count == data.First().Count))
            {
                throw new ArgumentException("Dimension of the data must be the same");
            }
        }


        public static List<MeanShiftDataPoint<T>> FindNeighbours<T>(MeanShiftDataPoint<T> dataPoint, List<MeanShiftDataPoint<T>> dataWithConvergence, List<double> bandwidth)
        {
            var neighbouringPoints = new List<MeanShiftDataPoint<T>>();

            foreach (var neighbourDataPoint in dataWithConvergence)
            {
                if (isWithinBandwith(dataPoint.Location, neighbourDataPoint.Location, bandwidth))
                {
                    neighbouringPoints.Add(neighbourDataPoint);
                }
            }
            return neighbouringPoints;
        }


        public static bool isWithinBandwith(List<double> point1, List<double> point2, List<double> bandwidth)
        {
            double distanceSquared = 0;
            for (int i = 0; i < point1.Count; i++)
            {
                double normalizedDistance = (point1[i] - point2[i]) / bandwidth[i];

                distanceSquared += Math.Pow(normalizedDistance, 2);
            }
            var distance = Math.Sqrt(distanceSquared);
            var isWithin = distance <= 1.0;
            return isWithin;
        }

        public static List<double> CalculateShift(List<double> dataPoint, List<List<double>> neighouringDataPoints, List<double> bandwidth)
        {
            List<List<double>> shifts = new List<List<double>>();
            if (!neighouringDataPoints.IsNullOrEmpty())
            {
                foreach (var neiDataPoint in neighouringDataPoints)
                {
                    List<double> vector = VectorFromTo(dataPoint, neiDataPoint);
                    List<double> absVectorValues = vector.Select(x => Math.Abs(x)).ToList();
                    double weight = absVectorValues.Zip(bandwidth, (dif, band) => 1 - (dif / band)).ToList().Average();
                    shifts.Add(vector.Select(x => x * weight).ToList());
                }
            }
            if (shifts.IsNullOrEmpty())
            {
                return Enumerable.Repeat(0.0, dataPoint.Count).ToList();
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

            List<double> average = sums.Select(sum => sum == 0 ? 0 : sum / count).ToList();

            return average;
        }

        public static List<List<T>> GroupPoints<T>(List<MeanShiftDataPoint<T>> AllPointsConverged, double tolerance)
        {
            List<List<MeanShiftDataPoint<T>>> Clusters = new List<List<MeanShiftDataPoint<T>>>();

            foreach (var dataPoint in AllPointsConverged)
            {
                if (dataPoint.IsClustered)
                {
                    continue;
                }

                foreach (var cluster in Clusters)
                {
                    if (cluster.Any(dataPointInCluster => CalculateDistance(dataPoint.Location, dataPointInCluster.Location) <= tolerance))
                    {
                        cluster.Add(dataPoint);
                        dataPoint.IsClustered = true;
                        break;
                    }
                }
                if (!dataPoint.IsClustered)
                {
                    Clusters.Add(new List<MeanShiftDataPoint<T>> { dataPoint });
                    dataPoint.IsClustered = true;
                }
            }

            return Clusters.Select(cluster => cluster.Select(dataPoint => dataPoint.Object).ToList()).ToList();
        }

        public static bool HasPointConverged(List<double> shift, double threshold)
        {
            var absVectorValues = shift.Select(x => Math.Abs(x)).ToList();
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

        public static double VectorLength(List<double> vector)
        {
            double sumOfSquares = 0.0;

            foreach (double component in vector)
            {
                sumOfSquares += component * component;
            }

            return Math.Sqrt(sumOfSquares);
        }
    }
}
