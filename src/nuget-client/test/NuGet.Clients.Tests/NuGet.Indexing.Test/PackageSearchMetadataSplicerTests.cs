// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Indexing.Test
{
    public class PackageSearchMetadataSplicerTests
    {
        [Fact]
        public void MergeEntries_WithDifferentPackageIds_Throws()
        {
            var packageA = PackageSearchMetadataBuilder
                .FromIdentity(new PackageIdentity("packageA", new Versioning.NuGetVersion("1.0.0")))
                .Build();
            var packageB = PackageSearchMetadataBuilder
                .FromIdentity(new PackageIdentity("packageB", new Versioning.NuGetVersion("1.0.0")))
                .Build();

            var splicer = new PackageSearchMetadataSplicer();

            var error = Assert.Throws<InvalidOperationException>(() => splicer.MergeEntries(packageA, packageB));
            Assert.Equal($"Cannot merge packages 'packageA.1.0.0' and 'packageB.1.0.0' because their ids are different.", error.Message);
        }

        [Fact]
        public void MergeEntries_PicksTheLatest()
        {
            var older = new PackageIdentity("packageA", new Versioning.NuGetVersion("1.0.0"));
            var newer = new PackageIdentity("packageA", new Versioning.NuGetVersion("1.0.2"));

            var packageA = PackageSearchMetadataBuilder
                .FromIdentity(older)
                .Build();
            var packageB = PackageSearchMetadataBuilder
                .FromIdentity(newer)
                .Build();

            var splicer = new PackageSearchMetadataSplicer();

            var result = splicer.MergeEntries(packageA, packageB);

            Assert.NotNull(result);
            Assert.Equal(newer, result.Identity);
        }

        [Fact]
        public async Task MergeEntries_MergesVersions()
        {
            var testData = TestUtility.LoadTestResponse("NuGet.Core.json");

            var package1 = testData[0];
            var v1 = await GetPackageVersionsAsync(package1);
            Assert.NotEmpty(v1);

            var package2 = testData[1];
            var v2 = await GetPackageVersionsAsync(package2);
            Assert.NotEmpty(v2);

            var splicer = new PackageSearchMetadataSplicer();

            var mergedPackage = splicer.MergeEntries(package1, package2);

            var vm = await GetPackageVersionsAsync(mergedPackage);
            Assert.Superset(v1, vm);
            Assert.Superset(v2, vm);
        }

        [Fact]
        public async Task MergeEntries_LeavesNoDuplicateVersions()
        {
            var testData = TestUtility.LoadTestResponse("NuGet.Core.json");
            var package1 = testData[0];
            var package2 = testData[1];

            var splicer = new PackageSearchMetadataSplicer();

            var mergedPackage = splicer.MergeEntries(package1, package2);

            var vm = (await mergedPackage.GetVersionsAsync()).Select(v => v.Version);
            Assert.Equal(vm, vm.Distinct());
        }

        private static async Task<ISet<NuGetVersion>> GetPackageVersionsAsync(IPackageSearchMetadata package)
        {
            return new HashSet<NuGetVersion>((await package.GetVersionsAsync()).Select(v => v.Version));
        }
    }
}
