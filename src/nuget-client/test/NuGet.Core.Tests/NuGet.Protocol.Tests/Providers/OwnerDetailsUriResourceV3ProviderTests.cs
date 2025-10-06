// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.Configuration;
using NuGet.Packaging;
using NuGet.Protocol.Core.Types;
using NuGet.Protocol.Providers;
using Xunit;

namespace NuGet.Protocol.Tests.Providers
{
    public class OwnerDetailsUriResourceV3ProviderTests
    {
        [Fact]
        public async Task TryCreate_NoResourceInServiceIndex_ReturnsFalseAsync()
        {
            // Arrange
            var serviceIndexProvider = MockServiceIndexResourceV3Provider.Create();
            var target = new OwnerDetailsUriResourceV3Provider();
            var providers = new INuGetResourceProvider[] { serviceIndexProvider, target };

            PackageSource packageSource = new PackageSource("https://nuget.test/v3/index.json");
            SourceRepository sourceRepository = new SourceRepository(packageSource, providers);

            // Act
            Tuple<bool, INuGetResource?> result = await target.TryCreate(sourceRepository, CancellationToken.None);

            // Assert
            bool providerHandlesInputSource = result.Item1;
            INuGetResource? resource = result.Item2;

            providerHandlesInputSource.Should().BeFalse();
            resource.Should().BeNull();
        }

        [Fact]
        public async Task TryCreate_ResourceInServiceIndex_ReturnsTrueAsync()
        {
            // Arrange
            var ownerDetailsResourceEntry = new ServiceIndexEntry(new Uri("https://nuget.test/profiles/{owner}?_src=template"), ServiceTypes.OwnerDetailsUriTemplate[0], MinClientVersionUtility.GetNuGetClientVersion());
            var serviceIndexProvider = MockServiceIndexResourceV3Provider.Create();
            var target = new OwnerDetailsUriResourceV3Provider();
            var providers = new INuGetResourceProvider[] { serviceIndexProvider, target };

            PackageSource packageSource = new PackageSource("https://nuget.test/v3/index.json");
            SourceRepository sourceRepository = new SourceRepository(packageSource, providers);

            // Act
            Tuple<bool, INuGetResource?> result = await target.TryCreate(sourceRepository, CancellationToken.None);

            // Assert
            bool providerHandlesInputSource = result.Item1;
            INuGetResource? resource = result.Item2;

            providerHandlesInputSource.Should().BeFalse();
            resource.Should().BeNull();
        }
    }
}
