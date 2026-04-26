namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.Timer
{
    using System;
    
    /// <summary>The timer.</summary>
    internal interface ITimer
    {
        #region Public Methods and Operators

        /// <summary>The change.</summary>
        /// <param name="dueTime">The due time.</param>
        void ScheduleNextTick(TimeSpan dueTime);

        #endregion
    }
}
