// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Reports;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks;

internal static class SummaryExtensions
{
    public static int ToExitCode(this IEnumerable<Summary> summaries)
    {
        // an empty summary means that initial filtering and validation did not allow
        // any benchmarks to run.
        if (!summaries.Any())
        {
            return 1;
        }

        // If anything has failed, it's an error.
        if (summaries.Any(summary => summary.HasAnyErrors()))
        {
            return 1;
        }

        return 0;
    }

    public static bool HasAnyErrors(this Summary summary)
        => summary.HasCriticalValidationErrors ||
           summary.Reports.Any(report => report.HasAnyErrors());

    public static bool HasAnyErrors(this BenchmarkReport report)
        => !report.BuildResult.IsBuildSuccess ||
           !report.AllMeasurements.Any();
}
