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

    /// <summary>
    /// Gets or sets an optional factory the composition root uses to supply pre-configured instances
    /// of its own built-in extensions (keyed by extension URI) instead of reflection-activating them.
    /// Returns <see langword="null"/> for unknown / third-party extensions, which continue to be
    /// created by reflection. This is an internal, closed injection seam for our own extensions and is
    /// deliberately not part of the public <see cref="IRequestData"/> contract, so it never becomes a
    /// third-party extension point.
    /// </summary>
    internal Func<Uri, object?>? KnownExtensionInstanceFactory { get; set; }
}
