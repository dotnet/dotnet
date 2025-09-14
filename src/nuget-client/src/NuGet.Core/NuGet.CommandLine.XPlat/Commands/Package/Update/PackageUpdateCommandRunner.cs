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
using NuGet.CommandLine.XPlat.Utility;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Credentials;
using NuGet.LibraryModel;
using NuGet.Packaging.Core;
using NuGet.ProjectModel;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Protocol.Model;
using NuGet.Versioning;

namespace NuGet.CommandLine.XPlat.Commands.Package.Update;

internal static class PackageUpdateCommandRunner
{
    // This overload sets static state, so should not be used in tests.
    internal static Task<int> Run(PackageUpdateArgs args, CancellationToken cancellationToken)
    {
        ILoggerWithColor logger = new CommandOutputLogger(args.LogLevel)
        {
            HidePrefixForInfoAndMinimal = true
        };

        XPlatUtility.ConfigureProtocol();
        DefaultCredentialServiceUtility.SetupDefaultCredentialService(logger, nonInteractive: !args.Interactive);

        // MSBuildAPIUtility's output is different to what we want for package update.
        // While it would probably be a good idea to align the output of all commands using MSBuildAPIUtility,
        // in order to meet deadlines, we'll suppress its output, and leave improvements for later.
        MSBuildAPIUtility msBuild = new(NullLogger.Instance);

        var restoreHelper = new PackageUpdateIO(msBuild, EnvironmentVariableWrapper.Instance);

        return Run(args, logger, restoreHelper, cancellationToken);
    }

    internal static async Task<int> Run(PackageUpdateArgs args, ILoggerWithColor logger, IPackageUpdateIO packageUpdateIO, CancellationToken cancellationToken)
    {
        // 1. Get DGSpec for project/solution
        // 2. Find suitable version of package(s) to update
        // 3. Preview restore to validate changes
        // 4. Update MSBuild files
        // 5. Commit restore if everything successful

        // 1. Get DGSpec for project/solution
        logger.LogVerbose(Strings.PackageUpdate_LoadingDGSpec);
        var dgSpec = packageUpdateIO.GetDependencyGraphSpec(args.Project);

        if (dgSpec is null || dgSpec.Restore is null || dgSpec.Restore.Count == 0)
        {
            logger.LogMinimal(
                string.Format(CultureInfo.CurrentCulture, Strings.Error_PathIsMissingOrInvalid, args.Project),
                ConsoleColor.Red);
            return ExitCodes.Error;
        }

        if (args.Vulnerable)
        {
            logger.LogInformation(Format.PackageUpdate_UpdatingVulnerablePackages(args.Project));
        }
        else
        {
            logger.LogInformation(Format.PackageUpdate_UpdatingOutdatedPackages(args.Project));
        }

        if (dgSpec.Restore.Count > 1)
        {
            logger.LogMinimal(Strings.Unsupported_UpdatingMoreThanOneProject, ConsoleColor.Red);
            return ExitCodes.Error;
        }

        PackageSpec projectSpec = dgSpec.Projects.Count == 1
            ? dgSpec.Projects[0]
            : dgSpec.GetProjectSpec(dgSpec.Restore[0]);

        // 2. Find suitable version of package(s) to update
        // Source provider will be needed to find the package version and to restore, so create it here.
        logger.LogVerbose(Strings.PackageUpdate_FindingUpdateVersions);

        string settingsRoot = Directory.Exists(args.Project) ? args.Project : Path.GetDirectoryName(args.Project)!;
        ISettings settings = packageUpdateIO.LoadSettings(settingsRoot);

        CachingSourceProvider sourceProvider = new CachingSourceProvider(new PackageSourceProvider(settings));
        using SourceCacheContext sourceCacheContext = new();
        var versionChooser = new VersionChooser(sourceProvider, settings, sourceCacheContext);

        bool noPackagesSpecified = args.Packages is null || args.Packages.Count == 0;
        int totalPackagesScanned = 0;
        List<PackageUpdateResult> packagesToUpdateResult;

        if (args.Vulnerable)
        {
            if (!NuGetAuditEnabled(projectSpec))
            {
                logger.LogError(Strings.PackageUpdate_AuditDisabled);
                return ExitCodes.InvalidArgs;
            }

            if (!IsNuGetAuditModeSetToAll(projectSpec))
            {
                logger.LogWarning(Strings.PackageUpdate_AuditModeIsDirect);
            }

            (packagesToUpdateResult, totalPackagesScanned) = await SelectVulnerablePackagesToUpdateAsync(args.Packages, dgSpec, versionChooser, settings, logger, packageUpdateIO, cancellationToken);
        }
        else if (noPackagesSpecified)
        {
            var allProjectPackages = GetAllPackagesReferencedByProject(projectSpec);
            totalPackagesScanned = allProjectPackages.Count;
            packagesToUpdateResult = await SelectAllPackagesWithUpdatesAsync(projectSpec, versionChooser, settings, logger, cancellationToken);
        }
        else
        {
            totalPackagesScanned = args.Packages!.Count;
            packagesToUpdateResult = await SelectPackagesToUpdateAsync(args.Packages, projectSpec, versionChooser, settings, logger, cancellationToken);
        }

        if (packagesToUpdateResult.Count == 0)
        {
            if (args.Vulnerable)
            {
                logger.LogMinimal(Strings.PackageUpdate_NoVulnerablePackages, ConsoleColor.Green);
                return ExitCodes.Success;
            }
            else if (noPackagesSpecified)
            {
                // When no packages are specified and all packages are already up to date, return exit code 2
                logger.LogMinimal(Strings.PackageUpdate_AlreadyUpToDate, ConsoleColor.Green);
                return ExitCodes.NoPackagesNeedUpdating;
            }
            else
            {
                // GetPackagesToUpdateAsync is responsible for outputting error messages.
                return ExitCodes.Error;
            }
        }

        var projectName = Path.GetFileNameWithoutExtension(dgSpec.Projects[0].FilePath);
        logger.LogInformation($"  {projectName}:");

        foreach (var packageResult in packagesToUpdateResult)
        {
            logger.LogInformation($"    " + Format.PackageUpdate_UpdatedMessage(packageResult.Package.Id, packageResult.Package.CurrentVersion.ToShortString(), packageResult.Package.NewVersion.ToShortString()));
        }

        // New line in between projects, or before the final summary.
        logger.LogInformation("");

        // 3. Preview restore to validate changes
        logger.LogDebug(Strings.PackageUpdate_PreviewRestore);
        var updatedDgSpec = GetUpdatedDependencyGraphSpec(dgSpec, packagesToUpdateResult);
        var restorePreviewResult = await packageUpdateIO.PreviewUpdatePackageReferenceAsync(updatedDgSpec, sourceCacheContext, logger, cancellationToken);

        if (!restorePreviewResult.Success)
        {
            logger.LogMinimal(Strings.PackageUpdate_PreviewRestoreFailed, ConsoleColor.Red);
            return ExitCodes.Error;
        }

        // 4. Update MSBuild files

        var updatedPackageSpec = updatedDgSpec.Projects[0];
        int updatedCount = 0;

        foreach (var packageResult in packagesToUpdateResult)
        {
            packageUpdateIO.UpdatePackageReference(updatedPackageSpec, restorePreviewResult, packageResult.TargetFrameworkAliases, packageResult.Package, logger);
            updatedCount++;
        }

        // 5. Commit restore if everything successful
        await packageUpdateIO.CommitAsync(restorePreviewResult, CancellationToken.None);

        int scannedCount = totalPackagesScanned;
        logger.LogMinimal(Format.PackageUpdate_FinalSummary(updatedCount, scannedCount), ConsoleColor.Green);

        return ExitCodes.Success;
    }

    private static async Task<(List<PackageUpdateResult> vulnerablePackages, int packagesScanned)> SelectVulnerablePackagesToUpdateAsync(
        IReadOnlyList<Package>? packages,
        DependencyGraphSpec dgSpec,
        IVersionChooser versionChooser,
        ISettings settings,
        ILoggerWithColor logger,
        IPackageUpdateIO packageUpdateIO,
        CancellationToken cancellationToken)
    {
        LockFile assetsFile = await GetProjectAssetsFileAsync(dgSpec, logger, packageUpdateIO, cancellationToken);

        // Log messages don't have package version in a usable way, so we have to first find the list of package ids,
        // then check each TFM's package list against that list.
        HashSet<string> packageIdsWithVulnerabilities = assetsFile
            .LogMessages
            .Where(log => log.Code >= NuGetLogCode.NU1901 && log.Code <= NuGetLogCode.NU1904 && !string.IsNullOrEmpty(log.LibraryId))
            .Select(log => log.LibraryId!)
            .Where(id => packages is null || packages.Count == 0 || packages.Any(p => string.Equals(p.Id, id, StringComparison.OrdinalIgnoreCase)))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        int scannedPackages = assetsFile.Libraries.Count(l => l.Type == "package" && (packages is null || packages.Count == 0 || packages.Any(p => string.Equals(p.Id, l.Name, StringComparison.OrdinalIgnoreCase))));

        if (packageIdsWithVulnerabilities.Count > 0)
        {
            IReadOnlyList<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>> knownVulnerabilities = await GetKnownVulnerabilitiesAsync(settings, logger, cancellationToken);

            List<(PackageIdentity package, List<string> targetFrameworkAliases)> packagesToUpdate = assetsFile
                .Targets
                .SelectMany(tf => tf.Libraries.Select(library => (tf.TargetFramework, library)))
                .Where(tuple => tuple.library.Type == "package" && packageIdsWithVulnerabilities.Contains(tuple.library.Name!) && PackageHasVulnerability(tuple.library.Name!, tuple.library.Version!, knownVulnerabilities))
                .GroupBy(
                    pair => new PackageIdentity(pair.library.Name, pair.library.Version),
                    pair => assetsFile.PackageSpec.TargetFrameworks.Single(tfm => tfm.FrameworkName == pair.TargetFramework).TargetAlias,
                    (key, g) => (key, g.Distinct().ToList()))
                .ToList();

            var packagesToUpdateResult = new List<PackageUpdateResult>(packagesToUpdate.Count);
            foreach (var (packageIdentity, tfmAliases) in packagesToUpdate)
            {
                var nonVulnerableVersion = await versionChooser.GetNonVulnerableAsync(packageIdentity.Id, packageIdentity.Version, NullLogger.Instance, knownVulnerabilities, cancellationToken);
                if (nonVulnerableVersion is null)
                {
                    logger.LogMinimal(Format.PackageUpdate_AllVersionsHaveAdvisories(packageIdentity.Id), ConsoleColor.Yellow);
                }
                else
                {
                    packagesToUpdateResult.Add(new PackageUpdateResult
                    {
                        Package = new PackageToUpdate
                        {
                            Id = packageIdentity.Id,
                            CurrentVersion = new VersionRange(packageIdentity.Version),
                            NewVersion = new VersionRange(nonVulnerableVersion)
                        },
                        TargetFrameworkAliases = tfmAliases
                    });
                }
            }

            return (packagesToUpdateResult, scannedPackages);
        }

        return ([], scannedPackages);

        bool PackageHasVulnerability(string packageId, NuGetVersion version, IReadOnlyList<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>> knownVulnerabilities)
        {
            if (version is null)
            {
                throw new ArgumentException("Package version must be specified when checking for vulnerabilities.");
            }

            foreach (var vulnDict in knownVulnerabilities)
            {
                if (vulnDict.TryGetValue(packageId, out var vulnInfoList))
                {
                    foreach (var vulnInfo in vulnInfoList)
                    {
                        if (vulnInfo.Versions.Satisfies(version))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }

    private static async Task<IReadOnlyList<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>>> GetKnownVulnerabilitiesAsync(ISettings settings, ILoggerWithColor logger, CancellationToken cancellationToken)
    {
        var packageSourceProvider = new PackageSourceProvider(settings);
        var auditSources = packageSourceProvider.LoadAuditSources()?.Where(s => s.IsEnabled).ToList();
        if (auditSources is null || auditSources.Count == 0)
        {
            auditSources = packageSourceProvider.LoadPackageSources().Where(s => s.IsEnabled).ToList();
        }

        var remappedLogger = new RemappedLevelLogger(
            logger,
            new RemappedLevelLogger.Mapping
            {
                Information = LogLevel.Verbose,
            });

        using var cacheContext = new SourceCacheContext();
        var tasks = new List<Task<GetVulnerabilityInfoResult?>>(auditSources.Count);
        foreach (var auditSource in auditSources)
        {
            tasks.Add(Task.Run(async () =>
            {
                var sourceRepository = Repository.Factory.GetCoreV3(auditSource.Source);
                var vulnerabilityResource = await sourceRepository.GetResourceAsync<IVulnerabilityInfoResource>(cancellationToken);
                if (vulnerabilityResource is not null)
                {
                    var vulnerabilities = await vulnerabilityResource.GetVulnerabilityInfoAsync(cacheContext, remappedLogger, cancellationToken);
                    return vulnerabilities;
                }
                return null;
            }, cancellationToken));
        }

        List<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>> allVulnerabilities = new();
        foreach (var task in tasks)
        {
            var result = await task;
            if (result is not null)
            {
                if (result.KnownVulnerabilities?.Count > 0)
                {
                    foreach (var vulnDict in result.KnownVulnerabilities)
                    {
                        allVulnerabilities.Add(vulnDict);
                    }
                }
            }
        }

        return allVulnerabilities;
    }

    private static async Task<LockFile> GetProjectAssetsFileAsync(
        DependencyGraphSpec dgSpec,
        ILoggerWithColor logger,
        IPackageUpdateIO packageUpdateIO,
        CancellationToken cancellationToken)
    {
        var previewRestoreResult = await packageUpdateIO.PreviewUpdatePackageReferenceAsync(dgSpec, new SourceCacheContext(), NullLogger.Instance, cancellationToken);
        await packageUpdateIO.CommitAsync(previewRestoreResult, cancellationToken);
        if (!previewRestoreResult.Success)
        {
            logger.LogError("Restore failed");
            throw new NotSupportedException();
        }

        LockFile assetsFile;
        if (previewRestoreResult.AssetsFile is not null)
        {
            assetsFile = previewRestoreResult.AssetsFile;
        }
        else
        {
            var packageSpec = dgSpec.GetProjectSpec(dgSpec.Restore.Single());
            var assetsFilePath = Path.Combine(packageSpec.RestoreMetadata.OutputPath, LockFileFormat.AssetsFileName);
            assetsFile = new LockFileFormat().Read(assetsFilePath);
        }

        return assetsFile;
    }

    internal static async Task<List<PackageUpdateResult>> SelectPackagesToUpdateAsync(
        IReadOnlyList<Package> packages,
        PackageSpec project,
        IVersionChooser versionChooser,
        ISettings settings,
        ILoggerWithColor logger,
        CancellationToken cancellationToken)
    {
        if (packages is null || packages.Count == 0)
        {
            return await SelectAllPackagesWithUpdatesAsync(project, versionChooser, settings, logger, cancellationToken);
        }

        var sourceMapping = PackageSourceMapping.GetPackageSourceMapping(settings);
        if (sourceMapping.IsEnabled)
        {
            logger.LogMinimal(Strings.Unsupported_PackageSourceMapping, ConsoleColor.Red);
            return new List<PackageUpdateResult>();
        }

        var packagesToUpdate = new List<PackageUpdateResult>();
        bool hasErrors = false;

        foreach (var package in packages)
        {
            var result = await GetSinglePackageToUpdateAsync(package, project, versionChooser, settings, logger, suppressWarnings: false, cancellationToken);
            if (result != null)
            {
                packagesToUpdate.Add(result);
            }
            else
            {
                hasErrors = true;
            }
        }

        return hasErrors ? new List<PackageUpdateResult>() : packagesToUpdate;
    }

    private static async Task<List<PackageUpdateResult>> SelectAllPackagesWithUpdatesAsync(
        PackageSpec project,
        IVersionChooser versionChooser,
        ISettings settings,
        ILoggerWithColor logger,
        CancellationToken cancellationToken)
    {
        var sourceMapping = PackageSourceMapping.GetPackageSourceMapping(settings);
        if (sourceMapping.IsEnabled)
        {
            logger.LogMinimal(Strings.Unsupported_PackageSourceMapping, ConsoleColor.Red);
            return new List<PackageUpdateResult>();
        }

        var allProjectPackages = GetAllPackagesReferencedByProject(project);
        var packagesToUpdate = new List<PackageUpdateResult>();

        foreach (var package in allProjectPackages)
        {
            // GetAllPackagesReferencedByProject returns packages with the requested version, so clear it since we want to find the highest version.
            var packageToCheck = package with { VersionRange = null };
            var result = await GetSinglePackageToUpdateAsync(packageToCheck, project, versionChooser, settings, logger, suppressWarnings: true, cancellationToken);
            // GetSinglePackageToUpdateAsync returns null if the package is already using the highest version.
            // This is not an error when updating all packages in a project.
            if (result != null)
            {
                packagesToUpdate.Add(result);
            }
        }

        return packagesToUpdate;
    }

    private static List<Package> GetAllPackagesReferencedByProject(PackageSpec project)
    {
        var allPackageIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var allPackages = new List<Package>();

        foreach (var tfm in project.TargetFrameworks)
        {
            foreach (var dependency in tfm.Dependencies)
            {
                // Skip auto-referenced packages and packages without explicit names
                if (!string.IsNullOrEmpty(dependency.Name) && !dependency.AutoReferenced)
                {
                    if (allPackageIds.Add(dependency.Name))
                    {
                        allPackages.Add(new Package
                        {
                            Id = dependency.Name,
                            VersionRange = dependency.LibraryRange.VersionRange
                        });
                    }
                }
            }
        }

        return allPackages;
    }

    private static async Task<PackageUpdateResult?> GetSinglePackageToUpdateAsync(
        Package package,
        PackageSpec project,
        IVersionChooser versionChooser,
        ISettings settings,
        ILoggerWithColor logger,
        bool suppressWarnings,
        CancellationToken cancellationToken)
    {
        VersionRange? existingVersion = null;
        List<string> frameworks = new();

        foreach (var tfm in project.TargetFrameworks)
        {
            foreach (var dependency in tfm.Dependencies)
            {
                if (string.Equals(package.Id, dependency.Name, StringComparison.OrdinalIgnoreCase))
                {
                    frameworks.Add(tfm.TargetAlias);

                    VersionRange tfmVersionRange;
                    if (project.RestoreMetadata.CentralPackageFloatingVersionsEnabled)
                    {
                        if (!tfm.CentralPackageVersions.TryGetValue(
                            package.Id,
                            out CentralPackageVersion? centralVersion))
                        {
                            logger.LogMinimal(
                                Messages.Error_CouldNotFindPackageVersionForCpmPackage(package.Id),
                                ConsoleColor.Red);
                            return null;
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
                                Messages.Unsupported_UpdatePackageWithDifferentPerTfmVersions(package.Id, project.FilePath),
                                ConsoleColor.Red);
                            return null;
                        }
                    }
                }
            }
        }

        if (existingVersion is null)
        {
            if (!suppressWarnings)
            {
                logger.LogMinimal(Messages.Error_PackageNotReferenced(package.Id, project.FilePath), ConsoleColor.Red);
            }
            return null;
        }

        VersionRange newVersion;
        if (package.VersionRange is null)
        {
            // NuGet.Protocol outputs request and response info at normal verbosity, which update doesn't want.
            var protocolLogger = new RemappedLevelLogger(
                logger,
                new RemappedLevelLogger.Mapping
                {
                    Information = LogLevel.Verbose,
                });

            NuGetVersion? highestVersion = await versionChooser.GetLatestVersionAsync(
                package.Id,
                protocolLogger,
                cancellationToken);

            if (highestVersion is null)
            {
                if (!suppressWarnings)
                {
                    logger.LogMinimal(Messages.Error_NoVersionsAvailable(package.Id), ConsoleColor.Red);
                }
                return null;
            }

            if (existingVersion.MinVersion == highestVersion)
            {
                if (!suppressWarnings)
                {
                    logger.LogMinimal(Messages.Warning_AlreadyHighestVersion(package.Id, highestVersion.OriginalVersion, project.FilePath), ConsoleColor.Yellow);
                }
                return null;
            }

            newVersion = new VersionRange(highestVersion);
        }
        else
        {
            newVersion = package.VersionRange;
            if (newVersion == existingVersion)
            {
                if (!suppressWarnings)
                {
                    logger.LogMinimal(Messages.Warning_AlreadyUsingSameVersion(package.Id, newVersion.OriginalString), ConsoleColor.Yellow);
                }
                return null;
            }
        }

        var packageToUpdate = new PackageToUpdate
        {
            Id = package.Id,
            CurrentVersion = existingVersion,
            NewVersion = newVersion
        };

        return new PackageUpdateResult
        {
            Package = packageToUpdate,
            TargetFrameworkAliases = frameworks!
        };
    }

    private static DependencyGraphSpec GetUpdatedDependencyGraphSpec(DependencyGraphSpec currentDgSpec, List<PackageUpdateResult> packagesToUpdate)
    {
        var updatedPackageSpec = currentDgSpec.Projects[0].Clone();

        foreach (var packageResult in packagesToUpdate)
        {
            PackageDependency packageDependency = new PackageDependency(packageResult.Package.Id, packageResult.Package.NewVersion);
            PackageSpecOperations.AddOrUpdateDependency(updatedPackageSpec, packageDependency);
        }

        var updatedDgSpec = currentDgSpec.WithReplacedSpec(updatedPackageSpec).WithoutRestores();
        updatedDgSpec.AddRestore(updatedPackageSpec.RestoreMetadata.ProjectUniqueName);

        return updatedDgSpec;
    }

    private static bool NuGetAuditEnabled(PackageSpec projectSpec) =>
        bool.TryParse(projectSpec?.RestoreMetadata?.RestoreAuditProperties?.EnableAudit, out bool result)
            ? result
            : true;

    private static bool IsNuGetAuditModeSetToAll(PackageSpec projectSpec) =>
        string.Equals(projectSpec?.RestoreMetadata?.RestoreAuditProperties?.AuditMode, "all", StringComparison.OrdinalIgnoreCase);

    internal record PackageToUpdate
    {
        public required string Id { get; init; }
        public required VersionRange CurrentVersion { get; init; }
        public required VersionRange NewVersion { get; init; }
    }

    internal record PackageUpdateResult
    {
        public required PackageToUpdate Package { get; init; }

        public required List<string> TargetFrameworkAliases { get; init; }
    }

    private static class Format
    {
        internal static string PackageUpdate_UpdatingOutdatedPackages(string projectPath)
        {
            return string.Format(CultureInfo.CurrentCulture, Strings.PackageUpdate_UpdatingOutdatedPackages, projectPath);
        }

        internal static string PackageUpdate_UpdatingVulnerablePackages(string projectPath)
        {
            return string.Format(CultureInfo.CurrentCulture, Strings.PackageUpdate_UpdatingVulnerablePackages, projectPath);
        }

        internal static string PackageUpdate_UpdatedMessage(string packageId, string currentVersion, string newVersion)
        {
            return string.Format(CultureInfo.CurrentCulture, Strings.PackageUpdate_UpdatedMessage, packageId, currentVersion, newVersion);
        }

        internal static string PackageUpdate_FinalSummary(int updatedCount, int scannedCount)
        {
            return string.Format(CultureInfo.CurrentCulture, Strings.PackageUpdate_FinalSummary, updatedCount, scannedCount);
        }

        internal static string PackageUpdate_AllVersionsHaveAdvisories(string packageId)
        {
            return string.Format(CultureInfo.CurrentCulture, Strings.PackageUpdate_AllVersionsHaveAdvisories, packageId);
        }
    }

    // These exit codes are documented, so consider changing them or adding new ones a breaking change.
    internal static class ExitCodes
    {
        public const int Success = 0;
        // System.CommandLine returns 1 on parse ererors, so even if this const isn't used, the value 1 is still returned.
        public const int InvalidArgs = 1;
        public const int NoPackagesNeedUpdating = 2;
        public const int Error = 3;
    }
}
