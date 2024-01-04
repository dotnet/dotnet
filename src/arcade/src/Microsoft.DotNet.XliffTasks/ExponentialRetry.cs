// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace XliffTasks
{
    public class ExponentialRetry
    {
        public static void ExecuteWithRetryOnIOException(
           Action action,
           int maxRetryCount)
        {
            int count = 1;
            foreach (TimeSpan t in Intervals)
            {
                try
                {
                    action();
                    return;
                }
                catch (IOException e)
                {
                    if (count == maxRetryCount)
                        throw new IOException($"Retry failed after {count} times", e);
                    count++;
                }
                Thread.Sleep(t);
            }
            throw new Exception("Timer should not be exhausted");
        }

        private static IEnumerable<TimeSpan> Intervals
        {
            get
            {
                int milliseconds = 5;
                while (true)
                {
                    yield return TimeSpan.FromMilliseconds(milliseconds);
                    milliseconds *= 2;
                }
            }
        }
    }
}
