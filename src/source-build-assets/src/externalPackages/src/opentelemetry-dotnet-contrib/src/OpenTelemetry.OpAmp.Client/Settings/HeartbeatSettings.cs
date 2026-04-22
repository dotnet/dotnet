// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.OpAmp.Client.Settings;

/// <summary>
/// Configuration settings for the heartbeat mechanism of the client.
/// </summary>
public sealed class HeartbeatSettings
{
    /// <summary>
    /// Gets or sets a value indicating whether the heartbeat mechanism is enabled.
    /// </summary>
    /// <value>
    /// <c>true</c> if heartbeat messages should be sent periodically; otherwise, <c>false</c>.
    /// Default is <c>true</c>.
    /// </value>
    public bool IsEnabled { get; set; } = true;

    /// <summary>
    /// Gets or sets the interval between heartbeat messages.
    /// </summary>
    /// <value>
    /// A <see cref="TimeSpan"/> representing how often heartbeat messages are sent.
    /// The default value is 30 seconds.
    /// </value>
    public TimeSpan Interval { get; set; } = TimeSpan.FromSeconds(30);
}
