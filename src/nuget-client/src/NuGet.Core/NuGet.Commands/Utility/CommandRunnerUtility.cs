// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;

namespace NuGet.Commands
{
    /// <summary>
    /// Helper functions for shared command runners (push, delete, etc)
    /// </summary>
    internal static class CommandRunnerUtility
    {
        public static string ResolveSource(IPackageSourceProvider sourceProvider, string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                source = sourceProvider.DefaultPushSource;
            }

            if (!string.IsNullOrEmpty(source))
            {
                source = sourceProvider.ResolveAndValidateSource(source);
            }

            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentException(Strings.Error_MissingSourceParameter);
            }

            return source;
        }

        public static string ResolveSymbolSource(IPackageSourceProvider sourceProvider, string symbolSource)
        {
            if (!string.IsNullOrEmpty(symbolSource))
            {
                symbolSource = sourceProvider.ResolveAndValidateSource(symbolSource);
            }

            return symbolSource;
        }

        public static string GetApiKey(ISettings settings, string endpoint, string source)
        {
            // try searching API key by endpoint first
            // needed to support config key mappings like 'https://www.nuget.org/api/v2/package'
            var apiKey = SettingsUtility.GetDecryptedValueForAddItem(settings, ConfigurationConstants.ApiKeys, endpoint);

            // if not found try finding it by source url
            apiKey = apiKey ?? SettingsUtility.GetDecryptedValueForAddItem(settings, ConfigurationConstants.ApiKeys, source);

            // fallback for a case of nuget.org source
            // try to retrieve an api key mapped to a default "gallery" url
            if (apiKey == null &&
                UriUtility.IsNuGetOrg(source))
            {
                var defaultConfigKey = NuGetConstants.DefaultGalleryServerUrl;
                apiKey = SettingsUtility.GetDecryptedValueForAddItem(settings, ConfigurationConstants.ApiKeys, defaultConfigKey);
            }

            // return an API key when found or null when not found
            return apiKey;
        }

        public static async Task<PackageUpdateResource> GetPackageUpdateResource(IPackageSourceProvider sourceProvider, PackageSource packageSource, CancellationToken cancellationToken)
        {
            var sourceRepositoryProvider = new CachingSourceProvider(sourceProvider);
            var sourceRepository = sourceRepositoryProvider.CreateRepository(packageSource);

            return await sourceRepository.GetResourceAsync<PackageUpdateResource>(cancellationToken);
        }

        public static PackageSource GetOrCreatePackageSource(IPackageSourceProvider sourceProvider, string source)
        {
            // Use a loaded PackageSource if possible since it contains credential info
            PackageSource packageSource = null;
            foreach (var loadedPackageSource in sourceProvider.LoadPackageSources())
            {
                if (loadedPackageSource.IsEnabled && source == loadedPackageSource.Source)
                {
                    packageSource = loadedPackageSource;
                    break;
                }
            }

            if (packageSource == null)
            {
                packageSource = new PackageSource(source);
            }

            return packageSource;
        }

        public static async Task<SymbolPackageUpdateResourceV3> GetSymbolPackageUpdateResource(IPackageSourceProvider sourceProvider, string source, CancellationToken cancellationToken)
        {
            // Use a loaded PackageSource if possible since it contains credential info
            var packageSource = sourceProvider.LoadPackageSources()
                .Where(e => e.IsEnabled && string.Equals(source, e.Source, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();

            if (packageSource == null)
            {
                packageSource = new PackageSource(source);
            }

            var sourceRepositoryProvider = new CachingSourceProvider(sourceProvider);
            var sourceRepository = sourceRepositoryProvider.CreateRepository(packageSource);

            return await sourceRepository.GetResourceAsync<SymbolPackageUpdateResourceV3>(cancellationToken);
        }
    }
}
