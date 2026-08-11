// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Arcade.Common
{
    public class ExponentialRetry : IRetryHandler
    {
        private Random _random = new Random();

        public int MaxAttempts { get; set; } = 10;

        /// <summary>
        /// Base, in seconds, raised to the power of the number of retries so far.
        /// </summary>
        public double DelayBase { get; set; } = 6;

        /// <summary>
        /// A constant, in seconds, added to (base^retries) to find the delay before retrying.
        /// 
        /// The default is -1 to make the first retry instant, because ((base^0)-1) == 0.
        /// </summary>
        public double DelayConstant { get; set; } = -1;

        public double MinRandomFactor { get; set; } = 0.5;
        public double MaxRandomFactor { get; set; } = 1.0;

        /// <summary>
        /// Maximum exponential delay. A longer server-provided retry delay is still honored.
        /// </summary>
        public TimeSpan? MaximumDelay { get; set; }

        /// <summary>
        /// Invoked after a failed attempt when another attempt will be made. The first argument
        /// is the one-based number of the failed attempt and the second is the computed delay.
        /// </summary>
        public Action<int, TimeSpan> RetryDelayCallback { get; set; }
        public CancellationToken DefaultCancellationToken { get; set; } = CancellationToken.None;

        public Task<bool> RunAsync(Func<int, Task<RetryResult>> actionAsync)
        {
            return RunAsync(actionAsync, DefaultCancellationToken);
        }

        public async Task<bool> RunAsync(
            Func<int, Task<RetryResult>> actionAsync,
            CancellationToken cancellationToken)
        {
            for (int i = 0; i < MaxAttempts; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                string attempt = $"Attempt {i + 1}/{MaxAttempts}";
                Trace.TraceInformation(attempt);

                RetryResult result = await actionAsync(i);
                if (result.Succeeded)
                {
                    return true;
                }

                if (i == MaxAttempts - 1)
                {
                    return false;
                }

                double randomFactor =
                    _random.NextDouble() * (MaxRandomFactor - MinRandomFactor) + MinRandomFactor;

                TimeSpan exponentialDelay = TimeSpan.FromSeconds(
                    (Math.Pow(DelayBase, i) + DelayConstant) * randomFactor);
                if (MaximumDelay is TimeSpan maximumDelay && exponentialDelay > maximumDelay)
                {
                    exponentialDelay = maximumDelay;
                }

                TimeSpan delay = result.RetryAfter is TimeSpan retryAfter
                    ? TimeSpan.FromTicks(Math.Max(exponentialDelay.Ticks, retryAfter.Ticks))
                    : exponentialDelay;

                Trace.TraceInformation($"{attempt} failed. Waiting {delay} before next try.");
                RetryDelayCallback?.Invoke(i + 1, delay);

                await Task.Delay(delay, cancellationToken);
            }
            return false;
        }
    }
}
