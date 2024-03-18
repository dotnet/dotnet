// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Arcade.Common;
using Microsoft.DotNet.VersionTools.Dependencies.BuildManifest;
using Microsoft.DotNet.VersionTools.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.DotNet.VersionTools.Dependencies.BuildOutput.OrchestratedBuild
{
    public class FileRegexOrchestratedBuildCustomUpdater : FileRegexUpdater
    {
        public Func<OrchestratedBuildDependencyInfo[], DependencyReplacement> GetDesiredValue { get; set; }

        protected override string TryGetDesiredValue(
            IEnumerable<IDependencyInfo> dependencyInfos,
            out IEnumerable<IDependencyInfo> usedDependencyInfos)
        {
            DependencyReplacement replacement = GetDesiredValue(
                dependencyInfos
                    .OfType<OrchestratedBuildDependencyInfo>()
                    .ToArray());

            usedDependencyInfos = (replacement?.UsedDependencyInfos).NullAsEmpty();
            return replacement?.Content;
        }
    }
}
