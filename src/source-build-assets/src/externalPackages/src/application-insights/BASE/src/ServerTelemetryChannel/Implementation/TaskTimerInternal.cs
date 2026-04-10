// <copyright file="TaskTimerInternal.cs" company="Microsoft">
// Copyright © Microsoft. All Rights Reserved.
// </copyright>

namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

    /// <summary>
    /// Runs a task after a certain delay and log any error.
    /// </summary>
    internal class TaskTimerInternal : IDisposable
    {
        /// <summary>
        /// Represents an infinite time span.
        /// </summary>
        public static readonly TimeSpan InfiniteTimeSpan = new TimeSpan(0, 0, 0, 0, Timeout.Infinite);

        private TimeSpan delay = TimeSpan.FromMinutes(1);
        private CancellationTokenSource tokenSource;

        /// <summary>
        /// Gets or sets the delay before the task starts. 
        /// </summary>
        public TimeSpan Delay
        {
            get
            {
                return this.delay;
            }

            set
            {
                if ((value <= TimeSpan.Zero || value.TotalMilliseconds > int.MaxValue) && value != InfiniteTimeSpan)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                this.delay = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether value that indicates if a task has already started.
        /// </summary>
        public bool IsStarted
        {
            get { return this.tokenSource != null; }
        }

        /// <summary>
        /// Start the task.
        /// </summary>
        /// <param name="elapsed">The task to run.</param>
        public void Start(Func<Task> elapsed)
        {
            var newTokenSource = new CancellationTokenSource();

            Task.Delay(this.Delay, newTokenSource.Token)
                .ContinueWith(
                async previousTask =>
                    {
                        CancelAndDispose(Interlocked.CompareExchange(ref this.tokenSource, null, newTokenSource));
                        try
                        {
                            Task task = elapsed();

                            // Task may be executed synchronously
                            // It should return Task.FromResult but just in case we check for null if someone returned null
                            if (task != null)
                            {
                                await task.ConfigureAwait(false);
                            }
                        }
                        catch (Exception exception)
                        {
                            LogException(exception);
                        }
                    },
                CancellationToken.None,
                TaskContinuationOptions.OnlyOnRanToCompletion | TaskContinuationOptions.ExecuteSynchronously,
                TaskScheduler.Default);

            CancelAndDispose(Interlocked.Exchange(ref this.tokenSource, newTokenSource));
        }

        /// <summary>
        /// Cancels the current task.
        /// </summary>
        public void Cancel()
        {
            CancelAndDispose(Interlocked.Exchange(ref this.tokenSource, null));
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Log exception thrown by outer code.
        /// </summary>
        /// <param name="exception">Exception to log.</param>
        private static void LogException(Exception exception)
        {
            if (exception is AggregateException aggregateException)
            {
                aggregateException = aggregateException.Flatten();
                foreach (Exception e in aggregateException.InnerExceptions)
                {
                    TelemetryChannelEventSource.Log.LogError(e.ToInvariantString());
                }
            }

            TelemetryChannelEventSource.Log.LogError(exception.ToInvariantString());
        }

        private static void CancelAndDispose(CancellationTokenSource tokenSource)
        {
            if (tokenSource != null)
            {
                tokenSource.Cancel();
                tokenSource.Dispose();
            }
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Cancel();
            }
        }
    }
}
