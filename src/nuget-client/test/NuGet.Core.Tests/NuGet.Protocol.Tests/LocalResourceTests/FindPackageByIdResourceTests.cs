// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Configuration;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class FindPackageByIdResourceTests
    {
        [Fact]
        public async Task FindPackageByIdResource_V2V3CompareAsync()
        {
            using (var rootV3 = TestDirectory.Create())
            using (var rootV2 = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();

                var a1 = new PackageIdentity("a", NuGetVersion.Parse("1.0.0-alpha.1.2"));
                var a2 = new PackageIdentity("a", NuGetVersion.Parse("1.0.0+server.2"));
                var b = new PackageIdentity("b", NuGetVersion.Parse("1.0.0.0"));

                await SimpleTestPackageUtility.CreateFolderFeedV2Async(rootV2, a1, a2, b);
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(rootV3, a1, a2, b);

                var resourceV2 = new LocalV2FindPackageByIdResource(new PackageSource(rootV2));
                var resourceV3 = new LocalV3FindPackageByIdResource(new PackageSource(rootV3));

                using (var cacheContext = new SourceCacheContext())
                {
                    var bNonNorm = new PackageIdentity("B", NuGetVersion.Parse("1.0"));

                    // Act
                    var versionsV2 = new HashSet<NuGetVersion>(await resourceV2.GetAllVersionsAsync(
                        "A",
                        cacheContext,
                        testLogger,
                        CancellationToken.None));

                    var versionsV3 = new HashSet<NuGetVersion>(await resourceV3.GetAllVersionsAsync(
                        "A",
                        cacheContext,
                        testLogger,
                        CancellationToken.None));

                    var emptyV2 = (await resourceV2.GetAllVersionsAsync(
                        "c",
                        cacheContext,
                        testLogger,
                        CancellationToken.None))
                        .ToList();

                    var emptyV3 = (await resourceV3.GetAllVersionsAsync("c",
                        cacheContext,
                        testLogger,
                        CancellationToken.None))
                        .ToList();

                    var v2Stream = new MemoryStream();
                    await resourceV2.CopyNupkgToStreamAsync(
                        bNonNorm.Id,
                        bNonNorm.Version,
                        v2Stream,
                        cacheContext,
                        testLogger,
                        CancellationToken.None);

                    var v3Stream = new MemoryStream();
                    await resourceV3.CopyNupkgToStreamAsync(
                        bNonNorm.Id,
                        bNonNorm.Version,
                        v3Stream,
                        cacheContext,
                        testLogger,
                        CancellationToken.None);

                    var depV2 = await resourceV2.GetDependencyInfoAsync(
                        bNonNorm.Id,
                        bNonNorm.Version,
                        cacheContext,
                        testLogger,
                        CancellationToken.None);

                    var depV3 = await resourceV3.GetDependencyInfoAsync(
                        bNonNorm.Id,
                        bNonNorm.Version,
                        cacheContext,
                        testLogger,
                        CancellationToken.None);

                    var depEmptyV2 = await resourceV2.GetDependencyInfoAsync(
                        bNonNorm.Id,
                        NuGetVersion.Parse("2.9"),
                        cacheContext,
                        testLogger,
                        CancellationToken.None);

                    var depEmptyV3 = await resourceV3.GetDependencyInfoAsync(
                        bNonNorm.Id,
                        NuGetVersion.Parse("2.9"),
                        cacheContext,
                        testLogger,
                        CancellationToken.None);

                    // Assert
                    Assert.True(versionsV2.SetEquals(versionsV3));
                    Assert.Equal(0, emptyV2.Count);
                    Assert.Equal(0, emptyV3.Count);
                    Assert.True(v2Stream.Length > 0);
                    Assert.True(v3Stream.Length > 0);
                    Assert.Equal(0, depV2.DependencyGroups.Count);
                    Assert.Equal(0, depV3.DependencyGroups.Count);
                    Assert.Null(depEmptyV2);
                    Assert.Null(depEmptyV3);
                }
            }
        }
    }
}
