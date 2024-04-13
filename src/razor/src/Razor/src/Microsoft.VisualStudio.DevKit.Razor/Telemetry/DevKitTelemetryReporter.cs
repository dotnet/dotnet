// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Composition;
using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.VisualStudio.Razor.Telemetry;
using Microsoft.VisualStudio.Telemetry;

namespace Microsoft.VisualStudio.DevKit.Razor.Telemetry;

[Shared]
[Export(typeof(ITelemetryReporter))]
internal sealed class DevKitTelemetryReporter : TelemetryReporter, ITelemetryReporterInitializer
{
    private const string CollectorApiKey = "0c6ae279ed8443289764825290e4f9e2-1a736e7c-1324-4338-be46-fc2a58ae4d14-7255";

    [ImportingConstructor]
    public DevKitTelemetryReporter()
        : base(telemetrySessions: default)
    {
    }

    public void InitializeSession(string telemetryLevel, string? sessionId, bool isDefaultSession)
    {
        Debug.Assert(TelemetrySessions.IsDefaultOrEmpty);

        var sessionSettingsJson = CreateSessionSettingsJson(telemetryLevel, sessionId);
        var session = new TelemetrySession(sessionSettingsJson);

        if (isDefaultSession)
        {
            TelemetryService.SetDefaultSession(session);
        }

        session.Start();
        session.RegisterForReliabilityEvent();

        TelemetrySessions = ImmutableArray.Create<TelemetrySession>(session);
    }

    private static string CreateSessionSettingsJson(string telemetryLevel, string? sessionId)
    {
        sessionId ??= Guid.NewGuid().ToString();

        // Generate a new startTime for process to be consumed by Telemetry Settings
        using var curProcess = Process.GetCurrentProcess();
        var processStartTime = curProcess.StartTime.ToFileTimeUtc().ToString();

        using var _ = StringBuilderPool.GetPooledObject(out var builder);

        builder.Append('{');

        AppendNameValuePair(builder, "Id", sessionId);
        AppendNameValuePair(builder, "HostName", "Default");

        // Insert Telemetry Level instead of Opt-Out status. The telemetry service handles
        // validation of this value so there is no need to do so on this end. If it's invalid,
        // it defaults to off.
        AppendNameValuePair(builder, "TelemetryLevel", telemetryLevel);

        // this sets the Telemetry Session Created by LSP Server to be the Root Initial session
        // This means that the SessionID set here by "Id" will be the SessionID used by cloned session
        // further down stream
        AppendNameValuePair(builder, "IsInitialSession", "true", quoteValue: false);
        AppendNameValuePair(builder, "CollectorApiKey", CollectorApiKey);

        // using 1010 to indicate VS Code and not to match it to devenv 1000
        AppendNameValuePair(builder, "AppId", "1010", quoteValue: false);

        // Don't add a comma to the last property.
        AppendNameValuePair(builder, "ProcessStartTime", processStartTime, quoteValue: false, addComma: false);

        builder.Append('}');

        return builder.ToString();

        static void AppendNameValuePair(StringBuilder builder, string name, string? value, bool quoteValue = true, bool addComma = true)
        {
            builder.Append('"');
            builder.Append(name);
            builder.Append('"');
            builder.Append(':');

            if (value is string s)
            {
                if (quoteValue)
                {
                    builder.Append('"');
                }

                builder.Append(value);

                if (quoteValue)
                {
                    builder.Append('"');
                }
            }
            else
            {
                builder.Append("null");
            }

            if (addComma)
            {
                builder.Append(',');
            }
        }
    }
}
