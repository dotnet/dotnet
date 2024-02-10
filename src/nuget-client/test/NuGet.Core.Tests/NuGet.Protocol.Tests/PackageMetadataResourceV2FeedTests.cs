// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.Common;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using Test.Utility;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class PackageMetadataResourceV2FeedTests
    {
        [Fact]
        public async Task PackageMetadataResource_Basic()
        {
            // Arrange
            var serviceAddress = ProtocolUtility.CreateServiceAddress();

            var responses = new Dictionary<string, string>();
            responses.Add(serviceAddress + "FindPackagesById()?id='WindowsAzure.Storage'&semVerLevel=2.0.0",
                 ProtocolUtility.GetResource("NuGet.Protocol.Tests.compiler.resources.WindowsAzureStorageFindPackagesById.xml", GetType()));
            responses.Add(serviceAddress, string.Empty);

            var repo = StaticHttpHandler.CreateSource(serviceAddress, Repository.Provider.GetCoreV3(), responses);

            var packageMetadataResource = await repo.GetResourceAsync<PackageMetadataResource>();

            // Act
            var metadata = await packageMetadataResource.GetMetadataAsync("WindowsAzure.Storage", true, false, NullSourceCacheContext.Instance, NullLogger.Instance, CancellationToken.None);
            var latestPackage = metadata.OrderByDescending(e => e.Identity.Version, VersionComparer.VersionRelease).FirstOrDefault();

            // Assert
            Assert.Equal(44, metadata.Count());

            Assert.Equal("WindowsAzure.Storage", latestPackage.Identity.Id);
            Assert.Equal("6.2.2-preview", latestPackage.Identity.Version.ToNormalizedString());
            Assert.Equal("WindowsAzure.Storage", latestPackage.Title);
            Assert.Equal("Microsoft", latestPackage.Authors);
            Assert.Equal("", latestPackage.Owners);
            Assert.StartsWith("This client library enables", latestPackage.Description);
            Assert.Equal(3957668, latestPackage.DownloadCount);
            Assert.Equal("http://go.microsoft.com/fwlink/?LinkID=288890", latestPackage.IconUrl.AbsoluteUri);
            Assert.Equal("http://go.microsoft.com/fwlink/?LinkId=331471", latestPackage.LicenseUrl.AbsoluteUri);
            Assert.Equal("http://go.microsoft.com/fwlink/?LinkId=235168", latestPackage.ProjectUrl.AbsoluteUri);
            Assert.Equal(DateTimeOffset.Parse("2015-12-11T01:25:11.37"), latestPackage.Published.Value);
            Assert.Equal("https://www.nuget.org/package/ReportAbuse/WindowsAzure.Storage/6.2.2-preview", latestPackage.ReportAbuseUrl.AbsoluteUri);
            Assert.True(latestPackage.RequireLicenseAcceptance);
            Assert.Equal("A client library for working with Microsoft Azure storage services including blobs, files, tables, and queues.", latestPackage.Summary);
            Assert.Equal("Microsoft Azure Storage Table Blob File Queue Scalable windowsazureofficial", latestPackage.Tags);
            Assert.Equal(6, latestPackage.DependencySets.Count());
            Assert.Equal("dotnet54", latestPackage.DependencySets.First().TargetFramework.GetShortFolderName());
        }

        [Fact]
        public async Task PackageMetadataResource_UsesReferenceCache()
        {
            // Arrange
            var serviceAddress = ProtocolUtility.CreateServiceAddress();

            var responses = new Dictionary<string, string>();
            responses.Add(serviceAddress + "FindPackagesById()?id='afine'&semVerLevel=2.0.0",
                ProtocolUtility.GetResource("NuGet.Protocol.Tests.compiler.resources.FindPackagesByIdWithDuplicateBesidesVersion.xml", GetType()));
            responses.Add(serviceAddress, string.Empty);

            var repo = StaticHttpHandler.CreateSource(serviceAddress, Repository.Provider.GetCoreV3(), responses);

            var packageMetadataResource = await repo.GetResourceAsync<PackageMetadataResource>();

            // Act
            var metadata = await packageMetadataResource.GetMetadataAsync("afine", true, false, NullSourceCacheContext.Instance, NullLogger.Instance, CancellationToken.None);

            var first = metadata.ElementAt(0);
            var second = metadata.ElementAt(1);

            // Assert
            MetadataReferenceCacheTestUtility.AssertPackagesHaveSameReferences(first, second);
        }

        [Fact]
        public async Task PackageMetadataResource_NotFound()
        {
            // Arrange
            var serviceAddress = ProtocolUtility.CreateServiceAddress();

            var responses = new Dictionary<string, string>();
            responses.Add(serviceAddress + "FindPackagesById()?id='not-found'&semVerLevel=2.0.0",
                 ProtocolUtility.GetResource("NuGet.Protocol.Tests.compiler.resources.NotFoundFindPackagesById.xml", GetType()));
            responses.Add(serviceAddress, string.Empty);

            var repo = StaticHttpHandler.CreateSource(serviceAddress, Repository.Provider.GetCoreV3(), responses);

            var packageMetadataResource = await repo.GetResourceAsync<PackageMetadataResource>();

            // Act
            var metadata = await packageMetadataResource.GetMetadataAsync("not-found", true, false, NullSourceCacheContext.Instance, NullLogger.Instance, CancellationToken.None);

            // Assert
            Assert.Equal(0, metadata.Count());
        }

        [Fact]
        public async Task PackageMetadataResource_PackageIdentity()
        {
            // Arrange
            var serviceAddress = ProtocolUtility.CreateServiceAddress();

            var responses = new Dictionary<string, string>();
            responses.Add(serviceAddress + "Packages(Id='WindowsAzure.Storage',Version='4.3.2-preview')",
                 ProtocolUtility.GetResource("NuGet.Protocol.Tests.compiler.resources.WindowsAzureStorageGetPackages.xml", GetType()));
            responses.Add(serviceAddress, string.Empty);

            var repo = StaticHttpHandler.CreateSource(serviceAddress, Repository.Provider.GetCoreV3(), responses);

            var packageMetadataResource = await repo.GetResourceAsync<PackageMetadataResource>();

            var packageIdentity = new PackageIdentity("WindowsAzure.Storage", new NuGetVersion("4.3.2-preview"));

            // Act
            var metadata = await packageMetadataResource.GetMetadataAsync(packageIdentity, NullSourceCacheContext.Instance, NullLogger.Instance, CancellationToken.None);

            // Assert
            Assert.Equal("WindowsAzure.Storage", metadata.Identity.Id);
            Assert.Equal("4.3.2-preview", metadata.Identity.Version.ToNormalizedString());
        }

        [Fact]
        public async Task PackageMetadataResource_PackageIdentity_NotFound()
        {
            // Arrange
            var serviceAddress = ProtocolUtility.CreateServiceAddress();

            var responses = new Dictionary<string, string>();
            responses.Add(serviceAddress + "FindPackagesById()?id='WindowsAzure.Storage'&semVerLevel=2.0.0",
                 ProtocolUtility.GetResource("NuGet.Protocol.Tests.compiler.resources.NotFoundFindPackagesById.xml", GetType()));
            responses.Add(serviceAddress + "Packages(Id='WindowsAzure.Storage',Version='0.0.0')",
                 ProtocolUtility.GetResource("NuGet.Protocol.Tests.compiler.resources.NotFoundFindPackagesById.xml", GetType()));
            responses.Add(serviceAddress, string.Empty);

            var repo = StaticHttpHandler.CreateSource(serviceAddress, Repository.Provider.GetCoreV3(), responses);

            var packageMetadataResource = await repo.GetResourceAsync<PackageMetadataResource>();

            var packageIdentity = new PackageIdentity("WindowsAzure.Storage", new NuGetVersion("0.0.0"));

            // Act
            var metadata = await packageMetadataResource.GetMetadataAsync(packageIdentity, NullSourceCacheContext.Instance, NullLogger.Instance, CancellationToken.None);

            // Assert
            Assert.Null(metadata);
        }
    }
}
