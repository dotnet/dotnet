// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;

namespace NuGet.Common
{
    /// <summary> Utility for managing stopwatch timers. </summary>
    public static class TelemetryServiceUtility
    {
        private static Stopwatch? StopWatch;

        /// <summary> Starts or resumes timer. </summary>
        public static void StartOrResumeTimer()
        {
            if (StopWatch == null)
            {
                StopWatch = Stopwatch.StartNew();
            }
            else
            {
                StopWatch.Start();
            }
        }

        /// <summary> Stops timer. </summary>
        public static void StopTimer()
        {
            StopWatch?.Stop();
        }

        /// <summary> Gets elapsed time. </summary>
        /// <returns> Elapsed time. </returns>
        public static TimeSpan GetTimerElapsedTime()
        {
            if (StopWatch != null)
            {
                var duration = StopWatch.Elapsed;
                StopWatch.Reset();
                return duration;
            }

            return TimeSpan.MinValue;
        }

        /// <summary> Gets elapsed time in seconds. </summary>
        /// <returns> Elapsed time in seconds. </returns>
        public static double GetTimerElapsedTimeInSeconds()
        {
            return GetTimerElapsedTime().TotalSeconds;
        }
    }
}
