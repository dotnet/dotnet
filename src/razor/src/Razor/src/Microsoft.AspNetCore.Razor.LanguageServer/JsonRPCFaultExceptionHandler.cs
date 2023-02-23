// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.Telemetry;
using StreamJsonRpc;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class JsonRPCFaultExceptionHandler : IFaultExceptionHandler
{
    public bool HandleException(ITelemetryReporter reporter, Exception exception, string? message, object?[] @params)
    {
        if (exception is not RemoteInvocationException remoteInvocationException)
        {
            return false;
        }

        reporter.ReportFault(remoteInvocationException, remoteInvocationException.Message, remoteInvocationException.ErrorCode, remoteInvocationException.ErrorData, remoteInvocationException.DeserializedErrorData);
        return true;
    }
}
