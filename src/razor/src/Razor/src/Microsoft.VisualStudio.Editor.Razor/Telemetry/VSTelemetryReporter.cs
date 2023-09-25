// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Telemetry;

namespace Microsoft.AspNetCore.Razor.Telemetry;

[Shared]
[Export(typeof(ITelemetryReporter))]
internal class VSTelemetryReporter : TelemetryReporter
{
    private readonly IEnumerable<IFaultExceptionHandler> _faultExceptionHandlers;
    private readonly ILogger? _logger;

    [ImportingConstructor]
    public VSTelemetryReporter(
        [Import(AllowDefault = true)] ILoggerFactory? loggerFactory = null,
        [ImportMany] IEnumerable<IFaultExceptionHandler>? faultExceptionHandlers = null)
            // Get the DefaultSession for telemetry. This is set by VS with
            // TelemetryService.SetDefaultSession and provides the correct
            // appinsights keys etc
            : base(ImmutableArray.Create(TelemetryService.DefaultSession))
    {
        _faultExceptionHandlers = faultExceptionHandlers ?? Array.Empty<IFaultExceptionHandler>();
        _logger = loggerFactory?.CreateLogger<VSTelemetryReporter>();
    }

    public override bool HandleException(Exception exception, string? message, params object?[] @params)
    {
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

        return handled;
    }

    public override void InitializeSession(string telemetryLevel, string? sessionId, bool isDefaultSession)
    {
        // We don't need to do anything here. We're using the default session
        // which is already initialized by VS.
        throw new Exception("InitializeSession should not be called in VS.");
    }

    public override void LogTrace(string? message, params object?[] args)
        => _logger?.LogTrace(message, args);

    public override void LogError(Exception exception, string? message, params object?[] args)
        => _logger?.LogError(exception, message, args);


}
