// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.VisualStudio.Telemetry;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.OmniSharp;

internal class OmniSharpTelemetryReporter : ITelemetryReporter
{
    public void ReportEvent(string name, TelemetrySeverity severity)
    {
    }

    public void ReportEvent<T>(string name, TelemetrySeverity severity, ImmutableDictionary<string, T> values)
    {
    }

    public void ReportFault(Exception exception, string? message, params object?[] @params)
    {
    }
}
