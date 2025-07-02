namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.DiagnosticsModule
{
    using System;
    using System.Globalization;

    internal class DiagnoisticsEventThrottlingManager<T> : IDiagnoisticsEventThrottlingManager
        where T : IDiagnoisticsEventThrottling
    {
        private readonly T snapshotContainer;

        internal DiagnoisticsEventThrottlingManager(
            T snapshotContainer, 
            IDiagnoisticsEventThrottlingScheduler scheduler,
            uint throttlingRecycleIntervalInMinutes)
        {
            if (snapshotContainer == null)
            {
                throw new ArgumentNullException(nameof(snapshotContainer));
            }

            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            if (!throttlingRecycleIntervalInMinutes.IsInRangeThrottlingRecycleInterval())
            {
                throw new ArgumentOutOfRangeException(nameof(throttlingRecycleIntervalInMinutes));
            }

            this.snapshotContainer = snapshotContainer;

            var throttlingRecycleIntervalInMilliseconds = (int)throttlingRecycleIntervalInMinutes * 60 * 1000;
            scheduler.ScheduleToRunEveryTimeIntervalInMilliseconds(
                throttlingRecycleIntervalInMilliseconds,
                this.ResetThrottling);
        }

        public bool ThrottleEvent(int eventId, long keywords)
        {
            bool justExceededThreshold;
            
            var throttleEvent = this.snapshotContainer.ThrottleEvent(
                eventId,
                keywords,
                out justExceededThreshold);

            if (justExceededThreshold)
            {
                CoreEventSource.Log.DiagnosticsEventThrottlingHasBeenStartedForTheEvent(eventId.ToString(CultureInfo.InvariantCulture));
            }

            return throttleEvent;
        }

        private void ResetThrottling()
        {
            var snapshot = this.snapshotContainer.CollectSnapshot();

            foreach (var record in snapshot)
            {
                CoreEventSource.Log.DiagnosticsEventThrottlingHasBeenResetForTheEvent(
                    record.Key, 
                    record.Value.ExecCount);
            }
        }
    }
}