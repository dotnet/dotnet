// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Microsoft.DotNet.VersionTools.Automation
{
    public class GitHubBranch
    {
        public string Name { get; }
        public GitHubProject Project { get; }

        public GitHubBranch(string name, GitHubProject project)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            Name = name;

            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }
            Project = project;
        }
    }
}
