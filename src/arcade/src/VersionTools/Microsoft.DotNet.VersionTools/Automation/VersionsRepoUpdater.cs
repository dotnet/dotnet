// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.VersionTools.Automation.GitHubApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.DotNet.VersionTools.Automation
{
    public abstract class VersionsRepoUpdater
    {
        private readonly INupkgInfoFactory _nupkgInfoFactory;

        public VersionsRepoUpdater(INupkgInfoFactory nupkgInfoFactory)
        {
            _nupkgInfoFactory = nupkgInfoFactory;
        }

        protected IEnumerable<NupkgInfo> CreatePackageInfos(IEnumerable<string> packagePaths)
        {
            return packagePaths
                // Ignore symbol packages.
                .Where(path => !NupkgInfo.IsSymbolPackagePath(path))
                .Select(path => _nupkgInfoFactory.CreateNupkgInfo(path));
        }

        protected static Dictionary<string, string> CreatePackageInfoDictionary(IEnumerable<NupkgInfo> infos)
        {
            return infos.ToDictionary(i => i.Id, i => i.Version);
        }

        protected static string CreatePackageListContent(Dictionary<string, string> packages)
        {
            return string.Join(
                Environment.NewLine,
                packages
                    .OrderBy(t => t.Key)
                    .Select(t => $"{t.Key} {t.Value}"));
        }

        protected static async Task AddExistingPackages(
            GitHubClient client,
            GitHubBranch branch,
            string versionsRepoPath,
            Dictionary<string, string> packages)
        {
            Dictionary<string, string> existingPackages = await GetPackagesAsync(
                client,
                branch,
                $"{versionsRepoPath}/{BuildInfo.LatestPackagesTxtFilename}");

            if (existingPackages == null)
            {
                Trace.TraceInformation(
                    "No existing Latest_Packages file found; one will be " +
                    $"created in '{versionsRepoPath}'");
            }
            else
            {
                // Add each existing package if there isn't a new package with the same id.
                foreach (var package in existingPackages)
                {
                    if (!packages.ContainsKey(package.Key))
                    {
                        packages[package.Key] = package.Value;
                    }
                }
            }
        }

        private static async Task<Dictionary<string, string>> GetPackagesAsync(
            GitHubClient client,
            GitHubBranch branch,
            string path)
        {
            string latestPackages = await client.GetGitHubFileContentsAsync(path, branch);

            if (latestPackages == null)
            {
                return null;
            }

            using (var reader = new StringReader(latestPackages))
            {
                return await BuildInfo.ReadPackageListAsync(reader);
            }
        }

        protected static string GetPrereleaseVersion(NupkgInfo[] packages)
        {
            return packages
                .Select(t => t.Prerelease)
                .FirstOrDefault(prerelease => !string.IsNullOrEmpty(prerelease))
                ?? "stable";
        }
    }
}
