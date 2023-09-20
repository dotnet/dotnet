// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Packaging.PackageExtraction;
using NuGet.Packaging.Signing;
using NuGet.ProjectManagement;
using NuGet.ProjectManagement.Projects;
using NuGet.Protocol.Core.Types;

namespace NuGet.PackageManagement
{
    public class PackageRestoreManager : IPackageRestoreManager
    {
        private ISourceRepositoryProvider SourceRepositoryProvider { get; }
        private ISolutionManager SolutionManager { get; }
        private ISettings Settings { get; }

        public event EventHandler<PackagesMissingStatusEventArgs> PackagesMissingStatusChanged;
        public event EventHandler<PackageRestoredEventArgs> PackageRestoredEvent;
        public event EventHandler<PackageRestoreFailedEventArgs> PackageRestoreFailedEvent;

        private event AssetsFileMissingStatusChanged _assetsFileMissingStatusChanged;
        public event AssetsFileMissingStatusChanged AssetsFileMissingStatusChanged
        {
            add
            {
                _assetsFileMissingStatusChanged += value;
            }
            remove
            {
                _assetsFileMissingStatusChanged -= value;
            }
        }

        public PackageRestoreManager(
            ISourceRepositoryProvider sourceRepositoryProvider,
            ISettings settings,
            ISolutionManager solutionManager)
        {
            SourceRepositoryProvider = sourceRepositoryProvider ?? throw new ArgumentNullException(nameof(sourceRepositoryProvider));
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            SolutionManager = solutionManager ?? throw new ArgumentNullException(nameof(solutionManager));
        }

        public virtual async Task RaisePackagesMissingEventForSolutionAsync(string solutionDirectory, CancellationToken token)
        {
            // This method is called by different event handlers.
            // If the solutionDirectory is null, there's no need to do needless work.
            // Even though the solution closed even calls the synchronous ClearMissingEventForSolution
            // there's no guarantee that some weird ordering of events won't make the solutionDirectory null.
            var missing = false;
            if (!string.IsNullOrEmpty(solutionDirectory))
            {
                var packages = await GetPackagesInSolutionAsync(solutionDirectory, token);
                missing = packages.Any(p => p.IsMissing);
            }

            PackagesMissingStatusChanged?.Invoke(this, new PackagesMissingStatusEventArgs(missing));
        }

        public virtual void RaiseAssetsFileMissingEventForProjectAsync(bool isAssetsFileMissing)
        {
            if (_assetsFileMissingStatusChanged != null)
            {
                foreach (var handler in _assetsFileMissingStatusChanged.GetInvocationList())
                {
                    try
                    {
                        handler.DynamicInvoke(this, isAssetsFileMissing);
                    }
                    catch { }
                }
            }
        }

        // A synchronous method called during the solution closed event. This is done to avoid needless thread switching
        protected void ClearMissingEventForSolution()
        {
            PackagesMissingStatusChanged?.Invoke(this, new PackagesMissingStatusEventArgs(packagesMissing: false));
        }

        /// <summary>
        /// Get the missing packages in the solution given the <paramref name="solutionDirectory"></paramref>.
        /// </summary>
        /// <returns>
        /// Returns a read-only dictionary of missing package references and the corresponding project names on which
        /// each missing package is installed.
        /// </returns>
        public async Task<IEnumerable<PackageRestoreData>> GetPackagesInSolutionAsync(string solutionDirectory, CancellationToken token)
        {
            var packageReferencesDictionary = await GetPackagesReferencesDictionaryAsync(token);

            return GetPackagesRestoreData(solutionDirectory, packageReferencesDictionary);
        }

        /// <summary>
        /// Get packages restore data for given package references.
        /// </summary>
        /// <param name="solutionDirectory">Current solution directory</param>
        /// <param name="packageReferencesDict">Dictionary of package reference with project names</param>
        /// <returns>List of packages restore data with missing package details.</returns>
	    public IEnumerable<PackageRestoreData> GetPackagesRestoreData(string solutionDirectory,
        Dictionary<PackageReference, List<string>> packageReferencesDict)
        {
            var packages = new List<PackageRestoreData>();

            if (packageReferencesDict?.Any() == true)
            {
                var nuGetPackageManager = GetNuGetPackageManager(solutionDirectory);

                foreach ((var packageReference, var projectNames) in packageReferencesDict)
                {
                    var isMissing = false;
                    if (!nuGetPackageManager.PackageExistsInPackagesFolder(packageReference.PackageIdentity))
                    {
                        isMissing = true;
                    }

                    Debug.Assert(projectNames != null);
                    packages.Add(new PackageRestoreData(packageReference, projectNames, isMissing));
                }
            }

            return packages;
        }

        private async Task<Dictionary<PackageReference, List<string>>> GetPackagesReferencesDictionaryAsync(CancellationToken token)
        {
            var packageReferencesDict = new Dictionary<PackageReference, List<string>>(PackageReferenceComparer.Instance);
            if (!await SolutionManager.IsSolutionAvailableAsync())
            {
                return packageReferencesDict;
            }

            foreach (var nuGetProject in (await SolutionManager.GetNuGetProjectsAsync()))
            {
                // skip project k projects and build aware projects
                if (nuGetProject is INuGetIntegratedProject)
                {
                    continue;
                }

                try
                {
                    var nuGetProjectName = NuGetProject.GetUniqueNameOrName(nuGetProject);
                    var installedPackageReferences = await nuGetProject.GetInstalledPackagesAsync(token);
                    foreach (var installedPackageReference in installedPackageReferences)
                    {
                        List<string> projectNames = null;
                        if (!packageReferencesDict.TryGetValue(installedPackageReference, out projectNames))
                        {
                            projectNames = new List<string>();
                            packageReferencesDict.Add(installedPackageReference, projectNames);
                        }
                        projectNames.Add(nuGetProjectName);
                    }
                }
                catch (Exception)
                {
                    // ignore failed projects, and continue with other projects
                }
            }

            return packageReferencesDict;
        }

        /// <summary>
        /// Restores missing packages for the entire solution
        /// </summary>
        /// <returns></returns>
        public virtual async Task<PackageRestoreResult> RestoreMissingPackagesInSolutionAsync(
            string solutionDirectory,
            INuGetProjectContext nuGetProjectContext,
            CancellationToken token)
        {
            var packageReferencesDictionary = await GetPackagesReferencesDictionaryAsync(token);

            // When this method is called, the step to compute if a package is missing is implicit. Assume it is true
            var packages = packageReferencesDictionary.Select(p =>
            {
                Debug.Assert(p.Value != null);
                return new PackageRestoreData(p.Key, p.Value, isMissing: true);
            });

            using (var cacheContext = new SourceCacheContext())
            {
                var logger = new LoggerAdapter(nuGetProjectContext);

                var downloadContext = new PackageDownloadContext(cacheContext)
                {
                    ParentId = nuGetProjectContext.OperationId,
                    ClientPolicyContext = ClientPolicyContext.GetClientPolicy(Settings, logger)
                };

                return await RestoreMissingPackagesAsync(
                    solutionDirectory,
                    packages,
                    nuGetProjectContext,
                    downloadContext,
                    NullLogger.Instance,
                    token);
            }
        }

        /// <summary>
        /// Restores missing packages for the entire solution
        /// </summary>
        /// <returns></returns>
        public virtual async Task<PackageRestoreResult> RestoreMissingPackagesInSolutionAsync(
            string solutionDirectory,
            INuGetProjectContext nuGetProjectContext,
            ILogger logger,
            CancellationToken token)
        {
            var packageReferencesDictionary = await GetPackagesReferencesDictionaryAsync(token);

            // When this method is called, the step to compute if a package is missing is implicit. Assume it is true
            var packages = packageReferencesDictionary.Select(p =>
            {
                Debug.Assert(p.Value != null);
                return new PackageRestoreData(p.Key, p.Value, isMissing: true);
            });

            using (var cacheContext = new SourceCacheContext())
            {
                var adapterLogger = new LoggerAdapter(nuGetProjectContext);

                var downloadContext = new PackageDownloadContext(cacheContext)
                {
                    ParentId = nuGetProjectContext.OperationId,
                    ClientPolicyContext = ClientPolicyContext.GetClientPolicy(Settings, adapterLogger)
                };

                return await RestoreMissingPackagesAsync(
                    solutionDirectory,
                    packages,
                    nuGetProjectContext,
                    downloadContext,
                    logger,
                    token);
            }
        }

        public virtual Task<PackageRestoreResult> RestoreMissingPackagesAsync(string solutionDirectory,
            IEnumerable<PackageRestoreData> packages,
            INuGetProjectContext nuGetProjectContext,
            PackageDownloadContext downloadContext,
            CancellationToken token)
        {
            if (packages == null)
            {
                throw new ArgumentNullException(nameof(packages));
            }

            var nuGetPackageManager = GetNuGetPackageManager(solutionDirectory);

            var packageRestoreContext = new PackageRestoreContext(
                nuGetPackageManager,
                packages,
                token,
                PackageRestoredEvent,
                PackageRestoreFailedEvent,
                sourceRepositories: SourceRepositoryProvider.GetRepositories(),
                maxNumberOfParallelTasks: PackageManagementConstants.DefaultMaxDegreeOfParallelism,
                logger: NullLogger.Instance);

            if (nuGetProjectContext.PackageExtractionContext == null)
            {
                nuGetProjectContext.PackageExtractionContext = new PackageExtractionContext(
                    PackageSaveMode.Defaultv2,
                    PackageExtractionBehavior.XmlDocFileSaveMode,
                    ClientPolicyContext.GetClientPolicy(Settings, packageRestoreContext.Logger),
                    packageRestoreContext.Logger);
            }

            return RestoreMissingPackagesAsync(packageRestoreContext, nuGetProjectContext, downloadContext);
        }

        public virtual Task<PackageRestoreResult> RestoreMissingPackagesAsync(string solutionDirectory,
            IEnumerable<PackageRestoreData> packages,
            INuGetProjectContext nuGetProjectContext,
            PackageDownloadContext downloadContext,
            ILogger logger,
            CancellationToken token)
        {
            if (packages == null)
            {
                throw new ArgumentNullException(nameof(packages));
            }

            var nuGetPackageManager = GetNuGetPackageManager(solutionDirectory);

            var packageRestoreContext = new PackageRestoreContext(
                nuGetPackageManager,
                packages,
                token,
                PackageRestoredEvent,
                PackageRestoreFailedEvent,
                sourceRepositories: SourceRepositoryProvider.GetRepositories(),
                maxNumberOfParallelTasks: PackageManagementConstants.DefaultMaxDegreeOfParallelism,
                logger: logger);

            if (nuGetProjectContext.PackageExtractionContext == null)
            {
                nuGetProjectContext.PackageExtractionContext = new PackageExtractionContext(
                    PackageSaveMode.Defaultv2,
                    PackageExtractionBehavior.XmlDocFileSaveMode,
                    ClientPolicyContext.GetClientPolicy(Settings, packageRestoreContext.Logger),
                    packageRestoreContext.Logger);
            }

            return RestoreMissingPackagesAsync(packageRestoreContext, nuGetProjectContext, downloadContext);
        }

        private NuGetPackageManager GetNuGetPackageManager(string solutionDirectory)
        {
            var packagesFolderPath = PackagesFolderPathUtility.GetPackagesFolderPath(solutionDirectory, Settings);
            return new NuGetPackageManager(
                SourceRepositoryProvider,
                Settings,
                packagesFolderPath);
        }

        /// <summary>
        /// The static method which takes in all the possible parameters
        /// </summary>
        /// <returns>Returns true if at least one of the packages needed to be restored and got restored</returns>
        /// <remarks>
        /// Best use case is 'nuget.exe restore .sln' where there is no project loaded and there is no SolutionManager.
        /// The references are obtained by parsing of solution file and by using PackagesConfigReader. In this case,
        /// you don't construct an object of PackageRestoreManager,
        /// but just the NuGetPackageManager using constructor that does not need the SolutionManager, and, optionally
        /// register to events and/or specify the source repositories
        /// </remarks>
        public static async Task<PackageRestoreResult> RestoreMissingPackagesAsync(
            PackageRestoreContext packageRestoreContext,
            INuGetProjectContext nuGetProjectContext,
            PackageDownloadContext downloadContext)
        {
            if (packageRestoreContext == null)
            {
                throw new ArgumentNullException(nameof(packageRestoreContext));
            }

            if (nuGetProjectContext == null)
            {
                throw new ArgumentNullException(nameof(nuGetProjectContext));
            }

            ActivityCorrelationId.StartNew();

            var missingPackages = packageRestoreContext.Packages.Where(p => p.IsMissing).ToList();
            if (!missingPackages.Any())
            {
                return new PackageRestoreResult(true, Enumerable.Empty<PackageIdentity>());
            }

            // It is possible that the dictionary passed in may not have used the PackageReferenceComparer.
            // So, just to be sure, create a hashset with the keys from the dictionary using the PackageReferenceComparer
            // Now, we are guaranteed to not restore the same package more than once
            var hashSetOfMissingPackageReferences = new HashSet<PackageReference>(missingPackages.Select(p => p.PackageReference), PackageReferenceComparer.Instance);

            nuGetProjectContext.PackageExtractionContext.CopySatelliteFiles = false;

            packageRestoreContext.Token.ThrowIfCancellationRequested();

            foreach (SourceRepository enabledSource in packageRestoreContext.SourceRepositories)
            {
                PackageSource source = enabledSource.PackageSource;
                if (source.IsHttp && !source.IsHttps)
                {
                    packageRestoreContext.Logger.Log(LogLevel.Warning, string.Format(CultureInfo.CurrentCulture, Strings.Warning_HttpServerUsage, "restore", source.Source));
                }
            }

            var attemptedPackages = await ThrottledPackageRestoreAsync(
                hashSetOfMissingPackageReferences,
                packageRestoreContext,
                nuGetProjectContext,
                downloadContext);

            packageRestoreContext.Token.ThrowIfCancellationRequested();

            await ThrottledCopySatelliteFilesAsync(
                hashSetOfMissingPackageReferences,
                packageRestoreContext,
                nuGetProjectContext);

            return new PackageRestoreResult(
                attemptedPackages.All(p => p.Restored),
                attemptedPackages.Select(p => p.Package.PackageIdentity).ToList());
        }

        /// <summary>
        /// ThrottledPackageRestoreAsync method throttles the number of tasks created to perform package restore in
        /// parallel
        /// The maximum number of parallel tasks that may be created can be specified via
        /// <paramref name="packageRestoreContext" />
        /// The method creates a ConcurrentQueue of passed in <paramref name="packageReferences" />. And, creates a
        /// fixed number of tasks
        /// that dequeue from the ConcurrentQueue and perform package restore. So, this method should pre-populate the
        /// queue and must not enqueued to by other methods
        /// </summary>
        private static async Task<IEnumerable<AttemptedPackage>> ThrottledPackageRestoreAsync(
            HashSet<PackageReference> packageReferences,
            PackageRestoreContext packageRestoreContext,
            INuGetProjectContext nuGetProjectContext,
            PackageDownloadContext downloadContext)
        {
            var packageReferencesQueue = new ConcurrentQueue<PackageReference>(packageReferences);
            var tasks = new List<Task<List<AttemptedPackage>>>();
            for (var i = 0; i < Math.Min(packageRestoreContext.MaxNumberOfParallelTasks, packageReferences.Count); i++)
            {
                tasks.Add(Task.Run(() => PackageRestoreRunnerAsync(
                    packageReferencesQueue,
                    packageRestoreContext,
                    nuGetProjectContext,
                    downloadContext)));
            }

            return (await Task.WhenAll(tasks)).SelectMany(package => package);
        }

        /// <summary>
        /// This is the runner which dequeues package references from <paramref name="packageReferencesQueue" />, and
        /// performs package restore
        /// Note that this method should only Dequeue from the concurrent queue and not Enqueue
        /// </summary>
        private static async Task<List<AttemptedPackage>> PackageRestoreRunnerAsync(
            ConcurrentQueue<PackageReference> packageReferencesQueue,
            PackageRestoreContext packageRestoreContext,
            INuGetProjectContext nuGetProjectContext,
            PackageDownloadContext downloadContext)
        {
            PackageReference currentPackageReference = null;

            var attemptedPackages = new List<AttemptedPackage>();

            while (packageReferencesQueue.TryDequeue(out currentPackageReference))
            {
                var attemptedPackage = await RestorePackageAsync(
                    currentPackageReference,
                    packageRestoreContext,
                    nuGetProjectContext,
                    downloadContext);

                attemptedPackages.Add(attemptedPackage);
            }

            return attemptedPackages;
        }

        /// <summary>
        /// ThrottledCopySatelliteFilesAsync method throttles the number of tasks created to perform copy satellite
        /// files in parallel
        /// The maximum number of parallel tasks that may be created can be specified via
        /// <paramref name="packageRestoreContext" />
        /// The method creates a ConcurrentQueue of passed in <paramref name="packageReferences" />. And, creates a
        /// fixed number of tasks
        /// that dequeue from the ConcurrentQueue and perform copying of satellite files. So, this method should
        /// pre-populate the queue and must not enqueued to by other methods
        /// </summary>
        private static Task ThrottledCopySatelliteFilesAsync(HashSet<PackageReference> packageReferences,
            PackageRestoreContext packageRestoreContext,
            INuGetProjectContext nuGetProjectContext)
        {
            var packageReferencesQueue = new ConcurrentQueue<PackageReference>(packageReferences);
            var tasks = new List<Task>();
            for (var i = 0; i < Math.Min(packageRestoreContext.MaxNumberOfParallelTasks, packageReferences.Count); i++)
            {
                tasks.Add(Task.Run(() => CopySatelliteFilesRunnerAsync(packageReferencesQueue, packageRestoreContext, nuGetProjectContext)));
            }

            return Task.WhenAll(tasks);
        }

        private static async Task<AttemptedPackage> RestorePackageAsync(
            PackageReference packageReference,
            PackageRestoreContext packageRestoreContext,
            INuGetProjectContext nuGetProjectContext,
            PackageDownloadContext downloadContext)
        {
            Exception exception = null;
            var restored = false;
            try
            {
                restored = await packageRestoreContext.PackageManager.RestorePackageAsync(
                    packageReference.PackageIdentity,
                    nuGetProjectContext,
                    downloadContext,
                    packageRestoreContext.SourceRepositories,
                    packageRestoreContext.Token);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            packageRestoreContext.PackageRestoredEvent?.Invoke(null, new PackageRestoredEventArgs(packageReference.PackageIdentity, restored));

            // PackageReferences cannot be null here
            if (exception != null)
            {
                if (!string.IsNullOrEmpty(exception.Message))
                {
                    nuGetProjectContext.Log(MessageLevel.Error, exception.Message);
                }

                if (packageRestoreContext.PackageRestoreFailedEvent != null)
                {
                    var packageReferenceComparer = PackageReferenceComparer.Instance;

                    var packageRestoreData = packageRestoreContext.Packages
                        .Where(p => packageReferenceComparer.Equals(p.PackageReference, packageReference))
                        .SingleOrDefault();

                    if (packageRestoreData != null)
                    {
                        Debug.Assert(packageRestoreData.ProjectNames != null);
                        packageRestoreContext.PackageRestoreFailedEvent(
                            null,
                            new PackageRestoreFailedEventArgs(packageReference,
                                exception,
                                packageRestoreData.ProjectNames));
                    }
                }
            }

            return new AttemptedPackage
            {
                Restored = restored,
                Package = packageReference
            };
        }

        /// <summary>
        /// This is the runner which dequeues package references from <paramref name="packageReferencesQueue" />, and
        /// performs copying of satellite files
        /// Note that this method should only Dequeue from the concurrent queue and not Enqueue
        /// </summary>
        private static async Task CopySatelliteFilesRunnerAsync(ConcurrentQueue<PackageReference> packageReferencesQueue,
            PackageRestoreContext packageRestoreContext,
            INuGetProjectContext nuGetProjectContext)
        {
            PackageReference currentPackageReference = null;
            while (packageReferencesQueue.TryDequeue(out currentPackageReference))
            {
                var result = await packageRestoreContext.PackageManager.CopySatelliteFilesAsync(
                    currentPackageReference.PackageIdentity,
                    nuGetProjectContext,
                    packageRestoreContext.Token);
            }
        }

        private class AttemptedPackage
        {
            public bool Restored { get; set; }
            public PackageReference Package { get; set; }
        }
    }
}
