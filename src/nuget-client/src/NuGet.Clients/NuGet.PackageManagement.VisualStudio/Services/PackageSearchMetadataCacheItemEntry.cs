// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.VisualStudio
{
    internal sealed class PackageSearchMetadataCacheItemEntry
    {
        private readonly IPackageSearchMetadata _packageSearchMetadata;
        private readonly IPackageMetadataProvider _packageMetadataProvider;
        private readonly Lazy<Task<IPackageSearchMetadata>> _detailedPackageSearchMetadata;

        public PackageSearchMetadataCacheItemEntry(IPackageSearchMetadata packageSearchMetadata, IPackageMetadataProvider packageMetadataProvider)
        {
            _packageSearchMetadata = packageSearchMetadata;
            _packageMetadataProvider = packageMetadataProvider;
            _detailedPackageSearchMetadata = AsyncLazy.New(() =>
            {
                return _packageMetadataProvider.GetPackageMetadataForIdentityAsync(_packageSearchMetadata.Identity, CancellationToken.None);
            });
        }

        public ValueTask<PackageDeprecationMetadataContextInfo?> PackageDeprecationMetadataContextInfo => GetPackageDeprecationMetadataContextInfoAsync();

        public ValueTask<PackageSearchMetadataContextInfo> DetailedPackageSearchMetadataContextInfo => GetDetailedPackageSearchMetadataContextInfoAsync();

        private async ValueTask<PackageDeprecationMetadataContextInfo?> GetPackageDeprecationMetadataContextInfoAsync()
        {
            // If PackageSearchMetadata was added to the cache directly from search then deprecation data could be null even if it exists, we need
            // to check the package metadata provider to be certain
            PackageDeprecationMetadata? deprecationMetadata = await _packageSearchMetadata.GetDeprecationMetadataAsync();
            if (deprecationMetadata == null)
            {
                IPackageSearchMetadata detailedMetadata = await _detailedPackageSearchMetadata.Value;
                deprecationMetadata = await detailedMetadata.GetDeprecationMetadataAsync();
                if (deprecationMetadata == null)
                {
                    return null;
                }
            }
            return NuGet.VisualStudio.Internal.Contracts.PackageDeprecationMetadataContextInfo.Create(deprecationMetadata);
        }

        private async ValueTask<PackageSearchMetadataContextInfo> GetDetailedPackageSearchMetadataContextInfoAsync()
        {
            IPackageSearchMetadata detailedMetadata = await _detailedPackageSearchMetadata.Value;
            return PackageSearchMetadataContextInfo.Create(detailedMetadata);
        }
    }
}
