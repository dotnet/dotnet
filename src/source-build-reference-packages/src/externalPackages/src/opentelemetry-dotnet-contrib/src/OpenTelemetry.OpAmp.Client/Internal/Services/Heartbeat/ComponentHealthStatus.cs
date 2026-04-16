// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.OpAmp.Client.Internal.Services.Heartbeat;

/// <summary>
/// Represents the health status of a system component.
/// </summary>
internal sealed class ComponentHealthStatus
{
    public ComponentHealthStatus(string componentName)
    {
        this.ComponentName = componentName;
    }

    /// <summary>
    /// Gets or sets the name of the component.
    /// </summary>
    public string ComponentName { get; set; }

    /// <summary>
    /// Gets or sets the current status of the operation or entity.
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Gets or sets the description of the most recent error encountered.
    /// </summary>
    public string? LastError { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the system is in a healthy state.
    /// </summary>
    public bool IsHealthy { get; set; }

    /// <summary>
    /// Gets or sets the start time of the event or operation.
    /// </summary>
    public DateTimeOffset StartTime { get; set; }

    /// <summary>
    /// Gets or sets the timestamp indicating the current status update time.
    /// </summary>
    public DateTimeOffset StatusTime { get; set; }
}
