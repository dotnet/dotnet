// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using NuGet.Versioning;
using NuGet.VisualStudio.Etw;
using NuGet.VisualStudio.Telemetry;

namespace NuGet.VisualStudio.Implementation.Extensibility
{
    [Export(typeof(IVsSemanticVersionComparer))]
    public class VsSemanticVersionComparer : IVsSemanticVersionComparer
    {
        [ImportingConstructor]
        public VsSemanticVersionComparer(INuGetTelemetryProvider telemetryProvider)
        {
            // MEF components do not participate in Visual Studio's Package extensibility,
            // hence importing INuGetTelemetryProvider ensures that the ETW collector is
            // set up correctly.
            _ = telemetryProvider;
        }

        public VsSemanticVersionComparer()
        {

        }

        public int Compare(string versionA, string versionB)
        {
            const string eventName = nameof(IVsSemanticVersionComparer) + "." + nameof(Compare);
            using var _ = NuGetETW.ExtensibilityEventSource.StartStopEvent(eventName);

            if (versionA == null)
            {
                throw new ArgumentNullException(nameof(versionA));
            }

            if (versionB == null)
            {
                throw new ArgumentNullException(nameof(versionB));
            }

            var parsedVersionA = NuGetVersion.Parse(versionA);
            var parsedVersionB = NuGetVersion.Parse(versionB);

            return parsedVersionA.CompareTo(parsedVersionB);
        }
    }
}
