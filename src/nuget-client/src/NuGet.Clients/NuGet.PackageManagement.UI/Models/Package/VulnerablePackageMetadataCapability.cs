// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NuGet.PackageManagement.UI.Models.Package
{
    internal class VulnerablePackageMetadataCapability : VulnerableCapabilityBase
    {
        private readonly IPackageMetadataRetrievalAdapter _packageMetadataRetrievalAdapter;

        public VulnerablePackageMetadataCapability(IPackageMetadataRetrievalAdapter packageMetadataRetrievalAdapter)
        {
            _packageMetadataRetrievalAdapter = packageMetadataRetrievalAdapter ?? throw new ArgumentNullException(nameof(packageMetadataRetrievalAdapter));
        }

        public async override Task PopulateDataAsync(CancellationToken cancellationToken)
        {
            var packageMetadata = await _packageMetadataRetrievalAdapter.GetPackageMetadataAsync(cancellationToken);
            Vulnerabilities = packageMetadata.Vulnerabilities?.ToList() ?? [];
        }
    }
}
