// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;
using NuGet.Protocol.FuncTest.Helpers;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Test.Utility;
using Xunit;

namespace NuGet.Protocol.FuncTest
{
    public class FindPackageByIdResourceTests
    {
        [PackageSourceTheory]
        [PackageSourceData(TestSources.ProGet, TestSources.Klondike, TestSources.Artifactory, TestSources.MyGet)]
        public async Task FindPackageByIdResource_NormalizedVersion(string packageSource)
        {
            // Arrange
            var repo = Repository.Factory.GetCoreV3(packageSource);
            var findPackageByIdResource = await repo.GetResourceAsync<FindPackageByIdResource>();
            var logger = new TestLogger();

            using (var context = new SourceCacheContext())
            {
                context.NoCache = true;

                // Act
                var packages = await findPackageByIdResource.GetAllVersionsAsync(
                    "owin",
                    context,
                    logger,
                    CancellationToken.None);

                // Assert
                Assert.Single(packages);
                Assert.Equal("1.0", packages.Single().ToString());
            }
        }

        [PackageSourceTheory]
        [PackageSourceData(TestSources.ProGet, TestSources.Klondike, TestSources.Artifactory, TestSources.MyGet)]
        public async Task FindPackageByIdResource_NoDependencyVersion(string packageSource)
        {
            // Arrange
            var repo = Repository.Factory.GetCoreV3(packageSource);
            var findPackageByIdResource = await repo.GetResourceAsync<FindPackageByIdResource>();
            var logger = new TestLogger();

            using (var context = new SourceCacheContext())
            {
                context.NoCache = true;

                // Act
                var packages = await findPackageByIdResource.GetAllVersionsAsync(
                    "costura.fody",
                    context,
                    logger,
                    CancellationToken.None);

                // Assert
                Assert.Single(packages);
                Assert.Equal("1.3.3.0", packages.Single().ToString());
            }
        }

        [PackageSourceTheory]
        [PackageSourceData(TestSources.ProGet, TestSources.Klondike, TestSources.Artifactory, TestSources.MyGet)]
        public async Task FindPackageByIdResource_Basic(string packageSource)
        {
            // Arrange
            var repo = Repository.Factory.GetCoreV3(packageSource);
            var findPackageByIdResource = await repo.GetResourceAsync<FindPackageByIdResource>();
            var logger = new TestLogger();

            using (var context = new SourceCacheContext())
            {
                context.NoCache = true;

                // Act
                var packages = await findPackageByIdResource.GetAllVersionsAsync(
                    "Newtonsoft.json",
                    context,
                    logger,
                    CancellationToken.None);

                // Assert
                Assert.Single(packages);
                Assert.Equal("8.0.3", packages.Single().ToString());
            }
        }

        [PackageSourceTheory]
        [PackageSourceData(TestSources.NuGetServer, TestSources.VSTS)]
        public async Task FindPackageByIdResource_Credential(PackageSource packageSource)
        {
            // Arrange
            var repo = Repository.Factory.GetCoreV2(packageSource);
            var findPackageByIdResource = await repo.GetResourceAsync<FindPackageByIdResource>();
            var logger = new TestLogger();

            using (var context = new SourceCacheContext())
            {
                context.NoCache = true;

                // Act
                var packages = await findPackageByIdResource.GetAllVersionsAsync(
                    "Newtonsoft.json",
                    context,
                    logger,
                    CancellationToken.None);

                // Assert
                Assert.Single(packages);
                Assert.Equal("8.0.3", packages.Single().ToString());
            }
        }

        [PackageSourceTheory]
        [PackageSourceData(TestSources.NuGetServer, TestSources.VSTS)]
        public async Task FindPackageByIdResource_CredentialNoDependencyVersion(PackageSource packageSource)
        {
            // Arrange
            var repo = Repository.Factory.GetCoreV2(packageSource);
            var findPackageByIdResource = await repo.GetResourceAsync<FindPackageByIdResource>();
            var logger = new TestLogger();

            using (var context = new SourceCacheContext())
            {
                context.NoCache = true;

                // Act
                var packages = await findPackageByIdResource.GetAllVersionsAsync(
                    "costura.fody",
                    context,
                    logger,
                    CancellationToken.None);

                // Assert
                Assert.Single(packages);
                Assert.Equal("1.3.3.0", packages.Single().ToString());
            }
        }

        [PackageSourceTheory]
        [PackageSourceData(TestSources.NuGetServer, TestSources.VSTS)]
        public async Task FindPackageByIdResource_CredentialNormalizedVersion(PackageSource packageSource)
        {
            // Arrange
            var repo = Repository.Factory.GetCoreV2(packageSource);
            var findPackageByIdResource = await repo.GetResourceAsync<FindPackageByIdResource>();
            var logger = new TestLogger();

            using (var context = new SourceCacheContext())
            {
                context.NoCache = true;

                // Act
                var packages = await findPackageByIdResource.GetAllVersionsAsync(
                    "owin",
                    context,
                    logger,
                    CancellationToken.None);

                // Assert
                Assert.Single(packages);
                Assert.Equal("1.0", packages.Single().ToString());
            }
        }

        [Fact]
        public async Task CopyNupkgToStreamAsync_TimeoutOnFirstAttempt_FileSizeCorrectAfterRetry()
        {
            // Arrange

            // First create a mock package source with a package we can download
            using TestDirectory testDirectory = TestDirectory.Create();

            var packages = new List<string>();

            const string packageId = "packageId";
            const string packageVersionString = "1.0.0";
            FileInfo packageFileInfo = TestPackagesGroupedByFolder.GetLargePackage(testDirectory, packageId, packageVersionString);
            packages.Add(packageFileInfo.FullName);

            // Make sure it's the package source that times out every second nupkg download attempt
            NupkgDownloadTimeoutHttpClientHandler timeoutHandler = new NupkgDownloadTimeoutHttpClientHandler(packages);
            var source = MockSourceRepository.Create(timeoutHandler);

            // Now arrange the NuGet Client SDK experience
            var protocolResource = await source.GetResourceAsync<FindPackageByIdResource>();

            using (var destination = new MemoryStream())
            {
                var scc = new SourceCacheContext()
                {
                    DirectDownload = true,
                    NoCache = true
                };
                NuGetVersion packageVersion = NuGetVersion.Parse(packageVersionString);

                // Act
                var result = await protocolResource.CopyNupkgToStreamAsync(packageId, packageVersion, destination, scc, NullLogger.Instance, CancellationToken.None);

                // Assert
                Assert.True(result);
                Assert.Equal(1, timeoutHandler.FailedDownloads);
                Assert.Equal(packageFileInfo.Length, destination.Length);
            }
        }
    }
}
