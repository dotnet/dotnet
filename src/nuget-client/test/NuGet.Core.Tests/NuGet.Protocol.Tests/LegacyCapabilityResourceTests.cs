// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Test.Utility;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class LegacyCapabilityResourceTests
    {
        [Fact]
        public async Task LegacyResourceNuGetOrg()
        {
            // Arrange
            var serviceAddress = ProtocolUtility.CreateServiceAddress() + "api/v2";

            var responses = new Dictionary<string, string>();
            responses.Add(serviceAddress + "/$metadata",
                ProtocolUtility.GetResource("NuGet.Protocol.Tests.compiler.resources.MetadataTT.xml", GetType()));
            responses.Add(serviceAddress, string.Empty);

            var httpSource = new TestHttpSource(new PackageSource(serviceAddress), responses);

            V2FeedParser parser = new V2FeedParser(httpSource, serviceAddress);

            LegacyFeedCapabilityResourceV2Feed legacyResource = new LegacyFeedCapabilityResourceV2Feed(parser, serviceAddress);


            Assert.True(await legacyResource.SupportsSearchAsync(NullLogger.Instance, CancellationToken.None));
            Assert.True(await legacyResource.SupportsIsAbsoluteLatestVersionAsync(NullLogger.Instance, CancellationToken.None));
        }

        [Fact]
        public async Task LegacyResourceSearchNoAbsoluteLatestVersion()
        {
            // Arrange
            var serviceAddress = ProtocolUtility.CreateServiceAddress() + "api/v2";

            var responses = new Dictionary<string, string>();
            responses.Add(serviceAddress + "/$metadata",
                ProtocolUtility.GetResource("NuGet.Protocol.Tests.compiler.resources.MetadataTF.xml", GetType()));
            responses.Add(serviceAddress, string.Empty);

            var httpSource = new TestHttpSource(new PackageSource(serviceAddress), responses);

            V2FeedParser parser = new V2FeedParser(httpSource, serviceAddress);

            LegacyFeedCapabilityResourceV2Feed legacyResource = new LegacyFeedCapabilityResourceV2Feed(parser, serviceAddress);


            Assert.True(await legacyResource.SupportsSearchAsync(NullLogger.Instance, CancellationToken.None));
            Assert.False(await legacyResource.SupportsIsAbsoluteLatestVersionAsync(NullLogger.Instance, CancellationToken.None));
        }

        [Fact]
        public async Task LegacyResourceNoSearchAbsoluteLatestVersion()
        {
            // Arrange
            var serviceAddress = ProtocolUtility.CreateServiceAddress() + "api/v2";

            var responses = new Dictionary<string, string>();
            responses.Add(serviceAddress + "/$metadata",
                ProtocolUtility.GetResource("NuGet.Protocol.Tests.compiler.resources.MetadataFT.xml", GetType()));
            responses.Add(serviceAddress, string.Empty);

            var httpSource = new TestHttpSource(new PackageSource(serviceAddress), responses);

            V2FeedParser parser = new V2FeedParser(httpSource, serviceAddress);

            LegacyFeedCapabilityResourceV2Feed legacyResource = new LegacyFeedCapabilityResourceV2Feed(parser, serviceAddress);


            Assert.False(await legacyResource.SupportsSearchAsync(NullLogger.Instance, CancellationToken.None));
            Assert.True(await legacyResource.SupportsIsAbsoluteLatestVersionAsync(NullLogger.Instance, CancellationToken.None));
        }

        [Fact]
        public async Task LegacyResourceNoSearchNoAbsoluteLatestVersion()
        {
            // Arrange
            var serviceAddress = ProtocolUtility.CreateServiceAddress() + "api/v2";

            var responses = new Dictionary<string, string>();
            responses.Add(serviceAddress + "/$metadata",
                ProtocolUtility.GetResource("NuGet.Protocol.Tests.compiler.resources.MetadataFF.xml", GetType()));
            responses.Add(serviceAddress, string.Empty);

            var httpSource = new TestHttpSource(new PackageSource(serviceAddress), responses);

            V2FeedParser parser = new V2FeedParser(httpSource, serviceAddress);

            LegacyFeedCapabilityResourceV2Feed legacyResource = new LegacyFeedCapabilityResourceV2Feed(parser, serviceAddress);


            Assert.False(await legacyResource.SupportsSearchAsync(NullLogger.Instance, CancellationToken.None));
            Assert.False(await legacyResource.SupportsIsAbsoluteLatestVersionAsync(NullLogger.Instance, CancellationToken.None));
        }
    }
}
