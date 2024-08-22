// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using Test.Utility;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class DependencyInfoResourceV2FeedTests
    {
        [Fact]
        public async Task DependencyInfoResourceV2Feed_GetDependencyInfoById()
        {
            // Arrange
            var serviceAddress = ProtocolUtility.CreateServiceAddress();

            var responses = new Dictionary<string, string>();
            responses.Add(serviceAddress + "FindPackagesById()?id='WindowsAzure.Storage'&semVerLevel=2.0.0",
                 ProtocolUtility.GetResource("NuGet.Protocol.Tests.compiler.resources.WindowsAzureStorageFindPackagesById.xml", GetType()));
            responses.Add(serviceAddress, string.Empty);

            var repo = StaticHttpHandler.CreateSource(serviceAddress, Repository.Provider.GetCoreV3(), responses);

            var dependencyInfoResource = await repo.GetResourceAsync<DependencyInfoResource>(CancellationToken.None);

            // Act
            var dependencyInfoList = await dependencyInfoResource.ResolvePackages("WindowsAzure.Storage",
                                                                            NuGetFramework.Parse("aspnetcore50"),
                                                                            NullSourceCacheContext.Instance,
                                                                            NullLogger.Instance,
                                                                            CancellationToken.None);

            // Assert
            Assert.Equal(47, dependencyInfoList.Count());
        }

        [Fact]
        public async Task DependencyInfoResourceV2Feed_GetDependencyInfoByPackageIdentity()
        {
            // Arrange
            var serviceAddress = ProtocolUtility.CreateServiceAddress();

            var responses = new Dictionary<string, string>();
            responses.Add(serviceAddress + "Packages(Id='WindowsAzure.Storage',Version='4.3.2-preview')",
                 ProtocolUtility.GetResource("NuGet.Protocol.Tests.compiler.resources.WindowsAzureStorageGetPackages.xml", GetType()));
            responses.Add(serviceAddress, string.Empty);

            var repo = StaticHttpHandler.CreateSource(serviceAddress, Repository.Provider.GetCoreV3(), responses);

            var dependencyInfoResource = await repo.GetResourceAsync<DependencyInfoResource>(CancellationToken.None);

            var packageIdentity = new PackageIdentity("WindowsAzure.Storage", new NuGetVersion("4.3.2-preview"));

            // Act
            var dependencyInfo = await dependencyInfoResource.ResolvePackage(packageIdentity,
                                                                            NuGetFramework.Parse("aspnetcore50"),
                                                                            NullSourceCacheContext.Instance,
                                                                            NullLogger.Instance,
                                                                            CancellationToken.None);

            // Assert
            Assert.Equal(43, dependencyInfo.Dependencies.Count());
        }

        [Fact]
        public async Task DependencyInfo_RetrieveExactVersion()
        {
            // Arrange
            var serviceAddress = ProtocolUtility.CreateServiceAddress();

            var responses = new Dictionary<string, string>();
            responses.Add(serviceAddress + "Packages(Id='xunit',Version='2.1.0-beta1-build2945')",
                 ProtocolUtility.GetResource("NuGet.Protocol.Tests.compiler.resources.Xunit.2.1.0-beta1-build2945GetPackages.xml", GetType()));
            responses.Add(serviceAddress, string.Empty);

            var repo = StaticHttpHandler.CreateSource(serviceAddress, Repository.Provider.GetCoreV3(), responses);

            var dependencyInfoResource = await repo.GetResourceAsync<DependencyInfoResource>(CancellationToken.None);

            var package = new PackageIdentity("xunit", NuGetVersion.Parse("2.1.0-beta1-build2945"));
            var dep1 = new PackageIdentity("xunit.core", NuGetVersion.Parse("2.1.0-beta1-build2945"));
            var dep2 = new PackageIdentity("xunit.assert", NuGetVersion.Parse("2.1.0-beta1-build2945"));

            // Act
            var result = await dependencyInfoResource.ResolvePackage(package, NuGetFramework.Parse("net45"), NullSourceCacheContext.Instance, Common.NullLogger.Instance, CancellationToken.None);

            // Assert
            Assert.Equal(package, result, PackageIdentity.Comparer);
            Assert.Equal(2, result.Dependencies.Count());
            Assert.Equal("[2.1.0-beta1-build2945, 2.1.0-beta1-build2945]", result.Dependencies.Single(dep => dep.Id == "xunit.core").VersionRange.ToNormalizedString());
            Assert.Equal("[2.1.0-beta1-build2945, 2.1.0-beta1-build2945]", result.Dependencies.Single(dep => dep.Id == "xunit.assert").VersionRange.ToNormalizedString());
        }

        [Fact]
        public async Task DependencyInfo_RetrieveDependencies()
        {
            // Arrange
            var serviceAddress = ProtocolUtility.CreateServiceAddress();

            var responses = new Dictionary<string, string>();
            responses.Add(serviceAddress + "FindPackagesById()?id='xunit'&semVerLevel=2.0.0",
                 ProtocolUtility.GetResource("NuGet.Protocol.Tests.compiler.resources.XunitFindPackagesById.xml", GetType()));
            responses.Add(serviceAddress, string.Empty);

            var repo = StaticHttpHandler.CreateSource(serviceAddress, Repository.Provider.GetCoreV3(), responses);

            var dependencyInfoResource = await repo.GetResourceAsync<DependencyInfoResource>(CancellationToken.None);

            var package = new PackageIdentity("xunit", NuGetVersion.Parse("2.1.0-beta1-build2945"));

            // filter to keep this test consistent
            var filterRange = new VersionRange(NuGetVersion.Parse("2.0.0-rc4-build2924"), true, NuGetVersion.Parse("2.1.0-beta1-build2945"), true);

            // Act
            var results = await dependencyInfoResource.ResolvePackages("xunit", NuGetFramework.Parse("net45"), NullSourceCacheContext.Instance, Common.NullLogger.Instance, CancellationToken.None);

            var filtered = results.Where(result => filterRange.Satisfies(result.Version));

            var target = filtered.Single(p => PackageIdentity.Comparer.Equals(p, package));

            // Assert
            Assert.Equal(3, filtered.Count());
            Assert.Equal(2, target.Dependencies.Count());
            Assert.Equal("[2.1.0-beta1-build2945, 2.1.0-beta1-build2945]", target.Dependencies.Single(dep => dep.Id == "xunit.core").VersionRange.ToNormalizedString());
            Assert.Equal("[2.1.0-beta1-build2945, 2.1.0-beta1-build2945]", target.Dependencies.Single(dep => dep.Id == "xunit.assert").VersionRange.ToNormalizedString());
        }

        [Fact]
        public async Task DependencyInfo_RetrieveExactVersion_NotFound()
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

            var dependencyInfoResource = await repo.GetResourceAsync<DependencyInfoResource>(CancellationToken.None);

            var package = new PackageIdentity("xunit", NuGetVersion.Parse("1.0.0-notfound"));

            // Act
            var result = await dependencyInfoResource.ResolvePackage(package, NuGetFramework.Parse("net45"), NullSourceCacheContext.Instance, Common.NullLogger.Instance, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DependencyInfo_RetrieveDependencies_NotFound()
        {
            // Arrange
            var serviceAddress = ProtocolUtility.CreateServiceAddress();

            var responses = new Dictionary<string, string>();
            responses.Add(serviceAddress + "FindPackagesById()?id='not-found'&semVerLevel=2.0.0",
                ProtocolUtility.GetResource("NuGet.Protocol.Tests.compiler.resources.NotFoundFindPackagesById.xml", GetType()));
            responses.Add(serviceAddress, string.Empty);

            var repo = StaticHttpHandler.CreateSource(serviceAddress, Repository.Provider.GetCoreV3(), responses);

            var dependencyInfoResource = await repo.GetResourceAsync<DependencyInfoResource>(CancellationToken.None);

            // Act
            var results = await dependencyInfoResource.ResolvePackages("not-found", NuGetFramework.Parse("net45"), NullSourceCacheContext.Instance, Common.NullLogger.Instance, CancellationToken.None);

            // Assert
            Assert.Equal(0, results.Count());
        }

        [Fact]
        public async Task DependencyInfo_GetNearestFramework()
        {
            // Arrange
            var serviceAddress = ProtocolUtility.CreateServiceAddress();

            var responses = new Dictionary<string, string>();
            responses.Add(serviceAddress + "Packages(Id='DotNetOpenAuth.Core',Version='4.3.2.13293')",
                 ProtocolUtility.GetResource("NuGet.Protocol.Tests.compiler.resources.DotNetOpenAuth.Core.4.3.2.13293GetPackages.xml", GetType()));
            responses.Add(serviceAddress, string.Empty);

            var repo = StaticHttpHandler.CreateSource(serviceAddress, Repository.Provider.GetCoreV3(), responses);

            var dependencyInfoResource = await repo.GetResourceAsync<DependencyInfoResource>(CancellationToken.None);

            var package = new PackageIdentity("DotNetOpenAuth.Core", NuGetVersion.Parse("4.3.2.13293"));

            // Act
            var result = await dependencyInfoResource.ResolvePackage(package, NuGetFramework.Parse("net45"), NullSourceCacheContext.Instance, Common.NullLogger.Instance, CancellationToken.None);

            // Assert
            Assert.Equal(1, result.Dependencies.Count());
            Assert.Equal("Microsoft.Net.Http", result.Dependencies.FirstOrDefault().Id);
        }
    }
}
