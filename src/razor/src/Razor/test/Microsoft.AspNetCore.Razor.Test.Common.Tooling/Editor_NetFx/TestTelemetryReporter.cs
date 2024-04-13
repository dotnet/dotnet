// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.VisualStudio.Razor.Telemetry;
using Microsoft.VisualStudio.Telemetry;

namespace Microsoft.VisualStudio.Editor.Razor.Test.Shared;

internal class TestTelemetryReporter(ILoggerFactory loggerFactory) : VSTelemetryReporter(new Lazy<ILoggerFactory>(() => loggerFactory))
{
    public List<TelemetryEvent> Events { get; } = [];

    protected override void Report(TelemetryEvent telemetryEvent)
    {
        Events.Add(telemetryEvent);
    }
}
