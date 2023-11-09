// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.VisualStudio.Telemetry;

namespace Microsoft.VisualStudio.Editor.Razor.Test.Shared;

internal class TestTelemetryReporter() : TelemetryReporter(default)
{
    public List<TelemetryEvent> Events { get; } = new();

    protected override void Report(TelemetryEvent telemetryEvent)
    {
        Events.Add(telemetryEvent);
    }
}
