// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.VersionTools.Automation
{
    public class SingleBranchNamingStrategy : IUpdateBranchNamingStrategy
    {
        private string _branchName;

        public SingleBranchNamingStrategy(string branchName)
        {
            _branchName = branchName;
        }

        public string Prefix(string upstreamBranchName) => $"{upstreamBranchName}-{_branchName}";

        public string CreateFreshBranchNameSuffix(string upstreamBranchName) => string.Empty;
    }
}
