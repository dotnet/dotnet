// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.Publisher;

/// <summary>
/// This class will be initialized if Telemetry is opted out.
/// </summary>
public class NoOpMetricsPublisher : IMetricsPublisher
{
    /// <summary>
    /// Will do NO-OP.
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="metrics"></param>
    public void PublishMetrics(string eventName, IDictionary<string, object?> metrics)
    {
        // No Operation
    }

    /// <summary>
    /// Will do NO-OP
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        // No operation
    }
}
