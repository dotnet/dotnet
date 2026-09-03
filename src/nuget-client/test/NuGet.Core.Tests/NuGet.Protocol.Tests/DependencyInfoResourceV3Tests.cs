// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.Common;
using NuGet.Protocol.Core.Types;
using NuGet.Shared;
using NuGet.Versioning;
using Test.Utility;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class DependencyInfoResourceV3Tests
    {
        [Theory]
        [InlineData("true")]  // STJ path
        [InlineData("false")] // NSJ path
        public async Task ResolvePackages_BothPaths_ReturnsDependencyInfoAsync(string useStj)
        {
            // Arrange
            var responses = new Dictionary<string, string>
            {
                { "http://testsource.test/v3/index.json", JsonData.IndexWithoutFlatContainer },
                { "https://api.nuget.org/v3/registration0/deepequal/index.json", JsonData.DeepEqualRegistationIndex },
            };

            var repo = StaticHttpHandler.CreateSource("http://testsource.test/v3/index.json", Repository.Provider.GetCoreV3(), responses);
            HttpSource httpSource = (await repo.GetResourceAsync<HttpSourceResource>(CancellationToken.None)
                ?? throw new Xunit.Sdk.XunitException("Expected HttpSourceResource.")).HttpSource;
            var regResource = await repo.GetResourceAsync<RegistrationResourceV3>(CancellationToken.None)
                ?? throw new Xunit.Sdk.XunitException("Expected RegistrationResourceV3.");

            var envReader = new Mock<IEnvironmentVariableReader>();
            envReader.Setup(e => e.GetEnvironmentVariable(NuGetFeatureFlags.UseSystemTextJsonDeserializationEnvVar)).Returns(useStj);

            var resource = new DependencyInfoResourceV3(httpSource, regResource, repo, envReader.Object);

            // Act
            using var sourceCacheContext = new SourceCacheContext();
            IEnumerable<RemoteSourceDependencyInfo> results = await resource.ResolvePackages("deepequal", sourceCacheContext, NullLogger.Instance, CancellationToken.None);

            // Assert
            Assert.Equal(19, results.Count());
            RemoteSourceDependencyInfo target = results.Single(p => p.Identity.Version == NuGetVersion.Parse("0.9.0"));
            Assert.Equal("deepequal", target.Identity.Id, ignoreCase: true);
            Assert.Equal("https://api.nuget.org/packages/deepequal.0.9.0.nupkg", target.ContentUri);
        }
    }
}
