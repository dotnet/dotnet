// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Telemetry;

namespace Microsoft.AspNetCore.Razor.Telemetry;

[Shared]
[Export(typeof(ITelemetryReporter))]
internal class TelemetryReporter : ITelemetryReporter
{
    private readonly ImmutableArray<TelemetrySession> _telemetrySessions;
    private readonly ILogger? _logger;

    [ImportingConstructor]
    public TelemetryReporter([Import(AllowDefault = true)] ILoggerFactory? loggerFactory = null)
        : this(ImmutableArray.Create(TelemetryService.DefaultSession), loggerFactory)
    {
    }

    public TelemetryReporter(ImmutableArray<TelemetrySession> telemetrySessions, ILoggerFactory? loggerFactory)
    {
        _telemetrySessions = telemetrySessions;
        _logger = loggerFactory?.CreateLogger<TelemetryReporter>();
    }

    public void ReportEvent(string name, TelemetrySeverity severity)
    {
        var telemetryEvent = new TelemetryEvent(GetTelemetryName(name), severity);
        Report(telemetryEvent);
    }

    public void ReportEvent<T>(string name, TelemetrySeverity severity, ImmutableDictionary<string, T> values)
    {
        var telemetryEvent = new TelemetryEvent(GetTelemetryName(name), severity);
        foreach (var (propertyName, propertyValue) in values)
        {
            telemetryEvent.Properties.Add(GetPropertyName(propertyName), new TelemetryComplexProperty(propertyValue));
        }

        Report(telemetryEvent);
    }

    private static string GetTelemetryName(string name) => "razor/" + name;
    private static string GetPropertyName(string name) => "razor." + name;

    private void Report(TelemetryEvent telemetryEvent)
    {
        try
        {
#if !DEBUG
            foreach (var session in _telemetrySessions)
            {
                session.PostEvent(telemetryEvent);
            }
#else
            // In debug we only log to normal logging. This makes it much easier to add and debug telemetry events
            // before we're ready to send them to the cloud
            var name = telemetryEvent.Name;
            var propertyString = string.Join(",", telemetryEvent.Properties.Select(kvp => $"[ {kvp.Key}:{kvp.Value} ]"));
            _logger?.LogTrace("Telemetry Event: {name} \n Properties: {propertyString}\n", name, propertyString);
#endif
        }
        catch (Exception e)
        {
            // No need to do anything here. We failed to report telemetry
            // which isn't good, but not catastrophic for a user
            _logger?.LogError(e, "Failed logging telemetry event");
        }
    }
}
