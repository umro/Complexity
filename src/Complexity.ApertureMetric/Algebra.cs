using System;
using System.Collections.Generic;
using System.Linq;

namespace Complexity.ApertureMetric
{
    static class Algebra
    {
        public static double Sum(int start, int end, Func<int, double> func)
        {
            return Enumerable.Range(start, end - start + 1).Sum(func);
        }

        public static double Mean(int start, int end, Func<int, double> func)
        {
            return Sum(start, end, func) / (end - start + 1);
        }

        public static double Distance(double x, double y)
        {
            return Math.Abs(x - y);
        }

        public static IEnumerable<int> Sequence(int n)
        {
            return Enumerable.Range(0, n);
        }
    }
}
