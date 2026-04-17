// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Packaging.Signing;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Test.Utility;
using Xunit;

namespace NuGet.Protocol.FuncTest.Scenarios;

public class ServerReturnsWrongPackageTests(ServerReturnsWrongPackageTests.Fixture _fixture) : IClassFixture<ServerReturnsWrongPackageTests.Fixture>
{
    internal const string V2Url = "https://local.test/v2/";
    internal const string V3Url = $"{V3BaseUrl}/index.json";
    private const string V3BaseUrl = "https://local.test/v3/";
    internal const string ExpectedPackageName = "some.package";
    internal const string ActualPackageName = "another.package";

    [Theory]
    [InlineData(FeedType.HttpV3, false)]
    [InlineData(FeedType.HttpV2, false)]
    [InlineData(FeedType.HttpV3, true)]
    [InlineData(FeedType.HttpV2, true)]
    [InlineData(FeedType.FileSystemV3, false)]
    [InlineData(FeedType.FileSystemPackagesConfig, false)]
    [InlineData(FeedType.FileSystemV2, false)]
    public async Task FindPackageById_FailsToDownload(FeedType feedType, bool noHttpCache)
    {
        // Arrange
        string url = feedType switch
        {
            FeedType.HttpV3 => V3Url,
            FeedType.HttpV2 => V2Url,
            FeedType.FileSystemPackagesConfig => _fixture.LocalPackagesConfigPath,
            FeedType.FileSystemV3 => _fixture.LocalV3Path,
            FeedType.FileSystemV2 => _fixture.LocalV2Path,
            _ => throw new ArgumentException(nameof(feedType))
        };

        SourceRepository testSource = StaticHttpHandler.CreateSource(url, Repository.Provider.GetCoreV3(), _fixture.Responses);
        var findPackageByIdResource = await testSource.GetResourceAsync<FindPackageByIdResource>();

        using var destination1 = new MemoryStream();
        using var destination2 = new MemoryStream();
        using var cacheContext = new SourceCacheContext() { NoCache = noHttpCache };

        var testLogger = new TestLogger();

        // Act

        var packageVersions = await findPackageByIdResource.GetAllVersionsAsync(
            ExpectedPackageName,
            cacheContext,
            testLogger,
            CancellationToken.None);
        if (packageVersions == null || !packageVersions.Any())
        {
            // If a local source doesn't even recognise the package exists, then that's better behavior.
            return;
        }

        bool successful1 = await findPackageByIdResource.CopyNupkgToStreamAsync(
            ExpectedPackageName,
            new NuGetVersion("1.0.0"),
            destination1,
            cacheContext,
            testLogger,
            CancellationToken.None);

        // First time might populate the HTTP cache, make sure a cache hit doesn't bypass validation
        bool successful2 = await findPackageByIdResource.CopyNupkgToStreamAsync(
            ExpectedPackageName,
            new NuGetVersion("1.0.0"),
            destination2,
            cacheContext,
            testLogger,
            CancellationToken.None);

        // Assert
        using var scope = new AssertionScope();
        successful1.Should().BeFalse();
        destination1.Length.Should().Be(0);

        successful2.Should().BeFalse();
        destination2.Length.Should().Be(0);

        testLogger.ErrorMessages.Should().Contain(
            message =>
#if NET
                message.Contains(ExpectedPackageName, StringComparison.OrdinalIgnoreCase) &&
                message.Contains(ActualPackageName, StringComparison.OrdinalIgnoreCase));
#else
                message.Contains(ExpectedPackageName) &&
                message.Contains(ActualPackageName));
#endif
    }

    [Theory]
    [InlineData(FeedType.HttpV3, false, false)]
    [InlineData(FeedType.HttpV3, false, true)]
    [InlineData(FeedType.HttpV3, true, false)]
    [InlineData(FeedType.HttpV3, true, true)]
    [InlineData(FeedType.HttpV2, false, false)]
    [InlineData(FeedType.HttpV2, false, true)]
    [InlineData(FeedType.HttpV2, true, false)]
    [InlineData(FeedType.HttpV2, true, true)]
    [InlineData(FeedType.FileSystemV3, false, false)]
    [InlineData(FeedType.FileSystemV3, false, true)]
    [InlineData(FeedType.FileSystemPackagesConfig, false, false)]
    [InlineData(FeedType.FileSystemPackagesConfig, false, true)]
    [InlineData(FeedType.FileSystemV2, false, false)]
    [InlineData(FeedType.FileSystemV2, false, true)]
    public async Task DownloadResource_FailstoDownload(FeedType feedType, bool noHttpCache, bool directDownload)
    {
        // Arrange
        string url = feedType switch
        {
            FeedType.HttpV3 => V3Url,
            FeedType.HttpV2 => V2Url,
            FeedType.FileSystemV3 => _fixture.LocalV3Path,
            FeedType.FileSystemPackagesConfig => _fixture.LocalPackagesConfigPath,
            FeedType.FileSystemV2 => _fixture.LocalV2Path,
            _ => throw new ArgumentException(nameof(feedType))
        };

        SourceRepository testSource = StaticHttpHandler.CreateSource(url, Repository.Provider.GetCoreV3(), _fixture.Responses);
        var downloadResource = await testSource.GetResourceAsync<DownloadResource>();

        using var testDirectory = TestDirectory.Create();
        using var sourceCacheContext = new SourceCacheContext() { NoCache = noHttpCache };
        var packagesDirectory = Path.Combine(testDirectory, "packages");
        var globalPackagesDirectory = Path.Combine(testDirectory, "gpf");

        var downloadContext = new PackageDownloadContext(sourceCacheContext, packagesDirectory, directDownload);
        var packageIdentity = new PackageIdentity(ExpectedPackageName, NuGetVersion.Parse("1.0.0"));
        var testLogger = new TestLogger();

        // Act & Assert
        // Some local feeds use the package's nuspec identity, not the filename, and so return results saying the requested package doesn't exist.
        // In the context of this scenario, all we care about is that the wrong package isn't extracted.
        using var scope = new AssertionScope();
        Directory.EnumerateFiles(testDirectory, "*.nupkg", SearchOption.AllDirectories).Should().BeEmpty();
        try
        {
            var result = await downloadResource.GetDownloadResourceResultAsync(packageIdentity, downloadContext, globalPackagesDirectory, testLogger, CancellationToken.None);
            result.Status.Should().Be(DownloadResourceResultStatus.NotFound);
        }
        catch (Exception ex)
        {
            ex.Should().BeOfType(typeof(FatalProtocolException));
            var exceptionMessage = ExceptionUtilities.DisplayMessage(ex);
            exceptionMessage.Should().Contain(ExpectedPackageName)
                .And.Contain(ActualPackageName);
        }
    }

    public class Fixture : IAsyncLifetime
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        // Gets set during InitializeAsync
        private byte[] _packageBytes;
        internal Dictionary<string, Func<HttpRequestMessage, Task<HttpResponseMessage>>> Responses { get; private set; }
        private TestDirectory _testDirectory;
        internal string LocalV2Path { get; private set; }
        internal string LocalPackagesConfigPath { get; private set; }
        internal string LocalV3Path { get; private set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.


        public async Task InitializeAsync()
        {
            _packageBytes = await GetPackageBytesAsync(ActualPackageName);
            Responses = GetResponsesDictionary();

            _testDirectory = TestDirectory.Create();

            LocalV2Path = Path.Combine(_testDirectory, "v2");
            LocalPackagesConfigPath = Path.Combine(_testDirectory, "v2.5");
            LocalV3Path = Path.Combine(_testDirectory, "v3");

            // Extract to local v3 folder layout
            var expectedPackageIdentity = new PackageIdentity(ExpectedPackageName, NuGetVersion.Parse("1.0.0"));
            byte[] expectedPackageBytes = await GetPackageBytesAsync(ExpectedPackageName);
            var result = await GlobalPackagesFolderUtility.AddPackageAsync(
                "local",
                expectedPackageIdentity,
                new MemoryStream(expectedPackageBytes),
                LocalV3Path,
                Guid.NewGuid(),
                ClientPolicyContext.GetClientPolicy(NullSettings.Instance, NullLogger.Instance),
                NullLogger.Instance,
                CancellationToken.None);
            result.Status.Should().Be(DownloadResourceResultStatus.Available);

            // Extract to local v2 folder layout
            var packageExtractionContext = new PackageExtractionContext(
                PackageSaveMode.Defaultv2,
                XmlDocFileSaveMode.None,
                ClientPolicyContext.GetClientPolicy(NullSettings.Instance, NullLogger.Instance),
                NullLogger.Instance);
            var packagesConfigPathResolver = new PackagePathResolver(LocalPackagesConfigPath);
            await PackageExtractor.ExtractPackageAsync(
                "local",
                result.PackageStream,
                packagesConfigPathResolver,
                packageExtractionContext,
                CancellationToken.None);
            result.Dispose();

            // now replace the two nupkgs with the bad package
            var packagesConfigNupkgPath = packagesConfigPathResolver.GetInstalledPackageFilePath(expectedPackageIdentity);
            if (packagesConfigNupkgPath is null)
            {
                throw new InvalidOperationException($"Failed to find nupkg path for {expectedPackageIdentity} in packages.config layout");
            }
            File.WriteAllBytes(packagesConfigNupkgPath, _packageBytes);

            var v3PathResolver = new VersionFolderPathResolver(LocalV3Path);
            var v3NupkgPath = v3PathResolver.GetPackageFilePath(expectedPackageIdentity.Id, expectedPackageIdentity.Version);
            File.WriteAllBytes(v3NupkgPath, _packageBytes);

            Directory.CreateDirectory(LocalV2Path);
            var v2NupkgPath = Path.Combine(LocalV2Path, packagesConfigPathResolver.GetPackageFileName(expectedPackageIdentity));
            File.WriteAllBytes(v2NupkgPath, _packageBytes);
        }

        public Task DisposeAsync()
        {
            _testDirectory?.Dispose();
            return Task.CompletedTask;
        }

        private static async Task<byte[]> GetPackageBytesAsync(string packageName)
        {
            var packageContext = new SimpleTestPackageContext(packageName, "1.0.0");
            using MemoryStream packageBStream = await packageContext.CreateAsStreamAsync();
            var packageBytes = packageBStream.ToArray();
            return packageBytes;
        }

        private Dictionary<string, Func<HttpRequestMessage, Task<HttpResponseMessage>>> GetResponsesDictionary()
        {
            Dictionary<string, Func<HttpRequestMessage, Task<HttpResponseMessage>>> responses = new();

            responses[V3Url] = GetV3ServiceIndexResponse;
            responses[$"{V3BaseUrl}flatcontainer/{ExpectedPackageName}/index.json"] = GetV3PackageVersionList;
            responses[$"{V3BaseUrl}flatcontainer/{ExpectedPackageName}/1.0.0/{ExpectedPackageName}.1.0.0.nupkg"] = GetNupkgContents;

            responses[V2Url] = GetV2ServiceIndex;
            responses[$"{V2Url}FindPackagesById()?id='{ExpectedPackageName}'&semVerLevel=2.0.0"] = GetV2FindPackageByIdList;
            responses[$"{V2Url}Packages(Id='{ExpectedPackageName}',Version='1.0.0')"] = GetV2Package;
            responses[$"{V2Url}download/{ExpectedPackageName}/1.0.0"] = GetNupkgContents;

            return responses;
        }

        private Task<HttpResponseMessage> GetV2Package(HttpRequestMessage message)
        {
            string xml =
                $"""
                <?xml version="1.0" encoding="utf-8"?>
                <entry xml:base="https://www.nuget.org/api/v2" xmlns="http://www.w3.org/2005/Atom" xmlns:d="http://schemas.microsoft.com/ado/2007/08/dataservices" xmlns:m="http://schemas.microsoft.com/ado/2007/08/dataservices/metadata" xmlns:georss="http://www.georss.org/georss" xmlns:gml="http://www.opengis.net/gml">
                    <id>https://www.nuget.org/api/v2/Packages(Id='Newtonsoft.Json',Version='13.0.1')</id>
                    <category term="NuGetGallery.OData.V2FeedPackage" scheme="http://schemas.microsoft.com/ado/2007/08/dataservices/scheme" />
                    <link rel="edit" href="https://www.nuget.org/api/v2/Packages(Id='Newtonsoft.Json',Version='13.0.1')" />
                    <link rel="self" href="https://www.nuget.org/api/v2/Packages(Id='Newtonsoft.Json',Version='13.0.1')" />
                    <title type="text">Newtonsoft.Json</title>
                    <updated>2021-03-22T20:10:49Z</updated>
                    <author>
                        <name>James Newton-King</name>
                    </author>
                    <content type="application/zip" src="{V2Url}download/{ExpectedPackageName}/1.0.0" />
                    <m:properties>
                        <d:Id>{ExpectedPackageName}</d:Id>
                        <d:Version>1.0.0</d:Version>
                        <d:NormalizedVersion>1.0.0</d:NormalizedVersion>
                        <d:Authors m:null="true" />
                        <d:Copyright m:null="true" />
                        <d:Created m:type="Edm.DateTime">0001-01-02T00:00:00</d:Created>
                        <d:Dependencies m:null="true" />
                        <d:Description>testdesc</d:Description>
                        <d:DownloadCount m:type="Edm.Int32">0</d:DownloadCount>
                        <d:IconUrl m:null="true" />
                        <d:IsLatestVersion m:type="Edm.Boolean">true</d:IsLatestVersion>
                        <d:IsAbsoluteLatestVersion m:type="Edm.Boolean">true</d:IsAbsoluteLatestVersion>
                        <d:IsPrerelease m:type="Edm.Boolean">false</d:IsPrerelease>
                        <d:Language m:null="true" />
                        <d:LastUpdated m:type="Edm.DateTime">0001-01-02T00:00:00</d:LastUpdated>
                        <d:Published m:type="Edm.DateTime">0001-01-02T00:00:00</d:Published>
                        <d:PackageSize m:type="Edm.Int64">0</d:PackageSize>
                        <d:ProjectUrl m:null="true" />
                        <d:ReleaseNotes m:null="true" />
                        <d:RequireLicenseAcceptance m:type="Edm.Boolean">false</d:RequireLicenseAcceptance>
                        <d:Summary m:null="true" />
                        <d:Tags m:null="true" />
                        <d:Title m:null="true" />
                        <d:MinClientVersion m:null="true" />
                        <d:LastEdited m:type="Edm.DateTime">0001-01-02T00:00:00</d:LastEdited>
                        <d:LicenseUrl m:null="true" />
                        <d:Listed m:type="Edm.Boolean">true</d:Listed>
                    </m:properties>
                </entry>
                """;
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(xml)
            };
            return Task.FromResult(response);
        }

        private Task<HttpResponseMessage> GetV2ServiceIndex(HttpRequestMessage message)
        {
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            return Task.FromResult(response);
        }

        private Task<HttpResponseMessage> GetV2FindPackageByIdList(HttpRequestMessage request)
        {
            string xml =
            $"""
                    <?xml version="1.0" encoding="utf-8"?>
                    <feed xml:base="{V2Url}" xmlns="http://www.w3.org/2005/Atom" xmlns:d="http://schemas.microsoft.com/ado/2007/08/dataservices" xmlns:m="http://schemas.microsoft.com/ado/2007/08/dataservices/metadata" xmlns:georss="http://www.georss.org/georss" xmlns:gml="http://www.opengis.net/gml">
                      <id>http://schemas.datacontract.org/2004/07/</id>
                      <title />
                      <updated>2017-01-18T21:15:38Z</updated>
                      <link rel="self" href="{V2Url}Packages" />
                      <entry>
                        <id>{V2Url}Packages(Id='{ExpectedPackageName}',Version='1.0.0')</id>
                        <category term="Microsoft.VisualStudio.Services.NuGet.Server.Controllers.V2.ServerV2FeedPackage" scheme="http://schemas.microsoft.com/ado/2007/08/dataservices/scheme" />
                        <link rel="edit" href="{V2Url}Packages(Id='{ExpectedPackageName}',Version='1.0.0')" />
                        <link rel="self" href="{V2Url}Packages(Id='{ExpectedPackageName}',Version='1.0.0')" />
                        <title type="text">{ExpectedPackageName}</title>
                        <updated>0001-01-02T00:00:00Z</updated>
                        <author>
                          <name />
                        </author>
                        <content type="application/zip" src="{V2Url}download/{ExpectedPackageName}/1.0.0" />
                        <m:properties>
                          <d:Id>{ExpectedPackageName}</d:Id>
                          <d:Version>1.0.0</d:Version>
                          <d:NormalizedVersion>1.0.0</d:NormalizedVersion>
                          <d:Authors m:null="true" />
                          <d:Copyright m:null="true" />
                          <d:Created m:type="Edm.DateTime">0001-01-02T00:00:00</d:Created>
                          <d:Dependencies m:null="true" />
                          <d:Description>testdesc</d:Description>
                          <d:DownloadCount m:type="Edm.Int32">0</d:DownloadCount>
                          <d:IconUrl m:null="true" />
                          <d:IsLatestVersion m:type="Edm.Boolean">true</d:IsLatestVersion>
                          <d:IsAbsoluteLatestVersion m:type="Edm.Boolean">true</d:IsAbsoluteLatestVersion>
                          <d:IsPrerelease m:type="Edm.Boolean">false</d:IsPrerelease>
                          <d:Language m:null="true" />
                          <d:LastUpdated m:type="Edm.DateTime">0001-01-02T00:00:00</d:LastUpdated>
                          <d:Published m:type="Edm.DateTime">0001-01-02T00:00:00</d:Published>
                          <d:PackageSize m:type="Edm.Int64">0</d:PackageSize>
                          <d:ProjectUrl m:null="true" />
                          <d:ReleaseNotes m:null="true" />
                          <d:RequireLicenseAcceptance m:type="Edm.Boolean">false</d:RequireLicenseAcceptance>
                          <d:Summary m:null="true" />
                          <d:Tags m:null="true" />
                          <d:Title m:null="true" />
                          <d:MinClientVersion m:null="true" />
                          <d:LastEdited m:type="Edm.DateTime">0001-01-02T00:00:00</d:LastEdited>
                          <d:LicenseUrl m:null="true" />
                          <d:Listed m:type="Edm.Boolean">true</d:Listed>
                        </m:properties>
                      </entry>
                    </feed>
                    """;
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(xml)
            };
            return Task.FromResult(response);
        }

        private Task<HttpResponseMessage> GetNupkgContents(HttpRequestMessage request)
        {
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(_packageBytes)
            };
            return Task.FromResult(response);
        }

        private Task<HttpResponseMessage> GetV3PackageVersionList(HttpRequestMessage request)
        {
            string json = @"{""versions"":[""1.0.0""]}";
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(json)
            };
            return Task.FromResult(response);
        }

        private Task<HttpResponseMessage> GetV3ServiceIndexResponse(HttpRequestMessage request)
        {
            string json =
                $$"""
                {
                    "version": "3.0.0",
                    "resources": [
                    {
                        "@id": "{{V3BaseUrl}}flatcontainer/",
                        "@type": "PackageBaseAddress/3.0.0"
                    }
                    ]
                }
                """;
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(json)
            };
            return Task.FromResult(response);
        }
    }
}
