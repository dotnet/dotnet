// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.OpAmp.Client.Internal.Services.Heartbeat;

internal sealed class HealthReport
{
    public ulong StartTime { get; set; }

    public ulong StatusTime { get; set; }

    public bool IsHealthy { get; set; }

    public string? Status { get; set; }

    public string? LastError { get; set; }

    public IList<ComponentHealthStatus> Components { get; } = [];
}
