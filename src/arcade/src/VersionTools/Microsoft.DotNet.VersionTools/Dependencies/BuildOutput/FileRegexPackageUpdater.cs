// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Microsoft.DotNet.VersionTools.Dependencies.BuildOutput
{
    public class FileRegexPackageUpdater : FileRegexUpdater
    {
        public string PackageId { get; set; }

        protected override string TryGetDesiredValue(
            IEnumerable<IDependencyInfo> dependencyInfos,
            out IEnumerable<IDependencyInfo> usedDependencyInfos)
        {
            var matchingBuildInfo = dependencyInfos
                .OfType<BuildDependencyInfo>()
                .FirstOrDefault(d => d.RawPackages.ContainsKey(PackageId));

            if (matchingBuildInfo == null)
            {
                usedDependencyInfos = Enumerable.Empty<IDependencyInfo>();

                Trace.TraceError($"Could not find package version information for '{PackageId}'");
                return $"DEPENDENCY '{PackageId}' NOT FOUND";
            }

            usedDependencyInfos = new[] { matchingBuildInfo };

            return matchingBuildInfo.RawPackages[PackageId];
        }
    }
}
