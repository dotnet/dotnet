// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.VersionTools.Automation.GitHubApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microsoft.DotNet.VersionTools.Automation
{
    public class GitHubVersionsRepoUpdater : VersionsRepoUpdater
    {
        private const int MaxTries = 10;
        private const int RetryMillisecondsDelay = 5000;

        private GitHubAuth _gitHubAuth;
        private GitHubProject _project;

        public GitHubVersionsRepoUpdater(
            INupkgInfoFactory nupkgInfoFactory,
            GitHubAuth gitHubAuth,
            string versionsRepoOwner = null,
            string versionsRepo = null)
            : this(
                gitHubAuth,
                new GitHubProject(versionsRepo ?? "versions", versionsRepoOwner),
                nupkgInfoFactory)
        {
        }

        public GitHubVersionsRepoUpdater(GitHubAuth gitHubAuth, GitHubProject project, INupkgInfoFactory nupkgInfoFactory) : base(nupkgInfoFactory)
        {
            if (gitHubAuth == null)
            {
                throw new ArgumentNullException(nameof(gitHubAuth));
            }
            _gitHubAuth = gitHubAuth;

            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }
            _project = project;
        }

        /// <param name="updateLatestVersion">If true, updates Latest.txt with a prerelease moniker. If there isn't one, makes the file empty.</param>
        /// <param name="updateLatestPackageList">If true, updates Latest_Packages.txt.</param>
        /// <param name="updateLastBuildPackageList">If true, updates Last_Build_Packages.txt, and enables keeping old packages in Latest_Packages.txt.</param>
        public async Task UpdateBuildInfoAsync(
            IEnumerable<string> packagePaths,
            string versionsRepoPath,
            bool updateLatestPackageList = true,
            bool updateLatestVersion = true,
            bool updateLastBuildPackageList = true)
        {
            if (packagePaths == null)
            {
                throw new ArgumentNullException(nameof(packagePaths));
            }
            if (versionsRepoPath == null)
            {
                throw new ArgumentNullException(nameof(versionsRepoPath));
            }

            NupkgInfo[] packages = CreatePackageInfos(packagePaths).ToArray();

            string prereleaseVersion = GetPrereleaseVersion(packages);

            Dictionary<string, string> packageDictionary = CreatePackageInfoDictionary(packages);

            using (GitHubClient client = new GitHubClient(_gitHubAuth))
            {
                for (int i = 0; i < MaxTries; i++)
                {
                    try
                    {
                        // Master commit to use as new commit's parent.
                        string masterRef = "heads/master";
                        GitReference currentMaster = await client.GetReferenceAsync(_project, masterRef);
                        string masterSha = currentMaster.Object.Sha;

                        List<GitObject> objects = new List<GitObject>();

                        if (updateLastBuildPackageList)
                        {
                            objects.Add(new GitObject
                            {
                                Path = $"{versionsRepoPath}/{BuildInfo.LastBuildPackagesTxtFilename}",
                                Type = GitObject.TypeBlob,
                                Mode = GitObject.ModeFile,
                                Content = CreatePackageListContent(packageDictionary)
                            });
                        }

                        if (updateLatestPackageList)
                        {
                            var allPackages = new Dictionary<string, string>(packageDictionary);

                            if (updateLastBuildPackageList)
                            {
                                await AddExistingPackages(
                                    client,
                                    new GitHubBranch("master", _project),
                                    versionsRepoPath,
                                    allPackages);
                            }

                            objects.Add(new GitObject
                            {
                                Path = $"{versionsRepoPath}/{BuildInfo.LatestPackagesTxtFilename}",
                                Type = GitObject.TypeBlob,
                                Mode = GitObject.ModeFile,
                                Content = CreatePackageListContent(allPackages)
                            });
                        }

                        if (updateLatestVersion)
                        {
                            objects.Add(new GitObject
                            {
                                Path = $"{versionsRepoPath}/{BuildInfo.LatestTxtFilename}",
                                Type = GitObject.TypeBlob,
                                Mode = GitObject.ModeFile,
                                Content = prereleaseVersion
                            });
                        }

                        string message = $"Updating {versionsRepoPath}";
                        if (string.IsNullOrEmpty(prereleaseVersion))
                        {
                            message += ". No prerelease versions published.";
                        }
                        else
                        {
                            message += $" for {prereleaseVersion}";
                        }

                        GitTree tree = await client.PostTreeAsync(_project, masterSha, objects.ToArray());
                        GitCommit commit = await client.PostCommitAsync(_project, message, tree.Sha, new[] { masterSha });

                        // Only fast-forward. Don't overwrite other changes: throw exception instead.
                        await client.PatchReferenceAsync(_project, masterRef, commit.Sha, force: false);

                        Trace.TraceInformation($"Committed build-info update on attempt {i + 1}.");
                        break;
                    }
                    catch (Exception ex) when (ex is HttpRequestException || ex is NotFastForwardUpdateException)
                    {
                        int nextTry = i + 1;
                        if (nextTry < MaxTries)
                        {
                            Trace.TraceInformation($"Encountered exception committing build-info update: {ex.Message}");
                            Trace.TraceInformation($"Trying again in {RetryMillisecondsDelay}ms. {MaxTries - nextTry} tries left.");
                            await Task.Delay(RetryMillisecondsDelay);
                        }
                        else
                        {
                            Trace.TraceInformation("Encountered exception committing build-info update.");
                            throw;
                        }
                    }
                }
            }
        }
    }
}
