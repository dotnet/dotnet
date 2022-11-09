// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.VersionTools.Dependencies.BuildManifest;
using Microsoft.DotNet.VersionTools.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.DotNet.VersionTools.Dependencies.Submodule
{
    public class OrchestratedBuildSubmoduleUpdater : SubmoduleUpdater
    {
        /// <summary>
        /// A "fake" remote that branches are fetched to directly from the git url. This ensures the
        /// correct commit is fetched regardless how the remotes are set up in the submodule.
        /// </summary>
        private const string SyntheticRemoteName = "auto-update-remote";

        public string BuildName { get; set; }

        public string GitUrl { get; set; }

        protected override string GetDesiredCommitHash(
            IEnumerable<IDependencyInfo> dependencyInfos,
            out IEnumerable<IDependencyInfo> usedDependencyInfos)
        {
            OrchestratedBuildIdentityMatch match = OrchestratedBuildIdentityMatch.Find(
                BuildName,
                dependencyInfos);

            if (match == null)
            {
                usedDependencyInfos = null;
                return null;
            }

            match.EnsureMatchHasCommit();

            usedDependencyInfos = new[] { match.Info };
            return match.Match.Commit;
        }

        protected override void FetchRemoteBranch()
        {
            string refspec = $"+refs/heads/*:refs/remotes/{SyntheticRemoteName}/*";
            Trace.TraceInformation($"In '{Path}', fetching '{refspec}' from '{GitUrl}'...");
            GitCommand.Fetch(Path, GitUrl, refspec);
        }
    }
}
