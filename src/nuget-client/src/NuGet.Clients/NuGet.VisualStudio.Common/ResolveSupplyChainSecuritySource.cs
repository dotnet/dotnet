// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.PackageManagement.Telemetry;

namespace NuGet.VisualStudio
{
    /// <summary>
    /// Identifies the surface that launched a supply chain security resolution with GitHub Copilot.
    /// </summary>
    public sealed class ResolveSupplyChainSecuritySource
    {
        private const string CopilotClientIdPrefix = "Microsoft.VisualStudio.NuGet.";

        /// <summary>The Package Source Mapping Options page.</summary>
        public static readonly ResolveSupplyChainSecuritySource PackageSourceMappingOptions = new(
            NavigationOrigin.Options_PackageSourceMapping_Review,
            CopilotClientIdPrefix + "PackageSourceMapper");

        /// <summary>The Error List.</summary>
        public static readonly ResolveSupplyChainSecuritySource ErrorList = new(
            NavigationOrigin.ErrorList_ResolveSupplyChainSecurity,
            CopilotClientIdPrefix + "ErrorList");

        private ResolveSupplyChainSecuritySource(NavigationOrigin navigationOrigin, string copilotClientId)
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
