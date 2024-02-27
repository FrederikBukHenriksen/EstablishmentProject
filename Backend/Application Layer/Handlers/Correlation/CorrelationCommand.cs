using DMIOpenData;
using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Utils;

namespace WebApplication1.Application_Layer.Handlers.Correlation
{
    public abstract class CorrelationCommand : ICommand, ICmdField_SalesIds
    {
        public Guid EstablishmentId { get; set; }
        public List<Guid> SalesIds { get; set; }
        public DateTimePeriod TimePeriod { get; set; }
        public Coordinates Coordinates { get; set; }
        public TimeResolution TimeResolution { get; set; }
        public int UpperLag { get; set; }
        public int LowerLag { get; set; }
    }

    public class CorrelationReturn : ReturnBase
    {
        public List<(int, double)> LagAndCorrelation { get; set; }
        public List<(DateTime dateTime, List<double?> values)> calculationValues { get; set; }



        public CorrelationReturn(
            List<(int, double)> lagAndCorrelation,
            List<(DateTime, double)> referenceList,
            List<(DateTime, double)> shiftingList

        )
        {
            this.LagAndCorrelation = lagAndCorrelation.OrderBy(x => x.Item1).ToList();
            this.calculationValues = this.MergeLists(referenceList, shiftingList).OrderBy(x => x.Item1).ToList();
        }

        //public CorrelationReturn(
        //    Dictionary<int, double> lagAndCorrelation,
        //    Dictionary<DateTime, (double?, double?)> calculationValues
        //)
        //{
        //    this.LagAndCorrelation = lagAndCorrelation.Select(x => (x.Key, x.Value)).OrderBy(x => x.Key).ToList();
        //    this.calculationValues = calculationValues.Select(kv => (dateTime: kv.Key, values: new List<double?> { kv.Value.Item1, kv.Value.Item2 }))
        //    .OrderBy(x => x.dateTime).ToList();
        //}

        private List<(DateTime, List<double?>)> MergeLists(List<(DateTime, double)> reference, List<(DateTime, double)> shifting)
        {
            var mergedList = new List<(DateTime, List<double?>)>();

            // Get unique dates from both lists
            var allDates = reference.Select(x => x.Item1).Union(shifting.Select(x => x.Item1)).Distinct().ToList();

            foreach (var date in allDates)
            {
                var listEntry = new List<double?>();

                var list1findIndex = reference.FindIndex(x => x.Item1 == date);
                if (list1findIndex != -1)
                {
                    listEntry.Add(reference[list1findIndex].Item2);
                }
                else
                {
                    listEntry.Add(null);
                }

                var list2findIndex = shifting.FindIndex(x => x.Item1 == date);
                if (list2findIndex != -1)
                {
                    listEntry.Add(shifting[list2findIndex].Item2);
                }
                else
                {
                    listEntry.Add(null);
                }

                mergedList.Add((date, listEntry));
            }
            return mergedList;
        }

    }
}
