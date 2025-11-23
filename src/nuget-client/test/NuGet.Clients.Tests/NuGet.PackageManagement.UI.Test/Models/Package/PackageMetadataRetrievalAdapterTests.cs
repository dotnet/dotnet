// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.PackageManagement.UI.Models.Package;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using NuGet.VisualStudio.Internal.Contracts;
using Xunit;

namespace NuGet.PackageManagement.UI.Test.Models.Package
{
    public class PackageMetadataRetrievalAdapterTests
    {
        private readonly Mock<INuGetSearchService> _nugetSearchServiceMock;
        private readonly PackageIdentity _packageIdentity;
        private readonly PackageMetadataRetrievalAdapter _adapter;
        private readonly IReadOnlyCollection<PackageSourceContextInfo> _packageSources;
        private readonly bool _includePrerelease;

        public PackageMetadataRetrievalAdapterTests()
        {
            _nugetSearchServiceMock = new Mock<INuGetSearchService>();
            _packageIdentity = new PackageIdentity("TestPackage", new NuGetVersion("1.0.0"));
            _packageSources = new List<PackageSourceContextInfo>();
            _includePrerelease = false;
            _adapter = new PackageMetadataRetrievalAdapter(_nugetSearchServiceMock.Object, _packageIdentity, _packageSources, _includePrerelease);
        }

        [Fact]
        public async Task GetPackageMetadataAsync_WithValidMetadata_ReturnsPackageMetadata()
        {
            // Arrange
            var cancellationToken = It.IsAny<CancellationToken>();
            var expectedMetadata = new PackageSearchMetadataContextInfo();
            _nugetSearchServiceMock
                .Setup(s => s.GetPackageMetadataAsync(_packageIdentity, _packageSources, _includePrerelease, cancellationToken))
                .ReturnsAsync((expectedMetadata, null));

            // Act
            var result = await _adapter.GetPackageMetadataAsync(cancellationToken);

            // Assert
            Assert.Equal(expectedMetadata, result);
        }

        [Fact]
        public async Task GetPackageDeprecationInfoAsync_WithValidMetadata_ReturnsDeprecationInfo()
        {
            // Arrange
            var cancellationToken = It.IsAny<CancellationToken>();
            var expectedMetadata = new PackageSearchMetadataContextInfo();
            var expectedDeprecationInfo = PackageDeprecationMetadataContextInfo.Create(new Protocol.PackageDeprecationMetadata());
            _nugetSearchServiceMock
                .Setup(s => s.GetPackageMetadataAsync(_packageIdentity, _packageSources, _includePrerelease, cancellationToken))
                .ReturnsAsync((expectedMetadata, expectedDeprecationInfo));

            // Act
            var result = await _adapter.GetPackageDeprecationInfoAsync(cancellationToken);

            // Assert
            Assert.Equal(expectedDeprecationInfo, result);
        }

        [Fact]
        public void Constructor_WhenNuGetSearchServiceIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            INuGetSearchService? nullService = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new PackageMetadataRetrievalAdapter(nullService!, _packageIdentity, _packageSources, _includePrerelease));
        }

        [Fact]
        public void Constructor_WhenPackageIdentityIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            PackageIdentity? nullIdentity = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new PackageMetadataRetrievalAdapter(_nugetSearchServiceMock.Object, nullIdentity!, _packageSources, _includePrerelease));
        }

        [Fact]
        public async Task FetchMetadataAsync_CalledMultipleTimes_OnlyRunsOperationOnce()
        {
            // Arrange
            var cancellationToken = It.IsAny<CancellationToken>();
            var expectedMetadata = new PackageSearchMetadataContextInfo();
            var expectedDeprecationInfo = PackageDeprecationMetadataContextInfo.Create(new Protocol.PackageDeprecationMetadata());

            _nugetSearchServiceMock
                .Setup(s => s.GetPackageMetadataAsync(_packageIdentity, _packageSources, _includePrerelease, cancellationToken))
                .ReturnsAsync((expectedMetadata, expectedDeprecationInfo));

            // Act
            var task1 = _adapter.GetPackageMetadataAsync(cancellationToken);
            var task2 = _adapter.GetPackageDeprecationInfoAsync(cancellationToken);

            await Task.WhenAll(task1, task2);

            // Assert
            _nugetSearchServiceMock.Verify(s => s.GetPackageMetadataAsync(_packageIdentity, _packageSources, _includePrerelease, cancellationToken), Times.Once);
        }
    }
}
