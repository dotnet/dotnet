// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.Configuration;
using NuGet.Packaging;
using NuGet.Protocol.Core.Types;
using Xunit;

namespace NuGet.Protocol.Tests.Providers
{
    public class PackageStagingResourceV3ProviderTests
    {
        [Theory]
        [InlineData("https://nuget.test/staging")]
        [InlineData("https://nuget.test/staging/")]
        public async Task TryCreate_WhenSupportedResourceExists_ShouldPreserveExactEndpoint(string endpoint)
        {
            // Arrange
            var entry = new ServiceIndexEntry(new Uri(endpoint), ServiceTypes.PackageStaging[0], MinClientVersionUtility.GetNuGetClientVersion());
            var serviceIndexProvider = MockServiceIndexResourceV3Provider.Create(entry);
            var target = new PackageStagingResourceV3Provider();
            INuGetResourceProvider[] providers =
            {
                serviceIndexProvider,
                new HttpSourceResourceProvider(),
                target
            };
            var sourceRepository = new SourceRepository(new PackageSource("https://nuget.test/v3/index.json"), providers);

            // Act
            Tuple<bool, INuGetResource?> result = await target.TryCreate(sourceRepository, CancellationToken.None);

            // Assert
            result.Item1.Should().BeTrue();
            PackageStagingResourceV3 resource = result.Item2.Should().BeOfType<PackageStagingResourceV3>().Subject;
            resource.SourceUri.AbsoluteUri.Should().Be(endpoint);
        }

        [Fact]
        public async Task TryCreate_WhenStagingResourceDoesNotExist_ShouldReturnFalse()
        {
            // Arrange
            var serviceIndexProvider = MockServiceIndexResourceV3Provider.Create();
            var target = new PackageStagingResourceV3Provider();
            INuGetResourceProvider[] providers = { serviceIndexProvider, target };
            var sourceRepository = new SourceRepository(
                new PackageSource("https://nuget.test/v3/index.json"),
                providers);

            // Act
            Tuple<bool, INuGetResource?> result = await target.TryCreate(
                sourceRepository,
                CancellationToken.None);

            // Assert
            result.Item1.Should().BeFalse();
            result.Item2.Should().BeNull();
        }

        [Fact]
        public async Task TryCreate_WhenStagingVersionIsUnsupported_ShouldNotUsePackagePublish()
        {
            // Arrange
            var unsupportedStaging = new ServiceIndexEntry(new Uri("https://nuget.test/staging"), "PackageStaging/2.0.0", MinClientVersionUtility.GetNuGetClientVersion());
            var packagePublish = new ServiceIndexEntry(new Uri("https://nuget.test/publish"), ServiceTypes.PackagePublish[1], MinClientVersionUtility.GetNuGetClientVersion());
            var serviceIndexProvider = MockServiceIndexResourceV3Provider.Create(unsupportedStaging, packagePublish);
            var target = new PackageStagingResourceV3Provider();
            INuGetResourceProvider[] providers = { serviceIndexProvider, target };
            var sourceRepository = new SourceRepository(new PackageSource("https://nuget.test/v3/index.json"), providers);

            // Act
            Tuple<bool, INuGetResource?> result = await target.TryCreate(
                sourceRepository,
                CancellationToken.None);

            // Assert
            result.Item1.Should().BeFalse();
            result.Item2.Should().BeNull();
        }
    }
}
