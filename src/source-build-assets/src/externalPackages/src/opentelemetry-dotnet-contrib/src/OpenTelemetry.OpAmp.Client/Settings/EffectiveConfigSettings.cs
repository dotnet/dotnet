// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.OpAmp.Client.Settings;

/// <summary>
/// Configuration settings for the effective configuration reporting capability of the client.
/// </summary>
public sealed class EffectiveConfigSettings
{
    /// <summary>
    /// Gets or sets a value indicating whether the client can report effective configuration.
    /// </summary>
    /// <value>
    /// <see langword="true"/> if effective configuration reporting is enabled; otherwise, <see langword="false"/>.
    /// Default is <see langword="false"/>.
    /// </value>
    public bool EnableReporting { get; set; }
}
