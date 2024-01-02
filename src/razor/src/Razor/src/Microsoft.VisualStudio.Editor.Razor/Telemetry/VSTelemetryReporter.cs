// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Composition;
using Microsoft.CodeAnalysis.Razor.Logging;
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
    public VSTelemetryReporter(IRazorLoggerFactory loggerFactory)
            // Get the DefaultSession for telemetry. This is set by VS with
            // TelemetryService.SetDefaultSession and provides the correct
            // appinsights keys etc
            : base(ImmutableArray.Create(TelemetryService.DefaultSession))
    {
        _logger = loggerFactory.CreateLogger<VSTelemetryReporter>();
    }

    protected override bool HandleException(Exception exception, string? message, params object?[] @params)
    {
        if (exception is RemoteInvocationException remoteInvocationException)
        {
            if (ReportRemoteInvocationException(remoteInvocationException, @params))
            {
                return true;
            }
        }

        return false;
    }

    private bool ReportRemoteInvocationException(RemoteInvocationException remoteInvocationException, object?[] @params)
    {
        if (remoteInvocationException.InnerException is Exception innerException)
        {
            // innerException might be an OperationCancelled or Aggregate, use the full ReportFault to unwrap it consistently.
            ReportFault(innerException, "RIE: " + remoteInvocationException.Message);
            return true;
        }
        else if (@params.Length < 2)
        {
            // RIE has '2' extra pieces of data to report via @params, if we don't have those, then we unwrap and call one more time.
            // If we have both, though, we want the core code of ReportFault to do the reporting.
            ReportFault(
                remoteInvocationException,
                remoteInvocationException.Message,
                remoteInvocationException.ErrorCode,
                remoteInvocationException.DeserializedErrorData);
            return true;
        }
        else
        {
            return false;
        }
    }

    protected override void LogTrace(string? message, params object?[] args)
        => _logger?.LogTrace(message, args);

    protected override void LogError(Exception exception, string? message, params object?[] args)
        => _logger?.LogError(exception, message, args);
}
