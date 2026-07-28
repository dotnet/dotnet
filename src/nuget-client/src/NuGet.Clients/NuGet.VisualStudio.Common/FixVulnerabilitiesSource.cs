// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.PackageManagement.Telemetry;

namespace NuGet.VisualStudio
{
    /// <summary>
    /// Identifies the surface that launched the "Fix Vulnerabilities with GitHub Copilot" flow and
    /// carries the two things that surface needs: the <see cref="NavigationOrigin"/> used for
    /// telemetry attribution and the Copilot client id reported to Copilot. This is a closed set of
    /// well-known sources; adding a surface is a one-line addition here.
    /// </summary>
    public sealed class FixVulnerabilitiesSource
    {
        private const string CopilotClientIdPrefix = "Microsoft.VisualStudio.NuGet.";

        /// <summary>The Solution Explorer vulnerabilities info bar.</summary>
        public static readonly FixVulnerabilitiesSource VulnerabilityInfoBar = new(
            NavigationOrigin.VulnerabilityInfoBar_FixVulnerabilitiesWithCopilot,
            CopilotClientIdPrefix + "VulnerabilitiesInfoBar");

        /// <summary>The Error List.</summary>
        public static readonly FixVulnerabilitiesSource ErrorList = new(
            NavigationOrigin.ErrorList_FixVulnerabilitiesWithCopilot,
            CopilotClientIdPrefix + "ErrorList");

        private FixVulnerabilitiesSource(NavigationOrigin navigationOrigin, string copilotClientId)
        {
            NavigationOrigin = navigationOrigin;
            CopilotClientId = copilotClientId;
        }

        /// <summary>The telemetry origin for this source.</summary>
        public NavigationOrigin NavigationOrigin { get; }

        /// <summary>The Copilot client id reported to Copilot for this source.</summary>
        public string CopilotClientId { get; }
    }
}
