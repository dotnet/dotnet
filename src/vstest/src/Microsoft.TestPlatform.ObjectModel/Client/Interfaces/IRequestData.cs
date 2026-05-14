// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

/// <summary>
/// Provide common services and data for a discovery/run request.
/// </summary>
public interface IRequestData
{
    /// <summary>
    /// Gets an instance of <see cref="IMetricsCollection"/>.
    /// </summary>
    IMetricsCollection MetricsCollection { get; set; }

    /// <summary>
    /// Gets or sets the ProtocolConfig <see cref="ProtocolConfig"/>
    /// </summary>
    ProtocolConfig? ProtocolConfig { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether is telemetry opted in.
    /// </summary>
    bool IsTelemetryOptedIn { get; set; }
}
