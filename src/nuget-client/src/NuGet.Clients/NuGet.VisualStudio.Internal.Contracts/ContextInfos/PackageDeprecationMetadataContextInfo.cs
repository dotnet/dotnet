// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using NuGet.Protocol;

namespace NuGet.VisualStudio.Internal.Contracts
{
    public sealed class PackageDeprecationMetadataContextInfo
    {
        public string? Message { get; }
        public IReadOnlyCollection<string>? Reasons { get; }
        public AlternatePackageMetadataContextInfo? AlternatePackage { get; }

        public PackageDeprecationMetadataContextInfo(
            string? message,
            IReadOnlyCollection<string>? reasons,
            AlternatePackageMetadataContextInfo? alternatePackageContextInfo)
        {
            Message = message;
            Reasons = reasons;
            AlternatePackage = alternatePackageContextInfo;
        }

        public static PackageDeprecationMetadataContextInfo Create(PackageDeprecationMetadata packageDeprecationMetadata)
        {
            return new PackageDeprecationMetadataContextInfo(
                packageDeprecationMetadata.Message,
                packageDeprecationMetadata.Reasons?.ToList(),
                packageDeprecationMetadata.AlternatePackage != null ? AlternatePackageMetadataContextInfo.Create(packageDeprecationMetadata.AlternatePackage) : null);
        }
    }
}
