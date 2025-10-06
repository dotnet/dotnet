namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Metrics
{
    using System;

    internal class DurationBucketizer
    {
        private static Tuple<string, long>[] perfBuckets = new Tuple<string, long>[11]
        {
            new Tuple<string, long>("<250ms", 250),
            new Tuple<string, long>("250ms-500ms", 500),
            new Tuple<string, long>("500ms-1sec", 1000),
            new Tuple<string, long>("1sec-3sec", 3000),
            new Tuple<string, long>("3sec-7sec", 7000),
            new Tuple<string, long>("7sec-15sec", 15000),
            new Tuple<string, long>("15sec-30sec", 30000),
            new Tuple<string, long>("30sec-1min", 60000),
            new Tuple<string, long>("1min-2min", 120000),
            new Tuple<string, long>("2min-5min", 300000),
            new Tuple<string, long>(">=5min", int.MaxValue),
        };

        public static string GetPerformanceBucket(TimeSpan duration)
        {
            for (int i = 0; i < perfBuckets.Length; i++)
            {
                var bucket = perfBuckets[i];
                if (duration.TotalMilliseconds < bucket.Item2)
                {
                    return bucket.Item1;
                }
            }

            return perfBuckets[perfBuckets.Length - 1].Item1;
        }
    }
}
