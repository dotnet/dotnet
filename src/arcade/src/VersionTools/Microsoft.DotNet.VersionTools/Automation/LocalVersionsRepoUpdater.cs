// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.VersionTools.Automation.GitHubApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.DotNet.VersionTools.Automation
{
    public class LocalVersionsRepoUpdater : VersionsRepoUpdater
    {
        public LocalVersionsRepoUpdater(INupkgInfoFactory nupkgInfoFactory) : base(nupkgInfoFactory) { }

        public GitHubAuth GitHubAuth { get; set; }

        /// <summary>
        /// Create Latest_Packages, and if versionsRepoBranch is passed, Last_Build_Packages.
        /// </summary>
        public async Task UpdateBuildInfoFilesAsync(
            IEnumerable<string> packagePaths,
            string localBaseDir,
            string versionsRepoPath,
            GitHubBranch versionsRepoBranch)
        {
            if (packagePaths == null)
            {
                throw new ArgumentNullException(nameof(packagePaths));
            }
            if (string.IsNullOrEmpty(localBaseDir))
            {
                throw new ArgumentException(nameof(localBaseDir));
            }
            if (string.IsNullOrEmpty(versionsRepoPath))
            {
                throw new ArgumentException(nameof(versionsRepoPath));
            }

            string latestPackagesDir = Path.Combine(
                localBaseDir,
                versionsRepoPath);

            Directory.CreateDirectory(latestPackagesDir);

            NupkgInfo[] packages = CreatePackageInfos(packagePaths).ToArray();

            Dictionary<string, string> packageDictionary = CreatePackageInfoDictionary(packages);

            if (versionsRepoBranch != null)
            {
                File.WriteAllText(
                    Path.Combine(latestPackagesDir, BuildInfo.LastBuildPackagesTxtFilename),
                    CreatePackageListContent(packageDictionary));

                using (var client = new GitHubClient(GitHubAuth))
                {
                    await AddExistingPackages(
                        client,
                        versionsRepoBranch,
                        versionsRepoPath,
                        packageDictionary);
                }
            }

            File.WriteAllText(
                Path.Combine(latestPackagesDir, BuildInfo.LatestTxtFilename),
                GetPrereleaseVersion(packages));

            File.WriteAllText(
                Path.Combine(latestPackagesDir, BuildInfo.LatestPackagesTxtFilename),
                CreatePackageListContent(packageDictionary));
        }
    }
}
