namespace Microsoft.ApplicationInsights.Common
{
    using System;
    using System.Threading;

    /// <summary>
    /// This class will hold a timestamp and will perform a given action only if the current time has exceeded an interval.
    /// </summary>
    internal class InterlockedThrottle
    {
        private readonly TimeSpan interval;
        private long timeStamp = DateTimeOffset.MinValue.Ticks;

        /// <summary>
        /// Initializes a new instance of the <see cref="InterlockedThrottle"/> class.
        /// </summary>
        /// <param name="interval">Defines the time period to perform some action.</param>
        public InterlockedThrottle(TimeSpan interval) => this.interval = interval;

        /// <summary>
        /// Will execute the action only if the time period has elapsed.
        /// </summary>
        /// <param name="action">Action to be executed.</param>
        public void PerformThrottledAction(Action action)
        {
            var now = DateTimeOffset.UtcNow;
            if (now.Ticks > Interlocked.Read(ref this.timeStamp))
            {
                Interlocked.Exchange(ref this.timeStamp, now.Add(this.interval).Ticks);
                action();
            }
        }
    }
}
