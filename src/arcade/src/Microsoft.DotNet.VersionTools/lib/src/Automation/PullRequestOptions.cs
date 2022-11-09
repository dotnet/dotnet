// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.VersionTools.Automation
{
    public class PullRequestOptions
    {
        /// <summary>
        /// Always create new pull requests.
        /// </summary>
        public bool ForceCreate { get; set; }

        /// <summary>
        /// Allow repo maintainers to modify the pull request without explicitly giving them
        /// permission to the fork being used.
        /// </summary>
        public bool MaintainersCanModify { get; set; } = true;

        /// <summary>
        /// When force pushing to a branch, add a comment with a pointer to the commit that was
        /// discarded so it is still accessible, along with CI.
        /// </summary>
        public bool TrackDiscardedCommits { get; set; } = true;

        /// <summary>
        /// A custom branching strategy to use instead of the PullRequestCreator default.
        /// </summary>
        public IUpdateBranchNamingStrategy BranchNamingStrategy { get; set; }

        /// <summary>
        /// Disables the safety check that makes sure the fork being force pushed to is owned by
        /// the user running PR submission.
        ///
        /// This check makes sense on GitHub, where it's always reasonable to have a fork and the
        /// convention is well-known. On VSTS, forks are less common, and "owner" isn't a clear
        /// concept. The "owner" field is used by the VSTS client to represent the project the repo
        /// is in, since it is the closest match and the value needs to be moved through the
        /// machinery. The project name never meaningfully matches a user (let alone a PR
        /// submitter's name), so this check needs to be disabled.
        /// </summary>
        public bool AllowBranchOnAnyRepoOwner { get; set; }
    }
}
