// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Protocol.Core.Types;
using Test.Utility;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class ODataServiceDocumentV2Tests
    {
        [Fact]
        public async Task DefaultBaseAddressIsServiceAddressWithTrimmedSlash()
        {
            // Arrange
            var serviceAddress = ProtocolUtility.CreateServiceAddress() + '/';

            var responses = new Dictionary<string, string>();
            responses.Add(serviceAddress, string.Empty);

            var repo = StaticHttpHandler.CreateSource(serviceAddress, Repository.Provider.GetCoreV3(), responses);

            var oDataServiceDocumentResource = await repo.GetResourceAsync<ODataServiceDocumentResourceV2>(CancellationToken.None);

            // Act
            var baseAddress = oDataServiceDocumentResource.BaseAddress;

            // Assert
            Assert.Equal(serviceAddress.Trim('/'), baseAddress);
        }

        [Fact]
        public async Task IgnoresXmlBase()
        {
            // Arrange
            var serviceAddress = ProtocolUtility.CreateServiceAddress();

            var responses = new Dictionary<string, string>();
            responses.Add(serviceAddress, ProtocolUtility.GetResource("NuGet.Protocol.Tests.compiler.resources.ODataServiceDocument.xml", GetType()));

            var repo = StaticHttpHandler.CreateSource(serviceAddress, Repository.Provider.GetCoreV3(), responses);

            var oDataServiceDocumentResource = await repo.GetResourceAsync<ODataServiceDocumentResourceV2>(CancellationToken.None);

            // Act
            var baseAddress = oDataServiceDocumentResource.BaseAddress;

            // Assert
            Assert.NotEqual("https://bringing/it/all/back/home", baseAddress);
            Assert.Equal(serviceAddress.Trim('/'), baseAddress);
        }

        [Fact]
        public async Task IgnoresInvalidXml()
        {
            // Arrange
            var serviceAddress = ProtocolUtility.CreateServiceAddress();

            var responses = new Dictionary<string, string>();
            responses.Add(serviceAddress, "[1, 2, \"not XML\"]");

            var repo = StaticHttpHandler.CreateSource(serviceAddress, Repository.Provider.GetCoreV3(), responses);

            // Act
            var resource = await repo.GetResourceAsync<ODataServiceDocumentResourceV2>(CancellationToken.None);

            // Assert
            Assert.Equal(serviceAddress.Trim('/'), resource.BaseAddress);
        }

        [Fact]
        public async Task FollowsRedirect()
        {
            // Arrange
            var serviceAddress = ProtocolUtility.CreateServiceAddress();

            var responses = new Dictionary<string, string>();
            responses.Add(serviceAddress, "301 https://bringing/it/all/back/home");

            var repo = StaticHttpHandler.CreateSource(serviceAddress, Repository.Provider.GetCoreV3(), responses);

            var oDataServiceDocumentResource = await repo.GetResourceAsync<ODataServiceDocumentResourceV2>(CancellationToken.None);

            // Act
            var baseAddress = oDataServiceDocumentResource.BaseAddress;

            // Assert
            Assert.Equal("https://bringing/it/all/back/home", baseAddress);
        }

        [Fact]
        public async Task FollowsRedirectAndTrimsQueryStringAndSlashes()
        {
            // Arrange
            var serviceAddress = ProtocolUtility.CreateServiceAddress();

            var responses = new Dictionary<string, string>();
            responses.Add(serviceAddress, "301 https://bringing/it/all/back/home//?foo=bar");

            var repo = StaticHttpHandler.CreateSource(serviceAddress, Repository.Provider.GetCoreV3(), responses);

            var oDataServiceDocumentResource = await repo.GetResourceAsync<ODataServiceDocumentResourceV2>(CancellationToken.None);

            // Act
            var baseAddress = oDataServiceDocumentResource.BaseAddress;

            // Assert
            Assert.Equal("https://bringing/it/all/back/home", baseAddress);
        }

        [Fact]
        public async Task FollowsRedirectToJustDomainName()
        {
            // Arrange
            var serviceAddress = ProtocolUtility.CreateServiceAddress();

            var responses = new Dictionary<string, string>();
            responses.Add(serviceAddress, "301 https://bringing");

            var repo = StaticHttpHandler.CreateSource(serviceAddress, Repository.Provider.GetCoreV3(), responses);

            var oDataServiceDocumentResource = await repo.GetResourceAsync<ODataServiceDocumentResourceV2>(CancellationToken.None);

            // Act
            var baseAddress = oDataServiceDocumentResource.BaseAddress;

            // Assert
            Assert.Equal("https://bringing", baseAddress);
        }
    }
}
