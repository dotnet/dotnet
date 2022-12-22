// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Common
{
    /// <summary> Telemetry session. </summary>
    public interface ITelemetrySession
    {
        /// <summary> Post a telemetry event to current telemetry session. </summary>
        /// <param name="telemetryEvent"> Telemetry event. </param>
        void PostEvent(TelemetryEvent telemetryEvent);
    }
}
