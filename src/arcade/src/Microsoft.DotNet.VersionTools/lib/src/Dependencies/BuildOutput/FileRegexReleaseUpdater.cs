// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Microsoft.DotNet.VersionTools.Dependencies.BuildOutput
{
    public class FileRegexReleaseUpdater : FileRegexUpdater
    {
        public string BuildInfoName { get; set; }

        protected override string TryGetDesiredValue(
            IEnumerable<IDependencyInfo> dependencyInfos,
            out IEnumerable<IDependencyInfo> usedDependencyInfos)
        {
            BuildDependencyInfo project = dependencyInfos
                .OfType<BuildDependencyInfo>()
                .SingleOrDefault(d => d.BuildInfo.Name == BuildInfoName);

            if (project == null)
            {
                usedDependencyInfos = Enumerable.Empty<IDependencyInfo>();

                Trace.TraceError($"Could not find build info for project named {BuildInfoName}");
                return $"PROJECT '{BuildInfoName}' NOT FOUND";
            }

            usedDependencyInfos = new[] { project };

            return project.BuildInfo.LatestReleaseVersion;
        }
    }
}
