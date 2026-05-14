// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Moq;
using NuGet.PackageManagement.UI.Models.Package;
using NuGet.PackageManagement.VisualStudio;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using NuGet.VisualStudio.Internal.Contracts;
using Xunit;
using ContractItemFilter = NuGet.VisualStudio.Internal.Contracts.ItemFilter;

namespace NuGet.PackageManagement.UI.Test.Models.Package
{
    public class PackageModelFactoryTests
    {
        private readonly Mock<INuGetSearchService> _mockSearchService;
        private readonly Mock<INuGetPackageFileService> _mockPackageFileService;
        private readonly Mock<IPackageVulnerabilityService> _mockPackageVulnerabilityService;
        private readonly IReadOnlyCollection<PackageSourceContextInfo> _packageSources;
        private readonly PackageModelFactory _factory;

        public PackageModelFactoryTests()
        {
            _mockSearchService = new Mock<INuGetSearchService>();
            _mockPackageFileService = new Mock<INuGetPackageFileService>();
            _mockPackageVulnerabilityService = new Mock<IPackageVulnerabilityService>();
            _packageSources = new List<PackageSourceContextInfo> { new PackageSourceContextInfo("source") };
            _factory = new PackageModelFactory(
                _mockSearchService.Object,
                _mockPackageFileService.Object,
                _mockPackageVulnerabilityService.Object,
                includePrerelease: true,
                _packageSources);
        }

        [Fact]
        public void Constructor_WithNullSearchService_ThrowsArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new PackageModelFactory(
                null!,
                _mockPackageFileService.Object,
                _mockPackageVulnerabilityService.Object,
                includePrerelease: true,
                _packageSources));
        }

        [Fact]
        public void Create_WithValidMetadata_ReturnsPackageModel()
        {
            // Arrange
            var packageSearchMetadata = new PackageSearchMetadataContextInfo()
            {
                Identity = new PackageIdentity("TestPackage", NuGetVersion.Parse("4.3.0")),
            };

            // Act
            var result = _factory.Create(packageSearchMetadata, ContractItemFilter.All);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("TestPackage", result.Identity.Id);
        }

        [Fact]
        public void Create_RecommendedPackageOnAllTab_ReturnsRecommendedPackageModel()
        {
            // Arrange
            var packageSearchMetadata = new PackageSearchMetadataContextInfo()
            {
                Identity = new PackageIdentity("TestPackage", NuGetVersion.Parse("4.3.0")),
                RecommenderVersion = ("1.0.0", "1.0.0"),
                IsRecommended = true,
            };

            // Act
            var result = _factory.Create(packageSearchMetadata, ContractItemFilter.All);

            // Assert
            Assert.IsType<RecommendedPackageModel>(result);
        }

        [Theory]
        [InlineData(ContractItemFilter.All)]
        [InlineData(ContractItemFilter.UpdatesAvailable)]
        [InlineData(ContractItemFilter.Consolidate)]
        public void Create_PackageWithValidMetadata_ReturnsRemotePackageModel(ContractItemFilter itemFilter)
        {
            // Arrange
            var packageSearchMetadata = new PackageSearchMetadataContextInfo()
            {
                Identity = new PackageIdentity("TestPackage", NuGetVersion.Parse("4.3.0")),
            };

            // Act
            var result = _factory.Create(packageSearchMetadata, itemFilter);

            // Assert
            Assert.IsType<RemotePackageModel>(result);
        }

        [Fact]
        public void Create_PackageWithPackagePathOnBrowseTab_ReturnsLocalPackageModel()
        {
            // Arrange
            var packageSearchMetadata = new PackageSearchMetadataContextInfo()
            {
                Identity = new PackageIdentity("TestPackage", NuGetVersion.Parse("4.3.0")),
                PackagePath = "path",
            };

            // Act
            var result = _factory.Create(packageSearchMetadata, ContractItemFilter.All);

            // Assert
            Assert.IsType<LocalPackageModel>(result);
        }

        [Fact]
        public void Create_PackageWithPackagePathOnInstalledTab_ReturnsReferencedPackageModel()
        {
            // Arrange
            var packageSearchMetadata = new PackageSearchMetadataContextInfo()
            {
                Identity = new PackageIdentity("TestPackage", NuGetVersion.Parse("4.3.0")),
                PackagePath = "path",
            };

            // Act
            var result = _factory.Create(packageSearchMetadata, ContractItemFilter.Installed);

            // Assert
            Assert.IsType<DirectlyReferencedPackageModel>(result);
        }

        [Fact]
        public void Create_PackageWithTransitiveOrigins_ReturnsTransitivelyReferencedPackageModel()
        {
            // Arrange
            var packageSearchMetadata = new PackageSearchMetadataContextInfo()
            {
                Identity = new PackageIdentity("TestPackage", NuGetVersion.Parse("4.3.0")),
                TransitiveOrigins = new List<PackageIdentity> { new PackageIdentity("TestPackage2", NuGetVersion.Parse("4.3.0")) },
            };

            // Act
            var result = _factory.Create(packageSearchMetadata, ContractItemFilter.Installed);

            // Assert
            Assert.IsType<TransitivelyReferencedPackageModel>(result);
        }
    }
}
