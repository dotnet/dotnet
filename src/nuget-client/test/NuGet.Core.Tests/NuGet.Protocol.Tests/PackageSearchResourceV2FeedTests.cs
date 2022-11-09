// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Protocol.Core.Types;
using Test.Utility;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class PackageSearchResourceV2FeedTests
    {
        [Fact]
        public async Task PackageSearchResourceV2Feed_Basic()
        {
            // Arrange
            var serviceAddress = ProtocolUtility.CreateServiceAddress();

            var responses = new Dictionary<string, string>();
            responses.Add(
                serviceAddress + "Search()?$filter=IsLatestVersion&searchTerm='azure'&targetFramework='net40-client'" +
                "&includePrerelease=false&$skip=0&$top=1&semVerLevel=2.0.0",
                ProtocolUtility.GetResource("NuGet.Protocol.Tests.compiler.resources.AzureSearch.xml", GetType()));
            responses.Add(serviceAddress, string.Empty);

            var repo = StaticHttpHandler.CreateSource(serviceAddress, Repository.Provider.GetCoreV3(), responses);

            var packageSearchResource = await repo.GetResourceAsync<PackageSearchResource>();

            var searchFilter = new SearchFilter(includePrerelease: false)
            {
                SupportedFrameworks = new string[] { "net40-Client" }
            };

            // Act
            var searchResult = await packageSearchResource.SearchAsync("azure", searchFilter, 0, 1, NullLogger.Instance, CancellationToken.None);
            var package = searchResult.FirstOrDefault();

            // Assert
            Assert.Equal(1, searchResult.Count());
            Assert.Equal("WindowsAzure.Storage", package.Title);
            Assert.Equal("Microsoft", package.Authors);
            Assert.Equal("", package.Owners);
            Assert.True(package.Description.StartsWith("This client library enables"));
            Assert.Equal(3957668, package.DownloadCount);
            Assert.Equal("http://go.microsoft.com/fwlink/?LinkID=288890", package.IconUrl.AbsoluteUri);
            Assert.Equal("http://go.microsoft.com/fwlink/?LinkId=331471", package.LicenseUrl.AbsoluteUri);
            Assert.Equal("http://go.microsoft.com/fwlink/?LinkId=235168", package.ProjectUrl.AbsoluteUri);
            Assert.Equal(DateTimeOffset.Parse("2015-12-10T22:39:05.103"), package.Published.Value);
            Assert.Equal("https://www.nuget.org/package/ReportAbuse/WindowsAzure.Storage/6.2.0", package.ReportAbuseUrl.AbsoluteUri);
            Assert.True(package.RequireLicenseAcceptance);
            Assert.Equal("A client library for working with Microsoft Azure storage services including blobs, files, tables, and queues.", package.Summary);
            Assert.Equal("Microsoft Azure Storage Table Blob File Queue Scalable windowsazureofficial", package.Tags);
            Assert.Equal(4, package.DependencySets.Count());
            Assert.Equal("net40-client", package.DependencySets.First().TargetFramework.GetShortFolderName());
        }

        [Fact]
        public async Task PackageSearchResourceV2Feed_Search100()
        {
            // Arrange
            var serviceAddress = ProtocolUtility.CreateServiceAddress();

            var responses = new Dictionary<string, string>();
            responses.Add(
                serviceAddress + "Search()?$filter=IsLatestVersion&searchTerm='azure'&targetFramework='net40-client'" +
                "&includePrerelease=false&$skip=0&$top=100&semVerLevel=2.0.0",
                ProtocolUtility.GetResource("NuGet.Protocol.Tests.compiler.resources.AzureSearch100.xml", GetType()));
            responses.Add(serviceAddress, string.Empty);

            var repo = StaticHttpHandler.CreateSource(serviceAddress, Repository.Provider.GetCoreV3(), responses);

            var packageSearchResource = await repo.GetResourceAsync<PackageSearchResource>();

            var searchFilter = new SearchFilter(includePrerelease: false)
            {
                SupportedFrameworks = new string[] { "net40-Client" }
            };

            // Act
            var searchResult = await packageSearchResource.SearchAsync("azure", searchFilter, 0, 100, NullLogger.Instance, CancellationToken.None);

            // Assert
            Assert.Equal(100, searchResult.Count());
        }
    }
}
