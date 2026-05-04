// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.OpAmp.Client.Settings;

/// <summary>
/// Configuration settings for the remote configuration capability of the client.
/// </summary>
public sealed class RemoteConfigSettings
{
    /// <summary>
    /// Gets or sets a value indicating whether the client accepts remote configuration.
    /// </summary>
    /// <value>
    /// <c>true</c> if remote configuration is accepted and should be sent by the server; otherwise, <c>false</c>.
    /// Default is <c>false</c>.
    /// </value>
    public bool AcceptsRemoteConfig { get; set; }
}
