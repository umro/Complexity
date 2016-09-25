using System.Linq;

namespace Complexity.Helpers
{
    public class Histogram
    {
        public struct Bin
        {
            public double LowerBound;
            public double UpperBound;
            public int Count;
        }

        public Bin[] Bins { get; private set; }

        public Histogram(double[] values)
        {
            // For now, bins are hard-coded to start at 0.03, 0.04, ..., 0.28
            Bins = new Bin[25];

            for (int i = 0; i < 25; i++)
            {
                Bins[i].LowerBound = (i / 100.0) + 0.03;
                Bins[i].UpperBound = Bins[i].LowerBound + 0.01;
                Bins[i].Count = CountData(values, Bins[i].LowerBound, Bins[i].UpperBound);
            }
        }

        private int CountData(double[] values, double start, double end)
        {
            return (from x in values where (start <= x && x < end) select x).Count();
        }
    }
}
