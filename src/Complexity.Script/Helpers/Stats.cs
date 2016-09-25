using System;
using System.Linq;

namespace Complexity.Helpers
{
    public static class Stats
    {
        // Return the number of standard deviations
        // that x is relative to the mean of values
        public static double StdDevs(double x, double[] values)
        {
            return (x - values.Average()) / StdDev(values);
        }

        public static double StdDev(double[] values)
        {
            return Math.Sqrt(SumOfSquares(values) / values.Length);
        }

        public static double SumOfSquares(double[] values)
        {
            double mean = values.Average();
            return (from x in values select Square(x - mean)).Sum();
        }

        public static double Square(double x)
        {
            return x * x;
        }
    }
}
