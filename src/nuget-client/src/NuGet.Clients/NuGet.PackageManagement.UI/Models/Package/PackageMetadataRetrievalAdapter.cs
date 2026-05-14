// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Packaging.Core;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.UI.Models.Package
{
    internal class PackageMetadataRetrievalAdapter : IPackageMetadataRetrievalAdapter
    {
        private readonly INuGetSearchService _nugetSearchService;
        private readonly PackageIdentity _packageIdentity;
        private readonly IReadOnlyCollection<PackageSourceContextInfo> _packageSources;
        private readonly bool _includePrerelease;
        private Task<(PackageSearchMetadataContextInfo, PackageDeprecationMetadataContextInfo?)>? _packageMetadataTask;
        private readonly object _lock = new();

        public PackageMetadataRetrievalAdapter(INuGetSearchService nugetSearchService, PackageIdentity packageIdentity, IReadOnlyCollection<PackageSourceContextInfo> packageSources, bool includePrerelease)
        {
            _nugetSearchService = nugetSearchService ?? throw new ArgumentNullException(nameof(nugetSearchService));
            _packageIdentity = packageIdentity ?? throw new ArgumentNullException(nameof(packageIdentity));
            _packageSources = packageSources ?? throw new ArgumentNullException(nameof(packageSources));
            _includePrerelease = includePrerelease;
        }

        public async Task<PackageSearchMetadataContextInfo> GetPackageMetadataAsync(
            CancellationToken cancellationToken)
        {
            var packageMetadata = await FetchMetadataAsync(cancellationToken);
            return packageMetadata.Item1;
        }

        public async Task<PackageDeprecationMetadataContextInfo?> GetPackageDeprecationInfoAsync(
            CancellationToken cancellationToken)
        {
            var packageMetadata = await FetchMetadataAsync(cancellationToken);
            return packageMetadata.Item2;
        }

        private Task<(PackageSearchMetadataContextInfo, PackageDeprecationMetadataContextInfo?)> FetchMetadataAsync(
            CancellationToken cancellationToken)
        {
            if (_packageMetadataTask == null)
            {
                lock (_lock)
                {
                    _packageMetadataTask ??= _nugetSearchService.GetPackageMetadataAsync(
                        _packageIdentity,
                        _packageSources,
                        _includePrerelease,
                        cancellationToken).AsTask();
                }
            }

            return _packageMetadataTask;
        }
    }
}
