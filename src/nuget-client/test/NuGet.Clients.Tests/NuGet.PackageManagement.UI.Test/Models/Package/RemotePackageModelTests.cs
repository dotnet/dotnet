// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.PackageManagement.UI.Models.Package;
using NuGet.Packaging.Core;
using NuGet.Protocol;
using NuGet.Versioning;
using NuGet.VisualStudio.Internal.Contracts;
using Xunit;

namespace NuGet.PackageManagement.UI.Test.Models.Package
{
    public class RemotePackageModelTests
    {
        private readonly Mock<IVulnerableCapable> _vulnerableCapabilityMock;
        private readonly Mock<IDeprecationCapable> _deprecationCapabilityMock;
        private readonly Mock<IEmbeddedResourcesCapable> _embeddedResourcesMock;

        public RemotePackageModelTests()
        {
            _vulnerableCapabilityMock = new Mock<IVulnerableCapable>();
            _deprecationCapabilityMock = new Mock<IDeprecationCapable>();
            _embeddedResourcesMock = new Mock<IEmbeddedResourcesCapable>();
        }

        [Fact]
        public void RemotePackageModelCtr_IdAndVersion_ReturnsValueFromIdentity()
        {
            // Arrange
            var identity = new PackageIdentity("TestPackage", new NuGetVersion("1.0.0"));

            // Act
            var package = PackageModelCreationTestHelper.CreateRemotePackageModel(identity, _vulnerableCapabilityMock.Object, _deprecationCapabilityMock.Object, _embeddedResourcesMock.Object);

            // Assert
            Assert.Equal("TestPackage", package.Id);
            Assert.Equal(new NuGetVersion("1.0.0"), package.Version);
        }

        [Fact]
        public void RemotePackageModelCtr_OptionalParameters_ReturnsExpected()
        {
            // Arrange
            var identity = new PackageIdentity("TestPackage", new NuGetVersion("1.0.0"));
            var isListed = true;
            var packageDetailsUrl = new Uri("http://example.com");
            var downloadCount = 1000;

            // Act
            var package = PackageModelCreationTestHelper.CreateRemotePackageModel(identity, _vulnerableCapabilityMock.Object, _deprecationCapabilityMock.Object, _embeddedResourcesMock.Object, isListed, packageDetailsUrl, downloadCount);

            // Assert
            Assert.Equal("TestPackage", package.Id);
            Assert.Equal(new NuGetVersion("1.0.0"), package.Version);
            Assert.Equal(isListed, package.IsListed);
            Assert.Equal(packageDetailsUrl, package.PackageDetailsUrl);
            Assert.Equal(downloadCount, package.DownloadCount);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void RemotePackageModel_IsVulnerableProperty_ReturnsExpected(bool isPackageVulnerable)
        {
            // Arrange
            _vulnerableCapabilityMock.SetupGet(x => x.IsVulnerable).Returns(isPackageVulnerable);
            var identity = new PackageIdentity("TestPackage", new NuGetVersion("1.0.0"));

            var package = PackageModelCreationTestHelper.CreateRemotePackageModel(identity, _vulnerableCapabilityMock.Object, _deprecationCapabilityMock.Object, _embeddedResourcesMock.Object);

            // Act
            // Assert
            Assert.Equal(package.IsVulnerable, isPackageVulnerable);
        }

        [Fact]
        public void IsDeprecated_WithDeprecation_ReturnsTrue()
        {
            // Arrange
            _deprecationCapabilityMock.Setup(d => d.IsDeprecated).Returns(true);

            // Act
            var package = PackageModelCreationTestHelper.CreateRemotePackageModel(new PackageIdentity("TestPackage", new NuGetVersion("1.0.0")), _vulnerableCapabilityMock.Object, _deprecationCapabilityMock.Object, _embeddedResourcesMock.Object);

            // Assert
            Assert.True(package.IsDeprecated);
        }

        [Fact]
        public void PackageDeprecationReasons_WithDeprecation_ReturnsCorrectValue()
        {
            // Arrange
            var reasons = PackageDeprecationReason.CriticalBugs;
            _deprecationCapabilityMock.Setup(d => d.PackageDeprecationReasons).Returns(reasons);

            // Act
            var package = PackageModelCreationTestHelper.CreateRemotePackageModel(new PackageIdentity("TestPackage", new NuGetVersion("1.0.0")), _vulnerableCapabilityMock.Object, _deprecationCapabilityMock.Object, _embeddedResourcesMock.Object);

            // Assert
            Assert.Equal(reasons, package.PackageDeprecationReasons);
        }

        [Fact]
        public async Task PopulateDataAsync_WithCancellationToken_CompletesSuccessfully()
        {
            // Arrange
            var cancellationToken = new CancellationToken();

            // Act
            var package = PackageModelCreationTestHelper.CreateRemotePackageModel(new PackageIdentity("TestPackage", new NuGetVersion("1.0.0")), _vulnerableCapabilityMock.Object, _deprecationCapabilityMock.Object, _embeddedResourcesMock.Object);
            await package.PopulateDataAsync(cancellationToken);

            // Assert
            _deprecationCapabilityMock.Verify(d => d.PopulateDataAsync(cancellationToken), Times.Once);
            _vulnerableCapabilityMock.Verify(d => d.PopulateDataAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public void Vulnerabilities_WithVulnerabilities_ReturnsCorrectValue()
        {
            // Arrange
            var vulnerabilities = new List<PackageVulnerabilityMetadataContextInfo>
            {
                new PackageVulnerabilityMetadataContextInfo(new Uri("http://test.com/advisory1"), 1),
                new PackageVulnerabilityMetadataContextInfo(new Uri("http://test.com/advisory2"), 2)
            };
            _vulnerableCapabilityMock.Setup(v => v.Vulnerabilities).Returns(vulnerabilities);
            var identity = new PackageIdentity("TestPackage", new NuGetVersion("1.0.0"));

            // Act
            var package = PackageModelCreationTestHelper.CreateRemotePackageModel(identity, _vulnerableCapabilityMock.Object, _deprecationCapabilityMock.Object, _embeddedResourcesMock.Object);

            // Assert
            Assert.Equal(vulnerabilities, package.Vulnerabilities);
        }

        [Fact]
        public void IsVulnerable_WithVulnerabilities_ReturnsTrue()
        {
            // Arrange
            _vulnerableCapabilityMock.Setup(v => v.IsVulnerable).Returns(true);
            var identity = new PackageIdentity("TestPackage", new NuGetVersion("1.0.0"));

            // Act
            var package = PackageModelCreationTestHelper.CreateRemotePackageModel(identity, _vulnerableCapabilityMock.Object, _deprecationCapabilityMock.Object, _embeddedResourcesMock.Object);

            // Assert
            Assert.True(package.IsVulnerable);
        }

        [Fact]
        public void VulnerabilityMaxSeverity_WithVulnerabilities_ReturnsCorrectValue()
        {
            // Arrange
            var severity = PackageVulnerabilitySeverity.High;
            _vulnerableCapabilityMock.Setup(v => v.VulnerabilityMaxSeverity).Returns(severity);
            var identity = new PackageIdentity("TestPackage", new NuGetVersion("1.0.0"));

            // Act
            var package = PackageModelCreationTestHelper.CreateRemotePackageModel(identity, _vulnerableCapabilityMock.Object, _deprecationCapabilityMock.Object, _embeddedResourcesMock.Object);

            // Assert
            Assert.Equal(severity, package.VulnerabilityMaxSeverity);
        }
    }
}
