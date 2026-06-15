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
using NuGet.Test.Utility;
using Test.Utility;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class AutoCompleteResourceV3Tests
    {
        [Theory]
        [InlineData("true")]  // STJ path
        [InlineData("false")] // NSJ path
        public async Task IdStartsWith_BothPaths_ReturnsResultsAsync(string useStj)
        {
            // Arrange
            var responses = new Dictionary<string, string>();
            responses.Add("http://testsource.test/v3/index.json", JsonData.IndexWithoutFlatContainer);
            responses.Add("https://api-v3search-0.nuget.org/autocomplete?q=newt&prerelease=true&semVerLevel=2.0.0",
                JsonData.AutoCompleteEndpointNewtResult);

            var repo = StaticHttpHandler.CreateSource("http://testsource.test/v3/index.json", Repository.Provider.GetCoreV3(), responses);
            var resource = (AutoCompleteResourceV3)(await repo.GetResourceAsync<AutoCompleteResource>(CancellationToken.None)
                ?? throw new Xunit.Sdk.XunitException("Expected AutoCompleteResource."));

            var envReader = new Mock<IEnvironmentVariableReader>();
            envReader.Setup(e => e.GetEnvironmentVariable(NuGetFeatureFlags.UseSystemTextJsonDeserializationEnvVar)).Returns(useStj);
            var testResource = new AutoCompleteResourceV3(resource._client, resource._serviceIndex, resource._regResource, envReader.Object);
            var logger = new TestLogger();

            // Act
            var result = await testResource.IdStartsWith("newt", true, logger, CancellationToken.None);

            // Assert
            Assert.Equal(10, result.Count());
            Assert.NotEmpty(logger.Messages);
        }
    }
}
