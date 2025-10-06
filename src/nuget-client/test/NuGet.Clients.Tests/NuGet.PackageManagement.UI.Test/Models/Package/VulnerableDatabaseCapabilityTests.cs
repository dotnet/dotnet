// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

# nullable enable

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.PackageManagement.UI.Models.Package;
using NuGet.PackageManagement.VisualStudio;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using NuGet.VisualStudio.Internal.Contracts;
using Xunit;

namespace NuGet.PackageManagement.UI.Test.Models.Package
{
    public class VulnerableDatabaseCapabilityTests
    {
        [Fact]
        public void Constructor_WithNullVulnerabilityService_ThrowsArgumentNullException()
        {
            // Arrange
            IPackageVulnerabilityService? vulnerabilityService = null;
            var packageIdentity = new PackageIdentity("TestPackage", new NuGetVersion("1.0.0"));

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new VulnerableDatabaseCapability(vulnerabilityService!, packageIdentity));
        }

        [Fact]
        public void Constructor_WithNullPackageIdentity_ThrowsArgumentNullException()
        {
            // Arrange
            var vulnerabilityService = new Mock<IPackageVulnerabilityService>();
            PackageIdentity? packageIdentity = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new VulnerableDatabaseCapability(vulnerabilityService.Object, packageIdentity!));
        }

        [Fact]
        public async Task PopulateDataAsync_WithValidData_PopulatesVulnerabilities()
        {
            // Arrange
            var vulnerabilityServiceMock = new Mock<IPackageVulnerabilityService>();
            var packageIdentity = new PackageIdentity("TestPackage", new NuGetVersion("1.0.0"));
            var vulnerabilities = new List<PackageVulnerabilityMetadataContextInfo>
            {
                new PackageVulnerabilityMetadataContextInfo(new Uri("http://test.com/vuln1"), 2),
                new PackageVulnerabilityMetadataContextInfo(new Uri("http://test.com/vuln2"), 1)
            };
            vulnerabilityServiceMock.Setup(vs => vs.GetVulnerabilityInfoAsync(packageIdentity, It.IsAny<CancellationToken>()))
                .ReturnsAsync(vulnerabilities);

            var capability = new VulnerableDatabaseCapability(vulnerabilityServiceMock.Object, packageIdentity);

            // Act
            await capability.PopulateDataAsync(CancellationToken.None);

            // Assert
            Assert.Equal(vulnerabilities, capability.Vulnerabilities);
        }
    }
}
