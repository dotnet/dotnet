// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Arcade.Common;
using Xunit;

namespace Microsoft.DotNet.Arcade.Sdk.Tests
{
    public class ExponentialRetryTests
    {
        [Fact]
        public async Task CancellationBeforeFirstAttemptDoesNotRunAction()
        {
            using var cancellation = new CancellationTokenSource();
            cancellation.Cancel();
            var retry = new ExponentialRetry();
            bool actionRan = false;

            await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
                retry.RunAsync(
                    _ =>
                    {
                        actionRan = true;
                        return Task.FromResult(RetryResult.Success);
                    },
                    cancellation.Token));

            Assert.False(actionRan);
        }

        [Fact]
        public async Task CancellationDuringDelayIsPropagated()
        {
            using var cancellation = new CancellationTokenSource();
            var retry = new ExponentialRetry
            {
                MaxAttempts = 2,
                DelayBase = 60,
                DelayConstant = 0,
                MinRandomFactor = 1,
                MaxRandomFactor = 1,
            };

            Task<bool> retryTask = retry.RunAsync(
                _ =>
                {
                    cancellation.Cancel();
                    return Task.FromResult(RetryResult.Retry());
                },
                cancellation.Token);

            await Assert.ThrowsAnyAsync<OperationCanceledException>(() => retryTask);
        }

        [Fact]
        public async Task FinalFailedAttemptDoesNotDelay()
        {
            var retry = new ExponentialRetry
            {
                MaxAttempts = 1,
            };

            bool succeeded = await retry.RunAsync(
                _ => Task.FromResult(RetryResult.Retry(TimeSpan.FromHours(1))),
                CancellationToken.None);

            Assert.False(succeeded);
        }

        [Fact]
        public async Task MaximumDelayCapsExponentialBackoff()
        {
            var retry = new ExponentialRetry
            {
                MaxAttempts = 2,
                DelayBase = 60,
                DelayConstant = 0,
                MinRandomFactor = 1,
                MaxRandomFactor = 1,
                MaximumDelay = TimeSpan.Zero,
            };
            int attempts = 0;

            bool succeeded = await retry.RunAsync(
                _ =>
                {
                    attempts++;
                    return Task.FromResult(RetryResult.Retry());
                },
                CancellationToken.None);

            Assert.False(succeeded);
            Assert.Equal(2, attempts);
        }
    }
}
