// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using System.Reflection;
using System.Text;
using FluentAssertions;
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

        [Fact]
        public void CacheStrings_DeduplicatesStringsOnClonedMetadata()
        {
            // Arrange
            var cache = new MetadataReferenceCache();
            var metadata1 = new PackageSearchMetadataBuilder.ClonedPackageSearchMetadata
            {
                Authors = new StringBuilder().Append("Microsoft").ToString(),
                Description = "desc",
            };
            var metadata2 = new PackageSearchMetadataBuilder.ClonedPackageSearchMetadata
            {
                Authors = new StringBuilder().Append("Microsoft").ToString(),
                Description = "other",
            };
            Assert.NotSame(metadata1.Authors, metadata2.Authors);

            // Act
            metadata1.CacheStrings(cache);
            metadata2.CacheStrings(cache);

            // Assert
            Assert.Same(metadata1.Authors, metadata2.Authors);
        }

        [Fact]
        public void Verify_AllStringProperties_AccountedInCacheStrings()
        {
            var stringProperties = typeof(PackageSearchMetadataBuilder.ClonedPackageSearchMetadata)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.PropertyType == typeof(string))
                .ToArray();

            stringProperties.Should().HaveCount(8,
                $"the number of string properties changed in ClonedPackageSearchMetadata " +
                $"[{string.Join(", ", stringProperties.Select(p => p.Name))}]. " +
                "Please make sure this change is accounted for in the CacheStrings method");
        }
    }
}
