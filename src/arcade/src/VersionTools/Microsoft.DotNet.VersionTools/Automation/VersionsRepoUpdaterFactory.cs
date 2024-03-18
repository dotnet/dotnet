// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.VersionTools.Automation
{
    public interface IVersionsRepoUpdaterFactory
    {
        GitHubVersionsRepoUpdater CreateGitHubVersionsRepoUpdater(GitHubAuth gitHubAuth, string versionsRepoOwner = null, string versionsRepo = null);
        LocalVersionsRepoUpdater CreateLocalVersionsRepoUpdater();
    }
    
    public class VersionsRepoUpdaterFactory : IVersionsRepoUpdaterFactory
    {
        private INupkgInfoFactory _nupkgInfoFactory;

        public VersionsRepoUpdaterFactory(INupkgInfoFactory nupkgInfoFactory)
        {
            _nupkgInfoFactory = nupkgInfoFactory;
        }

        public GitHubVersionsRepoUpdater CreateGitHubVersionsRepoUpdater(GitHubAuth gitHubAuth, string versionsRepoOwner = null, string versionsRepo = null)
        {
            return new GitHubVersionsRepoUpdater(_nupkgInfoFactory, gitHubAuth, versionsRepoOwner, versionsRepo);
        }

        public LocalVersionsRepoUpdater CreateLocalVersionsRepoUpdater()
        {
            return new LocalVersionsRepoUpdater(_nupkgInfoFactory);
        }
    }
}
