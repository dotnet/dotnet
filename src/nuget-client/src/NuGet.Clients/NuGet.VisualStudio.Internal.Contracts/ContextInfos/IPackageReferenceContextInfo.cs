// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.Versioning;

namespace NuGet.VisualStudio.Internal.Contracts
{
    public interface IPackageReferenceContextInfo
    {
        PackageIdentity Identity { get; }
        NuGetFramework? Framework { get; }
        VersionRange? AllowedVersions { get; }
        bool IsAutoReferenced { get; }
        bool IsUserInstalled { get; }
        bool IsDevelopmentDependency { get; }
        VersionRange? VersionOverride { get; }
    }
}
