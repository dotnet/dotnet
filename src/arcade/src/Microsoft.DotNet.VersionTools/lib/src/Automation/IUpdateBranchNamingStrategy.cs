// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.VersionTools.Automation
{
    public interface IUpdateBranchNamingStrategy
    {
        /// <summary>
        /// Returns a string that can be used to find an existing update PR for the given branch.
        /// </summary>
        string Prefix(string upstreamBranchName);

        /// <summary>
        /// Creates a string to append to the Prefix when a fresh upgrade branch is needed.
        /// </summary>
        string CreateFreshBranchNameSuffix(string upstreamBranchName);
    }
}
