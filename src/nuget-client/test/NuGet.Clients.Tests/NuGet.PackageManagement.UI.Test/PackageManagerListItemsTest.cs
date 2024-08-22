// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceHub.Framework;
using Microsoft.VisualStudio.Sdk.TestFramework;
using Microsoft.VisualStudio.Threading;
using Moq;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using NuGet.VisualStudio.Internal.Contracts;
using Test.Utility;
using Xunit;

namespace NuGet.PackageManagement.UI.Test
{
    [Collection(MockedVS.Collection)]
    public class PackageManagerListItemsTest
    {
        [Fact]
        public async Task PackagePrefixReservation_FromOneSource()
        {
            var solutionManager = Mock.Of<INuGetSolutionManagerService>();
            var uiContext = new Mock<INuGetUIContext>();
            var searchService = Mock.Of<INuGetSearchService>();
            var packageFileService = Mock.Of<INuGetPackageFileService>();

            uiContext.Setup(x => x.SolutionManagerService)
                .Returns(solutionManager);

            uiContext.Setup(x => x.ServiceBroker)
                .Returns(Mock.Of<IServiceBroker>());

            // Arrange
            var responses = new Dictionary<string, string>
            {
                {
                    "https://api-v3search-0.nuget.org/query?q=EntityFramework&skip=0&take=26&prerelease=false&semVerLevel=2.0.0",
                    ProtocolUtility.GetResource("NuGet.PackageManagement.UI.Test.compiler.resources.EntityFrameworkSearch.json", GetType())
                },
                { "http://testsource.com/v3/index.json", JsonData.IndexWithoutFlatContainer }
            };

            var repo = StaticHttpHandler.CreateSource("http://testsource.com/v3/index.json", Repository.Provider.GetCoreV3(), responses);

            var context = new PackageLoadContext(isSolution: false, uiContext.Object);

            var loader = await PackageItemLoader.CreateAsync(
                Mock.Of<IServiceBroker>(),
                context,
                new List<PackageSourceContextInfo> { PackageSourceContextInfo.Create(repo.PackageSource) },
                NuGet.VisualStudio.Internal.Contracts.ItemFilter.All,
                searchService,
                packageFileService,
                "EntityFramework",
                includePrerelease: false);

            var packageSearchMetadata = new PackageSearchMetadataBuilder.ClonedPackageSearchMetadata()
            {
                Identity = new PackageIdentity("NuGet.org", new NuGetVersion("1.0")),
                PrefixReserved = true
            };

            var packageSearchMetadataContextInfo = new List<PackageSearchMetadataContextInfo>()
            {
                PackageSearchMetadataContextInfo.Create(packageSearchMetadata)
            };

            var searchResult = new SearchResultContextInfo(
                packageSearchMetadataContextInfo,
                new Dictionary<string, LoadingStatus> { { "Completed", LoadingStatus.Ready } },
                hasMoreItems: false);

            await loader.UpdateStateAndReportAsync(searchResult, progress: null, CancellationToken.None);
            var items = loader.GetCurrent();

            // Resource only has one item
            var item = items.First();
            Assert.True(item.PrefixReserved);
        }

        [Fact]
        public async Task PackagePrefixReservation_FromMultiSource()
        {
            var solutionManager = Mock.Of<INuGetSolutionManagerService>();
            var uiContext = new Mock<INuGetUIContext>();
            var searchService = Mock.Of<INuGetSearchService>();
            var packageFileService = Mock.Of<INuGetPackageFileService>();

            uiContext.Setup(x => x.SolutionManagerService)
                .Returns(solutionManager);

            uiContext.Setup(x => x.ServiceBroker)
                .Returns(Mock.Of<IServiceBroker>());

            // Arrange
            var responses = new Dictionary<string, string>
            {
                {
                    "https://api-v3search-0.nuget.org/query?q=EntityFramework&skip=0&take=26&prerelease=false&semVerLevel=2.0.0",
                    ProtocolUtility.GetResource("NuGet.PackageManagement.UI.Test.compiler.resources.EntityFrameworkSearch.json", GetType())
                },
                { "http://testsource.com/v3/index.json", JsonData.IndexWithoutFlatContainer },
                { "http://othersource.com/v3/index.json", JsonData.IndexWithoutFlatContainer }
            };

            var repo = StaticHttpHandler.CreateSource("http://testsource.com/v3/index.json", Repository.Provider.GetCoreV3(), responses);
            var repo1 = StaticHttpHandler.CreateSource("http://othersource.com/v3/index.json", Repository.Provider.GetCoreV3(), responses);

            var context = new PackageLoadContext(isSolution: false, uiContext.Object);

            var loader = await PackageItemLoader.CreateAsync(
                Mock.Of<IServiceBroker>(),
                context,
                new List<PackageSourceContextInfo>
                {
                    PackageSourceContextInfo.Create(repo.PackageSource),
                    PackageSourceContextInfo.Create(repo1.PackageSource)
                },
                NuGet.VisualStudio.Internal.Contracts.ItemFilter.All,
                searchService,
                packageFileService,
                "EntityFramework",
                includePrerelease: false);

            var packageSearchMetadata = new PackageSearchMetadataBuilder.ClonedPackageSearchMetadata()
            {
                Identity = new PackageIdentity("NuGet.org", new NuGetVersion("1.0")),
                PrefixReserved = true
            };

            var packageSearchMetadataContextInfo = new List<PackageSearchMetadataContextInfo>()
            {
                PackageSearchMetadataContextInfo.Create(packageSearchMetadata)
            };

            var searchResult = new SearchResultContextInfo(
                packageSearchMetadataContextInfo,
                new Dictionary<string, LoadingStatus> { { "Completed", LoadingStatus.Ready } },
                hasMoreItems: false);

            await loader.UpdateStateAndReportAsync(searchResult, progress: null, CancellationToken.None);
            var items = loader.GetCurrent();

            // Resource only has one item
            var item = items.First();
            // Assert that a multisource always has prefixreserved set to false
            Assert.False(item.PrefixReserved);
        }
    }
}
