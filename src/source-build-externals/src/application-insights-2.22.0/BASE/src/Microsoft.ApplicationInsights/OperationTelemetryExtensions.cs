namespace Microsoft.ApplicationInsights
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;

    /// <summary>
    /// Extension functions to operation telemetry that start and stop the timer.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class OperationTelemetryExtensions
    {
        /// <summary>
        /// An extension to telemetry item that starts the timer for the respective telemetry.
        /// </summary>
        /// <param name="telemetry">Telemetry item object that calls this extension method.</param>
        public static void Start(this OperationTelemetry telemetry)
        {
            Start(telemetry, Stopwatch.GetTimestamp());
        }

        /// <summary>
        /// An extension to telemetry item that initializes the timer for the respective telemetry
        /// using a timestamp from a high-resolution <see cref="Stopwatch"/>.
        /// </summary>
        /// <param name="telemetry">Telemetry item object that calls this extension method.</param>
        /// <param name="timestamp">A high-resolution timestamp from <see cref="Stopwatch"/>.</param>
        public static void Start(this OperationTelemetry telemetry, long timestamp)
        {
            if (telemetry == null)
            {
                throw new ArgumentNullException(nameof(telemetry));
            }

            telemetry.Timestamp = PreciseTimestamp.GetUtcNow(); 

            // Begin time is used internally for calculating duration of operation at the end call,
            // and hence is stored using higher precision Clock.
            // Stopwatch.GetTimestamp() is used (instead of ElapsedTicks) as it is thread-safe.
            telemetry.BeginTimeInTicks = timestamp;

            RichPayloadEventSource.Log.ProcessOperationStart(telemetry);
        }

        /// <summary>
        /// An extension method to telemetry item that stops the timer and computes the duration of the request or dependency.
        /// </summary>
        /// <param name="telemetry">Telemetry item object that calls this extension method.</param>
        public static void Stop(this OperationTelemetry telemetry)
        {
            if (telemetry == null)
            {
                throw new ArgumentNullException(nameof(telemetry));
            }

            if (telemetry.BeginTimeInTicks != 0L)
            {
                StopImpl(telemetry, timestamp: Stopwatch.GetTimestamp());
            }
            else
            {
                StopImpl(telemetry, duration: TimeSpan.Zero);
            }
        }

        /// <summary>
        /// An extension method to telemetry item that stops the timer and computes the duration of the request or dependency.
        /// </summary>
        /// <param name="telemetry">Telemetry item object that calls this extension method.</param>
        /// <param name="timestamp">A high-resolution timestamp from <see cref="Stopwatch"/>.</param>
        public static void Stop(this OperationTelemetry telemetry, long timestamp)
        {
            if (telemetry == null)
            {
                throw new ArgumentNullException(nameof(telemetry));
            }

            if (telemetry.BeginTimeInTicks != 0L)
            {
                StopImpl(telemetry, timestamp);
            }
            else
            {
                StopImpl(telemetry, duration: TimeSpan.Zero);
            }
        }

        /// <summary>
        /// Generate random operation Id and set it to OperationContext.
        /// </summary>
        /// <param name="telemetry">Telemetry to initialize Operation id for.</param>
        public static void GenerateOperationId(this OperationTelemetry telemetry)
        {
            if (telemetry == null)
            {
                throw new ArgumentNullException(nameof(telemetry));
            }

            telemetry.GenerateId();
        }

        /// <summary>
        /// Set the duration given a timestamp from a high-resolution <see cref="Stopwatch"/>.
        /// </summary>
        /// <param name="telemetry">Telemetry item object to update.</param>
        /// <param name="timestamp">The high resolution timestamp.</param>
        private static void StopImpl(OperationTelemetry telemetry, long timestamp)
        {
            long stopWatchTicksDiff = timestamp - telemetry.BeginTimeInTicks;
            double durationInTicks = stopWatchTicksDiff * PreciseTimestamp.StopwatchTicksToTimeSpanTicks;
            StopImpl(telemetry, TimeSpan.FromTicks((long)Math.Round(durationInTicks)));
        }

        /// <summary>
        /// Record the duration and, optionally, set the timestamp to the current time.
        /// </summary>
        /// <param name="telemetry">Telemetry item object to update.</param>
        /// <param name="duration">The duration of the operation.</param>
        private static void StopImpl(OperationTelemetry telemetry, TimeSpan duration)
        {
            telemetry.Duration = duration;

            if (telemetry.Timestamp == DateTimeOffset.MinValue)
            {
                telemetry.Timestamp = PreciseTimestamp.GetUtcNow();
            }

            RichPayloadEventSource.Log.ProcessOperationStop(telemetry);
        }
    }
}
