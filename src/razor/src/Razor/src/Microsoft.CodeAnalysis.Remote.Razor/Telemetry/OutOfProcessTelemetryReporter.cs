// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.VisualStudio.Razor.Telemetry;
using Microsoft.VisualStudio.Telemetry;

namespace Microsoft.CodeAnalysis.Remote.Razor.Telemetry;

[Export(typeof(ITelemetryReporter)), Shared]
internal class OutOfProcessTelemetryReporter : TelemetryReporter
{
    public OutOfProcessTelemetryReporter()
        : base([TelemetryService.DefaultSession])
    {
    }
}
