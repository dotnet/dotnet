// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.Common;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Shared;
using NuGet.Versioning;
using Test.Utility;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class MetadataResourceV3Tests
    {
        private static async Task<MetadataResourceV3> CreateResourceAsync(string useStj, Dictionary<string, string> responses)
        {
            var repo = StaticHttpHandler.CreateSource("http://testsource.test/v3/index.json", Repository.Provider.GetCoreV3(), responses);
            var regResource = await repo.GetResourceAsync<RegistrationResourceV3>(CancellationToken.None)
                ?? throw new Xunit.Sdk.XunitException("Expected RegistrationResourceV3.");

            var envReader = new Mock<IEnvironmentVariableReader>();
            envReader.Setup(e => e.GetEnvironmentVariable(NuGetFeatureFlags.UseSystemTextJsonDeserializationEnvVar)).Returns(useStj);

            return new MetadataResourceV3(regResource, envReader.Object);
        }

        private static Dictionary<string, string> DeepEqualResponses()
        {
            return new Dictionary<string, string>
            {
                { "http://testsource.test/v3/index.json", JsonData.IndexWithoutFlatContainer },
                { "https://api.nuget.org/v3/registration0/deepequal/index.json", JsonData.DeepEqualRegistationIndex },
            };
        }

        [Theory]
        [InlineData("true")]  // STJ path
        [InlineData("false")] // NSJ path
        public async Task GetVersions_BothPaths_ReturnsAllVersionsAsync(string useStj)
        {
            // Arrange
            MetadataResourceV3 resource = await CreateResourceAsync(useStj, DeepEqualResponses());

            // Act
            using var sourceCacheContext = new SourceCacheContext();
            IEnumerable<NuGetVersion> versions = await resource.GetVersions("deepequal", includePrerelease: true, includeUnlisted: true, sourceCacheContext, NullLogger.Instance, CancellationToken.None);

            // Assert
            Assert.Equal(19, versions.Count());
            Assert.Equal(1, versions.Count(v => v.IsPrerelease));
            Assert.Contains(versions, v => v.ToNormalizedString() == "0.9.0");
        }

        [Theory]
        [InlineData("true")]  // STJ path
        [InlineData("false")] // NSJ path
        public async Task GetLatestVersions_BothPaths_ReturnsLatestStableAsync(string useStj)
        {
            // Arrange
            MetadataResourceV3 resource = await CreateResourceAsync(useStj, DeepEqualResponses());

            // Act
            using var sourceCacheContext = new SourceCacheContext();
            IEnumerable<KeyValuePair<string, NuGetVersion>> latest = await resource.GetLatestVersions(new[] { "deepequal" }, includePrerelease: false, includeUnlisted: true, sourceCacheContext, NullLogger.Instance, CancellationToken.None);

            // Assert
            KeyValuePair<string, NuGetVersion> result = Assert.Single(latest);
            Assert.Equal("deepequal", result.Key);
            Assert.Equal("1.4.0", result.Value.ToNormalizedString());
        }

        [Theory]
        [InlineData("true")]  // STJ path
        [InlineData("false")] // NSJ path
        public async Task Exists_BothPaths_ReturnsExpectedAsync(string useStj)
        {
            // Arrange
            MetadataResourceV3 resource = await CreateResourceAsync(useStj, DeepEqualResponses());

            // Act
            using var sourceCacheContext = new SourceCacheContext();
            bool exists = await resource.Exists(new PackageIdentity("deepequal", NuGetVersion.Parse("0.9.0")), includeUnlisted: true, sourceCacheContext, NullLogger.Instance, CancellationToken.None);
            bool missing = await resource.Exists(new PackageIdentity("deepequal", NuGetVersion.Parse("99.0.0")), includeUnlisted: true, sourceCacheContext, NullLogger.Instance, CancellationToken.None);

            // Assert
            Assert.True(exists);
            Assert.False(missing);
        }
    }
}
