// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Diagnostics;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Telemetry;

namespace Microsoft.AspNetCore.Razor.Telemetry;

[Shared]
[Export(typeof(ITelemetryReporter))]
internal class TelemetryReporter : ITelemetryReporter
{
    private readonly ImmutableArray<TelemetrySession> _telemetrySessions;
    private readonly IEnumerable<IFaultExceptionHandler> _faultExceptionHandlers;
    private readonly ILogger? _logger;

    [ImportingConstructor]
    public TelemetryReporter(
        [Import(AllowDefault = true)] ILoggerFactory? loggerFactory = null,
        [ImportMany] IEnumerable<IFaultExceptionHandler>? faultExceptionHandlers = null)
    {
        // Get the DefaultSession for telemetry. This is set by VS with
        // TelemetryService.SetDefaultSession and provides the correct
        // appinsights keys etc
        _telemetrySessions = ImmutableArray.Create(TelemetryService.DefaultSession);
        _faultExceptionHandlers = faultExceptionHandlers ?? Array.Empty<IFaultExceptionHandler>();
        _logger = loggerFactory?.CreateLogger<TelemetryReporter>();
    }

    public void ReportEvent(string name, Severity severity)
    {
        var telemetryEvent = new TelemetryEvent(GetTelemetryName(name), ToTelemetrySeverity(severity));
        Report(telemetryEvent);
    }

    public void ReportEvent<T>(string name, Severity severity, ImmutableDictionary<string, T> values)
    {
        var telemetryEvent = new TelemetryEvent(GetTelemetryName(name), ToTelemetrySeverity(severity));
        foreach (var (propertyName, propertyValue) in values)
        {
            telemetryEvent.Properties.Add(GetPropertyName(propertyName), new TelemetryComplexProperty(propertyValue));
        }

        Report(telemetryEvent);
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

            var handled = false;
            foreach (var handler in _faultExceptionHandlers)
            {
                if (handler.HandleException(this, exception, message, @params))
                {
                    // This behavior means that each handler still gets a chance
                    // to respond to the exception. There's no real reason for this other
                    // than best guess. When it was added, there was only one handler but
                    // it was intended to be easy to add more.
                    handled = true;
                }
            }

            if (handled)
            {
                return;
            }

            var currentProcess = Process.GetCurrentProcess();

            var faultEvent = new FaultEvent(
                eventName: GetTelemetryName("fault"),
                description: GetDescription(exception),
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

    private static string GetTelemetryName(string name) => "dotnet/razor/" + name;
    private static string GetPropertyName(string name) => "dotnet.razor." + name;

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

    private static string GetDescription(Exception exception)
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

    private static TelemetrySeverity ToTelemetrySeverity(Severity severity)
        => severity switch
        {
            Severity.Normal => TelemetrySeverity.Normal,
            Severity.Low => TelemetrySeverity.Low,
            Severity.High => TelemetrySeverity.High,
            _ => throw new InvalidOperationException($"Unknown severity: {severity}")
        };
}
