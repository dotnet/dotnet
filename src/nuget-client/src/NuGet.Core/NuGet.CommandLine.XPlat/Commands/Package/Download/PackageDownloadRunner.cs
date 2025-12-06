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
using NuGet.Commands;
using NuGet.Configuration;
using NuGet.Credentials;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Packaging.PackageExtraction;
using NuGet.Packaging.Signing;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Repositories;
using NuGet.Versioning;

namespace NuGet.CommandLine.XPlat.Commands.Package.PackageDownload
{
    internal static class PackageDownloadRunner
    {
        internal const int ExitCodeError = 1;
        internal const int ExitCodeSuccess = 0;

        public static async Task<int> RunAsync(PackageDownloadArgs args, CancellationToken token)
        {
            ILoggerWithColor logger = new CommandOutputLogger(args.LogLevel)
            {
                HidePrefixForInfoAndMinimal = true
            };

            XPlatUtility.ConfigureProtocol();
            DefaultCredentialServiceUtility.SetupDefaultCredentialService(logger, !args.Interactive);
            ISettings settings = Settings.LoadDefaultSettings(
                Directory.GetCurrentDirectory(),
                args.ConfigFile,
                new XPlatMachineWideSetting());
            IReadOnlyList<PackageSource> packageSources = GetPackageSources(args.Sources, new PackageSourceProvider(settings));

            return await RunAsync(args, logger, packageSources, settings, token);
        }

        public static async Task<int> RunAsync(PackageDownloadArgs args, ILoggerWithColor logger, IReadOnlyList<PackageSource> packageSources, ISettings settings, CancellationToken token)
        {
            bool hasSourcesArg = args.Sources?.Count > 0;
            PackageSourceMapping? packageSourceMapping = null;
            if (!hasSourcesArg)
            {
                packageSourceMapping = PackageSourceMapping.GetPackageSourceMapping(settings);
            }

            bool ignorePackageSourceMapping =
                hasSourcesArg
                || packageSourceMapping is null
                || !packageSourceMapping.IsEnabled;

            // When package source mapping is disabled, validate all configured sources upfront.
            // When mapping is enabled, source validation is deferred to the per-package resolution step,
            // since each package may map to a different subset of sources.
            if (ignorePackageSourceMapping && DetectAndReportInsecureSources(args.AllowInsecureConnections, packageSources, logger))
            {
                return ExitCodeError;
            }

            string outputDirectory = args.OutputDirectory ?? Directory.GetCurrentDirectory();
            var cache = new SourceCacheContext();
            IReadOnlyList<SourceRepository> allRepositories = GetSourceRepositories(packageSources);
            bool downloadedAllSuccessfully = true;

            foreach (var package in args.Packages ?? [])
            {
                logger.LogMinimal(string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.PackageDownloadCommand_Starting,
                    package.Id,
                    string.IsNullOrEmpty(package.NuGetVersion?.ToNormalizedString()) ? Strings.PackageDownloadCommand_LatestVersion : package.NuGetVersion.ToNormalizedString()));

                // Resolve which repositories to use for this package
                IReadOnlyList<SourceRepository> sourceRepositories;
                if (ignorePackageSourceMapping)
                {
                    sourceRepositories = allRepositories;
                }
                else
                {
                    var mappedNames = packageSourceMapping!.GetConfiguredPackageSources(package.Id);

                    if (mappedNames.Count == 0)
                    {
                        // fail, no sources mapped for this package
                        var notConsideredSources = string.Join(
                            ", ",
                            allRepositories.Select(repository => repository.PackageSource));

                        logger.LogError(string.Format(
                            CultureInfo.CurrentCulture,
                            Strings.PackageDownloadCommand_PackageSourceMapping_NoSourcesMapped,
                            package.Id,
                            notConsideredSources));

                        downloadedAllSuccessfully &= false;
                        continue;
                    }

                    sourceRepositories = GetMappedRepositories(mappedNames, allRepositories, package.Id, logger);

                    if (DetectAndReportInsecureSources(args.AllowInsecureConnections, sourceRepositories.Select(r => r.PackageSource), logger))
                    {
                        downloadedAllSuccessfully &= false;
                        continue;
                    }
                }

                try
                {
                    (NuGetVersion? version, SourceRepository? downloadRepository) =
                        await ResolvePackageDownloadVersion(
                            package,
                            sourceRepositories,
                            cache,
                            logger,
                            args.IncludePrerelease,
                            token);

                    if (version == null)
                    {
                        // Unable to find a valid version
                        downloadedAllSuccessfully &= false;
                        continue;
                    }

                    bool success = await DownloadPackageAsync(
                        package.Id,
                        version,
                        downloadRepository!,
                        cache,
                        settings,
                        outputDirectory,
                        logger,
                        token);

                    if (success)
                    {
                        logger.LogMinimal(string.Format(
                            CultureInfo.CurrentCulture,
                            Strings.PackageDownloadCommand_Succeeded,
                            package.Id,
                            version,
                            outputDirectory));
                    }
                    else
                    {
                        logger.LogError(string.Format(
                            CultureInfo.CurrentCulture,
                            Strings.PackageDownloadCommand_Failed,
                            package.Id,
                            version));

                        downloadedAllSuccessfully &= false;
                    }
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception ex)
                {
                    logger.LogError(ex.ToString());
                    downloadedAllSuccessfully &= false;
                }
#pragma warning restore CA1031 // Do not catch general exception types
            }

            return downloadedAllSuccessfully ? ExitCodeSuccess : ExitCodeError;
        }

        internal static async Task<(NuGetVersion?, SourceRepository?)> ResolvePackageDownloadVersion(
            PackageWithNuGetVersion packageWithNuGetVersion,
            IReadOnlyList<SourceRepository> sourceRepositories,
            SourceCacheContext cache,
            ILoggerWithColor logger,
            bool includePrerelease,
            CancellationToken token)
        {
            NuGetVersion? versionToDownload = null;
            SourceRepository? downloadSourceRepository = null;
            bool versionSpecified = packageWithNuGetVersion.NuGetVersion != null;

            foreach (var repo in sourceRepositories)
            {
                var finder = await repo.GetResourceAsync<PackageMetadataResource>(token);
                var packages = await finder.GetMetadataAsync(
                    packageWithNuGetVersion.Id,
                    includePrerelease,
                    includeUnlisted: versionSpecified, // only load unlisted if an exact version is specified
                    sourceCacheContext: cache,
                    logger,
                    token);

                if (packages == null)
                {
                    continue;
                }

                if (versionSpecified)
                {
                    // If an exact version is specified, check if it exists at this source
                    foreach (var package in packages)
                    {
                        if (package?.Identity?.Version == packageWithNuGetVersion.NuGetVersion)
                        {
                            return (packageWithNuGetVersion.NuGetVersion, repo);
                        }
                    }

                    continue;
                }

                foreach (var package in packages)
                {
                    var version = package.Identity.Version;
                    if (versionToDownload == null || version > versionToDownload)
                    {
                        versionToDownload = version;
                        downloadSourceRepository = repo;
                    }
                }
            }

            if (versionToDownload == null)
            {
                logger.LogError(Strings.Error_PackageDownload_VersionNotFound);
            }

            return (versionToDownload, downloadSourceRepository);
        }

        internal static IReadOnlyList<SourceRepository> GetMappedRepositories(
            IReadOnlyList<string> mappedNames,
            IReadOnlyList<SourceRepository> allRepos,
            string packageId,
            ILoggerWithColor logger)
        {
            var mappedRepos = new List<SourceRepository>(mappedNames.Count);

            foreach (var mappedName in mappedNames)
            {
                SourceRepository? repo = FindRepositoryByName(mappedName, allRepos);

                if (repo != null)
                {
                    mappedRepos.Add(repo);
                }
                else
                {
                    logger.LogVerbose(
                        string.Format(
                            CultureInfo.CurrentCulture,
                            Strings.PackageDownloadCommand_PackageSourceMapping_NoSuchSource,
                            mappedName,
                            packageId));
                }
            }

            return mappedRepos;
        }

        private static SourceRepository? FindRepositoryByName(
            string mappedName,
            IReadOnlyList<SourceRepository> allRepos)
        {
            for (int i = 0; i < allRepos.Count; i++)
            {
                if (string.Equals(allRepos[i].PackageSource.Name, mappedName, StringComparison.OrdinalIgnoreCase))
                {
                    return allRepos[i];
                }
            }

            return null;
        }

        private static async Task<bool> DownloadPackageAsync(
            string id,
            NuGetVersion version,
            SourceRepository repo,
            SourceCacheContext cache,
            ISettings settings,
            string outputDirectory,
            Common.ILogger logger,
            CancellationToken token)
        {
            var extractionContext = new PackageExtractionContext(
                PackageSaveMode.Defaultv3,
                PackageExtractionBehavior.XmlDocFileSaveMode,
                ClientPolicyContext.GetClientPolicy(settings, logger),
                logger);

            var resolver = new VersionFolderPathResolver(outputDirectory);
            var userPackageFolder = new NuGetv3LocalRepository(outputDirectory);

            // no-op if already installed
            if (userPackageFolder.Exists(id, version))
            {
                logger.LogMinimal(string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.PackageDownloadCommand_AlreadyInstalled,
                    id,
                    version.ToNormalizedString(),
                    outputDirectory));

                return true;
            }

            var packageIdentity = new PackageIdentity(id, version);
            var provider = new SourceRepositoryDependencyProvider(sourceRepository: repo, logger: logger, cacheContext: cache, ignoreFailedSources: false, ignoreWarning: false);
            using var downloader = await provider.GetPackageDownloaderAsync(packageIdentity, cache, logger, token);
            bool success = await PackageExtractor.InstallFromSourceAsync(packageIdentity, downloader, resolver, extractionContext, token);

            if (!success)
            {
                logger.LogError(string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.PackageDownloadCommand_UnableToDownload,
                    id,
                    version.ToNormalizedString(),
                    repo.PackageSource.Source));
                return false;
            }

            return success;
        }

        private static IReadOnlyList<PackageSource> GetPackageSources(IList<string>? sources, IPackageSourceProvider sourceProvider)
        {
            IEnumerable<PackageSource> configuredSources = sourceProvider.LoadPackageSources()
                .Where(s => s.IsEnabled);

            if (sources != null && sources.Count > 0)
            {
                // Use sources specified on command line
                return [.. sources.Select(s => PackageSourceProviderExtensions.ResolveSource(configuredSources, s))];
            }

            return [.. configuredSources];
        }

        private static bool DetectAndReportInsecureSources(
            bool allowInsecureConnections,
            IEnumerable<PackageSource> packageSources,
            ILoggerWithColor logger)
        {
            if (!allowInsecureConnections)
            {
                var insecureSources = HttpSourcesUtility.GetDisallowedInsecureHttpSources([.. packageSources]);
                if (insecureSources.Any())
                {
                    logger.LogError(HttpSourcesUtility.BuildHttpSourceErrorMessage(insecureSources, "package download"));
                    return true;
                }
            }

            return false;
        }

        private static IReadOnlyList<SourceRepository> GetSourceRepositories(IReadOnlyList<PackageSource> packageSources)
        {
            IEnumerable<Lazy<INuGetResourceProvider>> providers = Repository.Provider.GetCoreV3();
            List<SourceRepository> sourceRepositories = [];
            foreach (var source in packageSources)
            {
                sourceRepositories.Add(Repository.CreateSource(providers, source, FeedType.Undefined));
            }

            return sourceRepositories;
        }
    }
}
