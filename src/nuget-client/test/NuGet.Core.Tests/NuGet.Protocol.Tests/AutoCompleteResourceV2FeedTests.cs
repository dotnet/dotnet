// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.Common;
using NuGet.Protocol.Core.Types;
using Test.Utility;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class AutoCompleteResourceV2FeedTests
    {
        [Fact]
        public async Task AutoCompleteResourceV2Feed_IdStartsWith()
        {
            // Arrange
            var serviceAddress = ProtocolUtility.CreateServiceAddress();

            var responses = new Dictionary<string, string>();
            responses.Add(serviceAddress + "package-ids?partialId=Azure&includePrerelease=False&semVerLevel=2.0.0",
                 ProtocolUtility.GetResource("NuGet.Protocol.Tests.compiler.resources.AzureAutoComplete.json", GetType()));
            responses.Add(serviceAddress, string.Empty);

            var repo = StaticHttpHandler.CreateSource(serviceAddress, Repository.Provider.GetCoreV3(), responses);

            var autoCompleteResource = await repo.GetResourceAsync<AutoCompleteResource>(CancellationToken.None);

            // Act
            var result = await autoCompleteResource.IdStartsWith("Azure", false, NullLogger.Instance, CancellationToken.None);

            // Assert
            Assert.Equal(30, result.Count());
        }

        [Fact]
        public async Task AutoCompleteResourceV2Feed_VersionStartsWith()
        {
            // Arrange
            var serviceAddress = ProtocolUtility.CreateServiceAddress();

            var responses = new Dictionary<string, string>();
            responses.Add(serviceAddress + "package-versions/xunit?includePrerelease=False&semVerLevel=2.0.0",
                 ProtocolUtility.GetResource("NuGet.Protocol.Tests.compiler.resources.XunitVersionAutoComplete.json", GetType()));
            responses.Add(serviceAddress, string.Empty);

            var repo = StaticHttpHandler.CreateSource(serviceAddress, Repository.Provider.GetCoreV3(), responses);

            var autoCompleteResource = await repo.GetResourceAsync<AutoCompleteResource>(CancellationToken.None);

            // Act
            var result = await autoCompleteResource.VersionStartsWith("xunit", "1", false, NullSourceCacheContext.Instance, NullLogger.Instance, CancellationToken.None);

            // Assert
            Assert.Equal(6, result.Count());
        }

        [Fact]
        public async Task AutoCompleteResourceV2Feed_VersionStartsWithInvalidId()
        {
            // Arrange
            var serviceAddress = ProtocolUtility.CreateServiceAddress();

            var responses = new Dictionary<string, string>();
            responses.Add(serviceAddress + "package-versions/azure?includePrerelease=False&semVerLevel=2.0.0", "[]");
            responses.Add(serviceAddress, string.Empty);

            var repo = StaticHttpHandler.CreateSource(serviceAddress, Repository.Provider.GetCoreV3(), responses);

            var autoCompleteResource = await repo.GetResourceAsync<AutoCompleteResource>(CancellationToken.None);

            // Act
            var result = await autoCompleteResource.VersionStartsWith("azure", "1", false, NullSourceCacheContext.Instance, NullLogger.Instance, CancellationToken.None);

            // Assert
            Assert.Equal(0, result.Count());
        }
    }
}
