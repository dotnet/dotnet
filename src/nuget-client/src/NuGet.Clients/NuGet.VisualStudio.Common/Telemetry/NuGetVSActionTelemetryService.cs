// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Common;
using NuGet.VisualStudio.Telemetry;

namespace NuGet.VisualStudio
{
    /// <summary>
    /// Telemetry service class for restore operation
    /// </summary>
    public class NuGetVSTelemetryService : INuGetTelemetryService
    {
        private ITelemetrySession _telemetrySession;

        public NuGetVSTelemetryService(ITelemetrySession telemetrySession)
        {
            _telemetrySession = telemetrySession ?? throw new ArgumentNullException(nameof(telemetrySession));
        }

        public NuGetVSTelemetryService() :
            this(VSTelemetrySession.Instance)
        {
        }

        public static void Initialize()
        {
            TelemetryActivity.NuGetTelemetryService ??= new NuGetVSTelemetryService();
        }

        public virtual void EmitTelemetryEvent(TelemetryEvent telemetryData)
        {
            if (telemetryData == null)
            {
                throw new ArgumentNullException(nameof(telemetryData));
            }

            _telemetrySession.PostEvent(telemetryData);
        }

        public virtual IDisposable StartActivity(string activityName)
        {
            if (activityName == null)
            {
                throw new ArgumentNullException(nameof(activityName));
            }

            return new EtwLogActivity(activityName);
        }
    }
}
