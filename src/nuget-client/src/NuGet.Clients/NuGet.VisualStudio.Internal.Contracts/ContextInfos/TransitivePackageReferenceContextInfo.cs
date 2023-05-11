// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.Versioning;

namespace NuGet.VisualStudio.Internal.Contracts
{
    public class TransitivePackageReferenceContextInfo : ITransitivePackageReferenceContextInfo
    {
        public TransitivePackageReferenceContextInfo(PackageIdentity identity, NuGetFramework? framework)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }
            // framework is null for project.json package references. 

            Identity = identity;
            Framework = framework;
            TransitiveOrigins = new List<IPackageReferenceContextInfo>();
        }

        // Only for testing purposes
        internal static TransitivePackageReferenceContextInfo Create(PackageIdentity identity, NuGetFramework? framework)
        {
            return new TransitivePackageReferenceContextInfo(identity, framework);
        }

        private static TransitivePackageReferenceContextInfo Create(IPackageReferenceContextInfo packageReference)
        {
            if (packageReference == null)
            {
                throw new ArgumentNullException(nameof(packageReference));
            }

            var tranPkgRefCtxInfo = new TransitivePackageReferenceContextInfo(packageReference.Identity, packageReference.Framework)
            {
                IsAutoReferenced = packageReference.IsAutoReferenced,
                AllowedVersions = packageReference.AllowedVersions,
                IsUserInstalled = packageReference.IsUserInstalled,
                IsDevelopmentDependency = packageReference.IsDevelopmentDependency,
            };

            return tranPkgRefCtxInfo;
        }

        public static TransitivePackageReferenceContextInfo Create(TransitivePackageReference transitivePackageReference)
        {
            if (transitivePackageReference == null)
            {
                throw new ArgumentNullException(nameof(transitivePackageReference));
            }

            var prCtxInfo = PackageReferenceContextInfo.Create(transitivePackageReference);

            var tranPkgRefCtxInfo = Create(prCtxInfo);

            tranPkgRefCtxInfo.TransitiveOrigins = transitivePackageReference.TransitiveOrigins.Select(pr => PackageReferenceContextInfo.Create(pr)).ToList();

            return tranPkgRefCtxInfo;
        }

        public IEnumerable<IPackageReferenceContextInfo> TransitiveOrigins { get; internal set; }

        public PackageIdentity Identity { get; internal set; }

        public NuGetFramework? Framework { get; internal set; }

        public VersionRange? AllowedVersions { get; internal set; }

        public bool IsAutoReferenced { get; internal set; }

        public bool IsUserInstalled { get; internal set; }

        public bool IsDevelopmentDependency { get; internal set; }

        public VersionRange? VersionOverride { get; internal set; }
    }
}
