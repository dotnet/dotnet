// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Packaging.Core;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Protocol.FuncTest
{
    public class ODataServiceDocumentResourceV2Tests
    {
        [Fact]
        public async Task ODataServiceDocumentResourceV2_Valid()
        {
            // Arrange
            var repo = Repository.Factory.GetCoreV3(TestSources.NuGetV2Uri);

            // Act 
            var resource = await repo.GetResourceAsync<ODataServiceDocumentResourceV2>(CancellationToken.None);

            // Assert
            Assert.NotNull(resource);
            Assert.Equal(TestSources.NuGetV2Uri, resource.BaseAddress);
        }

        [Fact]
        public async Task ODataServiceDocumentResourceV2_NotFound()
        {
            // Arrange
            var repo = Repository.Factory.GetCoreV3("https://www.nuget.org/api/v99///");

            // Act 
            var resource = await repo.GetResourceAsync<ODataServiceDocumentResourceV2>(CancellationToken.None);

            // Assert
            Assert.NotNull(resource);
            Assert.Equal("https://www.nuget.org/api/v99", resource.BaseAddress);
        }

        [Fact]
        public async Task ODataServiceDocumentResourceV2_Invalid()
        {
            // Arrange
            var randomName = Guid.NewGuid().ToString();
            var repo = Repository.Factory.GetCoreV3($"https://www.{randomName}.org/api/v2");

            // Act & Assert
            Exception ex = await Assert.ThrowsAsync<FatalProtocolException>(async () =>
                await repo.GetResourceAsync<ODataServiceDocumentResourceV2>(CancellationToken.None));

            Assert.Equal(
                $"Unable to load the service index for source https://www.{randomName}.org/api/v2.",
                ex.Message);
            Assert.NotNull(ex.InnerException);
            Assert.IsType<HttpRequestException>(ex.InnerException);
        }
    }
}
