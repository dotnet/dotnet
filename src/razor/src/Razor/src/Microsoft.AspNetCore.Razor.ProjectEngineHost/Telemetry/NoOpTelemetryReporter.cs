// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;

namespace Microsoft.AspNetCore.Razor.Telemetry;

internal class NoOpTelemetryReporter : ITelemetryReporter
{
    public static readonly NoOpTelemetryReporter Instance = new();

    private NoOpTelemetryReporter()
    {
    }

    public TelemetryScope BeginBlock(string name, Severity severity)
        => TelemetryScope.Null;

    public TelemetryScope BeginBlock(string name, Severity severity, Property property)
        => TelemetryScope.Null;

    public TelemetryScope BeginBlock(string name, Severity severity, Property property1, Property property2)
        => TelemetryScope.Null;

    public TelemetryScope BeginBlock(string name, Severity severity, Property property1, Property property2, Property property3)
        => TelemetryScope.Null;

    public TelemetryScope BeginBlock(string name, Severity severity, params Property[] properties)
        => TelemetryScope.Null;

    public void ReportEvent(string name, Severity severity)
    {
    }

    public void ReportEvent(string name, Severity severity, Property property)
    {
    }

    public void ReportEvent(string name, Severity severity, Property property1, Property property2)
    {
    }

    public void ReportEvent(string name, Severity severity, Property property1, Property property2, Property property3)
    {
    }

    public void ReportEvent(string name, Severity severity, params ReadOnlySpan<Property> properties)
    {
    }

    public void ReportFault(Exception exception, string? message, params object?[] @params)
    {
    }

    public TelemetryScope TrackLspRequest(string lspMethodName, string lspServerName, Guid correlationId)
        => TelemetryScope.Null;
}
