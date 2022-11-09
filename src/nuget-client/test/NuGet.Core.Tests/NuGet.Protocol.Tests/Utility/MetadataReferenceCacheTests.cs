// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
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

        private class ObjectCacheTest
        {
            // I'm using private setters in this class because we call the cache on other classes with only private setters and expect it to work.

            // Using StringBuilder because if assigning to "string cached ____" directly results in a constant that is an equal reference.
            public static string TestStringCachedBefore => new StringBuilder().Append("string").Append(" cached").Append(" before").ToString();
            public static string TestStringCachedDuring => new StringBuilder().Append("string").Append(" cached").Append(" during").ToString();
            public static string TestStringCachedOnce => new StringBuilder().Append("string").Append(" cached").Append(" never").ToString();

            public string StringCachedBefore { get; private set; } = TestStringCachedBefore;
            public string StringCachedDuring1 { get; private set; } = TestStringCachedDuring;
            public string StringCachedDuring2 { get; private set; } = TestStringCachedDuring;
            public string StringCachedOnce { get; private set; } = TestStringCachedOnce;

            public static NuGetVersion TestVersionCachedBefore => new NuGetVersion(9, 8, 7);
            public static NuGetVersion TestVersionCachedDuring => new NuGetVersion(8, 7, 6);
            public static NuGetVersion TestVersionCachedOnce => new NuGetVersion(7, 6, 5);

            public NuGetVersion VersionCachedBefore { get; private set; } = TestVersionCachedBefore;
            public NuGetVersion VersionCachedDuring1 { get; private set; } = TestVersionCachedDuring;
            public NuGetVersion VersionCachedDuring2 { get; private set; } = TestVersionCachedDuring;
            public NuGetVersion VersionCachedOnce { get; private set; } = TestVersionCachedOnce;

            public static DateTimeOffset TestDateTimeNeverCached => DateTimeOffset.MinValue;

            public DateTimeOffset DateTimeNeverCached { get; private set; } = TestDateTimeNeverCached;
        }

        [Fact]
        public void MetadataReferenceCache_CachesObjectCorrectly()
        {
            //// Arrange
            var objectToCache = new ObjectCacheTest();

            var cache = new MetadataReferenceCache();

            //// Act 1
            var stringCachedBefore = cache.GetString(ObjectCacheTest.TestStringCachedBefore);
            var stringCachedOnce = ObjectCacheTest.TestStringCachedOnce;

            //// Assert 1

            // Assert that all equal objects that will be cached should be:
            // 1 - Equal.
            // 2 - Not the same reference.

            // Strings
            Assert.NotSame(objectToCache.StringCachedBefore, stringCachedBefore);
            Assert.Equal(objectToCache.StringCachedBefore, stringCachedBefore);

            Assert.NotSame(objectToCache.StringCachedDuring1, objectToCache.StringCachedDuring2);
            Assert.Equal(objectToCache.StringCachedDuring1, objectToCache.StringCachedDuring2);

            Assert.NotSame(objectToCache.StringCachedOnce, stringCachedOnce);
            Assert.Equal(objectToCache.StringCachedOnce, stringCachedOnce);

            // Versions
            Assert.NotSame(objectToCache.VersionCachedDuring1, objectToCache.VersionCachedDuring2);
            Assert.Equal(objectToCache.VersionCachedDuring1, objectToCache.VersionCachedDuring2);

            //// Act 2
            var cachedObject = cache.GetObject(objectToCache);
            var cachedStringCachedOnce = cache.GetString(stringCachedOnce);

            //// Assert 2

            // Assert that all equal objects that were cached should be:
            // 1 - Equal to their original value.
            // 2 - The same reference.

            // Strings
            Assert.Same(objectToCache.StringCachedBefore, stringCachedBefore);
            Assert.Equal(objectToCache.StringCachedBefore, ObjectCacheTest.TestStringCachedBefore);

            Assert.Same(objectToCache.StringCachedDuring1, objectToCache.StringCachedDuring2);
            Assert.Equal(objectToCache.StringCachedDuring1, ObjectCacheTest.TestStringCachedDuring);

            Assert.Same(objectToCache.StringCachedOnce, cachedStringCachedOnce);
            Assert.Equal(objectToCache.StringCachedOnce, ObjectCacheTest.TestStringCachedOnce);

            // Check that uncached fields are untouched.
            Assert.Equal(objectToCache.DateTimeNeverCached, ObjectCacheTest.TestDateTimeNeverCached);
        }

        [Fact]
        public void AssertMetadataReferenceCache_CachesStringMethodCorrectly()
        {
            // Arrange
            var objectToCache = new ObjectCacheTest();
            var cache = new MetadataReferenceCache();

            // Act
            var cachedObject = cache.GetObject(objectToCache);

            // Assert
            Assert.True(ReferenceEquals(cachedObject, objectToCache));
            Assert.Equal(objectToCache.StringCachedBefore, ObjectCacheTest.TestStringCachedBefore);
            Assert.Equal(cache.CachableMethodTypes.Count(), 1);
            Assert.Equal(cache.CachableMethodTypes.Single().Key.Name, "MetadataReferenceCache");
            Assert.Equal(cache.CachableMethodTypes.Single().Value.Name, "GetString");
        }
    }
}
