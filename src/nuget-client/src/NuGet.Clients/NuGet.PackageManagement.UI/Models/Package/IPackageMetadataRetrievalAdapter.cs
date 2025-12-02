// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.UI.Models.Package
{
    internal interface IPackageMetadataRetrievalAdapter
    {
        public Task<PackageSearchMetadataContextInfo> GetPackageMetadataAsync(
            CancellationToken cancellationToken);

        public Task<PackageDeprecationMetadataContextInfo?> GetPackageDeprecationInfoAsync(
            CancellationToken cancellationToken);
    }
}
