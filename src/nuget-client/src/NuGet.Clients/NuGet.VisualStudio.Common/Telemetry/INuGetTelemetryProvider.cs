// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using NuGet.Common;

namespace NuGet.VisualStudio.Telemetry
{
    public interface INuGetTelemetryProvider
    {
        void EmitEvent(TelemetryEvent telemetryEvent);
        Task PostFaultAsync(Exception e, string callerClassName, [CallerMemberName] string callerMemberName = null, IDictionary<string, object> extraProperties = null);
        void PostFault(Exception e, string callerClassName, [CallerMemberName] string callerMemberName = null, IDictionary<string, object> extraProperties = null);
    }
}
