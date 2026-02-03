// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Packaging.Core;
using Xunit;
using NuGet.Versioning;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using NuGet.VisualStudio.Internal.Contracts;
using NuGet.Protocol;
using NuGet.PackageManagement.UI.Models.Package;

namespace NuGet.PackageManagement.UI.Test.Models.Package
{
    public class DirectlyReferencedPackageModelTests
    {
        private readonly Mock<IVulnerableCapable> _mockVulnerableCapability;
        private readonly Mock<IDeprecationCapable> _mockDeprecationCapable;
        private readonly Mock<IEmbeddedResourcesCapable> _mockEmbeddedResource;

        public DirectlyReferencedPackageModelTests()
        {
            _mockVulnerableCapability = new Mock<IVulnerableCapable>();
            _mockDeprecationCapable = new Mock<IDeprecationCapable>();
            _mockEmbeddedResource = new Mock<IEmbeddedResourcesCapable>();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("String")]
        public void Constructor_SetReportAbuseUrl_InitializeReportAbuseUrl(string? reportAbuseUrl)
        {
            // Arrange
            var identity = new PackageIdentity("TestPackage", new NuGetVersion("1.0.0"));
            var packagePath = "path/to/package";

            // Act
            var model = PackageModelCreationTestHelper.CreateDirectlyReferencedPackageModel(
                identity,
                packagePath,
                _mockVulnerableCapability.Object,
                _mockDeprecationCapable.Object,
                _mockEmbeddedResource.Object,
                reportAbuseUrl);

            // Assert
            Assert.Equal(reportAbuseUrl, model.ReportAbuseUrl);
        }

        [Fact]
        public void IsDeprecated_WithDeprecation_ReturnsTrue()
        {
            // Arrange
            _mockDeprecationCapable.Setup(d => d.IsDeprecated).Returns(true);
            var identity = new PackageIdentity("TestPackage", new NuGetVersion("1.0.0"));
            var packagePath = "path/to/package";

            // Act
            var model = PackageModelCreationTestHelper.CreateDirectlyReferencedPackageModel(
                identity,
                packagePath,
                _mockVulnerableCapability.Object,
                _mockDeprecationCapable.Object,
                _mockEmbeddedResource.Object);

            // Assert
            Assert.True(model.IsDeprecated);
        }

        [Fact]
        public void PackageDeprecationReasons_WithDeprecation_ReturnsCorrectValue()
        {
            // Arrange
            var reasons = PackageDeprecationReason.CriticalBugs;
            _mockDeprecationCapable.Setup(d => d.PackageDeprecationReasons).Returns(reasons);
            var identity = new PackageIdentity("TestPackage", new NuGetVersion("1.0.0"));
            var packagePath = "path/to/package";

            // Act
            var model = PackageModelCreationTestHelper.CreateDirectlyReferencedPackageModel(
                identity,
                packagePath,
                _mockVulnerableCapability.Object,
                _mockDeprecationCapable.Object,
                _mockEmbeddedResource.Object);

            // Assert
            Assert.Equal(reasons, model.PackageDeprecationReasons);
        }

        [Fact]
        public async Task PopulateDataAsync_WithCancellationToken_CompletesSuccessfully()
        {
            // Arrange
            var cancellationToken = new CancellationToken();
            var identity = new PackageIdentity("TestPackage", new NuGetVersion("1.0.0"));
            var packagePath = "path/to/package";

            // Act
            var model = PackageModelCreationTestHelper.CreateDirectlyReferencedPackageModel(
                identity,
                packagePath,
                _mockVulnerableCapability.Object,
                _mockDeprecationCapable.Object,
                _mockEmbeddedResource.Object);
            await model.PopulateDataAsync(cancellationToken);

            // Assert
            _mockDeprecationCapable.Verify(d => d.PopulateDataAsync(cancellationToken), Times.Once);
            _mockVulnerableCapability.Verify(d => d.PopulateDataAsync(cancellationToken), Times.Once);
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
            _mockVulnerableCapability.Setup(v => v.Vulnerabilities).Returns(vulnerabilities);
            var identity = new PackageIdentity("TestPackage", new NuGetVersion("1.0.0"));
            var packagePath = "path/to/package";

            // Act
            var model = PackageModelCreationTestHelper.CreateDirectlyReferencedPackageModel(
                identity,
                packagePath,
                _mockVulnerableCapability.Object,
                _mockDeprecationCapable.Object,
                _mockEmbeddedResource.Object);

            // Assert
            Assert.Equal(vulnerabilities, model.Vulnerabilities);
        }

        [Fact]
        public void IsVulnerable_WithVulnerabilities_ReturnsTrue()
        {
            // Arrange
            _mockVulnerableCapability.Setup(v => v.IsVulnerable).Returns(true);
            var identity = new PackageIdentity("TestPackage", new NuGetVersion("1.0.0"));
            var packagePath = "path/to/package";

            // Act
            var model = PackageModelCreationTestHelper.CreateDirectlyReferencedPackageModel(
                identity,
                packagePath,
                _mockVulnerableCapability.Object,
                _mockDeprecationCapable.Object,
                _mockEmbeddedResource.Object);

            // Assert
            Assert.True(model.IsVulnerable);
        }

        [Fact]
        public void VulnerabilityMaxSeverity_WithVulnerabilities_ReturnsCorrectValue()
        {
            // Arrange
            var severity = PackageVulnerabilitySeverity.High;
            _mockVulnerableCapability.Setup(v => v.VulnerabilityMaxSeverity).Returns(severity);
            var identity = new PackageIdentity("TestPackage", new NuGetVersion("1.0.0"));
            var packagePath = "path/to/package";

            // Act
            var model = PackageModelCreationTestHelper.CreateDirectlyReferencedPackageModel(
                identity,
                packagePath,
                _mockVulnerableCapability.Object,
                _mockDeprecationCapable.Object,
                _mockEmbeddedResource.Object);

            // Assert
            Assert.Equal(severity, model.VulnerabilityMaxSeverity);
        }
    }
}
