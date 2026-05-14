// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Protocol.FuncTest
{
    public class DownloadResourceV2FeedTests
    {
        [Fact]
        public async Task DownloadResourceFromUrl()
        {
            // Arrange
            var repo = Repository.Factory.GetCoreV3(TestSources.NuGetV2Uri);

            var downloadResource = await repo.GetResourceAsync<DownloadResource>(CancellationToken.None);

            var package = new SourcePackageDependencyInfo("WindowsAzure.Storage", new NuGetVersion("6.2.0"), null, true, repo, new Uri($@"{TestSources.NuGetV2Uri}/package/WindowsAzure.Storage/6.2.0"), "");

            // Act & Assert
            using (var packagesFolder = TestDirectory.Create())
            using (var cacheContext = new SourceCacheContext())
            using (var downloadResult = await downloadResource.GetDownloadResourceResultAsync(
                package,
                new PackageDownloadContext(cacheContext),
                packagesFolder,
                NullLogger.Instance,
                CancellationToken.None))
            {
                var packageReader = downloadResult.PackageReader;
                var files = packageReader.GetFiles();

                Assert.Equal(13, files.Count());
            }
        }

        [Fact]
        public async Task DownloadResourceFromIdentity()
        {
            // Arrange
            var repo = Repository.Factory.GetCoreV3(TestSources.NuGetV2Uri);

            var downloadResource = await repo.GetResourceAsync<DownloadResource>(CancellationToken.None);

            var package = new PackageIdentity("WindowsAzure.Storage", new NuGetVersion("6.2.0"));

            // Act & Assert
            using (var packagesFolder = TestDirectory.Create())
            using (var cacheContext = new SourceCacheContext())
            using (var downloadResult = await downloadResource.GetDownloadResourceResultAsync(
                package,
                new PackageDownloadContext(cacheContext),
                packagesFolder,
                NullLogger.Instance,
                CancellationToken.None))
            {
                var packageReader = downloadResult.PackageReader;
                var files = packageReader.GetFiles();

                Assert.Equal(13, files.Count());
            }
        }

        [Fact]
        public async Task DownloadResourceFromInvalidIdInUrl()
        {
            // Arrange
            var repo = Repository.Factory.GetCoreV3(TestSources.NuGetV2Uri);

            var downloadResource = await repo.GetResourceAsync<DownloadResource>(CancellationToken.None);

            var package = new SourcePackageDependencyInfo("not-found", new NuGetVersion("6.2.0"), null, true, repo, new Uri($@"{TestSources.NuGetV2Uri}/package/not-found/6.2.0"), "");

            // Act
            using (var packagesFolder = TestDirectory.Create())
            using (var cacheContext = new SourceCacheContext())
            using (var actual = await downloadResource.GetDownloadResourceResultAsync(
                package,
                new PackageDownloadContext(cacheContext),
                packagesFolder,
                NullLogger.Instance,
                CancellationToken.None))
            {
                // Assert
                Assert.NotNull(actual);
                Assert.Equal(DownloadResourceResultStatus.NotFound, actual.Status);
            }
        }

        [Fact]
        public async Task DownloadResourceFromIdentityInvalidSource()
        {
            // Arrange
            var randomName = Guid.NewGuid().ToString();
            var repo = Repository.Factory.GetCoreV3($"https://www.{randomName}.org/api/v2/");

            // Act & Assert
            Exception ex = await Assert.ThrowsAsync<FatalProtocolException>(async () => await repo.GetResourceAsync<DownloadResource>(CancellationToken.None));

            Assert.NotNull(ex);
            Assert.Equal($"Unable to load the service index for source https://www.{randomName}.org/api/v2/.", ex.Message);
        }

        [Fact]
        public async Task PackageMetadataVersionsFromIdentity()
        {
            // Arrange
            var repo = Repository.Factory.GetCoreV3(TestSources.NuGetV2Uri);

            var packageMetadataResource = await repo.GetResourceAsync<PackageMetadataResource>(CancellationToken.None);

            var package = new PackageIdentity("WindowsAzure.Storage", new NuGetVersion("6.2.0"));

            // Act & Assert
            using (var cacheContext = new SourceCacheContext())
            {
                var packageMetadata = await packageMetadataResource.GetMetadataAsync(
                    package,
                    cacheContext,
                    NullLogger.Instance,
                    CancellationToken.None);

                Assert.NotNull(packageMetadata);
                Assert.Equal(package.Id, packageMetadata.Identity.Id);

                var versions = await packageMetadata.GetVersionsAsync();

                Assert.NotNull(versions);
                Assert.True(versions.Count() > 0);
            }
        }
    }
}
