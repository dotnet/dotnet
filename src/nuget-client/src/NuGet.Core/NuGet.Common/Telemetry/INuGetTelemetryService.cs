// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.Common
{
    /// <summary> Abstraction of NuGet telemetry service. </summary>
    public interface INuGetTelemetryService
    {
        /// <summary> Send a <see cref="TelemetryEvent"/> to telemetry. </summary>
        /// <param name="telemetryData"> Telemetry event to send. </param>
        void EmitTelemetryEvent(TelemetryEvent telemetryData);

        /// <summary> Log a start of telemetry activity to the event log. </summary>
        /// <param name="activityName"> Name of telemetry activity to log. </param>
        /// <returns> <see cref="IDisposable"/> which will log end activity marker. </returns>
        IDisposable StartActivity(string activityName);
    }
}
