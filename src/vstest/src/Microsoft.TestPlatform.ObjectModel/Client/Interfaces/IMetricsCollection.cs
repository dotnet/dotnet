// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

/// <summary>
/// This Interface Provides API's to Collect Metrics.
/// </summary>
public interface IMetricsCollection
{
    /// <summary>
    /// Add Metric in the Metrics Cache
    /// </summary>
    /// <param name="metric">Metric Message</param>
    /// <param name="value">Value associated with Metric</param>
    void Add(string metric, object value);

    /// <summary>
    /// Get Metrics
    /// </summary>
    /// <value>Returns the Telemetry Data Points</value>
    IDictionary<string, object> Metrics { get; }
}
