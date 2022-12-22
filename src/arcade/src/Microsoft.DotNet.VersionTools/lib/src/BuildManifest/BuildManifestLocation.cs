// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.VersionTools.Automation;
using System;

namespace Microsoft.DotNet.VersionTools.BuildManifest
{
    public class BuildManifestLocation
    {
        public GitHubProject GitHubProject { get; }

        public string GitHubRef { get; }

        public string GitHubBasePath { get; }

        public BuildManifestLocation(
            GitHubProject gitHubProject,
            string gitHubRef,
            string gitHubBasePath)
        {
            if (gitHubProject == null)
            {
                throw new ArgumentNullException(nameof(gitHubProject));
            }

            if (string.IsNullOrEmpty(gitHubRef))
            {
                throw new ArgumentException(nameof(gitHubRef));
            }

            if (string.IsNullOrEmpty(gitHubBasePath))
            {
                throw new ArgumentException(nameof(gitHubBasePath));
            }

            GitHubProject = gitHubProject;
            GitHubRef = gitHubRef;
            GitHubBasePath = gitHubBasePath;
        }
    }
}
