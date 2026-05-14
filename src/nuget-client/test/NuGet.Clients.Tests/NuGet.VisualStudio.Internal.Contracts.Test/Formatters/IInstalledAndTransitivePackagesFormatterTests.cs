// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using Xunit;

namespace NuGet.VisualStudio.Internal.Contracts.Test
{
    public sealed class IInstalledAndTransitivePackagesFormatterTests : FormatterTests
    {
        private static readonly PackageIdentity DirectPackageIdentity1 = new PackageIdentity(id: "a", NuGetVersion.Parse("1.2.3"));
        private static readonly NuGetFramework DirectFramework1 = NuGetFramework.Parse("net50");
        private static readonly IPackageReferenceContextInfo DirectPackageReferenceContextInfo1 = PackageReferenceContextInfo.Create(DirectPackageIdentity1, DirectFramework1);
        private static readonly PackageIdentity DirectPackageIdentity2 = new PackageIdentity(id: "b", NuGetVersion.Parse("1.2.4"));
        private static readonly NuGetFramework DirectFramework2 = NuGetFramework.Parse("net50");
        private static readonly IPackageReferenceContextInfo DirectPackageReferenceContextInfo2 = PackageReferenceContextInfo.Create(DirectPackageIdentity2, DirectFramework2);
        private static readonly List<IPackageReferenceContextInfo> DirectPackages = new List<IPackageReferenceContextInfo>() { DirectPackageReferenceContextInfo1, DirectPackageReferenceContextInfo2 };

        private static readonly PackageIdentity TransitivePackageIdentity1 = new PackageIdentity(id: "cc", NuGetVersion.Parse("1.2.5"));
        private static readonly NuGetFramework TransitiveFramework1 = NuGetFramework.Parse("net50");
        private static readonly ITransitivePackageReferenceContextInfo TransitivePackageReferenceContextInfo1 = TransitivePackageReferenceContextInfo.Create(TransitivePackageIdentity1, TransitiveFramework1);
        private static readonly PackageIdentity TransitivePackageIdentity2 = new PackageIdentity(id: "dd", NuGetVersion.Parse("1.2.6"));
        private static readonly NuGetFramework TransitiveFramework2 = NuGetFramework.Parse("net50");
        private static readonly ITransitivePackageReferenceContextInfo TransitivePackageReferenceContextInfo2 = TransitivePackageReferenceContextInfo.Create(TransitivePackageIdentity2, TransitiveFramework2);
        private static readonly PackageIdentity TransitivePackageIdentity3 = new PackageIdentity(id: "ee", NuGetVersion.Parse("1.2.7"));
        private static readonly NuGetFramework TransitiveFramework3 = NuGetFramework.Parse("net50");
        private static readonly ITransitivePackageReferenceContextInfo TransitivePackageReferenceContextInfo3 = TransitivePackageReferenceContextInfo.Create(TransitivePackageIdentity3, TransitiveFramework3);
        private static readonly List<ITransitivePackageReferenceContextInfo> TransitivePackages = new List<ITransitivePackageReferenceContextInfo>() { TransitivePackageReferenceContextInfo1, TransitivePackageReferenceContextInfo2, TransitivePackageReferenceContextInfo3 };

        [Theory]
        [MemberData(nameof(IInstalledAndTransitivePackages))]
        public void SerializeThenDeserialize_WithValidArguments_RoundTrips(InstalledAndTransitivePackages expectedResult)
        {
            IInstalledAndTransitivePackages? actualResult = SerializeThenDeserialize(IInstalledAndTransitivePackagesFormatter.Instance, expectedResult);

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult.InstalledPackages.Count, actualResult!.InstalledPackages.Count);
            Assert.Equal(expectedResult.TransitivePackages.Count, actualResult.TransitivePackages.Count);

            foreach (IPackageReferenceContextInfo expectedPackage in expectedResult.InstalledPackages)
            {
                IPackageReferenceContextInfo actualPackage = actualResult.InstalledPackages.FirstOrDefault(p => p.Identity.Equals(expectedPackage.Identity));
                CheckPackageReferencesContextInfoAreEqual(actualPackage, expectedPackage);
            }

            foreach (IPackageReferenceContextInfo expectedPackage in expectedResult.TransitivePackages)
            {
                IPackageReferenceContextInfo actualPackage = actualResult.TransitivePackages.FirstOrDefault(p => p.Identity.Equals(expectedPackage.Identity));
                CheckPackageReferencesContextInfoAreEqual(actualPackage, expectedPackage);
            }
        }

        public static TheoryData<InstalledAndTransitivePackages> IInstalledAndTransitivePackages => new()
        {
            { new InstalledAndTransitivePackages(DirectPackages, TransitivePackages) },
            { new InstalledAndTransitivePackages(DirectPackages, Array.Empty<ITransitivePackageReferenceContextInfo>()) },
            { new InstalledAndTransitivePackages(Array.Empty<IPackageReferenceContextInfo>(), Array.Empty<ITransitivePackageReferenceContextInfo>()) }
        };

        private static void CheckPackageReferencesContextInfoAreEqual(IPackageReferenceContextInfo packageA, IPackageReferenceContextInfo packageB)
        {
            Assert.NotNull(packageA);
            Assert.NotNull(packageB);
            Assert.Equal(packageA.Identity, packageB.Identity);
            Assert.Equal(packageA.Framework, packageB.Framework);
            Assert.Equal(packageA.AllowedVersions, packageB.AllowedVersions);
            Assert.Equal(packageA.IsAutoReferenced, packageB.IsAutoReferenced);
            Assert.Equal(packageA.IsUserInstalled, packageB.IsUserInstalled);
            Assert.Equal(packageA.IsDevelopmentDependency, packageB.IsDevelopmentDependency);
        }
    }
}
