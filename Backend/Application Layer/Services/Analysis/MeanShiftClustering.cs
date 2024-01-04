using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Services.Analysis
{
    public static class MeanShiftClustering
    {

        //public static List<List<double>> Cluster(List<List<double>> data, List<double> bandwidth)
        //{
        //    List<List<double>> clusters = new List<List<double>>();

        //    while (data.Count > 0)
        //    {
        //        List<double> mean = new List<double>(data[0]);
        //        List<List<double>> inCluster = new List<List<double>>();

        //        for (int i = 0; i < data.Count; i++)
        //        {
        //            if (EuclideanDistance(mean, data[i]) < bandwidth[i])
        //            {
        //                inCluster.Add(data[i]);
        //            }
        //        }

        //        while (inCluster.Count > 0)
        //        {
        //            List<double> newMean = new List<double>(inCluster[0]);
        //            List<List<double>> newInCluster = new List<List<double>>();

        //            for (int i = 0; i < inCluster.Count; i++)
        //            {
        //                if (EuclideanDistance(newMean, inCluster[i]) < bandwidth[i])
        //                {
        //                    newInCluster.Add(inCluster[i]);
        //                }
        //            }

        //            if (newInCluster.Count == inCluster.Count)
        //            {
        //                break;
        //            }

        //            inCluster = newInCluster;
        //            mean = CalculateMean(inCluster);
        //        }

        //        clusters.AddRange(inCluster);
        //        foreach (List<double> point in inCluster)
        //        {
        //            data.Remove(point);
        //        }
        //    }

        //    return clusters;
        //}

        //private static double EuclideanDistance(List<double> a, List<double> b)
        //{
        //    double distance = 0;

        //    for (int i = 0; i < a.Count; i++)
        //    {
        //        distance += (a[i] - b[i]) * (a[i] - b[i]);
        //    }

        //    return System.Math.Sqrt(distance);
        //}

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

        private static bool EuclideanDistance(List<double> a, List<double> b, List<double> bandwidth)
        {
            if (a.Count != b.Count || a.Count != bandwidth.Count)
            {
                throw new ArgumentException("Dimensions of vectors and bandwidths must match");
            }

            double distanceSquared = 0;

            for (int i = 0; i < a.Count; i++)
            {
                distanceSquared += Math.Pow((a[i] - b[i]) / bandwidth[i], 2);
            }

            return Math.Sqrt(distanceSquared) < 1.0; // Adjust this threshold as needed
        }

        public static List<List<T>> Cluster<T>(List<(T, List<double>)> data, List<double> bandwidth)
        {
            List<List<T>> clusters = new List<List<T>>();

            while (data.Count > 0)
            {
                var mean = new List<double>(data[0].Item2);
                var inCluster = new List<(T, List<double>)>();

                for (int i = 0; i < data.Count; i++)
                {
                    if (EuclideanDistance(mean, data[i].Item2, bandwidth))
                    {
                        inCluster.Add(data[i]);
                    }
                }

                while (inCluster.Count > 0)
                {
                    var newMean = new List<double>(inCluster[0].Item2);
                    var newInCluster = new List<(T, List<double>)>();

                    for (int i = 0; i < inCluster.Count; i++)
                    {
                        if (EuclideanDistance(newMean, inCluster[i].Item2, bandwidth))
                        {
                            newInCluster.Add(inCluster[i]);
                        }
                    }

                    if (newInCluster.Count == inCluster.Count)
                    {
                        break;
                    }

                    inCluster = newInCluster;
                    mean = CalculateMean(inCluster.Select(item => item.Item2).ToList());
                }

                clusters.Add(inCluster.Select(item => item.Item1).ToList());
                foreach (var point in inCluster)
                {
                    data.Remove(point);
                }
            }

            return clusters;
        }

    }
    

}

    
