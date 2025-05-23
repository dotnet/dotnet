// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Credentials;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Packaging.Core;
using NuGet.ProjectModel;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace NuGet.CommandLine.XPlat.Commands.Package.Update
{
    internal static class PackageUpdateCommandRunner
    {
        internal static async Task<int> Run(PackageUpdateArgs args, ILoggerWithColor logger, IDGSpecFactory dGSpecFactory, MSBuildAPIUtility msbuild, CancellationToken cancellationToken)
        {
            XPlatUtility.ConfigureProtocol();
            DefaultCredentialServiceUtility.SetupDefaultCredentialService(logger, nonInteractive: false);

            // 1. Get DGSpec for project/solution
            // 2. Find suitable version of package(s) to update
            // 3. Preview restore to validate changes
            // 4. Update MSBuild files
            // 5. Commit restore if everything successful

            // 1. Get DGSpec for project/solution
            string settingsRoot = Directory.Exists(args.Project) ? args.Project : Path.GetDirectoryName(args.Project)!;
            ISettings settings = Settings.LoadDefaultSettings(settingsRoot);

            var dgSpec = dGSpecFactory.GetDependencyGraphSpec(args.Project);

            if (dgSpec is null || dgSpec.Restore is null || dgSpec.Restore.Count == 0)
            {
                logger.LogMinimal(
                    string.Format(CultureInfo.CurrentCulture, Strings.Error_PathIsMissingOrInvalid, args.Project),
                    ConsoleColor.Red);
                return ExitCodes.Error;
            }

            if (dgSpec.Restore.Count > 1)
            {
                logger.LogMinimal(Strings.Unsupported_UpdatingMoreThanOneProject, ConsoleColor.Red);
                return ExitCodes.Error;
            }

            // 2. Find suitable version of package(s) to update
            // Source provider will be needed to find the package version and to restore, so create it here.
            CachingSourceProvider sourceProvider = new CachingSourceProvider(new PackageSourceProvider(settings));
            using SourceCacheContext sourceCacheContext = new();
            (PackageDependency? packageToUpdate, List<NuGetFramework>? packageTfms) = await GetPackageToUpdateAsync(args.Packages, dgSpec.Projects.Single(), sourceProvider, settings, sourceCacheContext, logger, cancellationToken);

            if (packageToUpdate is null)
            {
                // GetPackagesToUpdateAsync is responsible for outputting an error message.
                return ExitCodes.Error;
            }

            // 3. Preview restore to validate changes
            var updatedPackageSpec = dgSpec.Projects[0].Clone();
            PackageSpecOperations.AddOrUpdateDependency(updatedPackageSpec, packageToUpdate);

            var updatedDgSpec = dgSpec.WithReplacedSpec(updatedPackageSpec).WithoutRestores();
            updatedDgSpec.AddRestore(updatedPackageSpec.RestoreMetadata.ProjectUniqueName);

            logger.LogDebug("Running Restore preview");

            var restorePreviewResult = await PreviewUpdatePackageReferenceAsync(updatedDgSpec, sourceCacheContext, logger, cancellationToken);

            logger.LogDebug("Restore preview completed");

            if (!restorePreviewResult.Result.Success)
            {
                logger.LogMinimal(Strings.PackageUpdate_PreviewRestoreFailed, ConsoleColor.Red);
                return ExitCodes.Error;
            }

            var libraryDependency = AddPackageReferenceCommandRunner.GenerateLibraryDependency(
                updatedPackageSpec,
                customPackagesPath: null,
                restorePreviewResult,
                packageTfms,
                packageToUpdate);

            // 4. Update MSBuild files
            if (packageTfms!.Count == dgSpec.Projects[0].TargetFrameworks.Count)
            {
                // package is used by all project TFMs (no condition)

                msbuild.AddPackageReference(dgSpec.Projects[0].FilePath, libraryDependency, noVersion: true);
            }
            else
            {
                var frameworkAliases = packageTfms
                    .Select(e => AddPackageReferenceCommandRunner.GetAliasForFramework(dgSpec.Projects[0], e))
                    .Where(originalFramework => originalFramework != null);

                msbuild.AddPackageReferencePerTFM(dgSpec.Projects[0].FilePath, libraryDependency, frameworkAliases, noVersion: true);
            }

            // 5. Commit restore if everything successful
            await RestoreRunner.CommitAsync(restorePreviewResult, CancellationToken.None);

            return ExitCodes.Success;
        }

        private static async Task<RestoreResultPair> PreviewUpdatePackageReferenceAsync(
            DependencyGraphSpec dgSpec,
            SourceCacheContext cacheContext,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            var providerCache = new RestoreCommandProvidersCache();

            // Pre-loaded request provider containing the graph file
            var providers = new List<IPreLoadedRestoreRequestProvider>
                {
                    new DependencyGraphSpecRequestProvider(providerCache, dgSpec)
                };

            var globalPackagesFolder = dgSpec.GetProjectSpec(dgSpec.Restore.Single()).RestoreMetadata.PackagesPath;

            var restoreContext = new RestoreArgs()
            {
                CacheContext = cacheContext,
                LockFileVersion = LockFileFormat.Version,
                Log = logger,
                MachineWideSettings = new XPlatMachineWideSetting(),
                GlobalPackagesFolder = globalPackagesFolder,
                PreLoadedRequestProviders = providers
                // Sources : No need to pass it, because SourceRepositories contains the already built SourceRepository objects
            };

            // Generate Restore Requests. There will always be 1 request here since we are restoring for 1 project.
            var restoreRequests = await RestoreRunner.GetRequests(restoreContext);

            // Run restore without commit. This will always return 1 Result pair since we are restoring for 1 request.
            var restoreResult = await RestoreRunner.RunWithoutCommitAsync(restoreRequests, restoreContext, cancellationToken);

            return restoreResult.Single();
        }

        private static async Task<(PackageDependency?, List<NuGetFramework>?)> GetPackageToUpdateAsync(
            IReadOnlyList<string>? packages,
            PackageSpec project,
            CachingSourceProvider sourceProvider,
            ISettings settings,
            SourceCacheContext sourceCacheContext,
            ILoggerWithColor logger,
            CancellationToken cancellationToken)
        {
            if (packages is null || packages.Count == 0)
            {
                logger.LogMinimal(Strings.Unsupported_UpgradeAllPackages, ConsoleColor.Red);
                return (null, null);
            }

            if (packages.Count > 1)
            {
                logger.LogMinimal(Strings.Unsupported_MoreThanOnePackage, ConsoleColor.Red);
                return (null, null);
            }

            var sourceMapping = PackageSourceMapping.GetPackageSourceMapping(settings);
            if (sourceMapping.IsEnabled)
            {
                logger.LogMinimal(Strings.Unsupported_PackageSourceMapping, ConsoleColor.Red);
                return (null, null);
            }

            var packageId = packages.Single();

            VersionRange? existingVersion = null;
            List<NuGetFramework>? frameworks = null;
            foreach (var tfm in project.TargetFrameworks)
            {
                foreach (var dependency in tfm.Dependencies)
                {
                    if (string.Equals(packageId, dependency.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        if (frameworks is null)
                        {
                            frameworks = new List<NuGetFramework>();
                        }
                        frameworks.Add(tfm.FrameworkName);

                        VersionRange tfmVersionRange;
                        if (project.RestoreMetadata.CentralPackageFloatingVersionsEnabled)
                        {
                            if (!tfm.CentralPackageVersions.TryGetValue(
                                packageId,
                                out CentralPackageVersion? centralVersion))
                            {
                                logger.LogMinimal(
                                    Messages.Error_CouldNotFindPackageVersionForCpmPackage(packageId),
                                    ConsoleColor.Red);
                                return (null, null);
                            }
                            tfmVersionRange = centralVersion.VersionRange;
                        }
                        else
                        {
                            tfmVersionRange = dependency.LibraryRange.VersionRange ?? VersionRange.All;
                        }

                        if (existingVersion == null)
                        {
                            existingVersion = tfmVersionRange;
                        }
                        else
                        {
                            if (tfmVersionRange != existingVersion)
                            {
                                logger.LogMinimal(
                                    Messages.Unsupported_UpdatePackageWithDifferentPerTfmVersions(packageId, project.FilePath),
                                    ConsoleColor.Red);
                                return (null, null);
                            }
                        }
                    }
                }
            }

            if (existingVersion is null)
            {
                logger.LogMinimal(Messages.Error_PackageNotReferenced(packageId, project.FilePath), ConsoleColor.Red);
                return (null, null);
            }

            NuGetVersion? highestVersion = await VersionChooser.GetLatestVersionAsync(
                packageId,
                sourceProvider,
                settings,
                sourceCacheContext,
                logger,
                cancellationToken);

            if (highestVersion is null)
            {
                logger.LogMinimal(Messages.Error_NoVersionsAvailable(packageId), ConsoleColor.Red);
                return (null, null);
            }

            if (existingVersion.MinVersion == highestVersion)
            {
                logger.LogMinimal(Messages.Warning_AlreadyHighestVersion(packageId, highestVersion.OriginalVersion, project.FilePath), ConsoleColor.Red);
                return (null, null);
            }

            PackageDependency packageToUpdate = new(packageId, new VersionRange(highestVersion));

            return (packageToUpdate, frameworks);
        }
    }
}
