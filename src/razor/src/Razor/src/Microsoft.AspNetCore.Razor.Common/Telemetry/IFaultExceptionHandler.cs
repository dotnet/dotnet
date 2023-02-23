// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;

namespace Microsoft.AspNetCore.Razor.Telemetry;

internal interface IFaultExceptionHandler
{
    bool HandleException(ITelemetryReporter reporter, Exception exception, string? message, object?[] @params);
}
