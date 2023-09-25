// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Diagnostics;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Microsoft.AspNetCore.Razor.Telemetry;

internal class TelemetryScope : IDisposable
{
    private readonly ITelemetryReporter _telemetryReporter;
    private readonly string _name;
    private readonly Severity _severity;
    private readonly ImmutableDictionary<string, object?> _values;
    private readonly Stopwatch _stopwatch;
    private bool _disposed;

    public TelemetryScope(
        ITelemetryReporter telemetryReporter,
        string name,
        Severity severity,
        ImmutableDictionary<string, object?> values)
    {
        _telemetryReporter = telemetryReporter;
        _name = name;
        _severity = severity;
        _values = values;
        _stopwatch = StopwatchPool.Default.Get();
        _stopwatch.Restart();
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;

        _stopwatch.Stop();
        var values = _values.Add("eventscope.ellapsedms", _stopwatch.ElapsedMilliseconds);
        _telemetryReporter.ReportEvent(_name, _severity, values);
        StopwatchPool.Default.Return(_stopwatch);
    }
}
