// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Configuration;
using NuGet.Protocol.LocalRepositories;
using NuGet.Protocol.Providers;

namespace NuGet.Protocol.Core.Types
{
    public static class Repository
    {
        private static ProviderFactory _providerFactory = new ProviderFactory();

        public static RepositoryFactory Factory { get; } = new RepositoryFactory();

        public static ProviderFactory Provider
        {
            get
            {
                return _providerFactory;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                _providerFactory = value;
            }
        }

        public class RepositoryFactory
        {
            // Methods are added by extension
        }

        public class ProviderFactory
        {
            public virtual IEnumerable<Lazy<INuGetResourceProvider>> GetCoreV3()
            {
                yield return new Lazy<INuGetResourceProvider>(() => new FeedTypeResourceProvider());
                yield return new Lazy<INuGetResourceProvider>(() => new DependencyInfoResourceV3Provider());
                yield return new Lazy<INuGetResourceProvider>(() => new DownloadResourcePluginProvider());
                yield return new Lazy<INuGetResourceProvider>(() => new DownloadResourceV3Provider());
                yield return new Lazy<INuGetResourceProvider>(() => new MetadataResourceV3Provider());
#pragma warning disable CS0618 // Type or member is obsolete
                yield return new Lazy<INuGetResourceProvider>(() => new RawSearchResourceV3Provider());
#pragma warning restore CS0618 // Type or member is obsolete
                yield return new Lazy<INuGetResourceProvider>(() => new RegistrationResourceV3Provider());
                yield return new Lazy<INuGetResourceProvider>(() => new SymbolPackageUpdateResourceV3Provider());
                yield return new Lazy<INuGetResourceProvider>(() => new ReportAbuseResourceV3Provider());
                yield return new Lazy<INuGetResourceProvider>(() => new ReadmeUriTemplateResourceProvider());
                yield return new Lazy<INuGetResourceProvider>(() => new PackageDetailsUriResourceV3Provider());
                yield return new Lazy<INuGetResourceProvider>(() => new ServiceIndexResourceV3Provider());
                yield return new Lazy<INuGetResourceProvider>(() => new ODataServiceDocumentResourceV2Provider());
                yield return new Lazy<INuGetResourceProvider>(() => new HttpHandlerResourceV3Provider());
                yield return new Lazy<INuGetResourceProvider>(() => new HttpSourceResourceProvider());
                yield return new Lazy<INuGetResourceProvider>(() => new PluginFindPackageByIdResourceProvider());
                yield return new Lazy<INuGetResourceProvider>(() => new HttpFileSystemBasedFindPackageByIdResourceProvider());
                yield return new Lazy<INuGetResourceProvider>(() => new RemoteV3FindPackageByIdResourceProvider());
                yield return new Lazy<INuGetResourceProvider>(() => new RemoteV2FindPackageByIdResourceProvider());
                yield return new Lazy<INuGetResourceProvider>(() => new LocalV3FindPackageByIdResourceProvider());
                yield return new Lazy<INuGetResourceProvider>(() => new LocalV2FindPackageByIdResourceProvider());
                yield return new Lazy<INuGetResourceProvider>(() => new PackageUpdateResourceV2Provider());
                yield return new Lazy<INuGetResourceProvider>(() => new PackageUpdateResourceV3Provider());
                yield return new Lazy<INuGetResourceProvider>(() => new DependencyInfoResourceV2FeedProvider());
                yield return new Lazy<INuGetResourceProvider>(() => new DownloadResourceV2FeedProvider());
                yield return new Lazy<INuGetResourceProvider>(() => new MetadataResourceV2FeedProvider());
                yield return new Lazy<INuGetResourceProvider>(() => new V3FeedListResourceProvider());
                yield return new Lazy<INuGetResourceProvider>(() => new V2FeedListResourceProvider());
                yield return new Lazy<INuGetResourceProvider>(() => new LocalPackageListResourceProvider());
                yield return new Lazy<INuGetResourceProvider>(() => new PackageSearchResourceV2FeedProvider());
                yield return new Lazy<INuGetResourceProvider>(() => new PackageSearchResourceV3Provider());
                yield return new Lazy<INuGetResourceProvider>(() => new PackageMetadataResourceV2FeedProvider());
                yield return new Lazy<INuGetResourceProvider>(() => new PackageMetadataResourceV3Provider());
                yield return new Lazy<INuGetResourceProvider>(() => new AutoCompleteResourceV2FeedProvider());
                yield return new Lazy<INuGetResourceProvider>(() => new AutoCompleteResourceV3Provider());
                yield return new Lazy<INuGetResourceProvider>(() => new PluginResourceProvider());
                yield return new Lazy<INuGetResourceProvider>(() => new RepositorySignatureResourceProvider());
                yield return new Lazy<INuGetResourceProvider>(() => new VulnerabilityInfoResourceV3Provider());
                yield return new Lazy<INuGetResourceProvider>(() => new OwnerDetailsUriResourceV3Provider());

                // Local repository providers
                yield return new Lazy<INuGetResourceProvider>(() => new FindLocalPackagesResourceUnzippedProvider());
                yield return new Lazy<INuGetResourceProvider>(() => new FindLocalPackagesResourceV2Provider());
                yield return new Lazy<INuGetResourceProvider>(() => new FindLocalPackagesResourceV3Provider());
                yield return new Lazy<INuGetResourceProvider>(() => new FindLocalPackagesResourcePackagesConfigProvider());
                yield return new Lazy<INuGetResourceProvider>(() => new LocalAutoCompleteResourceProvider());
                yield return new Lazy<INuGetResourceProvider>(() => new LocalDependencyInfoResourceProvider());
                yield return new Lazy<INuGetResourceProvider>(() => new LocalDownloadResourceProvider());
                yield return new Lazy<INuGetResourceProvider>(() => new LocalMetadataResourceProvider());
                yield return new Lazy<INuGetResourceProvider>(() => new LocalPackageMetadataResourceProvider());
                yield return new Lazy<INuGetResourceProvider>(() => new LocalPackageSearchResourceProvider());
            }
        }

        /// <summary>
        /// Create the default source repository provider
        /// </summary>
        [Obsolete("https://github.com/NuGet/Home/issues/8479")]
        public static ISourceRepositoryProvider CreateProvider(IEnumerable<INuGetResourceProvider> resourceProviders)
        {
            return new SourceRepositoryProvider(Settings.LoadDefaultSettings(null, null, null), CreateLazy(resourceProviders));
        }

        /// <summary>
        /// Find sources from nuget.config based on the root path
        /// </summary>
        /// <param name="rootPath">lowest folder path</param>
        [Obsolete("https://github.com/NuGet/Home/issues/8479")]
        public static ISourceRepositoryProvider CreateProvider(IEnumerable<INuGetResourceProvider> resourceProviders, string rootPath)
        {
            return new SourceRepositoryProvider(Settings.LoadDefaultSettings(rootPath, null, null), CreateLazy(resourceProviders));
        }

        /// <summary>
        /// Create a SourceRepository
        /// </summary>
        public static SourceRepository CreateSource(IEnumerable<Lazy<INuGetResourceProvider>> resourceProviders, string sourceUrl)
        {
            return CreateSource(resourceProviders, new PackageSource(sourceUrl));
        }


        /// <summary>
        /// Create a SourceRepository
        /// </summary>
        public static SourceRepository CreateSource(IEnumerable<Lazy<INuGetResourceProvider>> resourceProviders, string sourceUrl, FeedType type)
        {
            return CreateSource(resourceProviders, new PackageSource(sourceUrl), type);
        }

        /// <summary>
        /// Create a SourceRepository
        /// </summary>
        public static SourceRepository CreateSource(IEnumerable<Lazy<INuGetResourceProvider>> resourceProviders, PackageSource source)
        {
            return CreateSource(resourceProviders, source, FeedType.Undefined);
        }

        /// <summary>
        /// Create a SourceRepository
        /// </summary>
        public static SourceRepository CreateSource(IEnumerable<Lazy<INuGetResourceProvider>> resourceProviders, PackageSource source, FeedType type)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (resourceProviders == null)
            {
                throw new ArgumentNullException(nameof(resourceProviders));
            }

            return new SourceRepository(source, resourceProviders, type);
        }

        private static IEnumerable<Lazy<INuGetResourceProvider>> CreateLazy(IEnumerable<INuGetResourceProvider> providers)
        {
            return providers.Select(e => new Lazy<INuGetResourceProvider>(() => e));
        }
    }
}
