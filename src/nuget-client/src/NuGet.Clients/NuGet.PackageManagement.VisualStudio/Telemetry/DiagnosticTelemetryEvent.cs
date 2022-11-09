// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Common;

namespace NuGet.PackageManagement.Telemetry
{
    public class DiagnosticTelemetryEvent : TelemetryEvent
    {
        public DiagnosticTelemetryEvent(string eventName) :
            base(eventName)
        {
            this[nameof(NuGetVersion)] = VSTelemetryServiceUtility.NuGetVersion.Value;
        }

        protected const string DiagnosticEventName = "Diagnostic";

        /// <summary>
        /// The version of NuGet that emitted this event.
        /// </summary>
        public string NuGetVersion => (string)base[nameof(NuGetVersion)];
    }
}
