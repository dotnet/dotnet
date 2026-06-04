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
using Test.Utility;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class AutoCompleteResourceV2FeedTests
    {
        private static AutoCompleteResourceV2Feed CreateAutoCompleteResource(
            HttpSourceResource httpSourceResource,
            string serviceAddress,
            Configuration.PackageSource packageSource,
            string useStj)
        {
            var envReader = new Mock<IEnvironmentVariableReader>();
            envReader.Setup(e => e.GetEnvironmentVariable(NuGetFeatureFlags.UseSystemTextJsonDeserializationEnvVar)).Returns(useStj);
            return new AutoCompleteResourceV2Feed(httpSourceResource, serviceAddress.TrimEnd('/'), packageSource, envReader.Object);
        }

        [Theory]
        [InlineData("true")]
        [InlineData("false")]
        public async Task AutoCompleteResourceV2Feed_IdStartsWith(string useStj)
        {
            // Arrange
            var serviceAddress = ProtocolUtility.CreateServiceAddress();

            var responses = new Dictionary<string, string>();
            responses.Add(serviceAddress + "package-ids?partialId=Azure&includePrerelease=False&semVerLevel=2.0.0",
                 ProtocolUtility.GetResource("NuGet.Protocol.Tests.compiler.resources.AzureAutoComplete.json", GetType()));
            responses.Add(serviceAddress, string.Empty);

            var repo = StaticHttpHandler.CreateSource(serviceAddress, Repository.Provider.GetCoreV3(), responses);
            var httpSourceResource = await repo.GetResourceAsync<HttpSourceResource>(CancellationToken.None);
            var autoCompleteResource = CreateAutoCompleteResource(httpSourceResource!, serviceAddress, repo.PackageSource, useStj);

            // Act
            var result = await autoCompleteResource.IdStartsWith("Azure", false, NullLogger.Instance, CancellationToken.None);

            // Assert
            Assert.Equal(30, result.Count());
        }

        [Theory]
        [InlineData("true")]
        [InlineData("false")]
        public async Task AutoCompleteResourceV2Feed_VersionStartsWith(string useStj)
        {
            // Arrange
            var serviceAddress = ProtocolUtility.CreateServiceAddress();

            var responses = new Dictionary<string, string>();
            responses.Add(serviceAddress + "package-versions/xunit?includePrerelease=False&semVerLevel=2.0.0",
                 ProtocolUtility.GetResource("NuGet.Protocol.Tests.compiler.resources.XunitVersionAutoComplete.json", GetType()));
            responses.Add(serviceAddress, string.Empty);

            var repo = StaticHttpHandler.CreateSource(serviceAddress, Repository.Provider.GetCoreV3(), responses);
            var httpSourceResource = await repo.GetResourceAsync<HttpSourceResource>(CancellationToken.None);
            var autoCompleteResource = CreateAutoCompleteResource(httpSourceResource!, serviceAddress, repo.PackageSource, useStj);

            // Act
            var result = await autoCompleteResource.VersionStartsWith("xunit", "1", false, NullSourceCacheContext.Instance, NullLogger.Instance, CancellationToken.None);

            // Assert
            Assert.Equal(6, result.Count());
        }

        [Theory]
        [InlineData("true")]
        [InlineData("false")]
        public async Task AutoCompleteResourceV2Feed_VersionStartsWithInvalidId(string useStj)
        {
            // Arrange
            var serviceAddress = ProtocolUtility.CreateServiceAddress();

            var responses = new Dictionary<string, string>();
            responses.Add(serviceAddress + "package-versions/azure?includePrerelease=False&semVerLevel=2.0.0", "[]");
            responses.Add(serviceAddress, string.Empty);

            var repo = StaticHttpHandler.CreateSource(serviceAddress, Repository.Provider.GetCoreV3(), responses);
            var httpSourceResource = await repo.GetResourceAsync<HttpSourceResource>(CancellationToken.None);
            var autoCompleteResource = CreateAutoCompleteResource(httpSourceResource!, serviceAddress, repo.PackageSource, useStj);

            // Act
            var result = await autoCompleteResource.VersionStartsWith("azure", "1", false, NullSourceCacheContext.Instance, NullLogger.Instance, CancellationToken.None);

            // Assert
            Assert.Equal(0, result.Count());
        }
    }
}
