// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/75662173e3918f2176b74e467dc8e41d4f01d4d4/src/libraries/System.Diagnostics.DiagnosticSource/src/System/Diagnostics/Activity.DateTime.netfx.cs

// Adjusted to our coding standards and moved to separate class

using System.Diagnostics;

namespace OpenTelemetry.Instrumentation.AspNet;

internal static class ActivityDateTimeHelper
{
#pragma warning disable CA1823 // suppress unused field warning, as it's used to keep the timer alive
    private static readonly Timer SyncTimeUpdater = InitializeSyncTimer();
#pragma warning restore CA1823
    private static readonly double TickFrequency = (double)TimeSpan.TicksPerSecond / Stopwatch.Frequency;

    private static TimeSync timeSync = new();

    /// <summary>
    /// Returns high resolution (1 DateTime tick) current UTC DateTime.
    /// </summary>
    /// <returns>High resolution UTC DateTime.</returns>
    internal static DateTime GetUtcNow()
    {
        // DateTime.UtcNow accuracy on .NET Framework is ~16ms, this method
        // uses combination of Stopwatch and DateTime to calculate accurate UtcNow.

        var tmp = timeSync;

        // Timer ticks need to be converted to DateTime ticks
        long dateTimeTicksDiff = (long)((Stopwatch.GetTimestamp() - tmp.SyncStopwatchTicks) * TickFrequency);

        // DateTime.AddSeconds (or Milliseconds) rounds value to 1 ms, use AddTicks to prevent it
        return tmp.SyncUtcNow.AddTicks(dateTimeTicksDiff);
    }

    private static void Sync()
    {
        // wait for DateTime.UtcNow update to the next granular value
        Thread.Sleep(1);
        timeSync = new TimeSync();
    }

    [System.Security.SecuritySafeCritical]
    private static Timer InitializeSyncTimer()
    {
        Timer timer;

        // Don't capture the current ExecutionContext and its AsyncLocals onto the timer causing them to live forever
        bool restoreFlow = false;
        try
        {
            if (!ExecutionContext.IsFlowSuppressed())
            {
                ExecutionContext.SuppressFlow();
                restoreFlow = true;
            }

            timer = new Timer(static _ => Sync(), null, 0, 7_200_000); // 2 hours
        }
        finally
        {
            // Restore the current ExecutionContext
            if (restoreFlow)
            {
                ExecutionContext.RestoreFlow();
            }
        }

        return timer;
    }

    private sealed class TimeSync
    {
        public readonly DateTime SyncUtcNow = DateTime.UtcNow;
        public readonly long SyncStopwatchTicks = Stopwatch.GetTimestamp();
    }
}
