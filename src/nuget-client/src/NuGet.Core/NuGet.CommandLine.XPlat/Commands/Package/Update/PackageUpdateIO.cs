// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.CommandLine.XPlat.Utility;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.ProjectModel;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Protocol.Model;
using NuGet.Shared;
using NuGet.Versioning;
using static NuGet.CommandLine.XPlat.Commands.Package.Update.PackageUpdateCommandRunner;

namespace NuGet.CommandLine.XPlat.Commands.Package.Update;

/// <summary>
/// Implementation of IPackageUpdateIO that handles package updates by performing restore operations.
/// </summary>
internal class PackageUpdateIO : IPackageUpdateIO, IDisposable
{
    private readonly MSBuildAPIUtility _msbuildUtility;
    private readonly IEnvironmentVariableReader _environmentVariableReader;
    private readonly ISettings _settings;
    private readonly IPackageSourceProvider _sourceProvider;
    private readonly CachingSourceProvider _cachingSourceProvider;
    private readonly IReadOnlyList<PackageSource> _enabledSources;
    private readonly SourceCacheContext _sourceCacheContext;

    public PackageUpdateIO(string solutionDirectory, MSBuildAPIUtility msbuildUtility, IEnvironmentVariableReader environmentVariableReader)
    {
        _msbuildUtility = msbuildUtility;
        _environmentVariableReader = environmentVariableReader;

        // the CommandLine option validates that an existing filesystem object is provided, so we can be confident that
        // we either have a directory or a file here.
        string settingsRoot = Directory.Exists(solutionDirectory) ? solutionDirectory : Path.GetDirectoryName(solutionDirectory)!;
        _settings = Settings.LoadDefaultSettings(solutionDirectory);

        _sourceProvider = new PackageSourceProvider(_settings);
        _cachingSourceProvider = new CachingSourceProvider(_sourceProvider);
        _enabledSources = SettingsUtility.GetEnabledSources(_settings).AsList();
        _sourceCacheContext = new SourceCacheContext();
    }

    public void Dispose()
    {
        _sourceCacheContext.Dispose();
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc cref="IPackageUpdateIO.GetDependencyGraphSpec(string)"/>
    public DependencyGraphSpec? GetDependencyGraphSpec(string project)
    {
        string tempFile = Path.GetTempFileName();
        try
        {
            if (!RunMsbuildTarget(project, tempFile))
            {
                return null;
            }

            DependencyGraphSpec result = DependencyGraphSpec.Load(tempFile);

            return result;
        }
        finally
        {
            File.Delete(tempFile);
        }

        bool RunMsbuildTarget(string project, string tempFile)
        {
            // When being run from the dotnet CLI, use the same dotnet executable, just in case the dotnet on the PATH is different
            // But when NuGet.CommandLine.XPlat is being called directly, call dotnet on the path, so this code is debuggable.
            string dotnetPath = _environmentVariableReader.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? "dotnet";

            // don't redirect stdout or stderr, so errors are output. But use quiet verbosity, so that success has no output.
            ProcessStartInfo processStartInfo = new ProcessStartInfo(dotnetPath)
            {
                Arguments = $"msbuild " +
                $"\"{project}\" " +
                $"-restore:false " +
                $"-target:GenerateRestoreGraphFile " +
                $"-property:RestoreGraphOutputPath=\"{tempFile}\" " +
                $"-property:RestoreRecursive=false " +
                $"-nologo " +
                $"-verbosity:quiet " +
                $"-tl:false " +
                $"-noautoresponse",
                UseShellExecute = false,
            };

            using var process = Process.Start(processStartInfo);
            if (process is null) throw new System.Exception("Unexpected error starting child process. Process.Start returned null.");
            process.WaitForExit();

            return process.ExitCode == 0;
        }
    }

    /// <inheritdoc cref="IPackageUpdateIO.PreviewUpdatePackageReferenceAsync(DependencyGraphSpec, ILogger, CancellationToken)"/>
    public async Task<IPackageUpdateIO.RestoreResult> PreviewUpdatePackageReferenceAsync(
        DependencyGraphSpec dgSpec,
        ILogger logger,
        CancellationToken cancellationToken)
    {
        var providerCache = new RestoreCommandProvidersCache();

        // Restore outputs a lot of messages at normal verbosity, which update doesn't want.
        var restoreLogger = new RemappedLevelLogger(
            logger,
            new RemappedLevelLogger.Mapping
            {
                Information = LogLevel.Verbose,
                Minimal = LogLevel.Verbose,
            });

        // Pre-loaded request provider containing the graph file
        var providers = new List<IPreLoadedRestoreRequestProvider>
            {
                new DependencyGraphSpecRequestProvider(providerCache, dgSpec)
            };

        var globalPackagesFolder = dgSpec.GetProjectSpec(dgSpec.Restore.Single()).RestoreMetadata.PackagesPath;

        var restoreContext = new RestoreArgs()
        {
            CacheContext = _sourceCacheContext,
            Log = restoreLogger,
            MachineWideSettings = new XPlatMachineWideSetting(),
            GlobalPackagesFolder = globalPackagesFolder,
            PreLoadedRequestProviders = providers
            // Sources : No need to pass it, because SourceRepositories contains the already built SourceRepository objects
        };

        // Generate Restore Requests. There will always be 1 request here since we are restoring for 1 project.
        var restoreRequests = await RestoreRunner.GetRequests(restoreContext);

        // Run restore without commit. This will always return 1 Result pair since we are restoring for 1 request.
        var restoreResult = await RestoreRunner.RunWithoutCommitAsync(restoreRequests, restoreContext, cancellationToken);

        var result = new RestoreResult
        {
            RestoreResultPair = restoreResult.Single()
        };
        return result;
    }

    /// <inheritdoc cref="IPackageUpdateIO.CommitAsync(IPackageUpdateIO.RestoreResult, CancellationToken)"/>
    public async Task CommitAsync(IPackageUpdateIO.RestoreResult restorePreviewResult, CancellationToken none)
    {
        await RestoreRunner.CommitAsync(((RestoreResult)restorePreviewResult).RestoreResultPair, CancellationToken.None);
    }

    /// <inheritdoc cref="IPackageUpdateIO.UpdatePackageReference(PackageSpec, IPackageUpdateIO.RestoreResult, List{string}, PackageToUpdate, ILogger)"/>
    public void UpdatePackageReference(PackageSpec updatedPackageSpec, IPackageUpdateIO.RestoreResult restorePreviewResult, List<string> packageTfmAliases, PackageToUpdate packageToUpdate, ILogger logger)
    {
        PackageDependency packageDependency = new PackageDependency(packageToUpdate.Id, packageToUpdate.NewVersion);

        List<NuGetFramework> packageTfms = new List<NuGetFramework>(packageTfmAliases.Count);
        foreach (var alias in packageTfmAliases)
        {
            var targetFramework = updatedPackageSpec.TargetFrameworks.Single(tfm => tfm.TargetAlias == alias);
            packageTfms.Add(targetFramework.FrameworkName);
        }

        if (!AddPackageReferenceCommandRunner.TryFindResolvedVersion(packageTfms,
            packageDependency.Id,
            ((RestoreResult)restorePreviewResult).RestoreResultPair.Result, logger, out NuGetVersion resolvedVersion))
        {
            return;
        }

        // Generate the LibraryDependency using the same logic as AddPackageReferenceCommandRunner
        var libraryDependency = AddPackageReferenceCommandRunner.GenerateLibraryDependency(
            updatedPackageSpec,
            customPackagesPath: null,
            packageDependency,
            resolvedVersion);

        // MSBuildUtility only updated CPM Directory.Packages.props when "noVersion" is false.
        const bool noVersion = false;

        // Determine whether to add package reference conditionally or unconditionally
        if (packageTfms.Count == updatedPackageSpec.TargetFrameworks.Count)
        {
            // package is used by all project TFMs (no condition)
            _msbuildUtility.AddPackageReference(updatedPackageSpec.FilePath, libraryDependency, noVersion);
        }
        else
        {
            var frameworkAliases = packageTfms
                .Select(e => AddPackageReferenceCommandRunner.GetAliasForFramework(updatedPackageSpec, e))
                .Where(originalFramework => originalFramework != null);

            _msbuildUtility.AddPackageReferencePerTFM(updatedPackageSpec.FilePath, libraryDependency, frameworkAliases, noVersion);
        }
    }

    /// <inheritdoc cref="IPackageUpdateIO.GetLatestVersionAsync(string, bool, IReadOnlyList{string}?, ILogger, CancellationToken)"/>
    public async Task<NuGetVersion?> GetLatestVersionAsync(
        string packageId,
        bool includePrerelease,
        IReadOnlyList<string>? allowedSources,
        ILogger logger,
        CancellationToken cancellationToken)
    {
        var sources = GetSourcesForPackage(packageId, allowedSources);
        var lookups = new Task<NuGetVersion?>[sources.Count];
        for (int source = 0; source < sources.Count; source++)
        {
            SourceRepository sourceRepository = sources[source];
            // If package source is a local folder feed, it might not actually be async
            lookups[source] = Task.Run(() => FindHighestPackageVersionAsync(sourceRepository, packageId, includePrerelease, logger, cancellationToken));
        }

        await Task.WhenAll(lookups);

        NuGetVersion? highestVersion = null;
        foreach (var task in lookups)
        {
            if (task.Result != null)
            {
                if (highestVersion == null || task.Result > highestVersion)
                {
                    highestVersion = task.Result;
                }
            }
        }

        return highestVersion;
    }

    /// <inheritdoc cref="IPackageUpdateIO.GetKnownVulnerabilitiesAsync(ILogger, CancellationToken)"/>
    public async Task<IReadOnlyList<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>>> GetKnownVulnerabilitiesAsync(ILogger logger, CancellationToken cancellationToken)
    {
        IReadOnlyList<PackageSource>? auditSources = _sourceProvider.LoadAuditSources()?.Where(s => s.IsEnabled).ToList();
        if (auditSources is null || auditSources.Count == 0)
        {
            auditSources = _enabledSources;
        }

        var tasks = new List<Task<GetVulnerabilityInfoResult?>>(auditSources.Count);
        foreach (var auditSource in auditSources)
        {
            tasks.Add(Task.Run(async () =>
            {
                var sourceRepository = Repository.Factory.GetCoreV3(auditSource.Source);
                var vulnerabilityResource = await sourceRepository.GetResourceAsync<IVulnerabilityInfoResource>(cancellationToken);
                if (vulnerabilityResource is not null)
                {
                    var vulnerabilities = await vulnerabilityResource.GetVulnerabilityInfoAsync(_sourceCacheContext, logger, cancellationToken);
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

    /// <inheritdoc cref="IPackageUpdateIO.GetNonVulnerableAsync(string, IReadOnlyList{string}?, NuGetVersion, ILogger, IReadOnlyList{IReadOnlyDictionary{string, IReadOnlyList{PackageVulnerabilityInfo}}}, CancellationToken)"/>
    public async Task<NuGetVersion?> GetNonVulnerableAsync(
        string packageId,
        IReadOnlyList<string>? allowedSources,
        NuGetVersion minVersion,
        ILogger logger,
        IReadOnlyList<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>> knownVulnerabilities,
        CancellationToken cancellationToken)
    {
        var sources = GetSourcesForPackage(packageId, allowedSources);
        var lookups = new Task<NuGetVersion?>[sources.Count];
        for (int source = 0; source < sources.Count; source++)
        {
            SourceRepository sourceRepository = sources[source];
            // If package source is a local folder feed, it might not actually be async
            lookups[source] = Task.Run(() => FindLowestNonVulnerablePackageVersionAsync(sourceRepository, packageId, minVersion, knownVulnerabilities, logger, cancellationToken));
        }

        await Task.WhenAll(lookups);

        NuGetVersion? lowestNonVulnerableVersion = null;
        foreach (var task in lookups)
        {
            if (task.Result != null)
            {
                if (lowestNonVulnerableVersion == null || task.Result < lowestNonVulnerableVersion)
                {
                    lowestNonVulnerableVersion = task.Result;
                }
            }
        }

        return lowestNonVulnerableVersion;
    }

    public PackageSourceMapping GetPackageSourceMapping()
    {
        return PackageSourceMapping.GetPackageSourceMapping(_settings);
    }

    private List<SourceRepository> GetSourcesForPackage(string packageId, IReadOnlyList<string>? allowedSources)
    {
        IReadOnlyList<PackageSource> packageSources;

        // Apply package source mapping if enabled
        if (allowedSources is not null)
        {
            if (allowedSources.Count == 0)
            {
                throw new ArgumentException("The allowedSources list must contain at least one source if specified.", nameof(allowedSources));
            }

            List<PackageSource> sourceMappedSources = new List<PackageSource>(allowedSources.Count);
            sourceMappedSources.AddRange(_enabledSources.Where(ps => allowedSources.Contains(ps.Name, StringComparer.OrdinalIgnoreCase)));
            packageSources = sourceMappedSources;
        }
        else
        {
            packageSources = _enabledSources;
        }

        var sources = new List<SourceRepository>(packageSources.Count);
        for (int i = 0; i < packageSources.Count; i++)
        {
            SourceRepository sourceRepository = _cachingSourceProvider.CreateRepository(packageSources[i]);
            sources.Add(sourceRepository);
        }
        return sources;
    }

    private async Task<NuGetVersion?>? FindLowestNonVulnerablePackageVersionAsync(
        SourceRepository source,
        string packageId,
        NuGetVersion minVersion,
        IReadOnlyList<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>> knownVulnerabilities,
        ILogger logger,
        CancellationToken cancellationToken)
    {
        var packageMetadataResource = await source.GetResourceAsync<PackageMetadataResource>(cancellationToken);

        var packageDetails = await packageMetadataResource.GetMetadataAsync(
            packageId,
            includePrerelease: false,
            includeUnlisted: false,
            _sourceCacheContext,
            logger,
            cancellationToken);

        if (packageDetails is null || !packageDetails.Any())
        {
            return null;
        }

        var versions = packageDetails
            .Select(p => p.Identity)
            .Where(p => p.Version >= minVersion && !PackageHasKnownVulnerability(p))
            .Select(p => p.Version);

        VersionRange versionRange = new VersionRange(minVersion, includeMinVersion: true, maxVersion: null, includeMaxVersion: true);
        NuGetVersion? result = versionRange.FindBestMatch(versions);

        return result;

        bool PackageHasKnownVulnerability(PackageIdentity package)
        {
            foreach (var sourceVulnerabilities in knownVulnerabilities)
            {
                if (sourceVulnerabilities.TryGetValue(packageId, out var vulnerabilities))
                {
                    foreach (var vulnerability in vulnerabilities)
                    {
                        if (vulnerability.Versions.Satisfies(package.Version))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }

    private async Task<NuGetVersion?> FindHighestPackageVersionAsync(
        SourceRepository source,
        string packageId,
        bool includePrerelease,
        ILogger logger,
        CancellationToken cancellationToken)
    {
        var packageMetadataResource = await source.GetResourceAsync<PackageMetadataResource>(cancellationToken);

        var packageDetails = await packageMetadataResource.GetMetadataAsync(
            packageId,
            includePrerelease: includePrerelease,
            includeUnlisted: false,
            _sourceCacheContext,
            logger,
            cancellationToken);

        if (packageDetails is null || !packageDetails.Any())
        {
            return null;
        }

        NuGetVersion highestVersion = packageDetails.Max(p => p.Identity.Version)!;
        return highestVersion;
    }

    /// <inheritdoc cref="IPackageUpdateIO.GetProjectAssetsFileAsync(DependencyGraphSpec, ILogger, CancellationToken)"/>
    public async Task<LockFile> GetProjectAssetsFileAsync(
        DependencyGraphSpec dgSpec,
        ILogger logger,
        CancellationToken cancellationToken)
    {
        var previewRestoreResult = (RestoreResult)await PreviewUpdatePackageReferenceAsync(dgSpec, NullLogger.Instance, cancellationToken);
        if (!previewRestoreResult.Success)
        {
            logger.LogError("Restore failed");
            throw new NotSupportedException();
        }

        LockFile? assetsFile = previewRestoreResult.RestoreResultPair.Result.LockFile;
        if (assetsFile is null)
        {
            var packageSpec = dgSpec.GetProjectSpec(dgSpec.Restore.Single());
            var assetsFilePath = Path.Combine(packageSpec.RestoreMetadata.OutputPath, LockFileFormat.AssetsFileName);
            assetsFile = new LockFileFormat().Read(assetsFilePath);
        }

        return assetsFile;
    }

    private class RestoreResult : IPackageUpdateIO.RestoreResult
    {
        internal required RestoreResultPair RestoreResultPair { get; init; }

        public override bool Success => RestoreResultPair.Result.Success;
    }
}
