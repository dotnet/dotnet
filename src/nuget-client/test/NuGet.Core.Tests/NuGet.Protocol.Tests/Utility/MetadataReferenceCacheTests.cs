// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Text;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class MetadataReferenceCacheTests
    {
        [Fact]
        public void MetadataReferenceCache_ReturnsCachedString()
        {
            // Arrange
            var hello = "hello";
            var there = " there";

            // Using StringBuilder because if assigning to "hello there" directly results in a constant that is an equal reference.
            var string1 = new StringBuilder().Append(hello).Append(there).ToString();
            var string2 = new StringBuilder().Append(hello).Append(there).ToString();

            var cache = new MetadataReferenceCache();

            // Act
            var cachedString1 = cache.GetString(string1);
            var cachedString2 = cache.GetString(string2);

            // Assert
            Assert.Equal(string1, string2);
            Assert.NotSame(string1, string2);
            Assert.Same(cachedString1, cachedString2);
        }

        [Fact]
        public void MetadataReferenceCache_ParsesVersion()
        {
            // Arrange
            var version = new NuGetVersion(3, 2, 1);

            var cache = new MetadataReferenceCache();

            // Act
            var cachedVersion = cache.GetVersion(version.ToString());

            // Assert
            Assert.Equal(version, cachedVersion);
        }

        [Fact]
        public void MetadataReferenceCache_ReturnsCachedParsedVersion()
        {
            // Arrange
            var version = new NuGetVersion(3, 2, 1);
            var versionString1 = version.ToString();
            var versionString2 = version.ToString();

            var cache = new MetadataReferenceCache();

            // Act
            var cachedVersion1 = cache.GetVersion(versionString1);
            var cachedVersion2 = cache.GetVersion(versionString2);

            // Assert
            Assert.Equal(versionString1, versionString2);
            Assert.NotSame(versionString1, versionString2);
            Assert.Same(cachedVersion1, cachedVersion2);
        }

        [Fact]
        public void MetadataReferenceCache_ReturnsCachedVersion()
        {
            // Arrange
            var version1 = "3.2.1";
            var version2 = "3.2.1";

            var cache = new MetadataReferenceCache();

            // Act
            var cachedVersion1 = cache.GetVersion(version1);
            var cachedVersion2 = cache.GetVersion(version2);

            // Assert
            Assert.Equal(version1, version2);
            Assert.Same(cachedVersion1, cachedVersion2);
        }

        [Fact]
        public void MetadataReferenceCache_ReturnsNonNormalizedVersion()
        {
            // Arrange
            var version1 = "1.0";
            var version2 = "1.0.0";
            var version3 = "1.0.0.0";

            var cache = new MetadataReferenceCache();

            // Act
            var cachedVersion1 = cache.GetVersion(version1);
            var cachedVersion2 = cache.GetVersion(version2);
            var cachedVersion3 = cache.GetVersion(version3);

            // Assert
            Assert.Equal("1.0", cachedVersion1.ToString());
            Assert.Equal("1.0.0", cachedVersion2.ToString());
            Assert.Equal("1.0.0.0", cachedVersion3.ToString());
        }
    }
}
