// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;

namespace Microsoft.AspNetCore.Razor.Telemetry;

internal class NullTelemetryScope : IDisposable
{
    public static NullTelemetryScope Instance { get; } = new NullTelemetryScope();
    private NullTelemetryScope() { }
    public void Dispose() { }
}
