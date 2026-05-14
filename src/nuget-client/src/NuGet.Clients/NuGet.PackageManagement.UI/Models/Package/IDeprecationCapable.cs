// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.UI.Models.Package
{
    internal interface IDeprecationCapable
    {
        public bool IsDeprecated { get; }

        public PackageDeprecationReason PackageDeprecationReasons { get; }

        public AlternatePackageMetadataContextInfo? AlternatePackage { get; }

        public Task PopulateDataAsync(CancellationToken cancellationToken);

        public PackageDeprecationMetadataContextInfo? DeprecationMetadata { get; }
    }
}
