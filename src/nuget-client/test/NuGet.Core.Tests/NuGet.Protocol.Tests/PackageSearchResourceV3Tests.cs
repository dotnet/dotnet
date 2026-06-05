// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;
using NuGet.Shared;
using Test.Utility;
using Xunit;

namespace NuGet.Protocol.Tests
{
    [Collection(nameof(NotThreadSafeResourceCollection))]
    public class PackageSearchResourceV3Tests
    {
        private static PackageSearchResourceV3 CreateSearchResource(HttpSource httpSource, Uri[] searchEndpoints, string useStj)
        {
            var envReader = new Mock<IEnvironmentVariableReader>();
            envReader.Setup(e => e.GetEnvironmentVariable(NuGetFeatureFlags.UseSystemTextJsonDeserializationEnvVar)).Returns(useStj);
            return new PackageSearchResourceV3(httpSource, searchEndpoints, envReader.Object);
        }

        [Theory]
        [InlineData("true", "EntityFrameworkSearch.json", true, true)]   // STJ path
        [InlineData("false", "EntityFrameworkSearch.json", true, true)]  // NSJ path
        [InlineData("true", "EntityFrameworkSearchWithStringTypes.json", true, false)]
        [InlineData("false", "EntityFrameworkSearchWithStringTypes.json", true, false)]
        [InlineData("true", "EntityFrameworkSearchWithoutOwner.json", false, false)]
        [InlineData("false", "EntityFrameworkSearchWithoutOwner.json", false, false)]
        public async Task PackageSearchResourceV3_GetMetadataAsync(string useStj, string jsonFileName, bool hasOwners, bool hasOwnersArray)
        {
            // Arrange
            var serviceAddress = ProtocolUtility.CreateHttpsServiceAddress();
            var searchUrl = serviceAddress + "?q=entityframework&skip=0&take=1&prerelease=false&semVerLevel=2.0.0";

            var responses = new Dictionary<string, string>
            {
                { searchUrl, ProtocolUtility.GetResource("NuGet.Protocol.Tests.compiler.resources." + jsonFileName, GetType()) },
                { serviceAddress, string.Empty }
            };

            var httpSource = new TestHttpSource(new PackageSource(serviceAddress), responses);
            var resource = CreateSearchResource(httpSource, new[] { new Uri(serviceAddress) }, useStj);

            // Act
            var packages = await resource.SearchAsync(
                "entityframework",
                new SearchFilter(false),
                skip: 0,
                take: 1,
                NullLogger.Instance,
                CancellationToken.None);

            var package = packages.SingleOrDefault();

            // Assert
            package.Should().NotBeNull();
            package.Identity.Id.Should().Be("EntityFramework");
            package.Identity.Version.OriginalVersion.Should().Be("6.4.4");
            package.Description.Should().Be("Entity Framework 6 (EF6) is a tried and tested object-relational mapper for .NET with many years of feature development and stabilization.");
            package.Summary.Should().Be(package.Description);
            package.Title.Should().Be("EntityFramework");

            package.IconUrl.AbsoluteUri.Should().Be("https://api.nuget.org/v3-flatcontainer/entityframework/6.4.4/icon");
            package.LicenseUrl.AbsoluteUri.Should().Be("https://www.nuget.org/packages/EntityFramework/6.4.4/license");
            package.ProjectUrl.AbsoluteUri.Should().Be("http://go.microsoft.com/fwlink/?LinkID=263480");

            package.IsListed.Should().BeTrue();
            package.LicenseMetadata.Should().BeNull();
            package.PackageDetailsUrl.Should().BeNull();
            package.PrefixReserved.Should().BeTrue();

            var tags = string.Join(", ",
                "Microsoft",
                "EntityFramework",
                "EF",
                "Database",
                "Data",
                "O/RM",
                "ADO.NET");
            package.Tags.Should().Be(tags);

            package.Authors.Should().Be("Microsoft");

            //we have owners, expect both list/str to be populated.
            if (hasOwners)
            {
                // we have array, expect list to contain N items
                if (hasOwnersArray)
                {
                    package.OwnersList.Should()
                        .NotBeNull()
                        .And.NotBeEmpty()
                        .And.HaveCount(3)
                        .And.ContainInOrder(["aspnet", "EntityFramework", "Microsoft"]);
                    package.Owners.Should().Be("aspnet, EntityFramework, Microsoft");
                }
                else // Data is a string
                {
                    string ownerString = "aspnet, EntityFramework, Microsoft";
                    package.OwnersList.Should().NotBeNull()
                        .And.NotBeEmpty()
                        .And.HaveCount(1)
                        .And.ContainSingle(ownerString);
                    package.Owners.Should().Be(ownerString);
                }
            }
            else // No owners data provided in response.
            {
                package.OwnersList.Should().BeNull();
                package.Owners.Should().BeNull();
            }

            package.DownloadCount.Should().Be(248620082);
            package.Published.Should().BeNull();
            package.ReadmeUrl.Should().BeNull();
            package.ReportAbuseUrl.Should().BeNull();
            package.RequireLicenseAcceptance.Should().BeFalse();
            package.Vulnerabilities.Should().BeEmpty();
        }

        [Fact]
        public async Task PackageSearchResourceV3_UsesReferenceCache()
        {
            // Arrange
            var responses = new Dictionary<string, string>();
            responses.Add("https://api-v3search-0.nuget.org/query?q=entityframework&skip=0&take=2&prerelease=false&semVerLevel=2.0.0",
                ProtocolUtility.GetResource("NuGet.Protocol.Tests.compiler.resources.SearchV3WithDuplicateBesidesVersion.json", GetType()));
            responses.Add("http://testsource.com/v3/index.json", JsonData.IndexWithoutFlatContainer);

            var repo = StaticHttpHandler.CreateSource("http://testsource.com/v3/index.json", Repository.Provider.GetCoreV3(), responses);

            var resource = await repo.GetResourceAsync<PackageSearchResource>(CancellationToken.None);

            // Act
            var packages = await resource.SearchAsync(
                "entityframework",
                new SearchFilter(false),
                skip: 0,
                take: 2,
                log: NullLogger.Instance,
                cancellationToken: CancellationToken.None);

            var first = (PackageSearchMetadataBuilder.ClonedPackageSearchMetadata)packages.ElementAt(0);
            var second = (PackageSearchMetadataBuilder.ClonedPackageSearchMetadata)packages.ElementAt(1);

            // Assert
            MetadataReferenceCacheTestUtility.AssertPackagesHaveSameReferences(first, second);
        }

        [Theory]
        [InlineData("true")]  // STJ path
        [InlineData("false")] // NSJ path
        public async Task PackageSearchResourceV3_GetMetadataAsync_NotFound(string useStj)
        {
            // Arrange
            var serviceAddress = ProtocolUtility.CreateHttpsServiceAddress();
            var searchUrl = serviceAddress + "?q=yabbadabbadoo&skip=0&take=1&prerelease=false&semVerLevel=2.0.0";

            var responses = new Dictionary<string, string>
            {
                { searchUrl, ProtocolUtility.GetResource("NuGet.Protocol.Tests.compiler.resources.EmptySearchResponse.json", GetType()) },
                { serviceAddress, string.Empty }
            };

            var httpSource = new TestHttpSource(new PackageSource(serviceAddress), responses);
            var resource = CreateSearchResource(httpSource, new[] { new Uri(serviceAddress) }, useStj);

            // Act
            var packages = await resource.SearchAsync(
                "yabbadabbadoo",
                new SearchFilter(false),
                skip: 0,
                take: 1,
                NullLogger.Instance,
                CancellationToken.None);

            // Assert
            Assert.Empty(packages);
        }

        [Theory]
        [InlineData("true")]  // STJ path
        [InlineData("false")] // NSJ path
        public async Task PackageSearchResourceV3_GetMetadataAsync_VersionsDownloadCount(string useStj)
        {
            // Arrange
            long largerThanIntMax = (long)int.MaxValue + 10;
            var serviceAddress = ProtocolUtility.CreateHttpsServiceAddress();
            var searchUrl = serviceAddress + "?q=entityframework&skip=0&take=1&prerelease=false&semVerLevel=2.0.0";

            var responses = new Dictionary<string, string>
            {
                { searchUrl, ProtocolUtility.GetResource("NuGet.Protocol.Tests.compiler.resources.EntityFrameworkSearch.json", GetType()) },
                { serviceAddress, string.Empty }
            };

            var httpSource = new TestHttpSource(new PackageSource(serviceAddress), responses);
            var resource = CreateSearchResource(httpSource, new[] { new Uri(serviceAddress) }, useStj);

            // Act
            var packages = await resource.SearchAsync(
                "entityframework",
                new SearchFilter(false),
                skip: 0,
                take: 1,
                NullLogger.Instance,
                CancellationToken.None);

            var package = packages.SingleOrDefault();

            var versions = (await package.GetVersionsAsync()).ToList();

            // Assert
            Assert.Equal(248620082, package.DownloadCount);
            Assert.Equal(17, versions.Count);
            Assert.Equal(1267747, versions[0].DownloadCount);
            // Make sure NuGet can handle package download count larger than int.MaxValue
            // EntityFrameworkSearch.json has a 2nd version with download count that is too large for an int32
            Assert.Equal(largerThanIntMax, versions[1].DownloadCount);
        }

        [Theory]
        [InlineData("true")]  // STJ path
        [InlineData("false")] // NSJ path
        public async Task PackageSearchResourceV3_SearchEncoding(string useStj)
        {
            // Arrange
            var serviceAddress = ProtocolUtility.CreateHttpsServiceAddress();

            var responses = new Dictionary<string, string>
            {
                {
                    serviceAddress + "?q=azure%20b&skip=0&take=1&prerelease=false" +
                    "&supportedFramework=.NETFramework,Version=v4.5&semVerLevel=2.0.0",
                    ProtocolUtility.GetResource("NuGet.Protocol.Tests.compiler.resources.V3Search.json", GetType())
                },
                { serviceAddress, string.Empty }
            };

            var httpSource = new TestHttpSource(new PackageSource(serviceAddress), responses);
            var resource = CreateSearchResource(httpSource, new[] { new Uri(serviceAddress) }, useStj);

            var searchFilter = new SearchFilter(includePrerelease: false)
            {
                SupportedFrameworks = new string[] { ".NETFramework,Version=v4.5" }
            };

            var skip = 0;
            var take = 1;

            // Act
            var packages = await resource.SearchAsync(
                        "azure b",
                        searchFilter,
                        skip,
                        take,
                        NullLogger.Instance,
                        CancellationToken.None);

            var packagesArray = packages.ToArray();

            // Assert
            // Verify that the url matches the one in the response dictionary
            Assert.True(packagesArray.Length > 0);
        }

        [Theory]
        [InlineData("true")]  // STJ path
        [InlineData("false")] // NSJ path
        public async Task Search_HttpResource_ThrowsAnException(string useStj)
        {
            // Arrange
            var serviceAddress = ProtocolUtility.CreateHttpsServiceAddress();
            var httpresource = ProtocolUtility.CreateServiceAddress();
            var requestUrl = httpresource + "?q=azure%20b&skip=0&take=1&prerelease=false" +
                "&supportedFramework=.NETFramework,Version=v4.5&semVerLevel=2.0.0";

            var responses = new Dictionary<string, string>
            {
                {
                    requestUrl,
                    ProtocolUtility.GetResource("NuGet.Protocol.Tests.compiler.resources.V3Search.json", GetType())
                },
                { serviceAddress, string.Empty }
            };

            var httpSource = new TestHttpSource(new PackageSource(serviceAddress), responses);

            var packageSearchResourceV3 = CreateSearchResource(httpSource, [new Uri(httpresource)], useStj);

            var searchFilter = new SearchFilter(includePrerelease: false)
            {
                SupportedFrameworks = [".NETFramework,Version=v4.5"]
            };

            var skip = 0;
            var take = 1;

            // Act
            var ex = await Assert.ThrowsAsync<HttpSourceException>(() => packageSearchResourceV3.SearchAsync(
                        "azure b",
                        searchFilter,
                        skip,
                        take,
                        NullLogger.Instance,
                        CancellationToken.None));

            // Assert
            ex.GetType().Should().Be(typeof(HttpSourceException));
            ex.Message.Should().Contain(string.Format(Protocol.Strings.Error_Insecure_HTTP, serviceAddress, requestUrl));
        }

        [Theory]
        [InlineData("true")]  // STJ path
        [InlineData("false")] // NSJ path
        public async Task Search_HttpResourceAndAllowInsecureConnections_Succeeds(string useStj)
        {
            // Arrange
            var serviceAddress = ProtocolUtility.CreateHttpsServiceAddress();
            var httpresource = ProtocolUtility.CreateServiceAddress();
            var requestUrl = httpresource + "?q=azure%20b&skip=0&take=1&prerelease=false" +
                "&supportedFramework=.NETFramework,Version=v4.5&semVerLevel=2.0.0";

            var responses = new Dictionary<string, string>
            {
                {
                    requestUrl,
                    ProtocolUtility.GetResource("NuGet.Protocol.Tests.compiler.resources.V3Search.json", GetType())
                },
                {
                    serviceAddress, string.Empty
                }
            };

            var httpSource = new TestHttpSource(
                new PackageSource(serviceAddress)
                {
                    AllowInsecureConnections = true
                },
                responses);

            var packageSearchResourceV3 = CreateSearchResource(httpSource, [new Uri(httpresource)], useStj);

            var searchFilter = new SearchFilter(includePrerelease: false)
            {
                SupportedFrameworks = [".NETFramework,Version=v4.5"]
            };

            var skip = 0;
            var take = 1;

            // Act
            var packages = await packageSearchResourceV3.SearchAsync(
                        "azure b",
                        searchFilter,
                        skip,
                        take,
                        NullLogger.Instance,
                        CancellationToken.None);

            var packagesArray = packages.ToArray();

            // Assert
            Assert.True(packagesArray.Length > 0);
        }

        [Theory]
        [InlineData("true")]  // STJ path
        [InlineData("false")] // NSJ path
        public async Task PackageSearchResourceV3_VerifyReadSyncIsNotUsed(string useStj)
        {
            // Arrange
            var serviceAddress = ProtocolUtility.CreateHttpsServiceAddress();

            var responses = new Dictionary<string, string>();
            responses.Add(
                serviceAddress + "?q=azure%20b&skip=0&take=1&prerelease=false" +
                "&supportedFramework=.NETFramework,Version=v4.5&semVerLevel=2.0.0",
                ProtocolUtility.GetResource("NuGet.Protocol.Tests.compiler.resources.V3Search.json", GetType()));
            responses.Add(serviceAddress, string.Empty);

            var httpSource = new TestHttpSource(new PackageSource(serviceAddress), responses);

            // throw if sync .Read is used
            httpSource.StreamWrapper = (stream) => new NoSyncReadStream(stream);

            var resource = CreateSearchResource(httpSource, new Uri[] { new Uri(serviceAddress) }, useStj);

            var searchFilter = new SearchFilter(includePrerelease: false)
            {
                SupportedFrameworks = new string[] { ".NETFramework,Version=v4.5" }
            };
            var skip = 0;
            var take = 1;

            // Act
            var packages = await resource.SearchAsync(
                        "azure b",
                        searchFilter,
                        skip,
                        take,
                        NullLogger.Instance,
                        CancellationToken.None);
            var packagesArray = packages.ToArray();

            // Assert
            // Verify that the url matches the one in the response dictionary
            // Verify no failures from Sync Read
            Assert.True(packagesArray.Length > 0);
        }

        [Theory]
        [InlineData("true")]  // STJ path
        [InlineData("false")] // NSJ path
        public async Task PackageSearchResourceV3_CancelledToken_ThrowsOperationCancelledException(string useStj)
        {
            // Arrange
            var serviceAddress = ProtocolUtility.CreateHttpsServiceAddress();

            var responses = new Dictionary<string, string>();
            responses.Add(
                serviceAddress + "?q=azure%20b&skip=0&take=1&prerelease=false" +
                "&supportedFramework=.NETFramework,Version=v4.5&semVerLevel=2.0.0",
                ProtocolUtility.GetResource("NuGet.Protocol.Tests.compiler.resources.V3Search.json", GetType()));
            responses.Add(serviceAddress, string.Empty);

            var httpSource = new TestHttpSource(new PackageSource(serviceAddress), responses);

            httpSource.StreamWrapper = (stream) => new NoSyncReadStream(stream);

            var packageSearchResourceV3 = CreateSearchResource(httpSource, new Uri[] { new Uri(serviceAddress) }, useStj);

            var searchFilter = new SearchFilter(includePrerelease: false)
            {
                SupportedFrameworks = new string[] { ".NETFramework,Version=v4.5" }
            };

            var tokenSource = new CancellationTokenSource();
            tokenSource.Cancel();
            var skip = 0;
            var take = 1;

            // Act/Assert
            await Assert.ThrowsAsync<TaskCanceledException>(() =>
               packageSearchResourceV3.SearchAsync(
                        "Sentry",
                        searchFilter,
                        skip,
                        take,
                        NullLogger.Instance,
                        tokenSource.Token));

        }

        [Theory]
        [InlineData("true")]  // STJ path
        [InlineData("false")] // NSJ path
        public async Task PackageSearchResourceV3_SearchAsync_AllMetadata(string useStj)
        {
            // Arrange
            var serviceAddress = ProtocolUtility.CreateHttpsServiceAddress();
            var searchUrl = serviceAddress + "?q=contoso.logging&skip=0&take=1&prerelease=false&semVerLevel=2.0.0";

            var responses = new Dictionary<string, string>
            {
                { searchUrl, ProtocolUtility.GetResource("NuGet.Protocol.Tests.compiler.resources.SearchWithAllMetadata.json", GetType()) },
                { serviceAddress, string.Empty }
            };

            var httpSource = new TestHttpSource(new PackageSource(serviceAddress), responses);
            var resource = CreateSearchResource(httpSource, new[] { new Uri(serviceAddress) }, useStj);

            // Act
            var packages = await resource.SearchAsync(
                "contoso.logging",
                new SearchFilter(false),
                skip: 0,
                take: 1,
                NullLogger.Instance,
                CancellationToken.None);

            var package = packages.SingleOrDefault();

            // Assert - basic metadata
            package.Should().NotBeNull();
            package.Identity.Id.Should().Be("Contoso.Logging");
            package.Identity.Version.OriginalVersion.Should().Be("2.0.0");

            // Assert - dependency groups
            var depSets = package.DependencySets.ToList();
            depSets.Should().HaveCount(2);

            var netStandard = depSets[0];
            netStandard.TargetFramework.GetShortFolderName().Should().Be("netstandard2.0");
            var netStandardDeps = netStandard.Packages.ToList();
            netStandardDeps.Should().HaveCount(2);
            netStandardDeps[0].Id.Should().Be("Contoso.Serialization");
            netStandardDeps[0].VersionRange.MinVersion.ToNormalizedString().Should().Be("13.0.1");
            netStandardDeps[1].Id.Should().Be("Contoso.Buffers");
            netStandardDeps[1].VersionRange.MinVersion.ToNormalizedString().Should().Be("4.5.4");

            var net6 = depSets[1];
            net6.TargetFramework.GetShortFolderName().Should().Be("net6.0");
            var net6Deps = net6.Packages.ToList();
            net6Deps.Should().HaveCount(1);
            net6Deps[0].Id.Should().Be("Contoso.Json");
            net6Deps[0].VersionRange.MinVersion.ToNormalizedString().Should().Be("6.0.0");

            // Assert - deprecation
            var deprecation = await package.GetDeprecationMetadataAsync();
            deprecation.Should().NotBeNull();
            deprecation.Message.Should().Be("This package is deprecated. Use Contoso.Logging.Modern instead.");
            deprecation.Reasons.Should().ContainInOrder("Legacy", "Other");
            deprecation.AlternatePackage.Should().NotBeNull();
            deprecation.AlternatePackage.PackageId.Should().Be("Contoso.Logging.Modern");
            deprecation.AlternatePackage.Range.MinVersion.ToNormalizedString().Should().Be("1.0.0");

            // Assert - vulnerabilities
            var vulns = package.Vulnerabilities.ToList();
            vulns.Should().HaveCount(2);
            vulns[0].AdvisoryUrl.AbsoluteUri.Should().Be("https://contoso.test/advisories/CTSO-2024-1234");
            vulns[0].Severity.Should().Be(2);
            vulns[1].AdvisoryUrl.AbsoluteUri.Should().Be("https://contoso.test/advisories/CTSO-2024-5678");
            vulns[1].Severity.Should().Be(3);

            // Assert - versions
            var versions = (await package.GetVersionsAsync()).ToList();
            versions.Should().HaveCount(2);
            versions[0].Version.ToNormalizedString().Should().Be("1.0.0");
            versions[0].DownloadCount.Should().Be(50000);
            versions[1].Version.ToNormalizedString().Should().Be("2.0.0");
            versions[1].DownloadCount.Should().Be(50000);
        }
    }
}
