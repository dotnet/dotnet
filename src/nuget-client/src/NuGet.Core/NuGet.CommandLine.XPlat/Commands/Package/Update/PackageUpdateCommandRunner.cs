// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Configuration;
using NuGet.CommandLine.XPlat.Utility;
using NuGet.Common;
using NuGet.Credentials;
using NuGet.LibraryModel;
using NuGet.Packaging.Core;
using NuGet.ProjectModel;
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

        var restoreHelper = new PackageUpdateIO(args.Project, msBuild, EnvironmentVariableWrapper.Instance);

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

        PackageSpec projectSpec = dgSpec.GetProjectSpec(dgSpec.Restore[0]);

        // 2. Find suitable version of package(s) to update
        // Source provider will be needed to find the package version and to restore, so create it here.
        logger.LogVerbose(Strings.PackageUpdate_FindingUpdateVersions);

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

            (packagesToUpdateResult, totalPackagesScanned) = await SelectVulnerablePackagesToUpdateAsync(args.Packages, dgSpec, logger, packageUpdateIO, cancellationToken);

            if (packagesToUpdateResult.Count == 0)
            {
                logger.LogMinimal(Strings.PackageUpdate_NoVulnerablePackages, ConsoleColor.Green);
                return ExitCodes.Success;
            }
        }
        else if (noPackagesSpecified)
        {
            (var selectedPackages, totalPackagesScanned) = await SelectAllPackagesWithUpdatesAsync(projectSpec, logger, packageUpdateIO, cancellationToken);
            if (selectedPackages is null)
            {
                // SelectAllPackagesWithUpdatesAsync logs the error.
                return ExitCodes.Error;
            }
            else if (selectedPackages.Count == 0)
            {
                // When no packages are specified and all packages are already up to date, return exit code 2
                logger.LogMinimal(Strings.PackageUpdate_AlreadyUpToDate, ConsoleColor.Green);
                return ExitCodes.NoPackagesNeedUpdating;
            }
            packagesToUpdateResult = selectedPackages;
        }
        else
        {
            totalPackagesScanned = args.Packages!.Count;
            packagesToUpdateResult = await SelectPackagesToUpdateAsync(args.Packages, projectSpec, logger, packageUpdateIO, cancellationToken);
            if (packagesToUpdateResult.Count == 0)
            {
                // SelectPackagesToUpdateAsync logs the error.
                return ExitCodes.Error;
            }
        }

        var projectName = Path.GetFileNameWithoutExtension(projectSpec.FilePath);
        logger.LogInformation($"  {projectName}:");

        foreach (var packageResult in packagesToUpdateResult)
        {
            logger.LogInformation($"    " + Format.PackageUpdate_UpdatedMessage(packageResult.Package.Id, packageResult.Package.CurrentVersion.ToShortString(), packageResult.Package.NewVersion.ToShortString()));
        }

        // New line in between projects, or before the final summary.
        logger.LogInformation("");

        // 3. Preview restore to validate changes
        logger.LogDebug(Strings.PackageUpdate_PreviewRestore);
        var updatedDgSpec = GetUpdatedDependencyGraphSpec(projectSpec, dgSpec, packagesToUpdateResult);
        var restorePreviewResult = await packageUpdateIO.PreviewUpdatePackageReferenceAsync(updatedDgSpec, logger, cancellationToken);

        if (!restorePreviewResult.Success)
        {
            logger.LogMinimal(Strings.PackageUpdate_PreviewRestoreFailed, ConsoleColor.Red);
            return ExitCodes.Error;
        }

        // 4. Update MSBuild files

        var updatedPackageSpec = updatedDgSpec.GetProjectSpec(projectSpec.FilePath);
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
        IReadOnlyList<PackageWithVersionRange>? packages,
        DependencyGraphSpec dgSpec,
        ILoggerWithColor logger,
        IPackageUpdateIO packageUpdateIO,
        CancellationToken cancellationToken)
    {
        LockFile assetsFile = await packageUpdateIO.GetProjectAssetsFileAsync(dgSpec, logger, cancellationToken);

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
            var remappedLogger = new RemappedLevelLogger(
                logger,
                new RemappedLevelLogger.Mapping
                {
                    Information = LogLevel.Verbose,
                });
            IReadOnlyList<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>> knownVulnerabilities =
                await packageUpdateIO.GetKnownVulnerabilitiesAsync(remappedLogger, cancellationToken);

            List<(PackageIdentity package, List<string> targetFrameworkAliases)> packagesToUpdate = assetsFile
                .Targets
                .SelectMany(tf => tf.Libraries.Select(library => (tf.TargetFramework, library)))
                .Where(tuple => tuple.library.Type == "package" && packageIdsWithVulnerabilities.Contains(tuple.library.Name!) && PackageHasVulnerability(tuple.library.Name!, tuple.library.Version!, knownVulnerabilities))
                .GroupBy(
                    pair => new PackageIdentity(pair.library.Name, pair.library.Version),
                    pair => assetsFile.PackageSpec.TargetFrameworks.Single(tfm => tfm.FrameworkName == pair.TargetFramework).TargetAlias,
                    (key, g) => (key, g.Distinct().ToList()))
                .ToList();

            PackageSourceMapping sourceMapping = packageUpdateIO.GetPackageSourceMapping();
            var packagesToUpdateResult = new List<PackageUpdateResult>(packagesToUpdate.Count);
            foreach (var (packageIdentity, tfmAliases) in packagesToUpdate)
            {
                IReadOnlyList<string>? mappedSources = sourceMapping.IsEnabled ? sourceMapping.GetConfiguredPackageSources(packageIdentity.Id) : null;
                if (mappedSources is not null && mappedSources.Count == 0)
                {
                    logger.LogError(Messages.Error_PackageSourceMappingNotFound(packageIdentity.Id));
                    continue;
                }

                var nonVulnerableVersion = await packageUpdateIO.GetNonVulnerableAsync(packageIdentity.Id, mappedSources, packageIdentity.Version, NullLogger.Instance, knownVulnerabilities, cancellationToken);
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
                            NewVersion = VersionRange.Parse(nonVulnerableVersion.OriginalVersion!)
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

    internal static async Task<List<PackageUpdateResult>> SelectPackagesToUpdateAsync(
        IReadOnlyList<PackageWithVersionRange> packages,
        PackageSpec project,
        ILoggerWithColor logger,
        IPackageUpdateIO packageUpdateIO,
        CancellationToken cancellationToken)
    {
        if (packages is null || packages.Count == 0)
        {
            throw new ArgumentException(Strings.ArgumentNullOrEmpty, nameof(packages));
        }

        var sourceMapping = packageUpdateIO.GetPackageSourceMapping();
        var packagesToUpdate = new List<PackageUpdateResult>();
        bool hasErrors = false;

        foreach (var package in packages)
        {
            IReadOnlyList<string>? mappedSources = sourceMapping.IsEnabled ? sourceMapping.GetConfiguredPackageSources(package.Id) : null;
            if (mappedSources is not null && mappedSources.Count == 0)
            {
                logger.LogError(Messages.Error_PackageSourceMappingNotFound(package.Id));
                hasErrors = true;
                continue;
            }

            var (existingVersion, packageTfms) = GetReferencedVersion(package.Id, project, logger);
            if (existingVersion is null)
            {
                if (packageTfms.Count > 0)
                {
                    hasErrors = true;
                }
                continue;
            }

            VersionRange upgradeVersion;
            if (package.VersionRange is not null)
            {
                upgradeVersion = package.VersionRange;
                if (upgradeVersion == existingVersion)
                {
                    logger.LogMinimal(Messages.Warning_AlreadyUsingSameVersion(package.Id, upgradeVersion.OriginalString!), ConsoleColor.Yellow);
                    continue;
                }
            }
            else
            {
                bool usePrerelease = existingVersion.HasLowerBound && existingVersion.MinVersion.IsPrerelease;
                var latestVersion = await packageUpdateIO.GetLatestVersionAsync(package.Id, usePrerelease, mappedSources, NullLogger.Instance, cancellationToken);
                if (latestVersion is null)
                {
                    logger.LogMinimal(Messages.Error_NoVersionsAvailable(package.Id), ConsoleColor.Red);
                    hasErrors = true;
                    continue;
                }

                upgradeVersion = VersionRange.Parse(latestVersion.OriginalVersion!);
                if (upgradeVersion == existingVersion)
                {
                    logger.LogMinimal(Messages.Warning_AlreadyHighestVersion(package.Id, latestVersion.OriginalVersion!, project.FilePath), ConsoleColor.Yellow);
                    continue;
                }
            }

            var packageToUpdate = new PackageToUpdate
            {
                Id = package.Id,
                CurrentVersion = existingVersion,
                NewVersion = upgradeVersion
            };
            packagesToUpdate.Add(new PackageUpdateResult
            {
                Package = packageToUpdate,
                TargetFrameworkAliases = packageTfms
            });
        }

        return hasErrors ? [] : packagesToUpdate;
    }

    /// <summary>Gets the package's referenced version range and TFMs which it's referenced in.</summary>
    /// <param name="packageId">The package identifier to find.</param>
    /// <param name="project">The project's restore inputs.</param>
    /// <param name="logger">The logger to output errors.</param>
    /// <returns>
    /// <para>When the package is found and no problems occur, it returns the requested <see cref="VersionRange"/>
    /// and list of target framework aliases that the package is used in.</para>
    /// <para>If the <see cref="VersionRange"/> is null and the target framework list contains at least one value,
    /// then there /// was an error, which the method will have logged. Note that the target framework list
    /// may not be complete.</para>
    /// <para>When the version range is null and the target framework list is empty, it means that the project
    /// does not reference the package (and no error was logged).</para>
    /// </returns>
    private static (VersionRange? version, List<string> targetFrameworks)
        GetReferencedVersion(string packageId, PackageSpec project, ILoggerWithColor logger)
    {
        VersionRange? existingVersion = null;
        List<string> frameworks = new();

        foreach (var tfm in project.TargetFrameworks)
        {
            foreach (var dependency in tfm.Dependencies)
            {
                if (string.Equals(packageId, dependency.Name, StringComparison.OrdinalIgnoreCase))
                {
                    frameworks.Add(tfm.TargetAlias);

                    if (dependency.AutoReferenced)
                    {
                        logger.LogMinimal(
                            Messages.Error_CannotUpgradeAutoReferencedPackage(project.FilePath, packageId),
                            ConsoleColor.Red);
                        return (null, []);
                    }

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
                            return (null, []);
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
                            return (null, []);
                        }
                    }
                }
            }
        }

        return (existingVersion, frameworks);
    }

    internal static async Task<(List<PackageUpdateResult>? packagesToUpdate, int packagesInProject)> SelectAllPackagesWithUpdatesAsync(
        PackageSpec project,
        ILoggerWithColor logger,
        IPackageUpdateIO packageUpdateIO,
        CancellationToken cancellationToken)
    {
        var allProjectPackages = GetAllPackagesReferencedByProject(project, logger);
        if (allProjectPackages is null)
        {
            return (null, 0);
        }

        var sourceMapping = packageUpdateIO.GetPackageSourceMapping();
        var packagesToUpdate = new List<PackageUpdateResult>();
        bool successful = true;

        foreach (var package in allProjectPackages)
        {
            IReadOnlyList<string>? mappedSources = sourceMapping.IsEnabled ? sourceMapping.GetConfiguredPackageSources(package.identity.Id) : null;
            if (mappedSources is not null && mappedSources.Count == 0)
            {
                logger.LogError(Messages.Error_PackageSourceMappingNotFound(package.identity.Id));
                successful = false;
                continue;
            }

            // package.identity.VersionRange is the project's referenced version.
            Debug.Assert(package.identity.VersionRange != null);
            bool usePrerelease = package.identity.VersionRange.HasLowerBound && package.identity.VersionRange.MinVersion.IsPrerelease;
            var latestVersion = await packageUpdateIO.GetLatestVersionAsync(package.identity.Id, usePrerelease, mappedSources, NullLogger.Instance, cancellationToken);

            if (latestVersion is null)
            {
                logger.LogMinimal(Messages.Error_NoVersionsAvailable(package.identity.Id), ConsoleColor.Red);
                successful = false;
                continue;
            }

            var upgradeVersion = VersionRange.Parse(latestVersion.OriginalVersion!);
            if (upgradeVersion.ToString() == package.identity.VersionRange.ToString())
            {
                // Already using the highest version.
                continue;
            }

            var result = new PackageUpdateResult
            {
                Package = new PackageToUpdate
                {
                    Id = package.identity.Id,
                    CurrentVersion = package.identity.VersionRange,
                    NewVersion = upgradeVersion
                },
                TargetFrameworkAliases = package.tfms
            };
            packagesToUpdate.Add(result);
        }

        return successful ? (packagesToUpdate, allProjectPackages.Count) : (null, allProjectPackages.Count);
    }

    private static List<(PackageWithVersionRange identity, List<string> tfms)>? GetAllPackagesReferencedByProject(PackageSpec project, ILoggerWithColor logger)
    {
        var allPackages = new Dictionary<string, (VersionRange version, List<string> tfms, bool hasError)>(StringComparer.OrdinalIgnoreCase);
        bool hasErrors = false;

        foreach (var tfm in project.TargetFrameworks)
        {
            foreach (var dependency in tfm.Dependencies)
            {
                if (!string.IsNullOrEmpty(dependency.Name) && !dependency.AutoReferenced)
                {
                    if (allPackages.TryGetValue(dependency.Name, out var existing))
                    {
                        if (existing.hasError)
                        {
                            continue;
                        }

                        if (existing.Item1 != dependency.LibraryRange.VersionRange)
                        {
                            logger.LogMinimal(
                                Messages.Unsupported_UpdatePackageWithDifferentPerTfmVersions(dependency.Name, project.FilePath),
                                ConsoleColor.Red);
                            hasErrors = true;
                            allPackages[dependency.Name] = (existing.version, existing.tfms, hasError: true);
                        }

                        existing.tfms.Add(tfm.TargetAlias);
                    }
                    else
                    {
                        VersionRange version = dependency.LibraryRange.VersionRange ?? VersionRange.All;
                        List<string> tfms = [tfm.TargetAlias];
                        allPackages[dependency.Name] = (version, tfms, hasError: false);
                    }
                }
            }
        }

        if (hasErrors)
        {
            return null;
        }

        List<(PackageWithVersionRange package, List<string> tfms)> result = new(allPackages.Count);
        foreach (var kvp in allPackages)
        {
            var package = new PackageWithVersionRange { Id = kvp.Key, VersionRange = kvp.Value.version };
            result.Add((package, kvp.Value.tfms));
        }

        return result;
    }

    private static DependencyGraphSpec GetUpdatedDependencyGraphSpec(PackageSpec projectSpec, DependencyGraphSpec currentDgSpec, List<PackageUpdateResult> packagesToUpdate)
    {
        var updatedPackageSpec = projectSpec.Clone();

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
