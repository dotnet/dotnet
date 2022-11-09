// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Protocol.Core.Types.Tests
{
    public class PackageSearchMetadataBuilderTests : IClassFixture<LocalPackageSearchMetadataFixture>
    {
        private readonly LocalPackageSearchMetadataFixture _testData;

        public PackageSearchMetadataBuilderTests(LocalPackageSearchMetadataFixture testData)
        {
            _testData = testData;
        }

        [Fact]
        public void LocalPackageInfo_NotNull()
        {
            var copy1 = PackageSearchMetadataBuilder
                .FromMetadata(_testData.TestData)
                .Build();
            Assert.True(copy1 is PackageSearchMetadataBuilder.ClonedPackageSearchMetadata);

            var clone1 = (PackageSearchMetadataBuilder.ClonedPackageSearchMetadata)copy1;

            var copy2 = PackageSearchMetadataBuilder
                .FromMetadata(copy1)
                .Build();
            Assert.True(copy2 is PackageSearchMetadataBuilder.ClonedPackageSearchMetadata);

            var clone2 = (PackageSearchMetadataBuilder.ClonedPackageSearchMetadata)copy2;
            Assert.NotNull(clone2.PackagePath);
            Assert.Equal(clone1.PackagePath, clone2.PackagePath);
        }
    }
}
