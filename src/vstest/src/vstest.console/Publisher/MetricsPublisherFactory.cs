// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.Publisher;

/// <summary>
/// Returns the Instance of <see cref="IMetricsPublisher"/> on basis of given parameters.
/// </summary>
public class MetricsPublisherFactory
{
    /// <summary>
    /// Gets the Metrics Publisher
    /// </summary>
    /// <param name="isTelemetryOptedIn">Is Telemetry opted in or not</param>
    /// <param name="isDesignMode">Is Design Mode enabled or not</param>
    /// <returns>Returns Instance of Metrics Publisher</returns>
    public static async Task<IMetricsPublisher> GetMetricsPublisher(bool isTelemetryOptedIn, bool isDesignMode)
    {
        var logEnabled = Environment.GetEnvironmentVariable("VSTEST_LOGTELEMETRY");
        bool logTelemery = string.Equals(logEnabled, "1", StringComparison.Ordinal);

        return isTelemetryOptedIn && !isDesignMode && logTelemery
            ? await Task.FromResult<IMetricsPublisher>(new TextFileTelemetryPublisher())
            : await Task.FromResult<IMetricsPublisher>(new NoOpMetricsPublisher());
    }
}
