// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Remoting;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.PackageManagement;
using NuGet.PackageManagement.Utility;
using NuGet.Packaging;
using NuGet.Packaging.PackageExtraction;
using NuGet.Packaging.Signing;
using NuGet.ProjectManagement;
using NuGet.ProjectModel;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Shared;

namespace NuGet.CommandLine
{
    [Command(typeof(NuGetCommand), "restore", "RestoreCommandDescription",
        MinArgs = 0, MaxArgs = 1, UsageSummaryResourceName = "RestoreCommandUsageSummary",
        UsageDescriptionResourceName = "RestoreCommandUsageDescription",
        UsageExampleResourceName = "RestoreCommandUsageExamples")]
    public class RestoreCommand : DownloadCommandBase
    {
        [Option(typeof(NuGetCommand), "RestoreCommandRequireConsent")]
        public bool RequireConsent { get; set; }

        [Option(typeof(NuGetCommand), "RestoreCommandP2PTimeOut")]
        public int Project2ProjectTimeOut { get; set; }

        [Option(typeof(NuGetCommand), "RestoreCommandPackagesDirectory", AltName = "OutputDirectory")]
        public string PackagesDirectory { get; set; }

        [Option(typeof(NuGetCommand), "RestoreCommandSolutionDirectory")]
        public string SolutionDirectory { get; set; }

        [Option(typeof(NuGetCommand), "CommandMSBuildVersion")]
        public string MSBuildVersion { get; set; }

        [Option(typeof(NuGetCommand), "CommandMSBuildPath")]
        public string MSBuildPath { get; set; }

        [Option(typeof(NuGetCommand), "RestoreCommandRecursive")]
        public bool Recursive { get; set; }

        [Option(typeof(NuGetCommand), "ForceRestoreCommand")]
        public bool Force { get; set; }

        [Option(typeof(NuGetCommand), "RestoreCommandUseLockFile")]
        public bool UseLockFile { get; set; }

        [Option(typeof(NuGetCommand), "RestoreCommandLockedMode")]
        public bool LockedMode { get; set; }

        [Option(typeof(NuGetCommand), "RestoreCommandLockFilePath")]
        public string LockFilePath { get; set; }

        [Option(typeof(NuGetCommand), "RestoreCommandForceEvaluate")]
        public bool ForceEvaluate { get; set; }

        [ImportingConstructor]
        public RestoreCommand()
        {
        }

        // The directory that contains msbuild
        private Lazy<MsBuildToolset> _msbuildDirectory;

        private Lazy<MsBuildToolset> MsBuildDirectory
        {
            get
            {
                if (_msbuildDirectory == null)
                {
                    _msbuildDirectory = MsBuildUtility.GetMsBuildDirectoryFromMsBuildPath(MSBuildPath, MSBuildVersion, Console);

                }
                return _msbuildDirectory;
            }
        }

        public override async Task ExecuteCommandAsync()
        {
            if (DisableParallelProcessing)
            {
                HttpSourceResourceProvider.Throttle = SemaphoreSlimThrottle.CreateBinarySemaphore();
            }

            CalculateEffectivePackageSaveMode();

            var restoreSummaries = new List<RestoreSummary>();

            if (!string.IsNullOrEmpty(SolutionDirectory))
            {
                SolutionDirectory = Path.GetFullPath(SolutionDirectory);
            }

            var restoreInputs = await DetermineRestoreInputsAsync();

            var hasPackagesConfigFiles = restoreInputs.PackagesConfigFiles.Count > 0;
            var hasProjectJsonOrPackageReferences = restoreInputs.RestoreV3Context.Inputs.Any();
            if (!hasPackagesConfigFiles && !hasProjectJsonOrPackageReferences)
            {
                Console.LogMinimal(LocalizedResourceManager.GetString(restoreInputs.RestoringWithSolutionFile
                        ? "SolutionRestoreCommandNoPackagesConfigOrProjectJson"
                        : "ProjectRestoreCommandNoPackagesConfigOrProjectJson"));
                return;
            }

            // packages.config
            if (hasPackagesConfigFiles)
            {
                var v2RestoreResults = await PerformNuGetV2RestoreAsync(restoreInputs);
                restoreSummaries.AddRange(v2RestoreResults);

                foreach (var restoreResult in v2RestoreResults.Where(r => !r.Success))
                {
                    restoreResult
                        .Errors
                        .Where(l => l.Level == LogLevel.Warning)
                        .ForEach(l => Console.LogWarning(l.FormatWithCode()));
                }
            }

            // project.json and PackageReference
            if (hasProjectJsonOrPackageReferences)
            {
                // Read the settings outside of parallel loops.
                ReadSettings(restoreInputs);

                // Check if we can restore based on the nuget.config settings
                CheckRequireConsent();

                using (var cacheContext = new SourceCacheContext())
                {
                    cacheContext.NoCache = NoCache;
                    cacheContext.DirectDownload = DirectDownload;

                    var restoreContext = restoreInputs.RestoreV3Context;
                    var providerCache = new RestoreCommandProvidersCache();

                    // Add restore args to the restore context
                    restoreContext.CacheContext = cacheContext;
                    restoreContext.DisableParallel = DisableParallelProcessing;
                    restoreContext.AllowNoOp = !Force; // if force, no-op is not allowed
                    restoreContext.ConfigFile = ConfigFile;
                    restoreContext.MachineWideSettings = MachineWideSettings;
                    restoreContext.Log = Console;
                    restoreContext.CachingSourceProvider = GetSourceRepositoryProvider();
                    restoreContext.RestoreForceEvaluate = ForceEvaluate;

                    var packageSaveMode = EffectivePackageSaveMode;
                    if (packageSaveMode != Packaging.PackageSaveMode.None)
                    {
                        restoreContext.PackageSaveMode = EffectivePackageSaveMode;
                    }

                    // Providers
                    // Use the settings loaded above in ReadSettings(restoreInputs)
                    if (restoreInputs.ProjectReferenceLookup.Restore.Count > 0)
                    {
                        // Remove input list, everything has been loaded already
                        restoreContext.Inputs.Clear();

                        restoreContext.PreLoadedRequestProviders.Add(new DependencyGraphSpecRequestProvider(
                            providerCache,
                            restoreInputs.ProjectReferenceLookup));
                    }
                    else
                    {
                        // Allow an external .dg file
                        restoreContext.RequestProviders.Add(new DependencyGraphFileRequestProvider(providerCache));
                    }

                    // Run restore
                    var v3Summaries = await RestoreRunner.RunAsync(restoreContext);
                    restoreSummaries.AddRange(v3Summaries);
                }
            }

            // Summaries
            RestoreSummary.Log(Console, restoreSummaries, logErrors: true);

            if (restoreSummaries.Any(x => !x.Success))
            {
                throw new ExitCodeException(exitCode: 1);
            }
        }

        private static CachingSourceProvider _sourceProvider;

        private CachingSourceProvider GetSourceRepositoryProvider()
        {
            if (_sourceProvider == null)
            {
                _sourceProvider = new CachingSourceProvider(SourceProvider);
            }

            return _sourceProvider;
        }

        private bool IsSolutionRestore(PackageRestoreInputs packageRestoreInputs)
        {
            return !string.IsNullOrEmpty(SolutionDirectory) || packageRestoreInputs.RestoringWithSolutionFile;
        }

        private string GetSolutionDirectory(PackageRestoreInputs packageRestoreInputs)
        {
            var solutionDirectory = packageRestoreInputs.RestoringWithSolutionFile ?
                    packageRestoreInputs.DirectoryOfSolutionFile :
                    SolutionDirectory;
            return solutionDirectory != null ? PathUtility.EnsureTrailingSlash(solutionDirectory) : null;
        }

        private void ReadSettings(PackageRestoreInputs packageRestoreInputs)
        {
            if (IsSolutionRestore(packageRestoreInputs))
            {
                var solutionDirectory = GetSolutionDirectory(packageRestoreInputs);

                // Read the solution-level settings
                var solutionSettingsFile = Path.Combine(
                    solutionDirectory,
                    NuGetConstants.NuGetSolutionSettingsFolder);
                if (ConfigFile != null)
                {
                    ConfigFile = Path.GetFullPath(ConfigFile);
                }

                Settings = Configuration.Settings.LoadDefaultSettings(
                    solutionSettingsFile,
                    configFileName: ConfigFile,
                    machineWideSettings: MachineWideSettings);

                // Recreate the source provider and credential provider
                SourceProvider = PackageSourceBuilder.CreateSourceProvider(Settings);
                SetDefaultCredentialProvider();
            }
        }

        protected override void SetDefaultCredentialProvider()
        {
            SetDefaultCredentialProvider(MsBuildDirectory);
        }

        private async Task<IReadOnlyList<RestoreSummary>> PerformNuGetV2RestoreAsync(PackageRestoreInputs packageRestoreInputs)
        {
            ReadSettings(packageRestoreInputs);
            var packagesFolderPath = Path.GetFullPath(GetPackagesFolder(packageRestoreInputs));

            var sourceRepositoryProvider = new CommandLineSourceRepositoryProvider(SourceProvider);
            var nuGetPackageManager = new NuGetPackageManager(sourceRepositoryProvider, Settings, packagesFolderPath);

            var installedPackageReferences = new HashSet<Packaging.PackageReference>(new PackageReferenceComparer());
            if (packageRestoreInputs.RestoringWithSolutionFile)
            {
                installedPackageReferences.AddRange(packageRestoreInputs
                    .PackagesConfigFiles
                    .SelectMany(file => GetInstalledPackageReferences(file, allowDuplicatePackageIds: true)));
            }
            else if (packageRestoreInputs.PackagesConfigFiles.Count > 0)
            {
                // By default the PackageReferenceFile does not throw
                // if the file does not exist at the specified path.
                // So we'll need to verify that the file exists.
                Debug.Assert(packageRestoreInputs.PackagesConfigFiles.Count == 1,
                    "Only one packages.config file is allowed to be specified " +
                    "at a time when not performing solution restore.");

                var packageReferenceFile = packageRestoreInputs.PackagesConfigFiles[0];
                if (!File.Exists(packageReferenceFile))
                {
                    var message = string.Format(
                        CultureInfo.CurrentCulture,
                        LocalizedResourceManager.GetString("RestoreCommandFileNotFound"),
                        packageReferenceFile);

                    throw new InvalidOperationException(message);
                }

                installedPackageReferences.AddRange(
                    GetInstalledPackageReferences(packageReferenceFile, allowDuplicatePackageIds: true));
            }

            // EffectivePackageSaveMode is None when -PackageSaveMode is not provided by the user. None is treated as
            // Defaultv3 for V3 restore and should be treated as Defaultv2 for V2 restore. This is the case in the
            // actual V2 restore flow and should match in this preliminary missing packages check.
            var packageSaveMode = EffectivePackageSaveMode == Packaging.PackageSaveMode.None ?
                Packaging.PackageSaveMode.Defaultv2 :
                EffectivePackageSaveMode;

            var missingPackageReferences = installedPackageReferences.Where(reference =>
                !nuGetPackageManager.PackageExistsInPackagesFolder(reference.PackageIdentity, packageSaveMode)).ToArray();

            if (missingPackageReferences.Length == 0)
            {
                var message = string.Format(
                    CultureInfo.CurrentCulture,
                    LocalizedResourceManager.GetString("InstallCommandNothingToInstall"),
                    "packages.config");

                Console.LogMinimal(message);

                var restoreSummaries = new List<RestoreSummary>();

                ValidatePackagesConfigLockFiles(
                    packageRestoreInputs.PackagesConfigFiles,
                    packageRestoreInputs.ProjectReferenceLookup.Projects,
                    packagesFolderPath,
                    restoreSummaries);

                if (restoreSummaries.Count == 0)
                {
                    restoreSummaries.Add(new RestoreSummary(success: true));
                }

                return restoreSummaries;
            }

            var packageRestoreData = missingPackageReferences.Select(reference =>
                new PackageRestoreData(
                    reference,
                    new[] { packageRestoreInputs.RestoringWithSolutionFile
                                ? packageRestoreInputs.DirectoryOfSolutionFile
                                : packageRestoreInputs.PackagesConfigFiles[0] },
                    isMissing: true));

            var packageSources = GetPackageSources(Settings);

            var repositories = packageSources
                .Select(sourceRepositoryProvider.CreateRepository)
                .ToArray();

            var installCount = 0;
            var failedEvents = new ConcurrentQueue<PackageRestoreFailedEventArgs>();
            var collectorLogger = new RestoreCollectorLogger(Console);

            var packageRestoreContext = new PackageRestoreContext(
                nuGetPackageManager,
                packageRestoreData,
                CancellationToken.None,
                packageRestoredEvent: (sender, args) => { Interlocked.Add(ref installCount, args.Restored ? 1 : 0); },
                packageRestoreFailedEvent: (sender, args) => { failedEvents.Enqueue(args); },
                sourceRepositories: repositories,
                maxNumberOfParallelTasks: DisableParallelProcessing
                        ? 1
                        : PackageManagementConstants.DefaultMaxDegreeOfParallelism,
                logger: collectorLogger);

            CheckRequireConsent();

            var clientPolicyContext = ClientPolicyContext.GetClientPolicy(Settings, collectorLogger);
            var projectContext = new ConsoleProjectContext(collectorLogger)
            {
                PackageExtractionContext = new PackageExtractionContext(
                    Packaging.PackageSaveMode.Defaultv2,
                    PackageExtractionBehavior.XmlDocFileSaveMode,
                    clientPolicyContext,
                    collectorLogger)
            };

            if (EffectivePackageSaveMode != Packaging.PackageSaveMode.None)
            {
                projectContext.PackageExtractionContext.PackageSaveMode = EffectivePackageSaveMode;
            }

            using (var cacheContext = new SourceCacheContext())
            {
                cacheContext.NoCache = NoCache;
                cacheContext.DirectDownload = DirectDownload;

                var packageSourceMapping = PackageSourceMapping.GetPackageSourceMapping(Settings);

                var downloadContext = new PackageDownloadContext(cacheContext, packagesFolderPath, DirectDownload, packageSourceMapping)
                {
                    ClientPolicyContext = clientPolicyContext
                };

                var result = await PackageRestoreManager.RestoreMissingPackagesAsync(
                    packageRestoreContext,
                    projectContext,
                    downloadContext);

                if (downloadContext.DirectDownload)
                {
                    GetDownloadResultUtility.CleanUpDirectDownloads(downloadContext);
                }

                IReadOnlyList<IRestoreLogMessage> errors = collectorLogger.Errors.Concat(ProcessFailedEventsIntoRestoreLogs(failedEvents)).ToList();
                var restoreSummaries = new List<RestoreSummary>()
                {
                    new RestoreSummary(
                        result.Restored,
                        "packages.config projects",
                        Settings.GetConfigFilePaths().ToList().AsReadOnly(),
                        packageSources.Select(x => x.Source).ToList().AsReadOnly(),
                        installCount,
                        errors)
                };

                if (result.Restored)
                {
                    ValidatePackagesConfigLockFiles(
                        packageRestoreInputs.PackagesConfigFiles,
                        packageRestoreInputs.ProjectReferenceLookup.Projects,
                        packagesFolderPath,
                        restoreSummaries);
                }

                return restoreSummaries;
            }
        }

        /// <summary>
        /// Processes List of PackageRestoreFailedEventArgs into a List of RestoreLogMessages.
        /// </summary>
        /// <param name="failedEvents">List of PackageRestoreFailedEventArgs.</param>
        /// <returns>List of RestoreLogMessages.</returns>
        private static IEnumerable<RestoreLogMessage> ProcessFailedEventsIntoRestoreLogs(ConcurrentQueue<PackageRestoreFailedEventArgs> failedEvents)
        {
            var result = new List<RestoreLogMessage>();

            foreach (var failedEvent in failedEvents)
            {
                if (failedEvent.Exception is SignatureException)
                {
                    var signatureException = failedEvent.Exception as SignatureException;

                    var errorsAndWarnings = signatureException
                        .Results.SelectMany(r => r.Issues)
                        .Where(i => i.Level == LogLevel.Error || i.Level == LogLevel.Warning)
                        .Select(i => i.AsRestoreLogMessage());

                    result.AddRange(errorsAndWarnings);
                }
                else
                {
                    result.Add(new RestoreLogMessage(LogLevel.Error, NuGetLogCode.Undefined, failedEvent.Exception.Message));
                }
            }

            return result;
        }

        private void CheckRequireConsent()
        {
            if (RequireConsent)
            {
                var packageRestoreConsent = new PackageRestoreConsent(Settings);

                if (packageRestoreConsent.IsGranted)
                {
                    var message = string.Format(
                        CultureInfo.CurrentCulture,
                        LocalizedResourceManager.GetString("RestoreCommandPackageRestoreOptOutMessage"),
                        NuGetResources.PackageRestoreConsentCheckBoxText.Replace("&", ""));

                    Console.LogInformation(message);
                }
                else
                {
                    var message = string.Format(
                        CultureInfo.CurrentCulture,
                        LocalizedResourceManager.GetString("InstallCommandPackageRestoreConsentNotFound"),
                        NuGetResources.PackageRestoreConsentCheckBoxText.Replace("&", ""));

                    throw new CommandException(message);
                }
            }
        }

        /// <summary>
        /// Discover all restore inputs, this checks for both v2 and v3
        /// </summary>
        private async Task<PackageRestoreInputs> DetermineRestoreInputsAsync()
        {
            var packageRestoreInputs = new PackageRestoreInputs();

            if (Arguments.Count == 0)
            {
                // If no arguments were provided use the current directory
                GetInputsFromDirectory(Directory.GetCurrentDirectory(), packageRestoreInputs);
            }
            else
            {
                // Restore takes multiple arguments, each could be a file or directory
                var argument = Arguments.Single();
                var fullPath = Path.GetFullPath(argument);

                if (Directory.Exists(fullPath))
                {
                    // Dir
                    GetInputsFromDirectory(fullPath, packageRestoreInputs);
                }
                else if (File.Exists(fullPath))
                {
                    // File
                    GetInputsFromFile(fullPath, packageRestoreInputs);
                }
                else
                {
                    // Not found
                    var message = string.Format(
                        CultureInfo.CurrentCulture,
                        LocalizedResourceManager.GetString("RestoreCommandFileNotFound"),
                        argument);

                    throw new InvalidOperationException(message);
                }
            }
            // Run inputs through msbuild to determine the
            // correct type and find dependencies as needed.
            await DetermineInputsFromMSBuildAsync(packageRestoreInputs);

            return packageRestoreInputs;
        }

        /// <summary>
        /// Read project inputs using MSBuild
        /// </summary>
        private async Task DetermineInputsFromMSBuildAsync(PackageRestoreInputs packageRestoreInputs)
        {
            // Find P2P graph for v3 inputs.
            // Ignore xproj files as top level inputs
            var projectsWithPotentialP2PReferences = packageRestoreInputs
                .ProjectFiles
                .Where(path => !path.EndsWith(".xproj", StringComparison.OrdinalIgnoreCase))
                .ToArray();

            if (projectsWithPotentialP2PReferences.Length > 0)
            {
                DependencyGraphSpec dgFileOutput = null;

                try
                {
                    dgFileOutput = await GetDependencyGraphSpecAsync(projectsWithPotentialP2PReferences,
                        GetSolutionDirectory(packageRestoreInputs),
                        packageRestoreInputs.NameOfSolutionFile,
                        ConfigFile);
                }
                catch (Exception ex)
                {
                    // At this point reading the project has failed, to keep backwards
                    // compatibility this should warn instead of error if
                    // packages.config files exist, but no project.json files.
                    // This will skip NETCore projects which is a problem, but there is
                    // not a good way to know if they exist, or if this is an old type of
                    // project that the targets file cannot handle.

                    // Log exception for debug
                    Console.LogDebug(ex.ToString());

                    // Check for packages.config but no project.json files
                    if (projectsWithPotentialP2PReferences.Where(HasPackagesConfigFile).Any()
                        && !projectsWithPotentialP2PReferences.Where(HasProjectJsonFile).Any())
                    {
                        // warn to let the user know that NETCore will be skipped
                        Console.LogWarning(LocalizedResourceManager.GetString("Warning_ReadingProjectsFailed"));

                        // Add packages.config
                        packageRestoreInputs.PackagesConfigFiles
                            .AddRange(projectsWithPotentialP2PReferences
                            .Select(GetPackagesConfigFile)
                            .Where(path => path != null));
                    }
                    else
                    {
                        // If there are project.json files or no packages.config files
                        // continue to fail
                        throw;
                    }
                }

                // Process the DG file and add both v2 and v3 inputs
                if (dgFileOutput != null)
                {
                    AddInputsFromDependencyGraphSpec(packageRestoreInputs, dgFileOutput);
                }
            }
        }

        private void AddInputsFromDependencyGraphSpec(PackageRestoreInputs packageRestoreInputs, DependencyGraphSpec dgFileOutput)
        {
            packageRestoreInputs.ProjectReferenceLookup = dgFileOutput;

            // Get top level entries
            var entryPointProjects = dgFileOutput
                .Projects
                .Where(project => dgFileOutput.Restore.Contains(project.RestoreMetadata.ProjectUniqueName, StringComparer.Ordinal))
                .ToList();

            // possible packages.config
            // Compare paths case-insenstive here since we do not know how msbuild modified them
            // find all projects that are not part of the v3 group
            var v2RestoreProjects =
                packageRestoreInputs.ProjectFiles
                  .Where(path => !entryPointProjects.Any(project => path.Equals(project.RestoreMetadata.ProjectPath, StringComparison.OrdinalIgnoreCase)));

            packageRestoreInputs.PackagesConfigFiles
                .AddRange(v2RestoreProjects
                .Select(GetPackagesConfigFile)
                .Where(path => path != null));

            // Filter down to just the requested projects in the file
            // that support transitive references.
            var v3RestoreProjects = entryPointProjects
                .Where(project => project.RestoreMetadata.ProjectStyle is ProjectStyle.PackageReference or ProjectStyle.ProjectJson);

            packageRestoreInputs.RestoreV3Context.Inputs.AddRange(v3RestoreProjects
                .Select(project => project.RestoreMetadata.ProjectPath));
        }

        private string GetPackagesConfigFile(string projectFilePath)
        {
            // Get possible packages.config path
            var packagesConfigPath = GetPackageReferenceFile(projectFilePath);
            if (File.Exists(packagesConfigPath))
            {
                return packagesConfigPath;
            }

            return null;
        }

        private bool HasPackagesConfigFile(string projectFilePath)
        {
            // Get possible packages.config path
            return GetPackagesConfigFile(projectFilePath) != null;
        }

        private bool HasProjectJsonFile(string projectFilePath)
        {
            // Get possible project.json path
            var projectFileName = Path.GetFileName(projectFilePath);
            var projectName = Path.GetFileNameWithoutExtension(projectFileName);
            var dir = Path.GetDirectoryName(projectFilePath);
            var projectJsonPath = ProjectJsonPathUtilities.GetProjectConfigPath(dir, projectName);

            return File.Exists(projectJsonPath);
        }

        /// <summary>
        ///  Create a dg v2 file using msbuild.
        /// </summary>
        private async Task<DependencyGraphSpec> GetDependencyGraphSpecAsync(string[] projectsWithPotentialP2PReferences, string solutionDirectory, string solutionName, string configFile)
        {
            // Create requests based on the solution directory if a solution was used read settings for the solution.
            // If the solution directory is null, then use config file if present
            // Then use restore directory last
            // If all 3 are null, then the directory of the project will be used to evaluate the settings

            int scaleTimeout;

            if (Project2ProjectTimeOut > 0)
            {
                scaleTimeout = Project2ProjectTimeOut * 1000;
            }
            else
            {
                scaleTimeout = MsBuildUtility.MsBuildWaitTime *
                    Math.Max(10, projectsWithPotentialP2PReferences.Length / 2) / 10;
            }

            Console.LogVerbose($"MSBuild P2P timeout [ms]: {scaleTimeout}");

            string restorePackagesWithLockFile = UseLockFile ? bool.TrueString : null;
            var restoreLockProperties = new RestoreLockProperties(restorePackagesWithLockFile, LockFilePath, LockedMode);

            // Call MSBuild to resolve P2P references.
            return await MsBuildUtility.GetProjectReferencesAsync(
                MsBuildDirectory.Value,
                projectsWithPotentialP2PReferences,
                scaleTimeout,
                Console,
                Recursive,
                solutionDirectory,
                solutionName,
                configFile,
                Source.ToArray(),
                PackagesDirectory,
                restoreLockProperties
                );
        }

        /// <summary>
        /// Determine if a file is v2 or v3
        /// </summary>
        private void GetInputsFromFile(string projectFilePath, PackageRestoreInputs packageRestoreInputs)
        {
            // An argument was passed in. It might be a solution file or directory,
            // project file, or packages.config file
            var projectFileName = Path.GetFileName(projectFilePath);

            if (IsPackagesConfig(projectFileName))
            {
                // restoring from packages.config or packages.projectname.config file
                packageRestoreInputs.PackagesConfigFiles.Add(projectFilePath);
            }
            else if (projectFileName.EndsWith("proj", StringComparison.OrdinalIgnoreCase))
            {
                packageRestoreInputs.ProjectFiles.Add(projectFilePath);
            }
            else if (projectFileName.EndsWith(".dg", StringComparison.OrdinalIgnoreCase))
            {
                packageRestoreInputs.RestoreV3Context.Inputs.Add(projectFilePath);
            }
            else if (projectFileName.EndsWith(".sln", StringComparison.OrdinalIgnoreCase)
                || projectFileName.EndsWith(".slnf", StringComparison.OrdinalIgnoreCase))
            {
                ProcessSolutionFile(projectFilePath, packageRestoreInputs);
            }
            else if (ProjectJsonPathUtilities.IsProjectConfig(projectFileName))
            {
                // project.json is no longer allowed
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, LocalizedResourceManager.GetString("Error_ProjectJsonNotAllowed"), projectFileName));
            }
            else
            {
                // Not a file we know about. Try to be helpful without response.
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, RestoreRunner.GetInvalidInputErrorMessage(projectFileName), projectFileName));
            }
        }

        /// <summary>
        /// Search a directory for v2 or v3 inputs, only the first type is taken.
        /// </summary>
        private void GetInputsFromDirectory(string directory, PackageRestoreInputs packageRestoreInputs)
        {
            var topLevelFiles = Directory.GetFiles(directory, "*.*", SearchOption.TopDirectoryOnly);

            //  Solution files
            var solutionFiles = topLevelFiles.Where(file =>
                file.EndsWith(".sln", StringComparison.OrdinalIgnoreCase))
                    .ToArray();

            if (solutionFiles.Length > 0)
            {
                if (solutionFiles.Length != 1)
                {
                    throw new InvalidOperationException(LocalizedResourceManager.GetString("Error_MultipleSolutions"));
                }

                var file = solutionFiles[0];

                if (Verbosity == Verbosity.Detailed)
                {
                    Console.WriteLine(
                        LocalizedResourceManager.GetString("RestoreCommandRestoringPackagesForSolution"),
                        file);
                }

                ProcessSolutionFile(file, packageRestoreInputs);

                return;
            }

            // Packages.config
            var packagesConfigFile = Path.Combine(directory, Constants.PackageReferenceFile);

            if (File.Exists(packagesConfigFile))
            {
                if (Verbosity == Verbosity.Detailed)
                {
                    Console.WriteLine(
                            LocalizedResourceManager.GetString(
                                "RestoreCommandRestoringPackagesFromPackagesConfigFile"));
                }

                // packages.confg with no solution file
                packageRestoreInputs.PackagesConfigFiles.Add(packagesConfigFile);

                return;
            }

            // The directory did not contain a valid target, fail!
            var noInputs = string.Format(
                CultureInfo.CurrentCulture,
                    LocalizedResourceManager.GetString(
                    "Error_UnableToLocateRestoreTarget"),
                    directory);

            throw new InvalidOperationException(noInputs);
        }

        private static bool IsSolutionOrProjectFile(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                var extension = Path.GetExtension(fileName);
                var lastFourCharacters = string.Empty;
                var length = extension.Length;

                if (length >= 4)
                {
                    lastFourCharacters = extension.Substring(length - 4);
                }

                return (string.Equals(extension, ".sln", StringComparison.OrdinalIgnoreCase)
                        || string.Equals(lastFourCharacters, "proj", StringComparison.OrdinalIgnoreCase));
            }
            return false;
        }

        /// <summary>
        /// True if the filename is a packages.config file
        /// </summary>
        private static bool IsPackagesConfig(string projectFileName)
        {
            return string.Equals(projectFileName, Constants.PackageReferenceFile, PathUtility.GetStringComparisonBasedOnOS())
                || (projectFileName.StartsWith("packages.", StringComparison.OrdinalIgnoreCase)
                && string.Equals(
                    Path.GetExtension(projectFileName),
                    Path.GetExtension(Constants.PackageReferenceFile), StringComparison.OrdinalIgnoreCase));
        }

        private string GetPackagesFolder(PackageRestoreInputs packageRestoreInputs)
        {
            if (!string.IsNullOrEmpty(PackagesDirectory))
            {
                return PackagesDirectory;
            }

            var repositoryPath = SettingsUtility.GetRepositoryPath(Settings);
            if (!string.IsNullOrEmpty(repositoryPath))
            {
                return repositoryPath;
            }

            if (!string.IsNullOrEmpty(SolutionDirectory))
            {
                return Path.Combine(SolutionDirectory, CommandLineConstants.PackagesDirectoryName);
            }

            if (packageRestoreInputs.RestoringWithSolutionFile)
            {
                return Path.Combine(
                    packageRestoreInputs.DirectoryOfSolutionFile,
                    CommandLineConstants.PackagesDirectoryName);
            }

            throw new InvalidOperationException(
                LocalizedResourceManager.GetString("RestoreCommandCannotDeterminePackagesFolder"));
        }

        private static string ConstructPackagesConfigFromProjectName(string projectName)
        {
            // we look for packages.<project name>.config file
            // but we don't want any space in the file name, so convert it to underscore.
            return "packages." + projectName.Replace(' ', '_') + ".config";
        }

        // returns the package reference file associated with the project
        private string GetPackageReferenceFile(string projectFile)
        {
            var projectName = Path.GetFileNameWithoutExtension(projectFile);
            var pathWithProjectName = Path.Combine(
                Path.GetDirectoryName(projectFile),
                ConstructPackagesConfigFromProjectName(projectName));
            if (File.Exists(pathWithProjectName))
            {
                return pathWithProjectName;
            }

            return Path.Combine(
                Path.GetDirectoryName(projectFile),
                Constants.PackageReferenceFile);
        }

        private void ProcessSolutionFile(string solutionFileFullPath, PackageRestoreInputs restoreInputs)
        {
            restoreInputs.DirectoryOfSolutionFile = Path.GetDirectoryName(solutionFileFullPath);
            restoreInputs.NameOfSolutionFile = Path.GetFileNameWithoutExtension(solutionFileFullPath);

            // restore packages for the solution
            string solutionLevelPackagesConfig;

            try
            {
                solutionLevelPackagesConfig = Path.Combine(
                    restoreInputs.DirectoryOfSolutionFile,
                    NuGetConstants.NuGetSolutionSettingsFolder,
                    Constants.PackageReferenceFile);
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                        LocalizedResourceManager.GetString("Error_InvalidSolutionDirectory"),
                        restoreInputs.DirectoryOfSolutionFile),
                    e);
            }

            if (File.Exists(solutionLevelPackagesConfig))
            {
                restoreInputs.PackagesConfigFiles.Add(solutionLevelPackagesConfig);
            }

            var projectFiles = MsBuildUtility.GetAllProjectFileNames(solutionFileFullPath, MsBuildDirectory.Value.Path);

            foreach (var projectFile in projectFiles)
            {
                if (!File.Exists(projectFile))
                {
                    var message = string.Format(CultureInfo.CurrentCulture,
                        LocalizedResourceManager.GetString("RestoreCommandProjectNotFound"),
                        projectFile);
                    Console.LogWarning(message);
                    continue;
                }

                var normalizedProjectFile = Path.GetFullPath(projectFile);

                // Add everything
                restoreInputs.ProjectFiles.Add(normalizedProjectFile);
            }
        }

        private void ValidatePackagesConfigLockFiles(IReadOnlyList<string> packagesConfigFiles, IReadOnlyList<PackageSpec> projects, string packagesFolderPath, List<RestoreSummary> restoreSummaries)
        {
            foreach (var pcFile in packagesConfigFiles)
            {
                var dgSpec = projects?.FirstOrDefault(p =>
                    {
                        if (p.RestoreMetadata is PackagesConfigProjectRestoreMetadata pcRestoreMetadata)
                        {
                            return StringComparer.OrdinalIgnoreCase.Equals(pcRestoreMetadata.PackagesConfigPath, pcFile);
                        }
                        return false;
                    });

                var projectFile = dgSpec?.FilePath ?? pcFile;
                var projectTfm = dgSpec?.TargetFrameworks.SingleOrDefault()?.FrameworkName ?? NuGetFramework.AnyFramework;
                var restoreLockedMode = LockedMode || (dgSpec?.RestoreMetadata?.RestoreLockProperties?.RestoreLockedMode ?? false);
                var lockFilePath = LockFilePath ?? dgSpec?.RestoreMetadata?.RestoreLockProperties?.NuGetLockFilePath;
                var useLockFile = UseLockFile ? bool.TrueString : dgSpec?.RestoreMetadata?.RestoreLockProperties?.RestorePackagesWithLockFile;

                IReadOnlyList<IRestoreLogMessage> result = PackagesConfigLockFileUtility.ValidatePackagesConfigLockFiles(
                    projectFile,
                    pcFile,
                    dgSpec?.Name,
                    lockFilePath,
                    useLockFile,
                    projectTfm,
                    packagesFolderPath,
                    restoreLockedMode,
                    CancellationToken.None);

                if (result != null)
                {
                    restoreSummaries.Add(new RestoreSummary(
                        success: false,
                        inputPath: projectFile,
                        configFiles: Array.Empty<string>(),
                        feedsUsed: Array.Empty<string>(),
                        installCount: 0,
                        errors: result));
                }
            }
        }

        private class PackageRestoreInputs
        {
            public PackageRestoreInputs()
            {
                ProjectReferenceLookup = new DependencyGraphSpec();
            }

            public bool RestoringWithSolutionFile => !string.IsNullOrEmpty(DirectoryOfSolutionFile);

            public string DirectoryOfSolutionFile { get; set; }

            public string NameOfSolutionFile { get; set; }

            public List<string> PackagesConfigFiles { get; } = new List<string>();

            public DependencyGraphSpec ProjectReferenceLookup { get; set; }

            public RestoreArgs RestoreV3Context { get; set; } = new RestoreArgs();

            /// <summary>
            /// Project files, type to be determined.
            /// </summary>
            public HashSet<string> ProjectFiles { get; set; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        }
    }
}
