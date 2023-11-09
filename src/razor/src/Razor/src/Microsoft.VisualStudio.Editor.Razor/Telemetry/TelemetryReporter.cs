// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using Microsoft.VisualStudio.Telemetry;

#if DEBUG
using System.Linq;
#endif

namespace Microsoft.AspNetCore.Razor.Telemetry;

internal abstract class TelemetryReporter : ITelemetryReporter
{
    protected ImmutableArray<TelemetrySession> TelemetrySessions { get; set; }

    protected TelemetryReporter(ImmutableArray<TelemetrySession> telemetrySessions)
    {
        // Get the DefaultSession for telemetry. This is set by VS with
        // TelemetryService.SetDefaultSession and provides the correct
        // appinsights keys etc
        TelemetrySessions = telemetrySessions.NullToEmpty();
    }

    private static string GetEventName(string name) => "dotnet/razor/" + name;
    private static string GetPropertyName(string name) => "dotnet.razor." + name;

    private static TelemetrySeverity ConvertSeverity(Severity severity)
        => severity switch
        {
            Severity.Normal => TelemetrySeverity.Normal,
            Severity.Low => TelemetrySeverity.Low,
            Severity.High => TelemetrySeverity.High,
            _ => throw new InvalidOperationException($"Unknown severity: {severity}")
        };

    public void ReportEvent(string name, Severity severity)
    {
        var telemetryEvent = new TelemetryEvent(GetEventName(name), ConvertSeverity(severity));

        Report(telemetryEvent);
    }

    public void ReportEvent(string name, Severity severity, Property property)
    {
        var telemetryEvent = new TelemetryEvent(GetEventName(name), ConvertSeverity(severity));

        AddToProperties(telemetryEvent.Properties, property);
        Report(telemetryEvent);
    }

    public void ReportEvent(string name, Severity severity, Property property1, Property property2)
    {
        var telemetryEvent = new TelemetryEvent(GetEventName(name), ConvertSeverity(severity));

        AddToProperties(telemetryEvent.Properties, property1);
        AddToProperties(telemetryEvent.Properties, property2);
        Report(telemetryEvent);
    }

    public void ReportEvent(string name, Severity severity, Property property1, Property property2, Property property3)
    {
        var telemetryEvent = new TelemetryEvent(GetEventName(name), ConvertSeverity(severity));

        AddToProperties(telemetryEvent.Properties, property1);
        AddToProperties(telemetryEvent.Properties, property2);
        AddToProperties(telemetryEvent.Properties, property3);
        Report(telemetryEvent);
    }

    public void ReportEvent(string name, Severity severity, params Property[] properties)
    {
        var telemetryEvent = new TelemetryEvent(GetEventName(name), ConvertSeverity(severity));

        foreach (var property in properties)
        {
            AddToProperties(telemetryEvent.Properties, property);
        }

        Report(telemetryEvent);
    }

    private static void AddToProperties(IDictionary<string, object?> properties, Property property)
    {
        if (IsComplexValue(property.Value))
        {
            properties.Add(GetPropertyName(property.Name), new TelemetryComplexProperty(property.Value));
        }
        else
        {
            properties.Add(GetPropertyName(property.Name), property.Value);
        }

        static bool IsComplexValue(object? o)
        {
            return o?.GetType() is Type type && Type.GetTypeCode(type) == TypeCode.Object;
        }
    }

    public void ReportFault(Exception exception, string? message, params object?[] @params)
    {
        try
        {
            if (exception is OperationCanceledException { InnerException: { } oceInnerException })
            {
                ReportFault(oceInnerException, message, @params);
                return;
            }

            if (exception is AggregateException aggregateException)
            {
                // We (potentially) have multiple exceptions; let's just report each of them
                foreach (var innerException in aggregateException.Flatten().InnerExceptions)
                {
                    ReportFault(innerException, message, @params);
                }

                return;
            }

            if (HandleException(exception, message, @params))
            {
                return;
            }

            var currentProcess = Process.GetCurrentProcess();

            var faultEvent = new FaultEvent(
                eventName: GetEventName("fault"),
                description: GetExceptionDetails(exception),
                FaultSeverity.General,
                exceptionObject: exception,
                gatherEventDetails: faultUtility =>
                {
                    foreach (var data in @params)
                    {
                        if (data is null)
                        {
                            continue;
                        }

                        faultUtility.AddErrorInformation(data.ToString());
                    }

                    // Returning "0" signals that, if sampled, we should send data to Watson.
                    // Any other value will cancel the Watson report. We never want to trigger a process dump manually,
                    // we'll let TargetedNotifications determine if a dump should be collected.
                    // See https://aka.ms/roslynnfwdocs for more details
                    return 0;
                });

            Report(faultEvent);
        }
        catch (Exception)
        {
        }
    }

    private static string GetExceptionDetails(Exception exception)
    {
        const string CodeAnalysisNamespace = nameof(Microsoft) + "." + nameof(CodeAnalysis);
        const string AspNetCoreNamespace = nameof(Microsoft) + "." + nameof(AspNetCore);

        // Be resilient to failing here.  If we can't get a suitable name, just fall back to the standard name we
        // used to report.
        try
        {
            // walk up the stack looking for the first call from a type that isn't in the ErrorReporting namespace.
            var frames = new StackTrace(exception).GetFrames();

            // On the .NET Framework, GetFrames() can return null even though it's not documented as such.
            // At least one case here is if the exception's stack trace itself is null.
            if (frames != null)
            {
                foreach (var frame in frames)
                {
                    var method = frame?.GetMethod();
                    var methodName = method?.Name;
                    if (methodName is null)
                    {
                        continue;
                    }

                    var declaringTypeName = method?.DeclaringType?.FullName;
                    if (declaringTypeName == null)
                    {
                        continue;
                    }

                    if (!declaringTypeName.StartsWith(CodeAnalysisNamespace) &&
                        !declaringTypeName.StartsWith(AspNetCoreNamespace))
                    {
                        continue;
                    }

                    return declaringTypeName + "." + methodName;
                }
            }
        }
        catch
        {
        }

        // If we couldn't get a stack, do this
        return exception.Message;
    }

    protected virtual void Report(TelemetryEvent telemetryEvent)
    {
        try
        {
#if !DEBUG
            foreach (var session in TelemetrySessions)
            {
                session.PostEvent(telemetryEvent);
            }
#else
            // In debug we only log to normal logging. This makes it much easier to add and debug telemetry events
            // before we're ready to send them to the cloud
            var name = telemetryEvent.Name;
            var propertyString = string.Join(",", telemetryEvent.Properties.Select(kvp => $"[ {kvp.Key}:{kvp.Value} ]"));
            LogTrace("Telemetry Event: {name} \n Properties: {propertyString}\n", name, propertyString);

            if (telemetryEvent is FaultEvent)
            {
                var eventType = telemetryEvent.GetType();
                var description = eventType.GetProperty("Description", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(telemetryEvent, null);
                var exception = eventType.GetProperty("ExceptionObject", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(telemetryEvent, null);
                var message = $"Fault Event: {name} \n Exception Info: {exception ?? description} \n Properties: {propertyString}";

                Debug.Fail(message);
            }
#endif
        }
        catch (Exception e)
        {
            // No need to do anything here. We failed to report telemetry
            // which isn't good, but not catastrophic for a user
            LogError(e, "Failed logging telemetry event");
        }
    }

    protected virtual bool HandleException(Exception exception, string? message, params object?[] @params)
        => false;

    protected virtual void LogTrace(string? message, params object?[] args)
    {
    }

    protected virtual void LogError(Exception exception, string? message, params object?[] args)
    {
    }

    public TelemetryScope BeginBlock(string name, Severity severity)
        => TelemetryScope.Create(this, name, severity);

    public TelemetryScope BeginBlock(string name, Severity severity, Property property)
        => TelemetryScope.Create(this, name, severity, property);

    public TelemetryScope BeginBlock(string name, Severity severity, Property property1, Property property2)
        => TelemetryScope.Create(this, name, severity, property1, property2);

    public TelemetryScope BeginBlock(string name, Severity severity, Property property1, Property property2, Property property3)
        => TelemetryScope.Create(this, name, severity, property1, property2, property3);

    public TelemetryScope BeginBlock(string name, Severity severity, params Property[] properties)
        => TelemetryScope.Create(this, name, severity, properties);

    public TelemetryScope TrackLspRequest(string lspMethodName, string languageServerName, Guid correlationId)
    {
        if (correlationId == Guid.Empty)
        {
            return TelemetryScope.Null;
        }

        ReportEvent("BeginLspRequest", Severity.Normal,
            new("eventscope.method", lspMethodName),
            new("eventscope.languageservername", languageServerName),
            new("eventscope.correlationid", correlationId));

        return BeginBlock("TrackLspRequest", Severity.Normal,
            new("eventscope.method", lspMethodName),
            new("eventscope.languageservername", languageServerName),
            new("eventscope.correlationid", correlationId));
    }
}
