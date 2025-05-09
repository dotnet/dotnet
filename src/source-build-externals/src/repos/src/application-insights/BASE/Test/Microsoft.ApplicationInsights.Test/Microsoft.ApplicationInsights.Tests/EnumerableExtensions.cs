
namespace Microsoft.ApplicationInsights
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal static class EnumerableExtensions
    {
        public static double StdDev(this IEnumerable<double> sequence)
        {
            return StdDev(sequence, (e) => e);
        }

        public static double StdDev<T>(this IEnumerable<T> sequence, Func<T, double> selector)
        {
            if (sequence.Count() <= 0)
            {
                return 0;
            }

            double avg = sequence.Average(selector);
            double sum = sequence.Sum(e => Math.Pow(selector(e) - avg, 2));

            return Math.Sqrt(sum / sequence.Count());
        }
    }
}
