// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.VersionTools.Dependencies.BuildManifest;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.DotNet.VersionTools.Dependencies.BuildOutput.OrchestratedBuild
{
    public class FileOrchestratedBuildCustomUpdater : FileUpdater
    {
        public Func<OrchestratedBuildDependencyInfo[], DependencyReplacement> GetDesiredValue { get; set; }

        public override DependencyReplacement GetDesiredReplacement(
            IEnumerable<IDependencyInfo> dependencyInfos)
        {
            return GetDesiredValue(
                dependencyInfos
                    .OfType<OrchestratedBuildDependencyInfo>()
                    .ToArray());
        }
    }
}
