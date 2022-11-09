// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestPlatform.Common.Telemetry;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

namespace Microsoft.VisualStudio.TestPlatform.Common;

/// <inheritdoc />
/// <summary>
/// Provide common services and data for a discovery/run request.
/// </summary>
public class RequestData : IRequestData
{
    /// <summary>
    /// The metrics collection.
    /// </summary>
    private IMetricsCollection _metricsCollection;

    /// <summary>
    /// The protocol config.
    /// </summary>
    private ProtocolConfig? _protocolConfig;

    /// <summary>
    /// The default constructor for request data.
    /// </summary>
    public RequestData()
    {
        _metricsCollection = new NoOpMetricsCollection();
        IsTelemetryOptedIn = false;
    }

    /// <summary>
    /// Gets or sets the metrics collection.
    /// </summary>
    public IMetricsCollection MetricsCollection
    {
        get => _metricsCollection;
        set => _metricsCollection = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Gets or sets the protocol config.
    /// </summary>
    public ProtocolConfig? ProtocolConfig
    {
        get => _protocolConfig;
        set => _protocolConfig = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Gets or sets a value indicating whether is telemetry opted in.
    /// </summary>
    public bool IsTelemetryOptedIn { get; set; }
}
