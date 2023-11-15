//namespace WebApplication1.Services.Analysis
//{
//    public class MeanShiftClustering
//    {
//        public List<List<double>> Cluster(List<List<double>> data, double bandwidth)
//        {
//            List<List<double>> clusters = new List<List<double>>();

//            while (data.Count > 0)
//            {
//                List<double> mean = new List<double>(data[0]);
//                List<List<double>> inCluster = new List<List<double>>();

//                for (int i = 0; i < data.Count; i++)
//                {
//                    if (EuclideanDistance(mean, data[i]) < bandwidth)
//                    {
//                        inCluster.Add(data[i]);
//                    }
//                }

//                while (inCluster.Count > 0)
//                {
//                    List<double> newMean = new List<double>(inCluster[0]);
//                    List<List<double>> newInCluster = new List<List<double>>();

//                    for (int i = 0; i < inCluster.Count; i++)
//                    {
//                        if (EuclideanDistance(newMean, inCluster[i]) < bandwidth)
//                        {
//                            newInCluster.Add(inCluster[i]);
//                        }
//                    }

//                    if (newInCluster.Count == inCluster.Count)
//                    {
//                        break;
//                    }

//                    inCluster = newInCluster;
//                    mean = CalculateMean(inCluster);
//                }

//                clusters.Add(inCluster);
//                foreach (List<double> point in inCluster)
//                {
//                    data.Remove(point);
//                }
//            }

//            return clusters;
//        }

//        private double EuclideanDistance(List<double> a, List<double> b)
//        {
//            double distance = 0;

//            for (int i = 0; i < a.Count; i++)
//            {
//                distance += (a[i] - b[i]) * (a[i] - b[i]);
//            }

//            return System.Math.Sqrt(distance);
//        }

//        private List<double> CalculateMean(List<List<double>> data)
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
//    }

    
//}
