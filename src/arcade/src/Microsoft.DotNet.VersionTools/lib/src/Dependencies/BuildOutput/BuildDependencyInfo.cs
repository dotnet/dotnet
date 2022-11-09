// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using NuGet.Packaging.Core;
using NuGet.Versioning;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.DotNet.VersionTools.Dependencies.BuildOutput
{
    public class BuildDependencyInfo : IDependencyInfo
    {
        public BuildInfo BuildInfo { get; }

        /// <summary>
        /// If true, upgraders will upgrade any version of this package to the one specified in the
        /// buildinfo, even if it's stable. Otherwise, prerelease versions are the only ones that
        /// are upgraded.
        /// </summary>
        public bool UpgradeStableVersions { get; }

        /// <summary>
        /// Package id/version pairs of this dependency build info. May be different from the
        /// BuildInfo's package dictionary based on dependency requirements.
        /// </summary>
        public Dictionary<string, string> RawPackages { get; }

        /// <summary>
        /// Parsed copies of packages in RawPackages.
        /// </summary>
        public IEnumerable<PackageIdentity> Packages { get; }

        public BuildDependencyInfo(
            BuildInfo buildInfo,
            bool upgradeStableVersions,
            IEnumerable<string> disabledPackages)
        {
            BuildInfo = buildInfo;
            UpgradeStableVersions = upgradeStableVersions;

            RawPackages = buildInfo.LatestPackages
                .Where(pair => !disabledPackages.Contains(pair.Key))
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            Packages = RawPackages
                .Select(pair => new PackageIdentity(pair.Key, new NuGetVersion(pair.Value)))
                .ToArray();
        }

        public string SimpleName => BuildInfo.Name;

        public string SimpleVersion => BuildInfo.LatestReleaseVersion;
    }
}
