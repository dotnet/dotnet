// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Configuration;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Test.Utility;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class GetDownloadResultUtilityTests
    {
        [Fact]
        public async Task GetDownloadResultUtility_WithSingleDirectDownload_ReturnsTemporaryDownloadResult()
        {
            // Arrange
            var uri = new Uri("http://fake/content.nupkg");
            var identity = new PackageIdentity("PackageA", NuGetVersion.Parse("1.0.0-Beta"));
            var expectedContent = await GetPackageContentAsync(identity);
            var testHttpSource = new TestHttpSource(
                new PackageSource("http://fake"),
                new Dictionary<string, Func<HttpRequestMessage, Task<HttpResponseMessage>>>
                {
                    {
                        uri.ToString(),
                        _ => Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                        {
                            Content = new ByteArrayContent(expectedContent)
                        })
                    }
                });
            var logger = new TestLogger();
            var token = CancellationToken.None;

            using (var cacheContext = new SourceCacheContext())
            using (var downloadDirectory = TestDirectory.Create())
            using (var globalPackagesFolder = TestDirectory.Create())
            {
                var downloadContext = new PackageDownloadContext(
                    cacheContext,
                    downloadDirectory,
                    directDownload: true);

                // Act
                using (var result = await GetDownloadResultUtility.GetDownloadResultAsync(
                    testHttpSource,
                    identity,
                    uri,
                    downloadContext,
                    globalPackagesFolder,
                    logger,
                    token))
                {
                    // Assert
                    Assert.Null(result.PackageReader);
                    Assert.Equal(DownloadResourceResultStatus.Available, result.Status);
                    Assert.NotNull(result.PackageStream);
                    Assert.True(result.PackageStream.CanSeek);
                    Assert.True(result.PackageStream.CanRead);
                    Assert.Equal(0, result.PackageStream.Position);

                    var files = Directory.EnumerateFileSystemEntries(downloadDirectory).ToArray();
                    Assert.Equal(1, files.Length);
                    Assert.EndsWith(".nugetdirectdownload", Path.GetFileName(files[0]));

                    Assert.Equal(0, Directory.EnumerateFileSystemEntries(globalPackagesFolder).Count());

                    var memoryStream = new MemoryStream();
                    result.PackageStream.CopyTo(memoryStream);
                    byte[] actualContent = memoryStream.ToArray();
                    Assert.Equal(expectedContent, actualContent);
                }

                Assert.Equal(0, Directory.EnumerateFileSystemEntries(downloadDirectory).Count());
                Assert.Equal(0, Directory.EnumerateFileSystemEntries(globalPackagesFolder).Count());
            }
        }

        [Fact]
        public async Task GetDownloadResultUtility_WithTwoOfTheSameDownloads_DoesNotCollide()
        {
            // Arrange
            var uri = new Uri("http://fake/content.nupkg");
            var identity = new PackageIdentity("PackageA", NuGetVersion.Parse("1.0.0-Beta"));
            var expectedContent = await GetPackageContentAsync(identity);
            var testHttpSource = new TestHttpSource(
                new PackageSource("http://fake"),
                new Dictionary<string, Func<HttpRequestMessage, Task<HttpResponseMessage>>>
                {
                    {
                        uri.ToString(),
                        _ => Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                        {
                            Content = new ByteArrayContent(expectedContent)
                        })
                    }
                });
            var logger = new TestLogger();
            var token = CancellationToken.None;

            using (var cacheContext = new SourceCacheContext())
            using (var downloadDirectory = TestDirectory.Create())
            using (var globalPackagesFolder = TestDirectory.Create())
            {
                var downloadContext = new PackageDownloadContext(
                    cacheContext,
                    downloadDirectory,
                    directDownload: true);

                // Act
                using (var resultA = await GetDownloadResultUtility.GetDownloadResultAsync(
                    testHttpSource,
                    identity,
                    uri,
                    downloadContext,
                    globalPackagesFolder,
                    logger,
                    token))
                using (var resultB = await GetDownloadResultUtility.GetDownloadResultAsync(
                    testHttpSource,
                    identity,
                    uri,
                    downloadContext,
                    globalPackagesFolder,
                    logger,
                    token))
                {
                    // Assert
                    var files = Directory.EnumerateFileSystemEntries(downloadDirectory).ToArray();
                    Assert.Equal(2, files.Length);
                    Assert.EndsWith(".nugetdirectdownload", Path.GetFileName(files[0]));
                    Assert.EndsWith(".nugetdirectdownload", Path.GetFileName(files[1]));

                    var memory = new MemoryStream();
                    resultA.PackageStream.CopyTo(memory);
                    var actualContentA = memory.ToArray();
                    Assert.Equal(expectedContent, actualContentA);

                    memory.Seek(0, SeekOrigin.Begin);
                    memory.SetLength(0);
                    resultB.PackageStream.CopyTo(memory);
                    var actualContentB = memory.ToArray();
                    Assert.Equal(expectedContent, actualContentB);
                }

                Assert.Equal(0, Directory.EnumerateFileSystemEntries(downloadDirectory).Count());
                Assert.Equal(0, Directory.EnumerateFileSystemEntries(globalPackagesFolder).Count());
            }
        }

        [Fact]
        public void GetDownloadResultUtility_OnlyCleansUpDirectDownloadFilesItCanAccess()
        {
            // Arrange
            using (var cacheContext = new SourceCacheContext())
            using (var downloadDirectory = TestDirectory.Create())
            {
                var downloadContext = new PackageDownloadContext(
                    cacheContext,
                    downloadDirectory,
                    directDownload: true);

                var deletePathA = Path.Combine(downloadDirectory, "packageA.nugetdirectdownload");
                File.WriteAllText(deletePathA, string.Empty);

                var deletePathB = Path.Combine(downloadDirectory, "packageB.nugetdirectdownload");
                File.WriteAllText(deletePathB, string.Empty);

                // Ignored because it's in a child directory.
                var nestedPath = Path.Combine(downloadDirectory, "nested", "packageB.nugetdirectdownload");
                Directory.CreateDirectory(Path.GetDirectoryName(nestedPath));
                File.WriteAllText(nestedPath, string.Empty);

                // Ignored because it doesn't match the file pattern.
                var ignorePath = Path.Combine(downloadDirectory, "something-else-entirely");
                File.WriteAllText(ignorePath, string.Empty);

                // Act
                GetDownloadResultUtility.CleanUpDirectDownloads(downloadContext);

                // Assert
                Assert.False(File.Exists(deletePathA), "The first direct download file should be deleted.");
                Assert.False(File.Exists(deletePathB), "The second direct download file should be deleted.");
                Assert.True(File.Exists(nestedPath), "Nested files should not be deleted.");
                Assert.True(File.Exists(ignorePath), "Files with the wrong extension should not be deleted.");
            }
        }

        private async Task<byte[]> GetPackageContentAsync(PackageIdentity identity)
        {
            var packageContext = new SimpleTestPackageContext(identity);
            var memoryStream = await packageContext.CreateAsStreamAsync();
            byte[] bytes = memoryStream.ToArray();
            return bytes;
        }
    }
}
