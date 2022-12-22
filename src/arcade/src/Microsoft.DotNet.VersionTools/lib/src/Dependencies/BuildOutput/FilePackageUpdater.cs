// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Microsoft.DotNet.VersionTools.Dependencies.BuildOutput
{
    public class FilePackageUpdater : FileUpdater
    {
        public string PackageId { get; set; }

        public override DependencyReplacement GetDesiredReplacement(
            IEnumerable<IDependencyInfo> dependencyInfos)
        {
            foreach (BuildDependencyInfo info in dependencyInfos.OfType<BuildDependencyInfo>())
            {
                string version;
                if (info.RawPackages.TryGetValue(PackageId, out version))
                {
                    return new DependencyReplacement(
                        version,
                        new[] { info });
                }
            }

            Trace.TraceError($"For '{Path}', Could not find '{PackageId}' package version information.");
            return null;
        }
    }
}
