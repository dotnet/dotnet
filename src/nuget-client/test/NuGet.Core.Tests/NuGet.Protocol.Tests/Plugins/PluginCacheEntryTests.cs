// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NuGet.Protocol.Plugins;
using NuGet.Shared;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Protocol.Tests.Plugins
{
    [Collection(nameof(NotThreadSafeResourceCollection))]
    public class PluginCacheEntryTests
    {
        [Fact]
        public void PluginCacheEntry_DoesNotThrowWithNoFile()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                var entry = new PluginCacheEntry(testDirectory.Path, "a", "b");
                entry.LoadFromFile();
                Assert.Null(entry.OperationClaims);
            }
        }

        [Fact]
        public void PluginCacheEntry_UsesShorterPaths()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                var pluginPath = @"C:\Users\Roki2\.nuget\plugins\netfx\CredentialProvider.Microsoft\CredentialProvider.Microsoft.exe";
                var url = @"https:\\nugetsspecialfeed.pkgs.visualstudio.com\packaging\ea8caa50-9cf8-4ed7-b410-5bca3b71ec1c\nuget\v3\index.json";

                var entry = new PluginCacheEntry(testDirectory.Path, pluginPath, url);
                entry.LoadFromFile();

                // This is the length of two truncated hashes and two "\" and ".dat", so it's 40 * 2 + 6 = 86
                Assert.Equal(86, entry.CacheFileName.Length - testDirectory.Path.Length);
                // This makes it about as long as http cache which is more important.
                // The http cache is 40 + 1 + [1,32] + packageName
                Assert.True(200 > entry.CacheFileName.Length, "The cache file should be short");
            }
        }

        [Theory]
        [MemberData(nameof(GetsRoundTripsValuesData))]
        public async Task PluginCacheEntry_RoundTripsValuesAsync(string[] values)
        {
            var list = new List<OperationClaim>();
            foreach (var val in values)
            {
                Enum.TryParse(val, out OperationClaim result);
                list.Add(result);
            }

            using (var testDirectory = TestDirectory.Create())
            {
                var entry = new PluginCacheEntry(testDirectory.Path, "a", "b");
                entry.LoadFromFile();
                entry.OperationClaims = list;
                await entry.UpdateCacheFileAsync();

                var newEntry = new PluginCacheEntry(testDirectory.Path, "a", "b");
                newEntry.LoadFromFile();

                Assert.True(EqualityUtility.SequenceEqualWithNullCheck(entry.OperationClaims, newEntry.OperationClaims));
            }
        }

        [Fact]
        public async Task PluginCacheEntry_DoesNotDeleteAnOpenedFile()
        {
            var list = new List<OperationClaim>() { OperationClaim.Authentication };

            using (var testDirectory = TestDirectory.Create())
            {
                var entry = new PluginCacheEntry(testDirectory.Path, "a", "b");
                entry.LoadFromFile();
                entry.OperationClaims = list;
                await entry.UpdateCacheFileAsync();

                var CacheFileName = Path.Combine(
                    Path.Combine(testDirectory.Path, CachingUtility.RemoveInvalidFileNameChars(CachingUtility.ComputeHash("a", false))),
                    CachingUtility.RemoveInvalidFileNameChars(CachingUtility.ComputeHash("b", false)) + ".dat");

                Assert.True(File.Exists(CacheFileName));

                using (var fileStream = new FileStream(
                   CacheFileName,
                   FileMode.Open,
                   FileAccess.ReadWrite,
                   FileShare.None,
                   CachingUtility.BufferSize,
                   useAsync: true))
                {
                    list.Add(OperationClaim.DownloadPackage);
                    entry.OperationClaims = list;
                    await entry.UpdateCacheFileAsync(); // this should not update
                }

                entry.LoadFromFile();
                Assert.True(EqualityUtility.SequenceEqualWithNullCheck(entry.OperationClaims, new List<OperationClaim>() { OperationClaim.Authentication }));
            }
        }

        public static IEnumerable<object[]> GetsRoundTripsValuesData()
        {
            yield return new object[] { new string[] { "Authentication", "DownloadPackage" } };
            yield return new object[] { new string[] { "Authentication" } };
            yield return new object[] { new string[] { "DownloadPackage" } };
            yield return new object[] { new string[] { } };
        }
    }
}
