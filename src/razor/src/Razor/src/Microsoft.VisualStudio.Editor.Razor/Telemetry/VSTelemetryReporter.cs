// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Composition;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Telemetry;
using StreamJsonRpc;

namespace Microsoft.AspNetCore.Razor.Telemetry;

[Shared]
[Export(typeof(ITelemetryReporter))]
internal class VSTelemetryReporter : TelemetryReporter
{
    private readonly ILogger? _logger;

    [ImportingConstructor]
    public VSTelemetryReporter(
        [Import(AllowDefault = true)] ILoggerFactory? loggerFactory = null)
            // Get the DefaultSession for telemetry. This is set by VS with
            // TelemetryService.SetDefaultSession and provides the correct
            // appinsights keys etc
            : base(ImmutableArray.Create(TelemetryService.DefaultSession))
    {
        _logger = loggerFactory?.CreateLogger<VSTelemetryReporter>();
    }

    protected override bool HandleException(Exception exception, string? message, params object?[] @params)
    {
        if (exception is RemoteInvocationException remoteInvocationException)
        {
            ReportRemoteInvocationException(remoteInvocationException);
            return true;
        }

        return false;
    }

    private void ReportRemoteInvocationException(RemoteInvocationException remoteInvocationException)
    {
        if (remoteInvocationException.InnerException is Exception innerException)
        {
            ReportFault(innerException, "RIE: " + remoteInvocationException.Message);
            return;
        }

        ReportFault(
            remoteInvocationException,
            remoteInvocationException.Message,
            remoteInvocationException.ErrorCode,
            remoteInvocationException.DeserializedErrorData);
    }

    protected override void LogTrace(string? message, params object?[] args)
        => _logger?.LogTrace(message, args);

    protected override void LogError(Exception exception, string? message, params object?[] args)
        => _logger?.LogError(exception, message, args);
}
