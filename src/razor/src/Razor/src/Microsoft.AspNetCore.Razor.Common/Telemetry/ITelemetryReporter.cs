// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using Microsoft.VisualStudio.Telemetry;

namespace Microsoft.AspNetCore.Razor.Telemetry;

public interface ITelemetryReporter
{
    void ReportEvent(string name, TelemetrySeverity severity);
    void ReportEvent<T>(string name, TelemetrySeverity severity, ImmutableDictionary<string, T> values);
    void ReportFault(Exception exception, string? message, object[] @params);
}
