// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;
using NuGet.Protocol.Tests;
using NuGet.Protocol.Tests.Providers;
using Test.Utility;
using Xunit;

namespace NuGet.Protocol.Providers.Tests
{
    using SemanticVersion = Versioning.SemanticVersion;

    [Collection(nameof(NotThreadSafeResourceCollection))]
    public class RepositorySignatureResourceProviderTests
    {
        private const string ResourceType470 = "RepositorySignatures/4.7.0";
        private const string ResourceType490 = "RepositorySignatures/4.9.0";
        private const string ResourceType500 = "RepositorySignatures/5.0.0";
        private const string ResourceUri470 = "https://unit.test/4.7.0";
        private const string ResourceUri490 = "https://unit.test/4.9.0";
        private const string ResourceUri500 = "https://unit.test/5.0.0";

        private static readonly SemanticVersion DefaultVersion = new SemanticVersion(0, 0, 0);

        private readonly PackageSource _packageSource;
        private readonly RepositorySignatureResourceProvider _repositorySignatureResourceProvider;

        public RepositorySignatureResourceProviderTests()
        {
            _packageSource = new PackageSource("https://unit.test");
            _repositorySignatureResourceProvider = new RepositorySignatureResourceProvider();
        }

        [Fact]
        public async Task TryCreate_WhenResourceDoesNotExist_ReturnsNoResource()
        {
            var resourceProviders = new ResourceProvider[]
            {
                MockServiceIndexResourceV3Provider.Create(),
                _repositorySignatureResourceProvider
            };
            var sourceRepository = new SourceRepository(_packageSource, resourceProviders);

            var result = await _repositorySignatureResourceProvider.TryCreate(sourceRepository, CancellationToken.None);

            Assert.False(result.Item1);
            Assert.Null(result.Item2);
        }

        [Theory]
        [InlineData("http://unit.test/4.9.0", ResourceType490)]
        [InlineData(@"\\localhost\unit\test\4.9.0", ResourceType490)]
        public async Task TryCreate_WhenUrlIsInvalid_Throws(string resourceUrl, string resourceType)
        {
            var serviceEntry = new ServiceIndexEntry(new Uri(resourceUrl), resourceType, DefaultVersion);
            var resourceProviders = new ResourceProvider[]
            {
                MockServiceIndexResourceV3Provider.Create(serviceEntry),
                StaticHttpSource.CreateHttpSource(
                    new Dictionary<string, string>()
                    {
                        { serviceEntry.Uri.AbsoluteUri, GetRepositorySignaturesResourceJson(resourceUrl) }
                    }),
                _repositorySignatureResourceProvider
            };
            var sourceRepository = new SourceRepository(_packageSource, resourceProviders);

            var exception = await Assert.ThrowsAsync<FatalProtocolException>(
                () => _repositorySignatureResourceProvider.TryCreate(sourceRepository, CancellationToken.None));

            Assert.Equal($"Repository Signatures resouce must be served over HTTPS. Source: {_packageSource.Source}", exception.Message);
        }

        [Theory]
        [InlineData(ResourceUri500, ResourceType500)]
        [InlineData(ResourceUri490, ResourceType490)]
        [InlineData(ResourceUri470, ResourceType470)]
        public async Task TryCreate_WhenOnlyOneResourceIsPresent_ReturnsThatResource(string resourceUrl, string resourceType)
        {
            var serviceEntry = new ServiceIndexEntry(new Uri(resourceUrl), resourceType, DefaultVersion);
            var resourceProviders = new ResourceProvider[]
            {
                MockServiceIndexResourceV3Provider.Create(serviceEntry),
                StaticHttpSource.CreateHttpSource(
                    new Dictionary<string, string>()
                    {
                        { serviceEntry.Uri.AbsoluteUri, GetRepositorySignaturesResourceJson(resourceUrl) }
                    }),
                _repositorySignatureResourceProvider
            };
            var sourceRepository = new SourceRepository(_packageSource, resourceProviders);

            var result = await _repositorySignatureResourceProvider.TryCreate(sourceRepository, CancellationToken.None);

            Assert.True(result.Item1);

            var resource = result.Item2 as RepositorySignatureResource;

            Assert.NotNull(resource);
            Assert.Equal(_packageSource.Source, resource.Source);
            Assert.Single(resource.RepositoryCertificateInfos);
        }

        [Fact]
        public async Task TryCreate_WhenMultipleResourcesArePresent_Returns500Resource()
        {
            var serviceEntry470 = new ServiceIndexEntry(new Uri(ResourceUri470), ResourceType470, DefaultVersion);
            var serviceEntry490 = new ServiceIndexEntry(new Uri(ResourceUri490), ResourceType490, DefaultVersion);
            var serviceEntry500 = new ServiceIndexEntry(new Uri(ResourceUri500), ResourceType500, DefaultVersion);
            var resourceProviders = new ResourceProvider[]
            {
                MockServiceIndexResourceV3Provider.Create(serviceEntry470, serviceEntry490, serviceEntry500),
                StaticHttpSource.CreateHttpSource(
                    new Dictionary<string, string>()
                    {
                        { serviceEntry470.Uri.AbsoluteUri, GetRepositorySignaturesResourceJson(serviceEntry470.Uri.AbsoluteUri) },
                        { serviceEntry490.Uri.AbsoluteUri, GetRepositorySignaturesResourceJson(serviceEntry490.Uri.AbsoluteUri) },
                        { serviceEntry500.Uri.AbsoluteUri, GetRepositorySignaturesResourceJson(serviceEntry500.Uri.AbsoluteUri) }
                    }),
                _repositorySignatureResourceProvider
            };
            var sourceRepository = new SourceRepository(_packageSource, resourceProviders);

            var result = await _repositorySignatureResourceProvider.TryCreate(sourceRepository, CancellationToken.None);

            Assert.True(result.Item1);

            var resource = result.Item2 as RepositorySignatureResource;

            Assert.NotNull(resource);
            Assert.Equal(_packageSource.Source, resource.Source);
            Assert.Single(resource.RepositoryCertificateInfos);
            Assert.StartsWith(serviceEntry500.Uri.AbsoluteUri, resource.RepositoryCertificateInfos.Single().ContentUrl);
        }

        [Theory]
        [InlineData(ResourceUri500, ResourceType500, "repository_signatures_5.0.0")]
        [InlineData(ResourceUri490, ResourceType490, "repository_signatures_4.9.0")]
        [InlineData(ResourceUri470, ResourceType470, "repository_signatures_4.7.0")]
        public async Task TryCreate_WhenResourceIsPresent_CreatesVersionedHttpCacheEntry(string resourceUrl, string resourceType, string expectedCacheKey)
        {
            var serviceEntry = new ServiceIndexEntry(new Uri(resourceUrl), resourceType, DefaultVersion);
            var responses = new Dictionary<string, string>()
            {
                { serviceEntry.Uri.AbsoluteUri, GetRepositorySignaturesResourceJson(resourceUrl) }
            };

            var httpSource = new TestHttpSource(_packageSource, responses);
            var resourceProviders = new ResourceProvider[]
            {
                MockServiceIndexResourceV3Provider.Create(serviceEntry),
                StaticHttpSource.CreateHttpSource(responses, httpSource: httpSource),
                _repositorySignatureResourceProvider
            };
            var sourceRepository = new SourceRepository(_packageSource, resourceProviders);
            string actualCacheKey = null;

            httpSource.HttpSourceCachedRequestInspector = request =>
            {
                actualCacheKey = request.CacheKey;
            };

            var result = await _repositorySignatureResourceProvider.TryCreate(sourceRepository, CancellationToken.None);

            Assert.True(result.Item1);
            Assert.Equal(expectedCacheKey, actualCacheKey);
        }

        private static string GetRepositorySignaturesResourceJson(string resourceBaseUri)
        {
            var jObject = new JObject(
                new JProperty("allRepositorySigned", false),
                new JProperty("signingCertificates",
                    new JArray(
                        new JObject(
                            new JProperty("fingerprints",
                                new JObject(
                                    new JProperty("2.16.840.1.101.3.4.2.1", "0e5f38f57dc1bcc806d8494f4f90fbcedd988b46760709cbeec6f4219aa6157d"))),
                            new JProperty("subject", "CN=NuGet.org Repository by Microsoft, O=NuGet.org Repository by Microsoft, L=Redmond, S=Washington, C=US"),
                            new JProperty("issuer", "CN=DigiCert SHA2 Assured ID Code Signing CA, OU=www.digicert.com, O=DigiCert Inc, C=US"),
                            new JProperty("notBefore", "2018-04-10T00:00:00.0000000Z"),
                            new JProperty("notAfter", "2021-04-14T12:00:00.0000000Z"),
                            new JProperty("contentUrl", $"{resourceBaseUri}/certificates/0e5f38f57dc1bcc806d8494f4f90fbcedd988b46760709cbeec6f4219aa6157d.crt")))));

            return jObject.ToString();
        }
    }
}
