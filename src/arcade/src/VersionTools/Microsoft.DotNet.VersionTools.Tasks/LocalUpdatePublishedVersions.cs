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
    public class LocalUpdatePublishedVersions : MSBuildTaskBase
    {
        [Required]
        public ITaskItem[] ShippedNuGetPackage { get; set; }

        [Required]
        public string VersionsRepoLocalBaseDir { get; set; }

        [Required]
        public string VersionsRepoPath { get; set; }

        public string GitHubAuthToken { get; set; }
        public string GitHubUser { get; set; }

        /// <summary>
        /// If specified, create the local build-infos based on the information available in the
        /// versions repo. Specifically, Latest_Packages.txt will contain the latest version of each
        /// package, even if this build didn't produce that certain package. Useful when servicing,
        /// where a subset of packages are built.
        /// </summary>
        public string VersionsRepo { get; set; }
        public string VersionsRepoOwner { get; set; }
        public string VersionsRepoBranch { get; set; } = "master";

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
                var updater = versionsRepoUpdaterFactory.CreateLocalVersionsRepoUpdater();

                if (!string.IsNullOrEmpty(GitHubAuthToken))
                {
                    updater.GitHubAuth = new GitHubAuth(GitHubAuthToken, GitHubUser);
                }

                GitHubBranch branch = null;
                if (!string.IsNullOrEmpty(VersionsRepo))
                {
                    branch = new GitHubBranch(
                        VersionsRepoBranch,
                        new GitHubProject(
                            VersionsRepo,
                            VersionsRepoOwner));
                }

                updater
                    .UpdateBuildInfoFilesAsync(
                        ShippedNuGetPackage.Select(i => i.ItemSpec),
                        VersionsRepoLocalBaseDir,
                        VersionsRepoPath,
                        branch)
                    .Wait();
            });
            return true;
        }
    }
}
