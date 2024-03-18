// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Arcade.Common;
using Microsoft.Build.Framework;
using Microsoft.DotNet.VersionTools.Automation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Diagnostics;
using System.Linq;

namespace Microsoft.DotNet.Build.Tasks.VersionTools
{
    public class UpdatePublishedVersions : MSBuildTaskBase
    {
        [Required]
        public ITaskItem[] ShippedNuGetPackage { get; set; }

        [Required]
        public string VersionsRepoPath { get; set; }

        [Required]
        public string GitHubAuthToken { get; set; }
        public string GitHubUser { get; set; }
        public string GitHubEmail { get; set; }

        public string VersionsRepo { get; set; }
        public string VersionsRepoOwner { get; set; }

        public override void ConfigureServices(IServiceCollection collection)
        {
            collection.TryAddSingleton<INupkgInfoFactory, NupkgInfoFactory>();
            collection.TryAddSingleton<IPackageArchiveReaderFactory, PackageArchiveReaderFactory>();
            collection.TryAddSingleton<IVersionsRepoUpdaterFactory, VersionsRepoUpdaterFactory>();
        }

        public bool ExecuteTask(IVersionsRepoUpdaterFactory versionsRepoUpdaterFactory)
        {
            Trace.Listeners.MsBuildListenedInvoke(Log, () =>
            {
                var gitHubAuth = new GitHubAuth(GitHubAuthToken, GitHubUser, GitHubEmail);

                var updater = versionsRepoUpdaterFactory.CreateGitHubVersionsRepoUpdater(gitHubAuth, VersionsRepoOwner, VersionsRepo);

                updater.UpdateBuildInfoAsync(
                    ShippedNuGetPackage.Select(item => item.ItemSpec),
                    VersionsRepoPath)
                    .Wait();
            });
            return true;
        }
    }
}
