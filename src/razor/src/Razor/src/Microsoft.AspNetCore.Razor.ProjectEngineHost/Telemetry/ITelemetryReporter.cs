// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;

namespace Microsoft.AspNetCore.Razor.Telemetry;

internal interface ITelemetryReporter
{
    TelemetryScope BeginBlock(string name, Severity severity);
    TelemetryScope BeginBlock(string name, Severity severity, Property property);
    TelemetryScope BeginBlock(string name, Severity severity, Property property1, Property property2);
    TelemetryScope BeginBlock(string name, Severity severity, Property property1, Property property2, Property property3);
    TelemetryScope BeginBlock(string name, Severity severity, params Property[] properties);

    TelemetryScope TrackLspRequest(string lspMethodName, string lspServerName, Guid correlationId);

    void ReportEvent(string name, Severity severity);
    void ReportEvent(string name, Severity severity, Property property);
    void ReportEvent(string name, Severity severity, Property property1, Property property2);
    void ReportEvent(string name, Severity severity, Property property1, Property property2, Property property3);
    void ReportEvent(string name, Severity severity, params Property[] properties);

    void ReportFault(Exception exception, string? message, params object?[] @params);
}
