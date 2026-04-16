namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse
{
    using System;
    using System.Threading;

    using Microsoft.ApplicationInsights.Extensibility.Filtering;

    /// <summary>
    /// Accumulator manager for QuickPulse data.
    /// </summary>
    internal class QuickPulseDataAccumulatorManager : IQuickPulseDataAccumulatorManager
    {
        private QuickPulseDataAccumulator currentDataAccumulator;
        private QuickPulseDataAccumulator completedDataAccumulator;

        public QuickPulseDataAccumulatorManager(CollectionConfiguration collectionConfiguration)
        {
            if (collectionConfiguration == null)
            {
                throw new ArgumentNullException(nameof(collectionConfiguration));
            }

            this.currentDataAccumulator = new QuickPulseDataAccumulator(collectionConfiguration);
        }

        public QuickPulseDataAccumulator CurrentDataAccumulator => this.currentDataAccumulator;

        public QuickPulseDataAccumulator CompleteCurrentDataAccumulator(CollectionConfiguration collectionConfiguration)
        {
            /* 
                Here we need to 
                    - promote currentDataAccumulator to completedDataAccumulator
                    - reset (zero out) the new currentDataAccumulator

                Certain telemetry items will be "sprayed" between two neighboring accumulators due to the fact that the snap might occur in the middle of a reader executing its Interlocked's.
            */

            this.completedDataAccumulator = Interlocked.Exchange(ref this.currentDataAccumulator, new QuickPulseDataAccumulator(collectionConfiguration));

            var timestamp = DateTimeOffset.UtcNow;
            this.completedDataAccumulator.EndTimestamp = timestamp;
            this.currentDataAccumulator.StartTimestamp = timestamp;

            return this.completedDataAccumulator;
        }
    }
}