// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using NuGet.VisualStudio.Internal.Contracts;
using Xunit;

namespace NuGet.PackageManagement.VisualStudio.Test
{
    public class PackageReferenceProjectTests
    {
        [Fact]
        public void MergeTransitiveOrigin_DuplicateTransitiveOrigins_Merges()
        {
            var net472 = NuGetFramework.Parse("net472");
            var pr = new PackageReference(new PackageIdentity("packageA", new NuGetVersion("1.0.0")), net472);

            var te = new Dictionary<FrameworkRuntimePair, IList<PackageReference>>
            {
                {
                    new FrameworkRuntimePair(net472, string.Empty),
                    new List<PackageReference>()
                    {
                        new PackageReference(new PackageIdentity("package1", new NuGetVersion("0.0.1")), net472),
                        new PackageReference(new PackageIdentity("package1", new NuGetVersion("0.0.2")), net472),
                    }
                }
            };

            TransitivePackageReference transitivePackageReference = PackageReferenceProject<List<object>, object>.MergeTransitiveOrigin(pr, te);

            var transitiveOrigin = transitivePackageReference.TransitiveOrigins.Single();

            Assert.Equal(NuGetVersion.Parse("0.0.2"), transitiveOrigin.PackageIdentity.Version);
        }

        [Fact]
        public void MergeTransitiveOrigin_MultiTargeting_MergesLatest()
        {
            var net472 = NuGetFramework.Parse("net472");
            var net60 = NuGetFramework.Parse("net6.0");
            var te = new Dictionary<FrameworkRuntimePair, IList<PackageReference>>
            {
                {
                    new FrameworkRuntimePair(net472, string.Empty),
                    new List<PackageReference>()
                    {
                        new PackageReference(new PackageIdentity("package1", new NuGetVersion("0.0.1")), net472),
                        new PackageReference(new PackageIdentity("package1", new NuGetVersion("0.0.2")), net472),
                    }
                },
                {
                    new FrameworkRuntimePair(net60, string.Empty),
                    new List<PackageReference>()
                    {
                        new PackageReference(new PackageIdentity("package1", new NuGetVersion("0.0.3")), net60),
                        new PackageReference(new PackageIdentity("package1", new NuGetVersion("0.0.4")), net60),
                    }
                }
            };

            var pr = new PackageReference(new PackageIdentity("packageA", new NuGetVersion("1.0.0")), net472);

            TransitivePackageReference transitivePackageReference = PackageReferenceProject<List<object>, object>.MergeTransitiveOrigin(pr, te);

            var transitiveOrigin = transitivePackageReference.TransitiveOrigins.Single();

            Assert.Equal(NuGetVersion.Parse("0.0.2"), transitiveOrigin.PackageIdentity.Version);
        }
    }
}
