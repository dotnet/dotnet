namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.Timer
{
    using System;
    using System.Threading;

    /// <summary>The timer implementation.</summary>
    internal class Timer : ITimer, IDisposable
    {
        #region Constants and Fields

        /// <summary>The timer.</summary>
        private System.Threading.Timer timer;

        #endregion

        #region Constructors and Destructors

        /// <summary>Initializes a new instance of the <see cref="Timer"/> class.</summary>
        /// <param name="callback">The callback.</param>
        public Timer(TimerCallback callback)
        {
            this.timer = new System.Threading.Timer(callback, null, Timeout.Infinite, Timeout.Infinite);
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>Changes the timer's parameters.</summary>
        /// <param name="dueTime">The due time.</param>
        public void ScheduleNextTick(TimeSpan dueTime)
        {
            this.timer.Change(dueTime, Timeout.InfiniteTimeSpan);
        }

        /// <summary>
        /// Stops the timer.
        /// </summary>
        public void Stop()
        {
            this.timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        #endregion

        /// <summary>
        /// Disposes resources allocated by this type.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose implementation.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.timer != null)
                {
                    this.timer.Dispose();
                    this.timer = null;
                }
            }
        }
    }
}
