// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class LocalPackageSearchMetadataTests : IClassFixture<LocalPackageSearchMetadataFixture>
    {
        private readonly LocalPackageSearchMetadataFixture _testData;

        public LocalPackageSearchMetadataTests(LocalPackageSearchMetadataFixture testInstance)
        {
            _testData = testInstance;
        }

        [Fact]
        public async Task DeprecationMetadataIsNull()
        {
            var localPackageInfo = new LocalPackageInfo(
                new PackageIdentity("id", NuGetVersion.Parse("1.0.0")),
                "path",
                new DateTime(2019, 8, 19),
                new Lazy<NuspecReader>(() => null),
                useFolder: false
                );

            var localPackageSearchMetadata = new LocalPackageSearchMetadata(localPackageInfo);

            Assert.Null(await localPackageSearchMetadata.GetDeprecationMetadataAsync());
        }

        [Fact]
        public void DownloadCount_Always_Null()
        {
            var localPackageInfo = new LocalPackageInfo(
                new PackageIdentity("id", NuGetVersion.Parse("1.0.0")),
                "path",
                new DateTime(2019, 8, 19),
                new Lazy<NuspecReader>(() => null),
                useFolder: false
                );

            var localPackageSearchMetadata = new LocalPackageSearchMetadata(localPackageInfo);

            Assert.Null(localPackageSearchMetadata.DownloadCount);
        }

        [Fact]
        public void PackageReader_NotNull()
        {
            Assert.NotNull(_testData.TestData.PackageReader);
        }

        [Fact]
        public void IconUrl_ReturnsEmbeddedIconUri()
        {
            Assert.NotNull(_testData.TestData.IconUrl);
            Assert.True(_testData.TestData.IconUrl.IsFile);
            Assert.True(_testData.TestData.IconUrl.IsAbsoluteUri);
            Assert.NotNull(_testData.TestData.IconUrl.Fragment);
        }
    }
}
