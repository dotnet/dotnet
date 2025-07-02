namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse.Helpers
{
    using System;

    internal class QuickPulseTimings
    {
        public QuickPulseTimings(
            TimeSpan servicePollingInterval,
            TimeSpan servicePollingBackedOffInterval,
            TimeSpan timeToServicePollingBackOff,
            TimeSpan collectionInterval,
            TimeSpan timeToCollectionBackOff,
            TimeSpan catastrophicFailuretimeout)
        {
            this.ServicePollingInterval = servicePollingInterval;
            this.ServicePollingBackedOffInterval = servicePollingBackedOffInterval;
            this.TimeToServicePollingBackOff = timeToServicePollingBackOff;
            this.CollectionInterval = collectionInterval;
            this.TimeToCollectionBackOff = timeToCollectionBackOff;
            this.CatastrophicFailureTimeout = catastrophicFailuretimeout;
        }

        public QuickPulseTimings(TimeSpan servicePollingInterval, TimeSpan collectionInterval)
        {
            this.ServicePollingInterval = servicePollingInterval;
            this.CollectionInterval = collectionInterval;

            this.ServicePollingBackedOffInterval = TimeSpan.MaxValue;
            this.TimeToServicePollingBackOff = TimeSpan.MaxValue;
            this.TimeToCollectionBackOff = TimeSpan.MaxValue;
            this.CatastrophicFailureTimeout = TimeSpan.MaxValue;
        }

        public static QuickPulseTimings Default
        {
            get
            {
                return new QuickPulseTimings(
                    servicePollingInterval: TimeSpan.FromSeconds(5),
                    servicePollingBackedOffInterval: TimeSpan.FromMinutes(1),
                    timeToServicePollingBackOff: TimeSpan.FromMinutes(1),
                    collectionInterval: TimeSpan.FromSeconds(1),
                    timeToCollectionBackOff: TimeSpan.FromSeconds(20),
                    catastrophicFailuretimeout: TimeSpan.FromSeconds(5));
            }
        }

        public TimeSpan ServicePollingInterval { get; private set; }

        public TimeSpan ServicePollingBackedOffInterval { get; private set; }

        public TimeSpan TimeToServicePollingBackOff { get; private set; }

        public TimeSpan CollectionInterval { get; private set; }

        public TimeSpan TimeToCollectionBackOff { get; private set; }

        public TimeSpan CatastrophicFailureTimeout { get; private set; }
    }
}