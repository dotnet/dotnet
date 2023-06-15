// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Internal.VisualStudio.Diagnostics;

namespace NuGet.VisualStudio.Telemetry
{
    internal class EtwLogActivity : IDisposable
    {
        private readonly VsEtwActivity _activity;

        public EtwLogActivity(string activityName)
        {
            if (VsEtwLogging.IsProviderEnabled(VsEtwKeywords.Ide, VsEtwLevel.Information))
            {
                var fullName = (VSTelemetrySession.VSEventNamePrefix + activityName).ToLowerInvariant();
                _activity = VsEtwLogging.CreateActivity(fullName, VsEtwKeywords.Ide, VsEtwLevel.Information);
            }
        }

        void IDisposable.Dispose()
        {
            _activity?.Dispose();
        }
    }
}

