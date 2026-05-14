// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Test.Utility;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class DownloadResourceV2FeedTests
    {
        [Fact]
        public async Task DownloadResourceFromIdentityInvalidId()
        {
            // Arrange
            var serviceAddress = ProtocolUtility.CreateServiceAddress();

            var responses = new Dictionary<string, string>();
            responses.Add(serviceAddress + "Packages(Id='xunit',Version='1.0.0-notfound')", string.Empty);
            responses.Add(serviceAddress + "FindPackagesById()?id='xunit'&semVerLevel=2.0.0",
                ProtocolUtility.GetResource("NuGet.Protocol.Tests.compiler.resources.XunitFindPackagesById.xml", GetType()));
            responses.Add(serviceAddress, string.Empty);

            var repo = StaticHttpHandler.CreateSource(serviceAddress, Repository.Provider.GetCoreV3(), responses,
                 ProtocolUtility.GetResource("NuGet.Protocol.Tests.compiler.resources.500Error.xml", GetType()));

            var downloadResource = await repo.GetResourceAsync<DownloadResource>(CancellationToken.None);

            // Act
            using (var packagesFolder = TestDirectory.Create())
            using (var sourceCacheContext = new SourceCacheContext())
            using (var actual = await downloadResource.GetDownloadResourceResultAsync(
                new PackageIdentity("xunit", new NuGetVersion("1.0.0-notfound")),
                new PackageDownloadContext(sourceCacheContext),
                packagesFolder,
                NullLogger.Instance,
                CancellationToken.None))
            {
                // Assert
                Assert.NotNull(actual);
                Assert.Equal(DownloadResourceResultStatus.NotFound, actual.Status);
            }
        }
    }
}
