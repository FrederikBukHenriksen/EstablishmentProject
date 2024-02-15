//namespace WebApplication1.Services.Analysis
//{
//    public static class MeanShiftClustering
//    {
//        //public static List<List<T>> Cluster<T>(List<(T element, List<double> point)> data, List<double> bandwidths)
//        //{
//        //    List<List<T>> result = new List<List<T>>();

//        //    foreach (var point in data)
//        //    {
//        //        // Find points within the bandwidth for each dimension
//        //        var neighbors = data.Where(p =>
//        //            Enumerable.Range(0, bandwidths.Count)
//        //                      .All(dim => Math.Abs(point.point[dim] - p.point[dim]) < bandwidths[dim])).ToList();

//        //        // Update the point's position to the mean of its neighbors
//        //        for (int i = 0; i < bandwidths.Count; i++)
//        //        {
//        //            point.point[i] = neighbors.Select(p => p.point[i]).Average();
//        //        }
//        //    }

//        //    // Assign clusters based on the final positions
//        //    foreach (var point in data)
//        //    {
//        //        if (!result.Any(cluster => cluster.Any(p => p.Equals(point.element))))
//        //        {
//        //            // Find and update points in the same cluster
//        //            var cluster = data.Where(p =>
//        //                Enumerable.Range(0, bandwidths.Count)
//        //                          .All(dim => Math.Abs(point.point[dim] - p.point[dim]) < bandwidths[dim])).ToList();

//        //            result.Add(cluster.Select(p => p.Item1).ToList());
//        //        }
//        //    }

//        //    return result;
//        //}

//        private static List<double> CalculateMean(List<List<double>> data)
//        {
//            List<double> mean = new List<double>();

//            for (int i = 0; i < data[0].Count; i++)
//            {
//                double sum = 0;

//                for (int j = 0; j < data.Count; j++)
//                {
//                    sum += data[j][i];
//                }

//                mean.Add(sum / data.Count);
//            }

//            return mean;
//        }

//        private static bool isWithinBandwith(List<double> a, List<double> b, List<double> bandwidth)
//        {
//            double distanceSquared = 0;

//            for (int i = 0; i < a.Count; i++)
//            {
//                double normalizedDistance = (a[i] - b[i]) / bandwidth[i];

//                distanceSquared += Math.Pow(normalizedDistance, 2);
//            }
//            return Math.Sqrt(distanceSquared) < 1.0;
//        }

//        public static List<List<T>> Cluster<T>(List<(T, List<double>)> data, List<double> bandwidth)
//        {
//            List<List<T>> clusters = new List<List<T>>();

//            while (data.Count > 0)
//            {
//                var mean = new List<double>(data[0].Item2);
//                var inCluster = new List<(T, List<double>)>();

//                //Find the neighbours of the mean (first data point in the list)
//                for (int i = 0; i < data.Count; i++)
//                {
//                    if (isWithinBandwith(mean, data[i].Item2, bandwidth))
//                    {
//                        inCluster.Add(data[i]);
//                    }
//                }

//                //Iterate over the neighbours until the mean converges
//                while (inCluster.Count > 0)
//                {
//                    var newMean = new List<double>(inCluster[0].Item2);
//                    var newInCluster = new List<(T, List<double>)>();

//                    for (int i = 0; i < inCluster.Count; i++)
//                    {
//                        if (isWithinBandwith(newMean, inCluster[i].Item2, bandwidth))
//                        {
//                            newInCluster.Add(inCluster[i]);
//                        }
//                    }

//                    if (newInCluster.Count == inCluster.Count)
//                    {
//                        break;
//                    }

//                    inCluster = newInCluster;
//                    mean = CalculateMean(inCluster.Select(item => item.Item2).ToList());
//                }

//                clusters.Add(inCluster.Select(item => item.Item1).ToList());
//                foreach (var point in inCluster)
//                {
//                    data.Remove(point);
//                }
//            }

//            return clusters;
//        }
//    }
//}






