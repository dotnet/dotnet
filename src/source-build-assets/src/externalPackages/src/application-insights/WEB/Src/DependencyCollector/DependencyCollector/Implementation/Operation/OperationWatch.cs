namespace Microsoft.ApplicationInsights.DependencyCollector.Implementation.Operation
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Single high precision clock used by operations.
    /// </summary>
    internal static class OperationWatch
    {
        /// <summary>
        /// High precision stopwatch.
        /// </summary>
        private static readonly Stopwatch Watch;

        /// <summary>
        /// Number of 100 nanoseconds per high-precision clock tick.
        /// </summary>
        private static readonly double HundredNanosecondsPerTick;

        /// <summary>
        /// The time clock started.
        /// </summary>
        private static readonly DateTimeOffset StartTime;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", 
            Justification = "Cannot really do Start() operation on stop watch in the variable initialization - need static ctor")]
        static OperationWatch()
        {
            StartTime = DateTimeOffset.UtcNow;

            Watch = new Stopwatch();
            Watch.Start();

            HundredNanosecondsPerTick = (1000.0 * 1000.0 * 10.0) / Stopwatch.Frequency;
        }

        /// <summary>
        /// Gets number of ticks elapsed on the clock since the start.
        /// </summary>
        public static long ElapsedTicks
        {
#if ALLOW_AGGRESSIVE_INLIGNING_ATTRIBUTE
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            get
            {
                return Watch.ElapsedTicks;
            }
        }

        /// <summary>
        /// Calculates time between two clock readings.
        /// </summary>
        /// <param name="fromTicks">Start time in ticks.</param>
        /// <param name="toTicks">End time in ticks.</param>
        /// <returns>Time between two clock readings.</returns>
        public static TimeSpan Duration(long fromTicks, long toTicks)
        {
            // Stopwatch ticks are different from TimeSpan.Ticks. 
            // Each tick in the TimeSpan.Ticks value represents one 100-nanosecond interval. 
            // Each tick in the ElapsedTicks value represents the time interval equal to 
            // 1 second divided by the Frequency.
            long elapsedTicks = toTicks - fromTicks;

            return TimeSpan.FromTicks(Convert.ToInt64(elapsedTicks * HundredNanosecondsPerTick));
        }

        /// <summary>
        /// Converts time on the operation clock (in ticks) to date and time structure.
        /// </summary>
        /// <param name="elapsedTicks">Ticks elapsed according to operation watch.</param>
        /// <returns>Date time structure representing the date and time that corresponds to the operation clock reading.</returns>
#if ALLOW_AGGRESSIVE_INLIGNING_ATTRIBUTE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static DateTimeOffset Timestamp(long elapsedTicks)
        {
            // Stopwatch ticks are different from DateTime.Ticks. 
            // Each tick in the DateTime.Ticks value represents one 100-nanosecond interval. 
            // Each tick in the ElapsedTicks value represents the time interval equal to 
            // 1 second divided by the Frequency.
            return StartTime.AddTicks(Convert.ToInt64(elapsedTicks * HundredNanosecondsPerTick));
        }
    }
}
