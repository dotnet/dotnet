// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Common;

namespace NuGet.VisualStudio.Telemetry
{
    public static class InstanceCloseTelemetryEmitter
    {
        private const string EventName = "InstanceClose";

        public static void OnShutdown()
        {
            var telemetryEvent = CreateTelemetryEvent();
            TelemetryActivity.EmitTelemetryEvent(telemetryEvent);
        }

        internal static TelemetryEvent CreateTelemetryEvent()
        {
            var telemetryEvent = new TelemetryEvent(EventName);

            AddEventsOnShutdown?.Invoke(null, telemetryEvent);

            telemetryEvent["faults.sessioncount"] = TelemetryUtility.TotalFaultEvents;

            return telemetryEvent;
        }

        public static event System.EventHandler<TelemetryEvent> AddEventsOnShutdown;
    }
}
