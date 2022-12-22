// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.Protocol.Core.Types;

namespace NuGet.PackageManagement
{
    public static class PackagePreFetcher
    {
        /// <summary>
        /// Download all needed packages for install actions.
        /// </summary>
        public static async Task<Dictionary<PackageIdentity, PackagePreFetcherResult>> GetPackagesAsync(
            IEnumerable<NuGetProjectAction> actions,
            FolderNuGetProject packagesFolder,
            PackageDownloadContext downloadContext,
            string globalPackagesFolder,
            ILogger logger,
            CancellationToken token)
        {
            if (actions == null)
            {
                throw new ArgumentNullException(nameof(actions));
            }

            if (packagesFolder == null)
            {
                throw new ArgumentNullException(nameof(packagesFolder));
            }

            if (downloadContext == null)
            {
                throw new ArgumentNullException(nameof(downloadContext));
            }

            if (globalPackagesFolder == null)
            {
                throw new ArgumentNullException(nameof(globalPackagesFolder));
            }

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            var result = new Dictionary<PackageIdentity, PackagePreFetcherResult>();
            var maxParallelTasks = PackageManagementConstants.DefaultMaxDegreeOfParallelism;
            var toDownload = new Queue<NuGetProjectAction>();
            var seen = new HashSet<PackageIdentity>();

            // Find all uninstalled packages
            var uninstalledPackages = new HashSet<PackageIdentity>(
                actions.Where(action => action.NuGetProjectActionType == NuGetProjectActionType.Uninstall)
                .Select(action => action.PackageIdentity));

            // Check the packages folder for each package
            // If the package is not found mark it for download
            // These actions need to stay in order!
            foreach (var action in actions)
            {
                // Ignore uninstalls here
                // Avoid duplicate downloads
                if (action.NuGetProjectActionType == NuGetProjectActionType.Install
                    && seen.Add(action.PackageIdentity))
                {
                    string installPath = null;

                    // Packages that are also being uninstalled cannot come from the
                    // packages folder since it will be gone. This is true for reinstalls.
                    if (!uninstalledPackages.Contains(action.PackageIdentity))
                    {
                        // Check the packages folder for the id and version
                        installPath = packagesFolder.GetInstalledPackageFilePath(action.PackageIdentity);

                        // Verify the nupkg exists
                        if (!File.Exists(installPath))
                        {
                            installPath = null;
                        }
                    }

                    // installPath will contain the full path of the already installed nupkg if it
                    // exists. If the path is empty it will need to be downloaded.
                    if (!string.IsNullOrEmpty(installPath))
                    {
                        // Create a download result using the already installed package
                        var downloadResult = new PackagePreFetcherResult(installPath, action.PackageIdentity);
                        result.Add(action.PackageIdentity, downloadResult);
                    }
                    else
                    {
                        // Download this package
                        toDownload.Enqueue(action);
                    }
                }
            }

            // Check if any packages are not already in the packages folder
            if (toDownload.Count > 0)
            {
                var downloadResults = new List<PackagePreFetcherResult>(maxParallelTasks);

                while (toDownload.Count > 0)
                {
                    // Throttle tasks
                    if (downloadResults.Count == maxParallelTasks)
                    {
                        // Wait for a task to complete
                        // This will not throw, exceptions are stored in the result
                        await Task.WhenAny(downloadResults.Select(e => e.EnsureResultAsync()));

                        // Remove all completed tasks
                        downloadResults.RemoveAll(e => e.IsComplete);
                    }

                    var action = toDownload.Dequeue();

                    // Download the package if it does not exist in the packages folder already
                    // Start the download task
                    var task = Task.Run(async () => await PackageDownloader.GetDownloadResourceResultAsync(
                                        action.SourceRepository,
                                        action.PackageIdentity,
                                        downloadContext,
                                        globalPackagesFolder,
                                        logger,
                                        token));

                    var downloadResult = new PackagePreFetcherResult(
                        task,
                        action.PackageIdentity,
                        action.SourceRepository.PackageSource);

                    downloadResults.Add(downloadResult);
                    result.Add(action.PackageIdentity, downloadResult);
                }
            }

            // Do not wait for the remaining tasks to finish, these will download
            // in the background while other operations such as uninstall run first.
            return result;
        }

        /// <summary>
        /// Log a message to indicate where each package is being downloaded from
        /// </summary>
        public static void LogFetchMessages(
            IEnumerable<PackagePreFetcherResult> fetchResults,
            string packagesFolderRoot,
            ILogger logger)
        {
            if (fetchResults == null)
            {
                throw new ArgumentNullException(nameof(fetchResults));
            }

            if (packagesFolderRoot == null)
            {
                throw new ArgumentNullException(nameof(packagesFolderRoot));
            }

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            // Order by package identity
            var preFetchTasks = fetchResults.OrderBy(
                result => result.Package,
                PackageIdentityComparer.Default);

            foreach (var fetchResult in preFetchTasks)
            {
                string message = null;

                if (fetchResult.InPackagesFolder)
                {
                    // Found package .. in packages folder
                    message = string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.FoundPackageInPackagesFolder,
                        fetchResult.Package.Id,
                        fetchResult.Package.Version.ToNormalizedString(),
                        packagesFolderRoot);
                }
                else
                {
                    // Retrieving package .. from source ..
                    message = string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.RetrievingPackageStart,
                        fetchResult.Package.Id,
                        fetchResult.Package.Version.ToNormalizedString(),
                        fetchResult.Source.Name);
                }

                logger.LogMinimal(message);
            }
        }
    }
}
