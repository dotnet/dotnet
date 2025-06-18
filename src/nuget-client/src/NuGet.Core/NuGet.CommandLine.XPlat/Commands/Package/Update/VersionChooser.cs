// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace NuGet.CommandLine.XPlat.Commands.Package.Update
{
    internal class VersionChooser : IVersionChooser
    {
        private readonly ISourceRepositoryProvider _sourceProvider;
        private readonly ISettings _settings;
        private readonly SourceCacheContext _sourceCacheContext;

        public VersionChooser(
            ISourceRepositoryProvider sourceProvider,
            ISettings settings,
            SourceCacheContext sourceCacheContext)
        {
            _sourceProvider = sourceProvider;
            _settings = settings;
            _sourceCacheContext = sourceCacheContext;
        }

        public async Task<NuGetVersion?> GetLatestVersionAsync(
            string packageId,
            ILoggerWithColor logger,
            CancellationToken cancellationToken)
        {
            var sources = new List<SourceRepository>();
            foreach (PackageSource packageSource in SettingsUtility.GetEnabledSources(_settings).NoAllocEnumerate())
            {
                SourceRepository sourceRepository = _sourceProvider.CreateRepository(packageSource);
                sources.Add(sourceRepository);
            }

            var lookups = new Task<NuGetVersion?>[sources.Count];
            for (int source = 0; source < sources.Count; source++)
            {
                SourceRepository sourceRepository = sources[source];
                // If package source is a local folder feed, it might not actually be async
                lookups[source] = Task.Run(() => FindHighestPackageVersionAsync(sourceRepository, packageId, _sourceCacheContext, logger, cancellationToken));
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

        private static async Task<NuGetVersion?> FindHighestPackageVersionAsync(
            SourceRepository source,
            string packageId,
            SourceCacheContext sourceCacheContext,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            var packageMetadataResource = await source.GetResourceAsync<PackageMetadataResource>(cancellationToken);

            var packageDetails = await packageMetadataResource.GetMetadataAsync(
                packageId,
                includePrerelease: false,
                includeUnlisted: false,
                sourceCacheContext,
                logger,
                cancellationToken);

            if (packageDetails is null || !packageDetails.Any())
            {
                return null;
            }

            NuGetVersion highestVersion = packageDetails.Max(p => p.Identity.Version)!;
            return highestVersion;
        }

    }
}
