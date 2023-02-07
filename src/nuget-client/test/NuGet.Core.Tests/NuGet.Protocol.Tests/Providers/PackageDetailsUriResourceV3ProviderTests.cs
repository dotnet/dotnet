// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json.Linq;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Protocol.Providers.Tests
{
    using SemanticVersion = Versioning.SemanticVersion;

    public class PackageDetailsUriResourceV3ProviderTests
    {
        private const string ResourceType = "PackageDetailsUriTemplate/5.1.0";

        private static readonly SemanticVersion DefaultVersion = new SemanticVersion(0, 0, 0);

        private readonly PackageSource _packageSource;
        private readonly PackageDetailsUriResourceV3Provider _target;

        public PackageDetailsUriResourceV3ProviderTests()
        {
            _packageSource = new PackageSource("https://unit.test");
            _target = new PackageDetailsUriResourceV3Provider();
        }

        [Fact]
        public async Task TryCreate_WhenResourceDoesNotExist_ReturnsNoResource()
        {
            var resourceProviders = new ResourceProvider[]
            {
                CreateServiceIndexResourceV3Provider(),
                _target
            };
            var sourceRepository = new SourceRepository(_packageSource, resourceProviders);

            var result = await _target.TryCreate(sourceRepository, CancellationToken.None);

            Assert.False(result.Item1);
            Assert.Null(result.Item2);
        }

        [Theory]
        [InlineData("foo")]
        [InlineData("../somepath")]
        [InlineData(@"C:\packages\{id}\{version}\index.html")]
        [InlineData("http://unit.test/packages/{id}/{version}")]
        [InlineData("file:///my/path")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  \t\n")]
        public async Task TryCreate_WhenResourceHasInvalidAbsoluteUri_ReturnsNoResource(string uri)
        {
            var serviceEntry = new RawServiceIndexEntry(uri, ResourceType);
            var resourceProviders = new ResourceProvider[]
            {
                CreateServiceIndexResourceV3Provider(serviceEntry),
                _target
            };
            var sourceRepository = new SourceRepository(_packageSource, resourceProviders);

            var result = await _target.TryCreate(sourceRepository, CancellationToken.None);

            Assert.False(result.Item1);
            Assert.Null(result.Item2);
        }

        [Fact]
        public async Task TryCreate_WhenResourceExists_ReturnsValidResource()
        {
            var serviceEntry = new RawServiceIndexEntry("https://unit.test/packages/{id}/{version}", ResourceType);
            var resourceProviders = new ResourceProvider[]
            {
                CreateServiceIndexResourceV3Provider(serviceEntry),
                _target
            };
            var sourceRepository = new SourceRepository(_packageSource, resourceProviders);

            var result = await _target.TryCreate(sourceRepository, CancellationToken.None);

            Assert.True(result.Item1);
            Assert.NotNull(result.Item2);
            var resource = Assert.IsType<PackageDetailsUriResourceV3>(result.Item2);
            Assert.Equal(
                "https://unit.test/packages/MyPackage/1.0.0",
                resource.GetUri("MyPackage", NuGetVersion.Parse("1.0.0")).OriginalString);
        }

        private static ServiceIndexResourceV3Provider CreateServiceIndexResourceV3Provider(params RawServiceIndexEntry[] entries)
        {
            var provider = new Mock<ServiceIndexResourceV3Provider>();

            provider.Setup(x => x.Name)
                .Returns(nameof(ServiceIndexResourceV3Provider));
            provider.Setup(x => x.ResourceType)
                .Returns(typeof(ServiceIndexResourceV3));

            var resources = new JArray();

            foreach (var entry in entries)
            {
                resources.Add(
                    new JObject(
                        new JProperty("@id", entry.Uri),
                        new JProperty("@type", entry.Type)));
            }

            var index = new JObject();

            index.Add("version", "3.0.0");
            index.Add("resources", resources);
            index.Add("@context",
                new JObject(
                    new JProperty("@vocab", "http://schema.nuget.org/schema#"),
                    new JProperty("comment", "http://www.w3.org/2000/01/rdf-schema#comment")));

            var serviceIndexResource = new ServiceIndexResourceV3(index, DateTime.UtcNow);
            var tryCreateResult = new Tuple<bool, INuGetResource>(true, serviceIndexResource);

            provider.Setup(x => x.TryCreate(It.IsAny<SourceRepository>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(tryCreateResult));

            return provider.Object;
        }

        private class RawServiceIndexEntry
        {
            public RawServiceIndexEntry(string uri, string type)
            {
                Uri = uri;
                Type = type ?? throw new ArgumentNullException(nameof(type));
            }

            public string Uri { get; }
            public string Type { get; }
        }
    }
}
