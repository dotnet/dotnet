namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.DiagnosticsModule
{
    using System;
    using System.Collections.Generic;

    internal class DiagnoisticsEventThrottling : IDiagnoisticsEventThrottling
    {
        private readonly int throttleAfterCount;
        private readonly object syncRoot = new object();

        private Dictionary<int, DiagnoisticsEventCounters> counters =
            new Dictionary<int, DiagnoisticsEventCounters>();

        internal DiagnoisticsEventThrottling(int throttleAfterCount)
        {
            if (!throttleAfterCount.IsInRangeThrottleAfterCount())
            {
                throw new ArgumentOutOfRangeException(nameof(throttleAfterCount));
            }

            this.throttleAfterCount = throttleAfterCount;
        }

        internal int ThrottleAfterCount
        {
            get { return this.throttleAfterCount; }
        }

        public bool ThrottleEvent(int eventId, long keywords, out bool justExceededThreshold)
        {
            if (!IsExcludedFromThrottling(keywords))
            {
                var counter = this.InternalGetEventCounter(eventId);

                justExceededThreshold = this.ThrottleAfterCount == counter.Increment() - 1;

                return this.ThrottleAfterCount < counter.ExecCount;
            }

            justExceededThreshold = false;

            return false;
        }

        public IDictionary<int, DiagnoisticsEventCounters> CollectSnapshot()
        {
            var snapshot = this.counters;

            this.syncRoot.ExecuteSpinWaitLock(
                () =>
                {
                    this.counters = new Dictionary<int, DiagnoisticsEventCounters>();
                });

            return snapshot;
        }

        private static bool IsExcludedFromThrottling(long keywords)
        {
            return (keywords & DiagnoisticsEventThrottlingDefaults.KeywordsExcludedFromEventThrottling) != 0;
        }

        private DiagnoisticsEventCounters InternalGetEventCounter(
            int eventId)
        {
            DiagnoisticsEventCounters result = null;
            this.syncRoot.ExecuteSpinWaitLock(
            () =>
            {
                if (!this.counters.TryGetValue(eventId, out result))
                {
                    result = new DiagnoisticsEventCounters();
                    this.counters.Add(eventId, result);
                }
            });

            return result;
        }
    }
}