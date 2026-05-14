// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using Test.Utility;
using Xunit;

namespace NuGet.Protocol.Tests
{
    [Collection(nameof(NotThreadSafeResourceCollection))]
    public class MetadataResourceV2FeedTests
    {
        [Fact]
        public async Task MetaDataResourceGetLatestVersion()
        {
            // Arrange
            var serviceAddress = ProtocolUtility.CreateServiceAddress();

            var responses = new Dictionary<string, string>();
            responses.Add(serviceAddress + "FindPackagesById()?id='WindowsAzure.Storage'&semVerLevel=2.0.0",
                 ProtocolUtility.GetResource("NuGet.Protocol.Tests.compiler.resources.WindowsAzureStorageFindPackagesById.xml", GetType()));
            responses.Add(serviceAddress, string.Empty);

            var repo = StaticHttpHandler.CreateSource(serviceAddress, Repository.Provider.GetCoreV3(), responses);

            var metadataResource = await repo.GetResourceAsync<MetadataResource>(CancellationToken.None);

            // Act
            var latestVersion = await metadataResource.GetLatestVersion("WindowsAzure.Storage", true, false, NullSourceCacheContext.Instance, NullLogger.Instance, CancellationToken.None);

            // Assert
            Assert.Equal("6.2.2-preview", latestVersion.ToNormalizedString());
        }

        [Fact]
        public async Task MetaDataResourceGetLatestVersionStable()
        {
            // Arrange
            var serviceAddress = ProtocolUtility.CreateServiceAddress();

            var responses = new Dictionary<string, string>();
            responses.Add(serviceAddress + "FindPackagesById()?id='WindowsAzure.Storage'&semVerLevel=2.0.0",
                 ProtocolUtility.GetResource("NuGet.Protocol.Tests.compiler.resources.WindowsAzureStorageFindPackagesById.xml", GetType()));
            responses.Add(serviceAddress, string.Empty);

            var repo = StaticHttpHandler.CreateSource(serviceAddress, Repository.Provider.GetCoreV3(), responses);

            var metadataResource = await repo.GetResourceAsync<MetadataResource>(CancellationToken.None);

            // Act
            var latestVersion = await metadataResource.GetLatestVersion("WindowsAzure.Storage", false, false, NullSourceCacheContext.Instance, NullLogger.Instance, CancellationToken.None);

            // Assert
            Assert.Equal("6.2.0", latestVersion.ToNormalizedString());
        }

        [Fact]
        public async Task MetaDataResourceGetVersionsStable()
        {
            // Arrange
            var serviceAddress = ProtocolUtility.CreateServiceAddress();

            var responses = new Dictionary<string, string>();
            responses.Add(serviceAddress + "FindPackagesById()?id='WindowsAzure.Storage'&semVerLevel=2.0.0",
                 ProtocolUtility.GetResource("NuGet.Protocol.Tests.compiler.resources.WindowsAzureStorageFindPackagesById.xml", GetType()));
            responses.Add(serviceAddress, string.Empty);

            var repo = StaticHttpHandler.CreateSource(serviceAddress, Repository.Provider.GetCoreV3(), responses);

            var metadataResource = await repo.GetResourceAsync<MetadataResource>(CancellationToken.None);

            // Act
            var versions = await metadataResource.GetVersions("WindowsAzure.Storage", false, false, NullSourceCacheContext.Instance, NullLogger.Instance, CancellationToken.None);

            // Assert
            Assert.Equal(34, versions.Count());
        }

        [Fact]
        public async Task MetaDataResourceGetVersions()
        {
            // Arrange
            var serviceAddress = ProtocolUtility.CreateServiceAddress();

            var responses = new Dictionary<string, string>();
            responses.Add(serviceAddress + "FindPackagesById()?id='WindowsAzure.Storage'&semVerLevel=2.0.0",
                 ProtocolUtility.GetResource("NuGet.Protocol.Tests.compiler.resources.WindowsAzureStorageFindPackagesById.xml", GetType()));
            responses.Add(serviceAddress, string.Empty);

            var repo = StaticHttpHandler.CreateSource(serviceAddress, Repository.Provider.GetCoreV3(), responses);

            var metadataResource = await repo.GetResourceAsync<MetadataResource>(CancellationToken.None);

            // Act
            var versions = await metadataResource.GetVersions("WindowsAzure.Storage", true, false, NullSourceCacheContext.Instance, NullLogger.Instance, CancellationToken.None);

            // Assert
            Assert.Equal(44, versions.Count());
        }

        [Fact]
        public async Task MetaDataResourceIdExist()
        {
            // Arrange
            var serviceAddress = ProtocolUtility.CreateServiceAddress();

            var responses = new Dictionary<string, string>();
            responses.Add(serviceAddress + "FindPackagesById()?id='WindowsAzure.Storage'&semVerLevel=2.0.0",
                 ProtocolUtility.GetResource("NuGet.Protocol.Tests.compiler.resources.WindowsAzureStorageFindPackagesById.xml", GetType()));
            responses.Add(serviceAddress, string.Empty);

            var repo = StaticHttpHandler.CreateSource(serviceAddress, Repository.Provider.GetCoreV3(), responses);

            var metadataResource = await repo.GetResourceAsync<MetadataResource>(CancellationToken.None);

            // Act
            var exist = await metadataResource.Exists("WindowsAzure.Storage", true, false, NullSourceCacheContext.Instance, NullLogger.Instance, CancellationToken.None);

            // Assert
            Assert.True(exist);
        }

        [Fact]
        public async Task MetaDataResourceIdentityExist()
        {
            // Arrange
            var serviceAddress = ProtocolUtility.CreateServiceAddress();

            var responses = new Dictionary<string, string>();
            responses.Add(
                serviceAddress + "Packages(Id='WindowsAzure.Storage',Version='4.3.2-preview')",
                ProtocolUtility.GetResource("NuGet.Protocol.Tests.compiler.resources.WindowsAzureStorageGetPackages.xml", GetType()));
            responses.Add(serviceAddress, string.Empty);

            var repo = StaticHttpHandler.CreateSource(serviceAddress, Repository.Provider.GetCoreV3(), responses);

            var metadataResource = await repo.GetResourceAsync<MetadataResource>(CancellationToken.None);

            var package = new PackageIdentity("WindowsAzure.Storage", new NuGetVersion("4.3.2-preview"));

            // Act
            var exist = await metadataResource.Exists(package, NullSourceCacheContext.Instance, NullLogger.Instance, CancellationToken.None);

            // Assert
            Assert.True(exist);
        }

        [Fact]
        public async Task MetaDataResourceGetLatestVersions()
        {
            // Arrange
            var serviceAddress = ProtocolUtility.CreateServiceAddress();

            var responses = new Dictionary<string, string>();
            responses.Add(
                serviceAddress + "FindPackagesById()?id='WindowsAzure.Storage'&semVerLevel=2.0.0",
                ProtocolUtility.GetResource("NuGet.Protocol.Tests.compiler.resources.WindowsAzureStorageFindPackagesById.xml", GetType()));
            responses.Add(
                serviceAddress + "FindPackagesById()?id='xunit'&semVerLevel=2.0.0",
                ProtocolUtility.GetResource("NuGet.Protocol.Tests.compiler.resources.XunitFindPackagesById.xml", GetType()));
            responses.Add(serviceAddress, string.Empty);

            var repo = StaticHttpHandler.CreateSource(serviceAddress, Repository.Provider.GetCoreV3(), responses);

            var metadataResource = await repo.GetResourceAsync<MetadataResource>(CancellationToken.None);

            var packageIdList = new List<string>() { "WindowsAzure.Storage", "xunit" };

            // Act
            var versions = (await metadataResource.GetLatestVersions(packageIdList, true, false, NullSourceCacheContext.Instance, NullLogger.Instance, CancellationToken.None)).ToList();

            // Assert
            Assert.Equal("WindowsAzure.Storage", versions[1].Key);
            Assert.Equal("6.2.2-preview", versions[1].Value.ToNormalizedString());
            Assert.Equal("xunit", versions[0].Key);
            Assert.Equal("2.2.0-beta1-build3239", versions[0].Value.ToNormalizedString());
        }

        [Fact]
        public async Task MetaDataResourceGetLatestVersionInvalidId()
        {
            // Arrange
            var serviceAddress = ProtocolUtility.CreateServiceAddress();

            var responses = new Dictionary<string, string>();
            responses.Add(
                serviceAddress + "FindPackagesById()?id='not-found'&semVerLevel=2.0.0",
                ProtocolUtility.GetResource("NuGet.Protocol.Tests.compiler.resources.NotFoundFindPackagesById.xml", GetType()));
            responses.Add(serviceAddress, string.Empty);

            var repo = StaticHttpHandler.CreateSource(serviceAddress, Repository.Provider.GetCoreV3(), responses);

            var metadataResource = await repo.GetResourceAsync<MetadataResource>(CancellationToken.None);

            // Act
            var latestVersion = await metadataResource.GetLatestVersion("not-found", true, false, NullSourceCacheContext.Instance, NullLogger.Instance, CancellationToken.None);

            // Assert
            Assert.Null(latestVersion);
        }

        [Fact]
        public async Task MetaDataResourceGetVersionsInvalidId()
        {
            // Arrange
            var serviceAddress = ProtocolUtility.CreateServiceAddress();

            var responses = new Dictionary<string, string>();
            responses.Add(
                serviceAddress + "FindPackagesById()?id='not-found'&semVerLevel=2.0.0",
                ProtocolUtility.GetResource("NuGet.Protocol.Tests.compiler.resources.NotFoundFindPackagesById.xml", GetType()));
            responses.Add(serviceAddress, string.Empty);

            var repo = StaticHttpHandler.CreateSource(serviceAddress, Repository.Provider.GetCoreV3(), responses);

            var metadataResource = await repo.GetResourceAsync<MetadataResource>(CancellationToken.None);

            // Act
            var versions = await metadataResource.GetVersions("not-found", true, false, NullSourceCacheContext.Instance, NullLogger.Instance, CancellationToken.None);

            // Assert
            Assert.Equal(0, versions.Count());
        }

        [Fact]
        public async Task MetaDataResourceIdentityExistInvalidIdentity()
        {
            var serviceAddress = ProtocolUtility.CreateServiceAddress();

            var responses = new Dictionary<string, string>();
            responses.Add(serviceAddress + "Packages(Id='xunit',Version='1.0.0-notfound')", string.Empty);
            responses.Add(
                serviceAddress + "FindPackagesById()?id='xunit'&semVerLevel=2.0.0",
                ProtocolUtility.GetResource("NuGet.Protocol.Tests.compiler.resources.XunitFindPackagesById.xml", GetType()));
            responses.Add(serviceAddress, string.Empty);

            var repo = StaticHttpHandler.CreateSource(serviceAddress, Repository.Provider.GetCoreV3(), responses,
                 ProtocolUtility.GetResource("NuGet.Protocol.Tests.compiler.resources.500Error.xml", GetType()));

            var metadataResource = await repo.GetResourceAsync<MetadataResource>(CancellationToken.None);

            var package = new PackageIdentity("xunit", new NuGetVersion("1.0.0-notfound"));

            // Act
            var exist = await metadataResource.Exists(package, NullSourceCacheContext.Instance, NullLogger.Instance, CancellationToken.None);

            // Assert
            Assert.False(exist);
        }

        [Fact]
        public async Task MetaDataResourceIdExistNotFoundId()
        {
            // Arrange
            var serviceAddress = ProtocolUtility.CreateServiceAddress();

            var responses = new Dictionary<string, string>();
            responses.Add(
                serviceAddress + "FindPackagesById()?id='not-found'&semVerLevel=2.0.0",
                ProtocolUtility.GetResource("NuGet.Protocol.Tests.compiler.resources.NotFoundFindPackagesById.xml", GetType()));
            responses.Add(serviceAddress, string.Empty);

            var repo = StaticHttpHandler.CreateSource(serviceAddress, Repository.Provider.GetCoreV3(), responses);

            var metadataResource = await repo.GetResourceAsync<MetadataResource>(CancellationToken.None);

            // Act
            var exist = await metadataResource.Exists("not-found", true, false, NullSourceCacheContext.Instance, NullLogger.Instance, CancellationToken.None);

            // Assert
            Assert.False(exist);
        }
    }
}
