// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
#if IS_CORECLR
using System.Runtime.InteropServices;
#endif
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Credentials;
using NuGet.Packaging.Signing;
using NuGet.ProjectModel;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;

#if IS_DESKTOP
using System.Collections.Concurrent;
using System.Xml;
using System.Xml.Linq;
using NuGet.Packaging;
using NuGet.Packaging.PackageExtraction;
using NuGet.PackageManagement;
using NuGet.ProjectManagement;
using NuGet.Shared;
using static NuGet.Shared.XmlUtility;
using System.Globalization;
#endif

namespace NuGet.Build.Tasks
{
    public static class BuildTasksUtility
    {
        /// <summary>
        /// Add all restorable projects to the restore list.
        /// This is the behavior for --recursive
        /// </summary>
        public static void AddAllProjectsForRestore(DependencyGraphSpec spec)
        {
            // Add everything from projects except for packages.config and unknown project types
            foreach (var project in spec.Projects.Where(DoesProjectSupportRestore))
            {
                spec.AddRestore(project.RestoreMetadata.ProjectUniqueName);
            }
        }

        public static void CopyPropertyIfExists(ITaskItem item, IDictionary<string, string> properties, string key)
        {
            CopyPropertyIfExists(item, properties, key, key);
        }

        public static void CopyPropertyIfExists(ITaskItem item, IDictionary<string, string> properties, string key, string toKey)
        {
            var wrapper = new MSBuildTaskItem(item);

            var propertyValue = wrapper.GetProperty(key);

            if (!string.IsNullOrEmpty(propertyValue)
                && !properties.ContainsKey(key))
            {
                properties.Add(toKey, propertyValue);
            }
        }

        /// <summary>
        /// Determines if the specified <see cref="PackageSpec" /> supports restore.
        /// </summary>
        /// <param name="packageSpec">A <see cref="PackageSpec" /> for a project.</param>
        /// <returns><code>true</code> if the project supports restore, otherwise <code>false</code>.</returns>
        public static bool DoesProjectSupportRestore(PackageSpec packageSpec)
        {
            return RestorableTypes.Contains(packageSpec.RestoreMetadata.ProjectStyle);
        }

        public static string GetPropertyIfExists(ITaskItem item, string key)
        {
            var wrapper = new MSBuildTaskItem(item);

            var propertyValue = wrapper.GetProperty(key);

            if (!string.IsNullOrEmpty(propertyValue))
            {
                return propertyValue;
            }

            return null;
        }

        public static void AddPropertyIfExists(IDictionary<string, string> properties, string key, string value)
        {
            if (!string.IsNullOrEmpty(value)
                && !properties.ContainsKey(key))
            {
                properties.Add(key, value);
            }
        }

        public static void AddPropertyIfExists(IDictionary<string, string> properties, string key, string[] value)
        {
            if (value != null && !properties.ContainsKey(key))
            {
                properties.Add(key, string.Concat(value.Select(e => e + ";")));
            }
        }

        private static HashSet<ProjectStyle> RestorableTypes = new HashSet<ProjectStyle>()
        {
            ProjectStyle.DotnetCliTool,
            ProjectStyle.PackageReference,
            ProjectStyle.Standalone,
            ProjectStyle.ProjectJson
        };

        public static Task<bool> RestoreAsync(
            DependencyGraphSpec dependencyGraphSpec,
            bool interactive,
            bool recursive,
            bool noCache,
            bool ignoreFailedSources,
            bool disableParallel,
            bool force,
            bool forceEvaluate,
            bool hideWarningsAndErrors,
            bool restorePC,
            Common.ILogger log,
            CancellationToken cancellationToken)
        {
            return RestoreAsync(dependencyGraphSpec, interactive, recursive, noCache, ignoreFailedSources, disableParallel, force, forceEvaluate, hideWarningsAndErrors, restorePC, cleanupAssetsForUnsupportedProjects: false, log, cancellationToken);
        }

        public static async Task<bool> RestoreAsync(
            DependencyGraphSpec dependencyGraphSpec,
            bool interactive,
            bool recursive,
            bool noCache,
            bool ignoreFailedSources,
            bool disableParallel,
            bool force,
            bool forceEvaluate,
            bool hideWarningsAndErrors,
            bool restorePC,
            bool cleanupAssetsForUnsupportedProjects,
            Common.ILogger log,
            CancellationToken cancellationToken)
        {
            if (dependencyGraphSpec == null)
            {
                throw new ArgumentNullException(nameof(dependencyGraphSpec));
            }

            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }

            try
            {
                DefaultCredentialServiceUtility.SetupDefaultCredentialService(log, !interactive);

                // Set connection limit
                NetworkProtocolUtility.SetConnectionLimit();

                // Set user agent string used for network calls
#if IS_CORECLR
                UserAgent.SetUserAgentString(new UserAgentStringBuilder("NuGet .NET Core MSBuild Task")
                    .WithOSDescription(RuntimeInformation.OSDescription));
#else
                // OS description is set by default on Desktop
                UserAgent.SetUserAgentString(new UserAgentStringBuilder("NuGet Desktop MSBuild Task"));
#endif

                X509TrustStore.InitializeForDotNetSdk(log);

                var restoreSummaries = new List<RestoreSummary>();
                var providerCache = new RestoreCommandProvidersCache();

#if IS_DESKTOP
                if (restorePC && dependencyGraphSpec.Projects.Any(i => i.RestoreMetadata.ProjectStyle == ProjectStyle.PackagesConfig))
                {
                    var v2RestoreResult = await PerformNuGetV2RestoreAsync(log, dependencyGraphSpec, noCache, disableParallel, interactive);
                    restoreSummaries.Add(v2RestoreResult);

                    if (restoreSummaries.Count < 1)
                    {
                        var message = string.Format(
                            CultureInfo.CurrentCulture,
                            Strings.InstallCommandNothingToInstall,
                            NuGetConstants.PackageReferenceFile
                        );

                        log.LogMinimal(message);
                    }

                    if (!v2RestoreResult.Success)
                    {
                        v2RestoreResult
                            .Errors
                            .Where(l => l.Level == LogLevel.Warning)
                            .ForEach(message =>
                            {
                                log.LogWarning(message.Message);
                            });
                    }
                }
#endif

                using (var cacheContext = new SourceCacheContext())
                {
                    cacheContext.NoCache = noCache;
                    cacheContext.IgnoreFailedSources = ignoreFailedSources;

                    // Pre-loaded request provider containing the graph file
                    var providers = new List<IPreLoadedRestoreRequestProvider>();

                    if (dependencyGraphSpec.Restore.Count > 0)
                    {
                        // Add all child projects
                        if (recursive)
                        {
                            AddAllProjectsForRestore(dependencyGraphSpec);
                        }

                        providers.Add(new DependencyGraphSpecRequestProvider(providerCache, dependencyGraphSpec));

                        var restoreContext = new RestoreArgs()
                        {
                            CacheContext = cacheContext,
                            LockFileVersion = LockFileFormat.Version,
                            // 'dotnet restore' fails on slow machines (https://github.com/NuGet/Home/issues/6742)
                            // The workaround is to pass the '--disable-parallel' option.
                            // We apply the workaround by default when the system has 1 cpu.
                            // This will fix restore failures on VMs with 1 CPU and containers with less or equal to 1 CPU assigned.
                            DisableParallel = Environment.ProcessorCount == 1 ? true : disableParallel,
                            Log = log,
                            MachineWideSettings = new XPlatMachineWideSetting(),
                            PreLoadedRequestProviders = providers,
                            AllowNoOp = !force,
                            HideWarningsAndErrors = hideWarningsAndErrors,
                            RestoreForceEvaluate = forceEvaluate
                        };

                        if (restoreContext.DisableParallel)
                        {
                            HttpSourceResourceProvider.Throttle = SemaphoreSlimThrottle.CreateBinarySemaphore();
                        }

                        cancellationToken.ThrowIfCancellationRequested();

                        restoreSummaries.AddRange(await RestoreRunner.RunAsync(restoreContext, cancellationToken));
                    }

                    if (cleanupAssetsForUnsupportedProjects)
                    {
                        // Restore assets are normally left on disk between restores for all projects.  This can cause a condition where a project that supports PackageReference was restored
                        // but then a user changes a branch or some other condition and now the project does not use PackageReference. Since the restore assets are left on disk, the build
                        // consumes them which can cause build errors. The code below cleans up all of the files that we write so that they are not used during build
                        Parallel.ForEach(dependencyGraphSpec.Projects.Where(i => !DoesProjectSupportRestore(i)), project =>
                        {
                            if (project.RestoreMetadata == null || string.IsNullOrWhiteSpace(project.RestoreMetadata.OutputPath) || string.IsNullOrWhiteSpace(project.RestoreMetadata.ProjectPath))
                            {
                                return;
                            }

                            // project.assets.json
                            FileUtility.Delete(Path.Combine(project.RestoreMetadata.OutputPath, LockFileFormat.AssetsFileName));

                            // project.csproj.nuget.cache
                            FileUtility.Delete(project.RestoreMetadata.CacheFilePath);

                            // project.csproj.nuget.g.props
                            FileUtility.Delete(BuildAssetsUtils.GetMSBuildFilePathForPackageReferenceStyleProject(project, BuildAssetsUtils.PropsExtension));

                            // project..csproj.nuget.g.targets
                            FileUtility.Delete(BuildAssetsUtils.GetMSBuildFilePathForPackageReferenceStyleProject(project, BuildAssetsUtils.TargetsExtension));

                            // project.csproj.nuget.dgspec.json
                            FileUtility.Delete(Path.Combine(project.RestoreMetadata.OutputPath, DependencyGraphSpec.GetDGSpecFileName(Path.GetFileName(project.RestoreMetadata.ProjectPath))));
                        });
                    }
                }

                if (restoreSummaries.Count < 1)
                {
                    log.LogMinimal(Strings.NoProjectsToRestore);
                }
                else
                {
                    RestoreSummary.Log(log, restoreSummaries);
                }
                return restoreSummaries.All(x => x.Success);
            }
            finally
            {
                // The CredentialService lifetime is for the duration of the process. We should not leave a potentially unavailable logger. 
                // We need to update the delegating logger with a null instance
                // because the tear downs of the plugins and similar rely on idleness and process exit.
                DefaultCredentialServiceUtility.UpdateCredentialServiceDelegatingLogger(NullLogger.Instance);
            }
        }

        /// <summary>
        /// Try to parse the <paramref name="restoreProjectStyleProperty"/> and return the <see cref="ProjectStyle"/> value.
        /// </summary>
        /// <param name="restoreProjectStyleProperty">The value of the RestoreProjectStyle property value. It can be null.</param>
        /// <returns>The <see cref="ProjectStyle"/>. If the <paramref name="restoreProjectStyleProperty"/> is null the return vale will be null. </returns>
        public static ProjectStyle? GetProjectRestoreStyleFromProjectProperty(string restoreProjectStyleProperty)
        {
            ProjectStyle projectStyle;
            // Allow a user to override by setting RestoreProjectStyle in the project.
            if (!string.IsNullOrWhiteSpace(restoreProjectStyleProperty))
            {
                if (!Enum.TryParse(restoreProjectStyleProperty, ignoreCase: true, out projectStyle))
                {
                    projectStyle = ProjectStyle.Unknown;
                }
                return projectStyle;
            }

            return null;
        }

        /// <summary>
        /// Determines the restore style of a project.
        /// </summary>
        /// <param name="restoreProjectStyle">An optional user supplied restore style.</param>
        /// <param name="hasPackageReferenceItems">A <see cref="bool"/> indicating whether or not the project has any PackageReference items.</param>
        /// <param name="projectJsonPath">An optional path to the project's project.json file.</param>
        /// <param name="projectDirectory">The full path to the project directory.</param>
        /// <param name="projectName">The name of the project file.</param>
        /// <param name="log">An <see cref="NuGet.Common.ILogger"/> object used to log messages.</param>
        /// <returns>A <see cref="Tuple{ProjectStyle, Boolean}"/> containing the project style and a value indicating if the project is using a style that is compatible with PackageReference.
        /// If the value of <paramref name="restoreProjectStyle"/> is not empty and could not be parsed, <code>null</code> is returned.</returns>
        public static (ProjectStyle ProjectStyle, bool IsPackageReferenceCompatibleProjectStyle, string PackagesConfigFilePath) GetProjectRestoreStyle(ProjectStyle? restoreProjectStyle, bool hasPackageReferenceItems, string projectJsonPath, string projectDirectory, string projectName, Common.ILogger log)
        {
            ProjectStyle projectStyle;
            string packagesConfigFilePath = null;

            // Allow a user to override by setting RestoreProjectStyle in the project.
            if (restoreProjectStyle.HasValue)
            {
                projectStyle = restoreProjectStyle.Value;
            }
            else if (hasPackageReferenceItems)
            {
                // If any PackageReferences exist treat it as PackageReference. This has priority over project.json.
                projectStyle = ProjectStyle.PackageReference;
            }
            else if (!string.IsNullOrWhiteSpace(projectJsonPath))
            {
                // If this is not a PackageReference project check if project.json or projectName.project.json exists.
                projectStyle = ProjectStyle.ProjectJson;
            }
            else if (ProjectHasPackagesConfigFile(projectDirectory, projectName, out packagesConfigFilePath))
            {
                // If this is not a PackageReference or ProjectJson project check if packages.config or packages.ProjectName.config exists
                projectStyle = ProjectStyle.PackagesConfig;
            }
            else
            {
                // This project is either a packages.config project or one that does not use NuGet at all.
                projectStyle = ProjectStyle.Unknown;
            }

            bool isPackageReferenceCompatibleProjectStyle = projectStyle == ProjectStyle.PackageReference || projectStyle == ProjectStyle.DotnetToolReference;

            return (projectStyle, isPackageReferenceCompatibleProjectStyle, packagesConfigFilePath);
        }


        /// <summary>
        /// Determines the restore style of a project.
        /// </summary>
        /// <param name="restoreProjectStyle">An optional user supplied restore style.</param>
        /// <param name="hasPackageReferenceItems">A <see cref="bool"/> indicating whether or not the project has any PackageReference items.</param>
        /// <param name="projectJsonPath">An optional path to the project's project.json file.</param>
        /// <param name="projectDirectory">The full path to the project directory.</param>
        /// <param name="projectName">The name of the project file.</param>
        /// <param name="log">An <see cref="NuGet.Common.ILogger"/> object used to log messages.</param>
        /// <returns>A <see cref="Tuple{ProjectStyle, Boolean}"/> containing the project style and a value indicating if the project is using a style that is compatible with PackageReference.
        /// If the value of <paramref name="restoreProjectStyle"/> is not empty and could not be parsed, <code>null</code> is returned.</returns>
        public static (ProjectStyle ProjectStyle, bool IsPackageReferenceCompatibleProjectStyle, string PackagesConfigFilePath) GetProjectRestoreStyle(string restoreProjectStyle, bool hasPackageReferenceItems, string projectJsonPath, string projectDirectory, string projectName, Common.ILogger log)
        {
            return GetProjectRestoreStyle(GetProjectRestoreStyleFromProjectProperty(restoreProjectStyle), hasPackageReferenceItems, projectJsonPath, projectDirectory, projectName, log);
        }

        /// <summary>
        /// Determines if the project has a packages.config file.
        /// </summary>
        /// <param name="projectDirectory">The full path of the project directory.</param>
        /// <param name="projectName">The name of the project file.</param>
        /// <param name="packagesConfigPath">Receives the full path to the packages.config file if one exists, otherwise <code>null</code>.</param>
        /// <returns><code>true</code> if a packages.config exists next to the project, otherwise <code>false</code>.</returns>
        private static bool ProjectHasPackagesConfigFile(string projectDirectory, string projectName, out string packagesConfigPath)
        {
            if (string.IsNullOrWhiteSpace(projectDirectory))
            {
                throw new ArgumentException(Strings.Argument_Cannot_Be_Null_Or_Empty, nameof(projectDirectory));
            }

            if (string.IsNullOrWhiteSpace(projectName))
            {
                throw new ArgumentException(Strings.Argument_Cannot_Be_Null_Or_Empty, nameof(projectName));
            }

            packagesConfigPath = GetPackagesConfigFilePath(projectDirectory, projectName);

            return packagesConfigPath != null;
        }

#if IS_DESKTOP
        private static async Task<RestoreSummary> PerformNuGetV2RestoreAsync(Common.ILogger log, DependencyGraphSpec dgFile, bool noCache, bool disableParallel, bool interactive)
        {
            string globalPackageFolder = null;
            string repositoryPath = null;
            IList<PackageSource> packageSources = null;
            ISettings settings = null;

            Dictionary<PackageReference, List<string>> packageReferenceToProjects = new(PackageReferenceComparer.Instance);

            foreach (PackageSpec packageSpec in dgFile.Projects.Where(i => i.RestoreMetadata.ProjectStyle == ProjectStyle.PackagesConfig))
            {
                var pcRestoreMetadata = (PackagesConfigProjectRestoreMetadata)packageSpec.RestoreMetadata;
                globalPackageFolder = globalPackageFolder ?? pcRestoreMetadata.PackagesPath;
                repositoryPath = repositoryPath ?? pcRestoreMetadata.RepositoryPath;

                if (packageSources == null)
                {
                    packageSources = new List<PackageSource>();
                    if (!noCache)
                    {
                        if (!string.IsNullOrEmpty(globalPackageFolder) && Directory.Exists(globalPackageFolder))
                        {
                            packageSources.Add(new FeedTypePackageSource(globalPackageFolder, FeedType.FileSystemV3));
                        }
                    }

                    packageSources.AddRange(pcRestoreMetadata.Sources);
                }

                settings = settings ?? Settings.LoadSettingsGivenConfigPaths(pcRestoreMetadata.ConfigFilePaths);

                string packagesConfigPath = GetPackagesConfigFilePath(pcRestoreMetadata.ProjectPath);

                foreach (PackageReference packageReference in GetInstalledPackageReferences(packagesConfigPath))
                {
                    if (!packageReferenceToProjects.TryGetValue(packageReference, out List<string> value))
                    {
                        value ??= new();
                        packageReferenceToProjects.Add(packageReference, value);
                    }
                    value.Add(packagesConfigPath);
                }
            }

            if (string.IsNullOrEmpty(repositoryPath))
            {
                throw new InvalidOperationException(Strings.RestoreNoSolutionFound);
            }

            PackageSourceProvider packageSourceProvider = new PackageSourceProvider(settings);
            var sourceRepositoryProvider = new CachingSourceProvider(packageSourceProvider);
            var nuGetPackageManager = new NuGetPackageManager(sourceRepositoryProvider, settings, repositoryPath);

            var effectivePackageSaveMode = CalculateEffectivePackageSaveMode(settings);

            var packageSaveMode = effectivePackageSaveMode == Packaging.PackageSaveMode.None ?
                Packaging.PackageSaveMode.Defaultv2 :
                effectivePackageSaveMode;

            List<PackageRestoreData> packageRestoreData = new(packageReferenceToProjects.Count);
            bool areAnyPackagesMissing = false;
            foreach (KeyValuePair<PackageReference, List<string>> package in packageReferenceToProjects)
            {
                var exists = nuGetPackageManager.PackageExistsInPackagesFolder(package.Key.PackageIdentity, packageSaveMode);
                packageRestoreData.Add(new PackageRestoreData(package.Key, package.Value, !exists));
                areAnyPackagesMissing |= !exists;
            }

            if (!areAnyPackagesMissing)
            {
                return new RestoreSummary(true);
            }

            var repositories = sourceRepositoryProvider.GetRepositories().ToArray();

            var installCount = 0;
            var failedEvents = new ConcurrentQueue<PackageRestoreFailedEventArgs>();
            var collectorLogger = new RestoreCollectorLogger(log);

            var packageRestoreContext = new PackageRestoreContext(
                nuGetPackageManager,
                packageRestoreData,
                CancellationToken.None,
                packageRestoredEvent: (sender, args) => { Interlocked.Add(ref installCount, args.Restored ? 1 : 0); },
                packageRestoreFailedEvent: (sender, args) => { failedEvents.Enqueue(args); },
                sourceRepositories: repositories,
                maxNumberOfParallelTasks: disableParallel
                    ? 1
                    : PackageManagementConstants.DefaultMaxDegreeOfParallelism,
                logger: collectorLogger);

            // TODO: Check require consent?
            // NOTE: This feature is currently not working at all. See https://github.com/NuGet/Home/issues/4327
            // CheckRequireConsent();

            var clientPolicyContext = ClientPolicyContext.GetClientPolicy(settings, collectorLogger);
            var projectContext = new ConsoleProjectContext(collectorLogger)
            {
                PackageExtractionContext = new PackageExtractionContext(
                    packageSaveMode,
                    PackageExtractionBehavior.XmlDocFileSaveMode,
                    clientPolicyContext,
                    collectorLogger)
            };

            if (effectivePackageSaveMode != Packaging.PackageSaveMode.None)
            {
                projectContext.PackageExtractionContext.PackageSaveMode = packageSaveMode;
            }

            using (var cacheContext = new SourceCacheContext())
            {
                cacheContext.NoCache = noCache;

                var packageSourceMapping = PackageSourceMapping.GetPackageSourceMapping(settings);

                var downloadContext = new PackageDownloadContext(cacheContext, repositoryPath, directDownload: false, packageSourceMapping)
                {
                    ClientPolicyContext = clientPolicyContext
                };

                DefaultCredentialServiceUtility.SetupDefaultCredentialService(log, !interactive);

                var result = await PackageRestoreManager.RestoreMissingPackagesAsync(
                    packageRestoreContext,
                    projectContext,
                    downloadContext);

                return new RestoreSummary(
                    result.Restored,
                    "packages.config projects",
                    settings.GetConfigFilePaths().ToArray(),
                    packageSources.Select(x => x.Source).ToArray(),
                    installCount,
                    collectorLogger.Errors.Concat(ProcessFailedEventsIntoRestoreLogs(failedEvents)).ToArray()
                );
            }
        }

        internal static PackageSaveMode CalculateEffectivePackageSaveMode(ISettings settings)
        {
            string packageSaveModeValue = string.Empty;
            PackageSaveMode effectivePackageSaveMode;
            if (string.IsNullOrEmpty(packageSaveModeValue))
            {
                packageSaveModeValue = SettingsUtility.GetConfigValue(settings, "PackageSaveMode");
            }

            if (!string.IsNullOrEmpty(packageSaveModeValue))
            {
                // The PackageSaveMode flag only determines if nuspec and nupkg are saved at the target location.
                // For install \ restore, we always extract files.
                effectivePackageSaveMode = Packaging.PackageSaveMode.Files;
                foreach (var v in packageSaveModeValue.Split(';'))
                {
                    if (v.Equals(Packaging.PackageSaveMode.Nupkg.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        effectivePackageSaveMode |= Packaging.PackageSaveMode.Nupkg;
                    }
                    else if (v.Equals(Packaging.PackageSaveMode.Nuspec.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        effectivePackageSaveMode |= Packaging.PackageSaveMode.Nuspec;
                    }
                    else
                    {
                        string message = string.Format(
                            CultureInfo.CurrentCulture,
                            Strings.Warning_InvalidPackageSaveMode,
                            v);

                        throw new InvalidOperationException(message);
                    }
                }
            }
            else
            {
                effectivePackageSaveMode = Packaging.PackageSaveMode.None;
            }
            return effectivePackageSaveMode;
        }


        private static IEnumerable<PackageReference> GetInstalledPackageReferences(string projectConfigFilePath)
        {
            if (File.Exists(projectConfigFilePath))
            {
                try
                {
                    XDocument xDocument = Load(projectConfigFilePath);
                    var reader = new PackagesConfigReader(xDocument);
                    return reader.GetPackages(allowDuplicatePackageIds: true);
                }
                catch (XmlException ex)
                {
                    var message = string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.Error_PackagesConfigParseError,
                        projectConfigFilePath,
                        ex.Message);

                    throw new XmlException(message, ex);
                }
            }

            return Enumerable.Empty<PackageReference>();
        }

        private static IEnumerable<RestoreLogMessage> ProcessFailedEventsIntoRestoreLogs(ConcurrentQueue<PackageRestoreFailedEventArgs> failedEvents)
        {
            var result = new List<RestoreLogMessage>();

            foreach (var failedEvent in failedEvents)
            {
                if (failedEvent.Exception is SignatureException signatureException)
                {
                    if (signatureException.Results != null)
                    {
                        IEnumerable<RestoreLogMessage> errorsAndWarnings = signatureException.Results
                            .SelectMany(r => r.Issues)
                            .Where(i => i.Level == LogLevel.Error || i.Level == LogLevel.Warning)
                            .Select(i => i.AsRestoreLogMessage());

                        result.AddRange(errorsAndWarnings);
                    }
                }
                else
                {
                    result.Add(new RestoreLogMessage(LogLevel.Error, NuGetLogCode.Undefined, failedEvent.Exception.Message));
                }
            }

            return result;
        }
#endif

        public static string[] GetSources(string startupDirectory, string projectDirectory, string[] sources, string[] sourcesOverride, IEnumerable<string> additionalProjectSources, ISettings settings)
        {
            // Sources
            var currentSources = RestoreSettingsUtils.GetValue(
                () => sourcesOverride?.Select(MSBuildRestoreUtility.FixSourcePath).Select(e => UriUtility.GetAbsolutePath(startupDirectory, e)).ToArray(),
                () => MSBuildRestoreUtility.ContainsClearKeyword(sources) ? Array.Empty<string>() : null,
                () => sources?.Select(MSBuildRestoreUtility.FixSourcePath).Select(e => UriUtility.GetAbsolutePath(projectDirectory, e)).ToArray(),
                () => (PackageSourceProvider.LoadPackageSources(settings)).Where(e => e.IsEnabled).Select(e => e.Source).ToArray());

            // Append additional sources
            // Escape strings to avoid xplat path issues with msbuild.
            var filteredAdditionalProjectSources = MSBuildRestoreUtility.AggregateSources(
                    values: additionalProjectSources,
                    excludeValues: Enumerable.Empty<string>())
                .Select(MSBuildRestoreUtility.FixSourcePath);

            return AppendItems(projectDirectory, currentSources, filteredAdditionalProjectSources);
        }

        /// <summary>
        /// Gets the package fallback folders for a project.
        /// </summary>
        /// <param name="startupDirectory">The start-up directory of the tool.</param>
        /// <param name="projectDirectory">The full path to the directory of the project.</param>
        /// <param name="fallbackFolders">A <see cref="T:string[]" /> containing the fallback folders for the project.</param>
        /// <param name="fallbackFoldersOverride">A <see cref="T:string[]" /> containing overrides for the fallback folders for the project.</param>
        /// <param name="additionalProjectFallbackFolders">An <see cref="IEnumerable{String}" /> containing additional fallback folders for the project.</param>
        /// <param name="additionalProjectFallbackFoldersExcludes">An <see cref="IEnumerable{String}" /> containing fallback folders to exclude.</param>
        /// <param name="settings">An <see cref="ISettings" /> object containing settings for the project.</param>
        /// <returns>A <see cref="T:string[]" /> containing the package fallback folders for the project.</returns>
        public static string[] GetFallbackFolders(string startupDirectory, string projectDirectory, string[] fallbackFolders, string[] fallbackFoldersOverride, IEnumerable<string> additionalProjectFallbackFolders, IEnumerable<string> additionalProjectFallbackFoldersExcludes, ISettings settings)
        {
            // Fallback folders
            var currentFallbackFolders = RestoreSettingsUtils.GetValue(
                () => fallbackFoldersOverride?.Select(e => UriUtility.GetAbsolutePath(startupDirectory, e)).ToArray(),
                () => MSBuildRestoreUtility.ContainsClearKeyword(fallbackFolders) ? Array.Empty<string>() : null,
                () => fallbackFolders?.Select(e => UriUtility.GetAbsolutePath(projectDirectory, e)).ToArray(),
                () => SettingsUtility.GetFallbackPackageFolders(settings).ToArray());

            // Append additional fallback folders after removing excluded folders
            var filteredAdditionalProjectFallbackFolders = MSBuildRestoreUtility.AggregateSources(
                    values: additionalProjectFallbackFolders,
                    excludeValues: additionalProjectFallbackFoldersExcludes);

            return AppendItems(projectDirectory, currentFallbackFolders, filteredAdditionalProjectFallbackFolders);
        }

        private static string[] AppendItems(string projectDirectory, string[] current, IEnumerable<string> additional)
        {
            if (additional == null || !additional.Any())
            {
                // noop
                return current;
            }

            var additionalAbsolute = additional.Select(e => UriUtility.GetAbsolutePath(projectDirectory, e));

            return current.Concat(additionalAbsolute).ToArray();
        }

        /// <summary>
        /// Gets the path to a packages.config for the specified project if one exists.
        /// </summary>
        /// <param name="projectFullPath">The full path to the project.</param>
        /// <returns>The path to the packages.config file if one exists, otherwise <see langword="null" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="projectFullPath" /> is <see langword="null" />.</exception>
        public static string GetPackagesConfigFilePath(string projectFullPath)
        {
            if (string.IsNullOrWhiteSpace(projectFullPath))
            {
                throw new ArgumentException(Strings.Argument_Cannot_Be_Null_Or_Empty, nameof(projectFullPath));
            }

            return GetPackagesConfigFilePath(Path.GetDirectoryName(projectFullPath), Path.GetFileNameWithoutExtension(projectFullPath));
        }

        /// <summary>
        /// Gets the path to a packages.config for the specified project if one exists.
        /// </summary>
        /// <param name="projectFullPath">The full path to the project directory.</param>
        /// <param name="projectName">The name of the project file.</param>
        /// <returns>The path to the packages.config file if one exists, otherwise <see langword="null" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="projectDirectory" /> -or- <paramref name="projectName" /> is <see langword="null" />.</exception>
        public static string GetPackagesConfigFilePath(string projectDirectory, string projectName)
        {
            if (string.IsNullOrWhiteSpace(projectDirectory))
            {
                throw new ArgumentException(Strings.Argument_Cannot_Be_Null_Or_Empty, nameof(projectDirectory));
            }

            if (string.IsNullOrWhiteSpace(projectName))
            {
                throw new ArgumentException(Strings.Argument_Cannot_Be_Null_Or_Empty, nameof(projectName));
            }

            string packagesConfigPath = Path.Combine(projectDirectory, NuGetConstants.PackageReferenceFile);

            if (File.Exists(packagesConfigPath))
            {
                return packagesConfigPath;
            }

            packagesConfigPath = Path.Combine(projectDirectory, "packages." + projectName + ".config");

            if (File.Exists(packagesConfigPath))
            {
                return packagesConfigPath;
            }

            return null;
        }

        /// <summary>
        /// Gets an integer indicating what files should be embedded in the MSBuild binary log based on the user-specified value:
        ///  - "0" or "false" = Do not embed any files
        ///  - "2" = Embed dgspec, project.assets.json, g.props, and g.targets
        /// Any other value embeds project.assets.json, g.props, and g.targets
        /// </summary>
        /// <param name="value">The user supplied value indicating what files to embed in the binary log.</param>
        /// <returns>An integer representing what to embed in the binary log.</returns>
        public static int GetFilesToEmbedInBinlogValue(string value)
        {
            return GetFilesToEmbedInBinlogValue(value, EnvironmentVariableWrapper.Instance);
        }

        internal static int GetFilesToEmbedInBinlogValue(string value, IEnvironmentVariableReader environmentVariableReader)
        {
            if (!string.Equals(environmentVariableReader.GetEnvironmentVariable("MSBUILDBINARYLOGGERENABLED"), bool.TrueString, StringComparison.OrdinalIgnoreCase))
            {
                return 0;
            }

            if (value == null)
            {
                return 1;
            }

            string trimmed = value.Trim();

            if (string.Equals(bool.FalseString, trimmed, StringComparison.OrdinalIgnoreCase) || string.Equals("0", trimmed, StringComparison.OrdinalIgnoreCase))
            {
                return 0;
            }

            if (string.Equals("2", trimmed, StringComparison.OrdinalIgnoreCase))
            {
                return 2;
            }

            return 1;
        }

    }
}

