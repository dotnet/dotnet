// -----------------------------------------------------------------------
// <copyright file="DiagnoisticsEventThrottlingDefaults.cs" company="Microsoft">
// Copyright © Microsoft. All Rights Reserved.
// </copyright>
// <author>Sergei Nikitin: sergeyni@microsoft.com</author>
// <summary></summary>
// -----------------------------------------------------------------------

namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.DiagnosticsModule
{
    internal static class DiagnoisticsEventThrottlingDefaults
    {
        internal const int MinimalThrottleAfterCount = 1;
        internal const int DefaultThrottleAfterCount = 5;
        internal const int MaxThrottleAfterCount = 10;

        internal const uint MinimalThrottlingRecycleIntervalInMinutes = 1;
        internal const uint DefaultThrottlingRecycleIntervalInMinutes = 5;
        internal const uint MaxThrottlingRecycleIntervalInMinutes = 60;

        internal const int KeywordsExcludedFromEventThrottling = (int)EventSourceKeywords.Diagnostics;

        internal static bool IsInRangeThrottleAfterCount(this int throttleAfterCount)
        {
            return throttleAfterCount >= MinimalThrottleAfterCount
                   && throttleAfterCount <= MaxThrottleAfterCount;
        }

        internal static bool IsInRangeThrottlingRecycleInterval(
            this uint throttlingRecycleIntervalInMinutes)
        {
            return throttlingRecycleIntervalInMinutes >= MinimalThrottlingRecycleIntervalInMinutes
                   && throttlingRecycleIntervalInMinutes <= MaxThrottlingRecycleIntervalInMinutes;
        }
    }
}