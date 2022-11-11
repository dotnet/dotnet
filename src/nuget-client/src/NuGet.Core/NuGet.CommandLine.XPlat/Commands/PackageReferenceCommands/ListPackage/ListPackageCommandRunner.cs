// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Build.Evaluation;
using NuGet.CommandLine.XPlat.ListPackage;
using NuGet.CommandLine.XPlat.Utility;
using NuGet.Configuration;
using NuGet.Packaging;
using NuGet.ProjectModel;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace NuGet.CommandLine.XPlat
{
    internal class ListPackageCommandRunner : IListPackageCommandRunner
    {
        private const string ProjectAssetsFile = "ProjectAssetsFile";
        private const string ProjectName = "MSBuildProjectName";
        private const int GenericSuccessExitCode = 0;
        private const int GenericFailureExitCode = 1;
        private Dictionary<PackageSource, SourceRepository> _sourceRepositoryCache;

        public ListPackageCommandRunner()
        {
            _sourceRepositoryCache = new Dictionary<PackageSource, SourceRepository>();
        }

        public async Task<int> ExecuteCommandAsync(ListPackageArgs listPackageArgs)
        {
            IReportRenderer reportRenderer = listPackageArgs.Renderer;
            (int exitCode, ListPackageReportModel reportModel) = await GetReportDataAsync(listPackageArgs);
            reportRenderer.Render(reportModel);
            return exitCode;
        }

        internal async Task<(int, ListPackageReportModel)> GetReportDataAsync(ListPackageArgs listPackageArgs)
        {
            // It's important not to print anything to console from below methods and sub method calls, because it'll affect both json/console outputs.
            var listPackageReportModel = new ListPackageReportModel(listPackageArgs);
            if (!File.Exists(listPackageArgs.Path))
            {
                listPackageArgs.Renderer.AddProblem(problemType: ProblemType.Error,
                    text: string.Format(CultureInfo.CurrentCulture,
                        Strings.ListPkg_ErrorFileNotFound,
                        listPackageArgs.Path));
                return (GenericFailureExitCode, listPackageReportModel);
            }
            //If the given file is a solution, get the list of projects
            //If not, then it's a project, which is put in a list
            var projectsPaths = Path.GetExtension(listPackageArgs.Path).Equals(".sln") ?
                           MSBuildAPIUtility.GetProjectsFromSolution(listPackageArgs.Path).Where(f => File.Exists(f)) :
                           new List<string>(new string[] { listPackageArgs.Path });

            MSBuildAPIUtility msBuild = listPackageReportModel.MSBuildAPIUtility;

            foreach (string projectPath in projectsPaths)
            {
                await GetProjectMetadataAsync(projectPath, listPackageReportModel, msBuild, listPackageArgs);
            }

            // if there is any error then return failure code.
            int exitCode = (
                listPackageArgs.Renderer.GetProblems().Any(p => p.ProblemType == ProblemType.Error)
                || listPackageReportModel.Projects.Where(p => p.ProjectProblems != null).SelectMany(p => p.ProjectProblems).Any(p => p.ProblemType == ProblemType.Error))
                ? GenericFailureExitCode : GenericSuccessExitCode;

            return (exitCode, listPackageReportModel);
        }

        private async Task GetProjectMetadataAsync(
            string projectPath,
            ListPackageReportModel listPackageReportModel,
            MSBuildAPIUtility msBuild,
            ListPackageArgs listPackageArgs)
        {
            //Open project to evaluate properties for the assets
            //file and the name of the project
            Project project = MSBuildAPIUtility.GetProject(projectPath);
            var projectName = project.GetPropertyValue(ProjectName);
            ListPackageProjectModel projectModel = listPackageReportModel.CreateProjectReportData(projectPath: projectPath, projectName);

            if (!MSBuildAPIUtility.IsPackageReferenceProject(project))
            {
                projectModel.AddProjectInformation(problemType: ProblemType.Error,
                    string.Format(CultureInfo.CurrentCulture, Strings.Error_NotPRProject, projectPath));
                return;
            }

            var assetsPath = project.GetPropertyValue(ProjectAssetsFile);

            if (!File.Exists(assetsPath))
            {
                projectModel.AddProjectInformation(ProblemType.Error,
                    string.Format(CultureInfo.CurrentCulture, Strings.Error_AssetsFileNotFound, projectPath));
            }
            else
            {
                var lockFileFormat = new LockFileFormat();
                LockFile assetsFile = lockFileFormat.Read(assetsPath);

                // Assets file validation
                if (assetsFile.PackageSpec != null &&
                    assetsFile.Targets != null &&
                    assetsFile.Targets.Count != 0)
                {
                    // Get all the packages that are referenced in a project
                    IEnumerable<FrameworkPackages> packages;
                    try
                    {
                        packages = msBuild.GetResolvedVersions(project.FullPath, listPackageArgs.Frameworks, assetsFile, listPackageArgs.IncludeTransitive, includeProjects: listPackageArgs.ReportType == ReportType.Default);
                    }
                    catch (InvalidOperationException ex)
                    {
                        projectModel.AddProjectInformation(ProblemType.Error, ex.Message);
                        return;
                    }

                    if (packages.Any())
                    {
                        if (listPackageArgs.ReportType != ReportType.Default)  // generic list package is offline -- no server lookups
                        {
                            PopulateSourceRepositoryCache(listPackageArgs);
                            WarnForHttpSources(listPackageArgs, projectModel);
                            await GetRegistrationMetadataAsync(packages, listPackageArgs);
                            await AddLatestVersionsAsync(packages, listPackageArgs);
                        }

                        bool printPackages = FilterPackages(packages, listPackageArgs);
                        printPackages = printPackages || ReportType.Default == listPackageArgs.ReportType;
                        if (printPackages)
                        {
                            var hasAutoReference = false;
                            List<ListPackageReportFrameworkPackage> projectFrameworkPackages = ProjectPackagesPrintUtility.GetPackagesMetadata(packages, listPackageArgs, ref hasAutoReference);
                            projectModel.TargetFrameworkPackages = projectFrameworkPackages;
                            projectModel.AutoReferenceFound = hasAutoReference;
                        }
                        else
                        {
                            projectModel.TargetFrameworkPackages = new List<ListPackageReportFrameworkPackage>();
                        }
                    }
                }
                else
                {
                    projectModel.AddProjectInformation(ProblemType.Error,
                        string.Format(CultureInfo.CurrentCulture, Strings.ListPkg_ErrorReadingAssetsFile, assetsPath));
                }

                // Unload project
                ProjectCollection.GlobalProjectCollection.UnloadProject(project);
            }
        }

        private static void WarnForHttpSources(ListPackageArgs listPackageArgs, ListPackageProjectModel projectModel)
        {
            List<PackageSource> httpPackageSources = null;
            foreach (PackageSource packageSource in listPackageArgs.PackageSources)
            {
                if (packageSource.IsHttp && !packageSource.IsHttps)
                {
                    if (httpPackageSources == null)
                    {
                        httpPackageSources = new();
                    }
                    httpPackageSources.Add(packageSource);
                }
            }

            if (httpPackageSources != null && httpPackageSources.Count != 0)
            {
                if (httpPackageSources.Count == 1)
                {
                    projectModel.AddProjectInformation(
                        ProblemType.Warning,
                        string.Format(CultureInfo.CurrentCulture,
                        Strings.Warning_HttpServerUsage,
                        "list package",
                        httpPackageSources[0]));
                }
                else
                {
                    projectModel.AddProjectInformation(
                        ProblemType.Warning,
                        string.Format(CultureInfo.CurrentCulture,
                        Strings.Warning_HttpServerUsage_MultipleSources,
                        "list package",
                        Environment.NewLine + string.Join(Environment.NewLine, httpPackageSources.Select(e => e.Name))));
                }
            }

        }

        public static bool FilterPackages(IEnumerable<FrameworkPackages> packages, ListPackageArgs listPackageArgs)
        {
            switch (listPackageArgs.ReportType)
            {
                case ReportType.Default: break; // No filtering in this case
                case ReportType.Outdated:
                    FilterPackages(
                        packages,
                        ListPackageHelper.TopLevelPackagesFilterForOutdated,
                        ListPackageHelper.TransitivePackagesFilterForOutdated);
                    break;
                case ReportType.Deprecated:
                    FilterPackages(
                        packages,
                        ListPackageHelper.PackagesFilterForDeprecated,
                        ListPackageHelper.PackagesFilterForDeprecated);
                    break;
                case ReportType.Vulnerable:
                    FilterPackages(
                        packages,
                        ListPackageHelper.PackagesFilterForVulnerable,
                        ListPackageHelper.PackagesFilterForVulnerable);
                    break;
            }

            return packages.Any(p => p.TopLevelPackages.Any() ||
                                     listPackageArgs.IncludeTransitive && p.TransitivePackages.Any());
        }

        /// <summary>
        /// Filters top-level and transitive packages.
        /// </summary>
        /// <param name="packages">The <see cref="FrameworkPackages"/> to filter.</param>
        /// <param name="topLevelPackagesFilter">The filter to be applied on all <see cref="FrameworkPackages.TopLevelPackages"/>.</param>
        /// <param name="transitivePackagesFilter">The filter to be applied on all <see cref="FrameworkPackages.TransitivePackages"/>.</param>
        private static void FilterPackages(
            IEnumerable<FrameworkPackages> packages,
            Func<InstalledPackageReference, bool> topLevelPackagesFilter,
            Func<InstalledPackageReference, bool> transitivePackagesFilter)
        {
            foreach (var frameworkPackages in packages)
            {
                frameworkPackages.TopLevelPackages = GetInstalledPackageReferencesWithFilter(
                    frameworkPackages.TopLevelPackages, topLevelPackagesFilter);

                frameworkPackages.TransitivePackages = GetInstalledPackageReferencesWithFilter(
                    frameworkPackages.TransitivePackages, transitivePackagesFilter);
            }
        }

        private static IEnumerable<InstalledPackageReference> GetInstalledPackageReferencesWithFilter(
            IEnumerable<InstalledPackageReference> references,
            Func<InstalledPackageReference, bool> filter)
        {
            var filteredReferences = new List<InstalledPackageReference>();
            foreach (var reference in references)
            {
                if (filter(reference))
                {
                    filteredReferences.Add(reference);
                }
            }

            return filteredReferences;
        }

        /// <summary>
        /// Fetches the latest versions for all of the packages that are
        /// to be listed
        /// </summary>
        /// <param name="packages">The packages found in a project</param>
        /// <param name="listPackageArgs">List args for the token and source provider</param>
        /// <returns>A data structure like packages, but includes the latest versions</returns>
        private async Task AddLatestVersionsAsync(
            IEnumerable<FrameworkPackages> packages,
            ListPackageArgs listPackageArgs)
        {
            //Unique Dictionary for packages and list of latest versions to handle different sources
            var packagesVersionsDict = new Dictionary<string, IList<IPackageSearchMetadata>>();
            AddPackagesToDict(packages, packagesVersionsDict);

            //Prepare requests for each of the packages
            var getLatestVersionsRequests = new List<Task>();
            foreach (var package in packagesVersionsDict)
            {
                getLatestVersionsRequests.AddRange(
                    PrepareLatestVersionsRequests(
                        package.Key,
                        listPackageArgs,
                        packagesVersionsDict));
            }

            // Make requests in parallel.
            await RequestNuGetResourcesInParallelAsync(getLatestVersionsRequests);

            //Save latest versions within the InstalledPackageReference
            await GetVersionsFromDictAsync(packages, packagesVersionsDict, listPackageArgs);
        }

        /// <summary>
        /// Fetches additional info (e.g. deprecation, vulnerability) for all of the packages found in a project.
        /// </summary>
        /// <param name="packages">The packages found in a project.</param>
        /// <param name="listPackageArgs">List args for the token and source provider</param>
        private async Task GetRegistrationMetadataAsync(
            IEnumerable<FrameworkPackages> packages,
            ListPackageArgs listPackageArgs)
        {
            // Unique dictionary for packages and list of versions to handle different sources
            var packagesVersionsDict = new Dictionary<string, IList<IPackageSearchMetadata>>();
            AddPackagesToDict(packages, packagesVersionsDict);

            // Clone and filter package versions to avoid duplicate requests
            // and to avoid collection being enumerated to be modified.
            var distinctPackageVersionsDict = GetUniqueResolvedPackages(packages);

            // Prepare requests for each of the packages
            var resourceRequestTasks = new List<Task>();
            foreach (var packageIdAndVersions in distinctPackageVersionsDict)
            {
                foreach (var packageVersion in packageIdAndVersions.Value)
                {
                    resourceRequestTasks.AddRange(
                        PrepareCurrentVersionsRequests(
                            packageIdAndVersions.Key,
                            packageVersion,
                            listPackageArgs,
                            packagesVersionsDict));
                }
            }

            // Make requests in parallel.
            await RequestNuGetResourcesInParallelAsync(resourceRequestTasks);

            // Save resolved versions within the InstalledPackageReference
            await GetVersionsFromDictAsync(packages, packagesVersionsDict, listPackageArgs);
        }

        /// <summary>
        /// Pre-populate _sourceRepositoryCache so source repository can be reused between different calls.
        /// </summary>
        /// <param name="listPackageArgs">List args for the token and source provider</param>
        private void PopulateSourceRepositoryCache(ListPackageArgs listPackageArgs)
        {
            IEnumerable<Lazy<INuGetResourceProvider>> providers = Repository.Provider.GetCoreV3();
            IEnumerable<PackageSource> sources = listPackageArgs.PackageSources;
            foreach (PackageSource source in sources)
            {
                SourceRepository sourceRepository = Repository.CreateSource(providers, source, FeedType.Undefined);
                _sourceRepositoryCache[source] = sourceRepository;
            }
        }

        /// <summary>
        /// Handles concurrency and throttling for a list of tasks that request NuGet resources.
        /// </summary>
        /// <param name="resourceRequestTasks"></param>
        /// <returns></returns>
        private static async Task RequestNuGetResourcesInParallelAsync(IReadOnlyList<Task> resourceRequestTasks)
        {
            // Handling concurrency and throttling variables
            var maxTasks = Environment.ProcessorCount;
            var contactSourcesRunningTasks = new List<Task>();

            // Make the calls to the sources
            foreach (var requestTask in resourceRequestTasks)
            {
                contactSourcesRunningTasks.Add(Task.Run(() => requestTask));

                // Throttle if needed
                if (maxTasks <= contactSourcesRunningTasks.Count)
                {
                    var finishedTask = await Task.WhenAny(contactSourcesRunningTasks);
                    contactSourcesRunningTasks.Remove(finishedTask);
                }
            }

            await Task.WhenAll(contactSourcesRunningTasks);
        }

        /// <summary>
        /// Adding the packages to a unique set to avoid attempting
        /// to get the latest versions for the same package multiple
        /// times
        /// </summary>
        /// <param name="packages"> Packages found in the project </param>
        /// <param name="packagesVersionsDict"> An empty dictionary to be filled with packages </param>
        private void AddPackagesToDict(IEnumerable<FrameworkPackages> packages,
            Dictionary<string, IList<IPackageSearchMetadata>> packagesVersionsDict)
        {
            foreach (var frameworkPackages in packages)
            {
                foreach (var topLevelPackage in frameworkPackages.TopLevelPackages)
                {
                    if (!packagesVersionsDict.ContainsKey(topLevelPackage.Name))
                    {
                        packagesVersionsDict.Add(topLevelPackage.Name, new List<IPackageSearchMetadata>());
                    }
                }

                foreach (var transitivePackage in frameworkPackages.TransitivePackages)
                {
                    if (!packagesVersionsDict.ContainsKey(transitivePackage.Name))
                    {
                        packagesVersionsDict.Add(transitivePackage.Name, new List<IPackageSearchMetadata>());
                    }
                }
            }
        }

        /// <summary>
        /// Returns the unique resolved packages to avoid duplicates.
        /// </summary>
        /// <param name="packages">Packages found in the project</param>
        private static IDictionary<string, IList<NuGetVersion>> GetUniqueResolvedPackages(IEnumerable<FrameworkPackages> packages)
        {
            var results = new Dictionary<string, IList<NuGetVersion>>();

            foreach (var frameworkPackages in packages)
            {
                foreach (var topLevelPackage in frameworkPackages.TopLevelPackages)
                {
                    if (!results.ContainsKey(topLevelPackage.Name))
                    {
                        results.Add(topLevelPackage.Name, new List<NuGetVersion>
                        {
                            topLevelPackage.ResolvedPackageMetadata.Identity.Version
                        });
                    }
                    else
                    {
                        var versions = results[topLevelPackage.Name];

                        if (!versions.Contains(topLevelPackage.ResolvedPackageMetadata.Identity.Version))
                        {
                            versions.Add(topLevelPackage.ResolvedPackageMetadata.Identity.Version);
                        }
                    }
                }

                foreach (var transitivePackage in frameworkPackages.TransitivePackages)
                {
                    if (!results.ContainsKey(transitivePackage.Name))
                    {
                        results.Add(transitivePackage.Name, new List<NuGetVersion>
                        {
                            transitivePackage.ResolvedPackageMetadata.Identity.Version
                        });
                    }
                    else
                    {
                        var versions = results[transitivePackage.Name];

                        if (!versions.Contains(transitivePackage.ResolvedPackageMetadata.Identity.Version))
                        {
                            versions.Add(transitivePackage.ResolvedPackageMetadata.Identity.Version);
                        }
                    }
                }
            }

            return results;
        }

        /// <summary>
        /// Get last versions for every package from the unique packages
        /// </summary>
        /// <param name="packages"> Project packages to get filled with latest versions </param>
        /// <param name="packagesVersionsDict"> Unique packages that are mapped to latest versions
        /// from different sources </param>
        /// <param name="listPackageArgs">Arguments for list package to get the right latest version</param>
        private async Task GetVersionsFromDictAsync(
            IEnumerable<FrameworkPackages> packages,
            Dictionary<string, IList<IPackageSearchMetadata>> packagesVersionsDict,
            ListPackageArgs listPackageArgs)
        {
            foreach (var frameworkPackages in packages)
            {
                foreach (var topLevelPackage in frameworkPackages.TopLevelPackages)
                {
                    var matchingPackage = packagesVersionsDict.Where(p => p.Key.Equals(topLevelPackage.Name, StringComparison.OrdinalIgnoreCase)).First();

                    // Get latest metadata *only* if this is a report requiring "outdated" metadata
                    if (listPackageArgs.ReportType == ReportType.Outdated && matchingPackage.Value.Count > 0)
                    {
                        var latestVersion = matchingPackage.Value.Where(newVersion => MeetsConstraints(newVersion.Identity.Version, topLevelPackage, listPackageArgs)).Max(i => i.Identity.Version);

                        topLevelPackage.LatestPackageMetadata = matchingPackage.Value.First(p => p.Identity.Version == latestVersion);
                        topLevelPackage.UpdateLevel = GetUpdateLevel(topLevelPackage.ResolvedPackageMetadata.Identity.Version, topLevelPackage.LatestPackageMetadata.Identity.Version);
                    }

                    var matchingPackagesWithDeprecationMetadata = await Task.WhenAll(
                        matchingPackage.Value.Select(async v => new { SearchMetadata = v, DeprecationMetadata = await v.GetDeprecationMetadataAsync() }));

                    // Update resolved version with additional metadata information returned by the server.
                    var resolvedVersionFromServer = matchingPackagesWithDeprecationMetadata
                        .Where(v => v.SearchMetadata.Identity.Version == topLevelPackage.ResolvedPackageMetadata.Identity.Version &&
                                (v.DeprecationMetadata != null || v.SearchMetadata?.Vulnerabilities != null))
                        .FirstOrDefault();

                    if (resolvedVersionFromServer != null)
                    {
                        topLevelPackage.ResolvedPackageMetadata = resolvedVersionFromServer.SearchMetadata;
                    }
                }

                foreach (var transitivePackage in frameworkPackages.TransitivePackages)
                {
                    var matchingPackage = packagesVersionsDict.Where(p => p.Key.Equals(transitivePackage.Name, StringComparison.OrdinalIgnoreCase)).First();

                    // Get latest metadata *only* if this is a report requiring "outdated" metadata
                    if (listPackageArgs.ReportType == ReportType.Outdated && matchingPackage.Value.Count > 0)
                    {
                        var latestVersion = matchingPackage.Value.Where(newVersion => MeetsConstraints(newVersion.Identity.Version, transitivePackage, listPackageArgs)).Max(i => i.Identity.Version);

                        transitivePackage.LatestPackageMetadata = matchingPackage.Value.First(p => p.Identity.Version == latestVersion);
                        transitivePackage.UpdateLevel = GetUpdateLevel(transitivePackage.ResolvedPackageMetadata.Identity.Version, transitivePackage.LatestPackageMetadata.Identity.Version);
                    }

                    var matchingPackagesWithDeprecationMetadata = await Task.WhenAll(
                        matchingPackage.Value.Select(async v => new { SearchMetadata = v, DeprecationMetadata = await v.GetDeprecationMetadataAsync() }));

                    // Update resolved version with additional metadata information returned by the server.
                    var resolvedVersionFromServer = matchingPackagesWithDeprecationMetadata
                        .Where(v => v.SearchMetadata.Identity.Version == transitivePackage.ResolvedPackageMetadata.Identity.Version &&
                                (v.DeprecationMetadata != null || v.SearchMetadata?.Vulnerabilities != null))
                        .FirstOrDefault();

                    if (resolvedVersionFromServer != null)
                    {
                        transitivePackage.ResolvedPackageMetadata = resolvedVersionFromServer.SearchMetadata;
                    }
                }
            }
        }

        /// <summary>
        /// Update Level is used to determine the print color for the latest
        /// version, which depends on changing major, minor or patch
        /// </summary>
        /// <param name="resolvedVersion"> Package's resolved version </param>
        /// <param name="latestVersion"> Package's latest version </param>
        /// <returns></returns>
        private UpdateLevel GetUpdateLevel(NuGetVersion resolvedVersion, NuGetVersion latestVersion)
        {
            if (latestVersion == null) return UpdateLevel.NoUpdate;
            if (resolvedVersion.Major != latestVersion.Major)
            {
                return UpdateLevel.Major;
            }
            else if (resolvedVersion.Minor != latestVersion.Minor)
            {
                return UpdateLevel.Minor;
            }
            //Patch or less important version props are different
            else if (resolvedVersion != latestVersion)
            {
                return UpdateLevel.Patch;
            }
            return UpdateLevel.NoUpdate;
        }

        /// <summary>
        /// Prepares the calls to sources for latest versions and updates
        /// the list of tasks with the requests
        /// </summary>
        /// <param name="package">The package to get the latest version for</param>
        /// <param name="listPackageArgs">List args for the token and source provider></param>
        /// <param name="packagesVersionsDict">A reference to the unique packages in the project
        /// to be able to handle different sources having different latest versions</param>
        /// <returns>A list of tasks for all latest versions for packages from all sources</returns>
        private IList<Task> PrepareLatestVersionsRequests(
            string package,
            ListPackageArgs listPackageArgs,
            Dictionary<string, IList<IPackageSearchMetadata>> packagesVersionsDict)
        {
            var latestVersionsRequests = new List<Task>();
            var sources = listPackageArgs.PackageSources;
            foreach (var packageSource in sources)
            {
                latestVersionsRequests.Add(GetLatestVersionPerSourceAsync(packageSource, listPackageArgs, package, packagesVersionsDict));
            }
            return latestVersionsRequests;
        }

        /// <summary>
        /// Prepares the calls to sources for current versions and updates
        /// the list of tasks with the requests
        /// </summary>
        /// <param name="packageId">The package ID to get the current version metadata for</param>
        /// <param name="requestedVersion">The version of the requested package</param>
        /// <param name="listPackageArgs">List args for the token and source provider></param>
        /// <param name="packagesVersionsDict">A reference to the unique packages in the project
        /// to be able to handle different sources having different latest versions</param>
        /// <returns>A list of tasks for all current versions for packages from all sources</returns>
        private IList<Task> PrepareCurrentVersionsRequests(
            string packageId,
            NuGetVersion requestedVersion,
            ListPackageArgs listPackageArgs,
            Dictionary<string, IList<IPackageSearchMetadata>> packagesVersionsDict)
        {
            var requests = new List<Task>();
            var sources = listPackageArgs.PackageSources;

            foreach (var packageSource in sources)
            {
                requests.Add(
                    GetPackageMetadataFromSourceAsync(
                        packageSource,
                        listPackageArgs,
                        packageId,
                        requestedVersion,
                        packagesVersionsDict));
            }

            return requests;
        }

        /// <summary>
        /// Gets the highest version of a package from a specific source
        /// </summary>
        /// <param name="packageSource">The source to look for packages at</param>
        /// <param name="listPackageArgs">The list args for the cancellation token</param>
        /// <param name="package">Package to look for updates for</param>
        /// <param name="packagesVersionsDict">A reference to the unique packages in the project
        /// to be able to handle different sources having different latest versions</param>
        /// <returns>An updated package with the highest version at a single source</returns>
        private async Task GetLatestVersionPerSourceAsync(
            PackageSource packageSource,
            ListPackageArgs listPackageArgs,
            string package,
            Dictionary<string, IList<IPackageSearchMetadata>> packagesVersionsDict)
        {
            SourceRepository sourceRepository = _sourceRepositoryCache[packageSource];
            var packageMetadataResource = await sourceRepository.GetResourceAsync<PackageMetadataResource>(listPackageArgs.CancellationToken);

            using var sourceCacheContext = new SourceCacheContext();
            var packages = await packageMetadataResource.GetMetadataAsync(
                package,
                includePrerelease: listPackageArgs.Prerelease,
                includeUnlisted: false,
                sourceCacheContext: sourceCacheContext,
                log: listPackageArgs.Logger,
                token: listPackageArgs.CancellationToken);

            var latestVersionsForPackage = packagesVersionsDict.Where(p => p.Key.Equals(package, StringComparison.OrdinalIgnoreCase)).Single().Value;
            latestVersionsForPackage.AddRange(packages);
        }

        /// <summary>
        /// Gets the requested version of a package from a specific source
        /// </summary>
        /// <param name="packageSource">The source to look for packages at</param>
        /// <param name="listPackageArgs">The list args for the cancellation token</param>
        /// <param name="packageId">Package to look for</param>
        /// <param name="requestedVersion">Requested package version</param>
        /// <param name="packagesVersionsDict">A reference to the unique packages in the project
        /// to be able to handle different sources having different latest versions</param>
        /// <returns>An updated package with the resolved version metadata from a single source</returns>
        private async Task GetPackageMetadataFromSourceAsync(
            PackageSource packageSource,
            ListPackageArgs listPackageArgs,
            string packageId,
            NuGetVersion requestedVersion,
            Dictionary<string, IList<IPackageSearchMetadata>> packagesVersionsDict)
        {
            SourceRepository sourceRepository = _sourceRepositoryCache[packageSource];
            var packageMetadataResource = await sourceRepository
                .GetResourceAsync<PackageMetadataResource>(listPackageArgs.CancellationToken);

            using var sourceCacheContext = new SourceCacheContext();
            var packages = await packageMetadataResource.GetMetadataAsync(
                packageId,
                includePrerelease: true,
                includeUnlisted: true, // Include unlisted because deprecated packages may be unlisted.
                sourceCacheContext: sourceCacheContext,
                log: listPackageArgs.Logger,
                token: listPackageArgs.CancellationToken);

            var resolvedVersionsForPackage = packagesVersionsDict
                .Where(p => p.Key.Equals(packageId, StringComparison.OrdinalIgnoreCase))
                .Single()
                .Value;

            var resolvedPackageVersionMetadata = packages.SingleOrDefault(p => p.Identity.Version.Equals(requestedVersion));
            if (resolvedPackageVersionMetadata != null)
            {
                // Package version metadata found on source
                resolvedVersionsForPackage.Add(resolvedPackageVersionMetadata);
            }
        }

        /// <summary>
        /// Given a found version from a source and the current version and the args
        /// of list package, this function checks if the found version meets the required
        /// highest-patch, highest-minor or prerelease
        /// </summary>
        /// <param name="newVersion">Version from a source</param>
        /// <param name="package">The required package with its current version</param>
        /// <param name="listPackageArgs">Used to get the constraints</param>
        /// <returns>Whether the new version meets the constraints or not</returns>
        private bool MeetsConstraints(NuGetVersion newVersion, InstalledPackageReference package, ListPackageArgs listPackageArgs)
        {
            var result = !newVersion.IsPrerelease || listPackageArgs.Prerelease || package.ResolvedPackageMetadata.Identity.Version.IsPrerelease;

            if (listPackageArgs.HighestPatch)
            {
                result = newVersion.Minor.Equals(package.ResolvedPackageMetadata.Identity.Version.Minor) && newVersion.Major.Equals(package.ResolvedPackageMetadata.Identity.Version.Major) && result;
            }

            if (listPackageArgs.HighestMinor)
            {
                result = newVersion.Major.Equals(package.ResolvedPackageMetadata.Identity.Version.Major) && result;
            }

            return result;
        }
    }
}
