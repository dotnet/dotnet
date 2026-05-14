// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Packaging;

namespace NuGet.VisualStudio.Internal.Contracts
{
    public class TransitivePackageReference : PackageReference
    {
        public TransitivePackageReference(PackageReference pr)
            : base(pr?.PackageIdentity ?? throw new ArgumentNullException(nameof(pr)), pr.TargetFramework, pr.IsUserInstalled, pr.IsDevelopmentDependency, pr.RequireReinstallation, pr.AllowedVersions)
        {
            TransitiveOrigins = new List<PackageReference>();
        }

        public IEnumerable<PackageReference> TransitiveOrigins { get; set; }
    }
}
