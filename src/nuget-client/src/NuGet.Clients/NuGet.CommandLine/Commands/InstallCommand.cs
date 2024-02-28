// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.PackageManagement;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Packaging.PackageExtraction;
using NuGet.Packaging.Signing;
using NuGet.ProjectManagement;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Resolver;
using NuGet.Versioning;

namespace NuGet.CommandLine
{
    [Command(typeof(NuGetCommand), "install", "InstallCommandDescription",
        MinArgs = 0, MaxArgs = 1, UsageSummaryResourceName = "InstallCommandUsageSummary",
        UsageDescriptionResourceName = "InstallCommandUsageDescription",
        UsageExampleResourceName = "InstallCommandUsageExamples")]
    public class InstallCommand : DownloadCommandBase
    {
        [Option(typeof(NuGetCommand), "InstallCommandOutputDirDescription")]
        public string OutputDirectory { get; set; }

        [Option(typeof(NuGetCommand), "InstallCommandVersionDescription")]
        public string Version { get; set; }

        [Option(typeof(NuGetCommand), "InstallCommandDependencyVersion")]
        public string DependencyVersion { get; set; }

        [Option(typeof(NuGetCommand), "InstallCommandFrameworkDescription")]
        public string Framework { get; set; }

        [Option(typeof(NuGetCommand), "InstallCommandExcludeVersionDescription", AltName = "x")]
        public bool ExcludeVersion { get; set; }

        [Option(typeof(NuGetCommand), "InstallCommandPrerelease")]
        public bool Prerelease { get; set; }

        [Option(typeof(NuGetCommand), "InstallCommandRequireConsent")]
        public bool RequireConsent { get; set; }

        [Option(typeof(NuGetCommand), "InstallCommandSolutionDirectory")]
        public string SolutionDirectory { get; set; }

        [ImportingConstructor]
        protected internal InstallCommand()
        {
        }

        public override Task ExecuteCommandAsync()
        {
            // On mono, parallel builds are broken for some reason. See https://gist.github.com/4201936 for the errors
            // That are thrown.
            DisableParallelProcessing |= RuntimeEnvironmentHelper.IsMono;

            if (DisableParallelProcessing)
            {
                HttpSourceResourceProvider.Throttle = SemaphoreSlimThrottle.CreateBinarySemaphore();
            }

            CalculateEffectivePackageSaveMode();
            CalculateEffectiveSettings();
            var installPath = Path.GetFullPath(ResolveInstallPath());

            var configFilePath = Path.GetFullPath(Arguments.Count == 0 ? Constants.PackageReferenceFile : Arguments[0]);
            var configFileName = Path.GetFileName(configFilePath);

            // If the first argument is a packages.xxx.config file, install everything it lists
            // Otherwise, treat the first argument as a package Id
            if (CommandLineUtility.IsValidConfigFileName(configFileName))
            {
                Prerelease = true;

                // display opt-out message if needed
                if (Console != null && RequireConsent &&
                    new PackageRestoreConsent(Settings).IsGranted)
                {
                    var message = string.Format(
                        CultureInfo.CurrentCulture,
                        LocalizedResourceManager.GetString("RestoreCommandPackageRestoreOptOutMessage"),
                        NuGetResources.PackageRestoreConsentCheckBoxText.Replace("&", ""));
                    Console.WriteLine(message);
                }

                return PerformV2RestoreAsync(configFilePath, installPath);
            }
            else
            {
                var packageId = Arguments[0];
                var version = Version != null ? new NuGetVersion(Version) : null;
                return InstallPackageAsync(packageId, version, installPath);
            }
        }

        private void CalculateEffectiveSettings()
        {
            // If the SolutionDir is specified, use the .nuget directory under it to determine the solution-level settings
            if (!string.IsNullOrEmpty(SolutionDirectory))
            {
                string path;
                try
                {
                    path = Path.Combine(SolutionDirectory, NuGetConstants.NuGetSolutionSettingsFolder);
                }
                catch (ArgumentException e)
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                            LocalizedResourceManager.GetString("Error_InvalidSolutionDirectory"),
                            SolutionDirectory),
                        e);
                }

                var solutionSettingsFile = Path.GetFullPath(path);

                Settings = Configuration.Settings.LoadDefaultSettings(
                    solutionSettingsFile,
                    configFileName: null,
                    machineWideSettings: MachineWideSettings);

                // Recreate the source provider and credential provider
                SourceProvider = PackageSourceBuilder.CreateSourceProvider(Settings);
                SetDefaultCredentialProvider();
            }
        }

        internal string ResolveInstallPath()
        {
            if (!string.IsNullOrEmpty(OutputDirectory))
            {
                // Use the OutputDirectory if specified.
                return OutputDirectory;
            }

            var installPath = SettingsUtility.GetRepositoryPath(Settings);
            if (!string.IsNullOrEmpty(installPath))
            {
                // If a value is specified in config, use that.
                return installPath;
            }

            if (!string.IsNullOrEmpty(SolutionDirectory))
            {
                // For package restore scenarios, deduce the path of the packages directory from the solution directory.
                return Path.Combine(SolutionDirectory, CommandLineConstants.PackagesDirectoryName);
            }

            // Use the current directory as output.
            return CurrentDirectory;
        }

        private async Task PerformV2RestoreAsync(string packagesConfigFilePath, string installPath)
        {
            var sourceRepositoryProvider = GetSourceRepositoryProvider();
            var nuGetPackageManager = new NuGetPackageManager(sourceRepositoryProvider, Settings, installPath, ExcludeVersion);

            var installedPackageReferences = GetInstalledPackageReferences(packagesConfigFilePath);

            var packageRestoreData = installedPackageReferences.Select(reference =>
                new PackageRestoreData(
                    reference,
                    new[] { packagesConfigFilePath },
                    isMissing: true));

            var packageSources = GetPackageSources(Settings);

            Console.PrintPackageSources(packageSources);

            var failedEvents = new ConcurrentQueue<PackageRestoreFailedEventArgs>();

            var packageRestoreContext = new PackageRestoreContext(
                nuGetPackageManager,
                packageRestoreData,
                CancellationToken.None,
                packageRestoredEvent: null,
                packageRestoreFailedEvent: (sender, args) => { failedEvents.Enqueue(args); },
                sourceRepositories: packageSources.Select(sourceRepositoryProvider.CreateRepository),
                maxNumberOfParallelTasks: DisableParallelProcessing ? 1 : PackageManagementConstants.DefaultMaxDegreeOfParallelism,
                enableNuGetAudit: true,
                restoreAuditProperties: new(),
                logger: Console);

            var packageSaveMode = Packaging.PackageSaveMode.Defaultv2;
            if (EffectivePackageSaveMode != Packaging.PackageSaveMode.None)
            {
                packageSaveMode = EffectivePackageSaveMode;
            }

            var missingPackageReferences = installedPackageReferences.Any(reference => !nuGetPackageManager.PackageExistsInPackagesFolder(reference.PackageIdentity, packageSaveMode));

            if (!missingPackageReferences)
            {
                var message = string.Format(
                    CultureInfo.CurrentCulture,
                    LocalizedResourceManager.GetString("InstallCommandNothingToInstall"),
                    packagesConfigFilePath);

                Console.LogMinimal(message);
            }
            using (var cacheContext = new SourceCacheContext())
            {
                cacheContext.NoCache = NoCache || NoHttpCache;
                cacheContext.DirectDownload = DirectDownload;

                var clientPolicyContext = ClientPolicyContext.GetClientPolicy(Settings, Console);

                var projectContext = new ConsoleProjectContext(Console)
                {
                    PackageExtractionContext = new PackageExtractionContext(
                        packageSaveMode,
                        PackageExtractionBehavior.XmlDocFileSaveMode,
                        clientPolicyContext,
                        Console)
                };

                var packageSourceMapping = PackageSourceMapping.GetPackageSourceMapping(Settings);

                var downloadContext = new PackageDownloadContext(cacheContext, installPath, DirectDownload, packageSourceMapping)
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

                // Use failure count to determine errors. result.Restored will be false for noop restores.
                if (failedEvents.Count > 0)
                {
                    // Log errors if they exist
                    foreach (var message in failedEvents.Select(e => new RestoreLogMessage(LogLevel.Error, NuGetLogCode.Undefined, e.Exception.Message)))
                    {
                        await Console.LogAsync(message);
                    }

                    throw new ExitCodeException(1);
                }
            }
        }

        private CommandLineSourceRepositoryProvider GetSourceRepositoryProvider()
        {
            return new CommandLineSourceRepositoryProvider(SourceProvider);
        }

        private async Task InstallPackageAsync(
            string packageId,
            NuGetVersion version,
            string installPath)
        {
            if (version == null)
            {
                // Avoid searching for the highest version in the global packages folder,
                // it needs to come from the feeds instead. Once found it may come from
                // the global packages folder unless NoCache is true.
                ExcludeCacheAsSource = true;
            }

            var framework = GetTargetFramework();

            // Create the project and set the framework if available.
            var project = new InstallCommandProject(
                root: installPath,
                packagePathResolver: new PackagePathResolver(installPath, !ExcludeVersion),
                targetFramework: framework);

            var sourceRepositoryProvider = GetSourceRepositoryProvider();
            var packageManager = new NuGetPackageManager(sourceRepositoryProvider, Settings, installPath);

            var packageSources = GetPackageSources(Settings);
            var primaryRepositories = packageSources.Select(sourceRepositoryProvider.CreateRepository);
            Console.PrintPackageSources(packageSources);

            var allowPrerelease = Prerelease || (version != null && version.IsPrerelease);

            var dependencyBehavior = DependencyBehaviorHelper.GetDependencyBehavior(DependencyBehavior.Lowest, DependencyVersion, Settings);

            using (var sourceCacheContext = new SourceCacheContext())
            {
                var resolutionContext = new ResolutionContext(
                dependencyBehavior,
                includePrelease: allowPrerelease,
                includeUnlisted: false,
                versionConstraints: VersionConstraints.None,
                gatherCache: new GatherCache(),
                sourceCacheContext: sourceCacheContext);

                if (version == null)
                {
                    // Write out a helpful message before the http messages are shown
                    Console.Log(LogLevel.Minimal, string.Format(
                        CultureInfo.CurrentCulture,
                        LocalizedResourceManager.GetString("InstallPackageMessage"), packageId, installPath));

                    // Find the latest version using NuGetPackageManager
                    var resolvePackage = await NuGetPackageManager.GetLatestVersionAsync(
                        packageId,
                        project,
                        resolutionContext,
                        primaryRepositories,
                        Console,
                        CancellationToken.None);

                    if (resolvePackage == null || resolvePackage.LatestVersion == null)
                    {
                        var message = string.Format(
                            CultureInfo.CurrentCulture,
                            LocalizedResourceManager.GetString("InstallCommandUnableToFindPackage"),
                            packageId);

                        throw new CommandException(message);
                    }

                    version = resolvePackage.LatestVersion;
                }

                // Get a list of packages already in the folder.
                var installedPackages = await project.GetFolderPackagesAsync(CancellationToken.None);

                // Find existing versions of the package
                var alreadyInstalledVersions = new HashSet<NuGetVersion>(installedPackages
                    .Where(e => StringComparer.OrdinalIgnoreCase.Equals(packageId, e.PackageIdentity.Id))
                    .Select(e => e.PackageIdentity.Version));

                var packageIdentity = new PackageIdentity(packageId, version);

                var PackageSaveMode = Packaging.PackageSaveMode.Defaultv2;
                if (EffectivePackageSaveMode != Packaging.PackageSaveMode.None)
                {
                    PackageSaveMode = EffectivePackageSaveMode;
                }

                // Check if the package already exists or a higher version exists already.
                var skipInstall = project.PackageExists(packageIdentity, PackageSaveMode);

                // For SxS allow other versions to install. For non-SxS skip if a higher version exists.
                skipInstall |= (ExcludeVersion && alreadyInstalledVersions.Any(e => e >= version));

                if (skipInstall)
                {
                    var message = string.Format(
                        CultureInfo.CurrentCulture,
                        LocalizedResourceManager.GetString("InstallCommandPackageAlreadyExists"),
                        packageIdentity);

                    Console.LogMinimal(message);
                }
                else
                {
                    var clientPolicyContext = ClientPolicyContext.GetClientPolicy(Settings, Console);

                    var projectContext = new ConsoleProjectContext(Console)
                    {
                        PackageExtractionContext = new PackageExtractionContext(
                            PackageSaveMode,
                            PackageExtractionBehavior.XmlDocFileSaveMode,
                            clientPolicyContext,
                            Console)
                    };

                    resolutionContext.SourceCacheContext.NoCache = NoCache || NoHttpCache;
                    resolutionContext.SourceCacheContext.DirectDownload = DirectDownload;

                    var packageSourceMapping = PackageSourceMapping.GetPackageSourceMapping(Settings);

                    var downloadContext = new PackageDownloadContext(resolutionContext.SourceCacheContext, installPath, DirectDownload, packageSourceMapping)
                    {
                        ClientPolicyContext = clientPolicyContext
                    };

                    await packageManager.InstallPackageAsync(
                        project,
                        packageIdentity,
                        resolutionContext,
                        projectContext,
                        downloadContext,
                        primaryRepositories,
                        Enumerable.Empty<SourceRepository>(),
                        CancellationToken.None);

                    if (downloadContext.DirectDownload)
                    {
                        GetDownloadResultUtility.CleanUpDirectDownloads(downloadContext);
                    }
                }
            }
        }

        /// <summary>
        /// Parse the Framework parameter or use Any as the default framework.
        /// </summary>
        private NuGetFramework GetTargetFramework()
        {
            var targetFramework = NuGetFramework.AnyFramework;

            if (!string.IsNullOrEmpty(Framework))
            {
                targetFramework = NuGetFramework.Parse(Framework);
            }

            if (targetFramework.IsUnsupported)
            {
                // Fail with a helpful message if the user provided an invalid framework.
                var message = string.Format(CultureInfo.CurrentCulture,
                    LocalizedResourceManager.GetString("UnsupportedFramework"),
                    Framework);

                throw new ArgumentException(message);
            }

            return targetFramework;
        }
    }
}
