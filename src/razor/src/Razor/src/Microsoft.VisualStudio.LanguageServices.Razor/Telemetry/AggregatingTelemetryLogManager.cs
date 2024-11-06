// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;

namespace Microsoft.VisualStudio.Razor.Telemetry;

/// <summary>
/// Manages creation and obtaining aggregated telemetry logs.
/// </summary>
internal sealed class AggregatingTelemetryLogManager
{
    private readonly TelemetryReporter _telemetryReporter;
    private ImmutableDictionary<string, AggregatingTelemetryLog> _aggregatingLogs = ImmutableDictionary<string, AggregatingTelemetryLog>.Empty;

    public AggregatingTelemetryLogManager(TelemetryReporter session)
    {
        _telemetryReporter = session;
    }

    public AggregatingTelemetryLog? GetLog(string name, double[]? bucketBoundaries = null)
    {
        if (!_telemetryReporter.IsEnabled)
            return null;

        return ImmutableInterlocked.GetOrAdd(
            ref _aggregatingLogs,
            name,
            static (functionId, arg) => new AggregatingTelemetryLog(arg._telemetryReporter, functionId, arg.bucketBoundaries),
            factoryArgument: (_telemetryReporter, bucketBoundaries));
    }

    public void Flush()
    {
        if (!_telemetryReporter.IsEnabled)
            return;

        foreach (var log in _aggregatingLogs.Values)
        {
            log.Flush();
        }
    }
}
