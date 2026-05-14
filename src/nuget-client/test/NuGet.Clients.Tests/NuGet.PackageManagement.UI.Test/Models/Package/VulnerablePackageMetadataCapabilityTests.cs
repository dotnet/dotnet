// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.PackageManagement.UI.Models.Package;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.VisualStudio.Internal.Contracts;
using Xunit;

namespace NuGet.PackageManagement.UI.Test.Models.Package
{
    public class VulnerablePackageMetadataCapabilityTests
    {
        [Fact]
        public void Constructor_WithNullPackageMetadataRetrievalAdapter_ThrowsArgumentNullException()
        {
            // Arrange
            IPackageMetadataRetrievalAdapter? packageMetadataRetrievalAdapter = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new VulnerablePackageMetadataCapability(packageMetadataRetrievalAdapter!));
        }

        [Fact]
        public async Task PopulateDataAsync_WithValidData_PopulatesVulnerabilities()
        {
            // Arrange
            var packageMetadataRetrievalAdapterMock = new Mock<IPackageMetadataRetrievalAdapter>();
            var vulnerabilities = new List<PackageVulnerabilityMetadata>
            {
                new PackageVulnerabilityMetadata(new Uri("http://test.com/vuln1"), 1),
                new PackageVulnerabilityMetadata(new Uri("http://test.com/vuln2"), 2)
            };
            var packageMetadata = PackageSearchMetadataContextInfo.Create(new PackageSearchMetadataBuilder.ClonedPackageSearchMetadata() { Vulnerabilities = vulnerabilities });
            packageMetadataRetrievalAdapterMock.Setup(pm => pm.GetPackageMetadataAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(packageMetadata);
            var vulnExpected = packageMetadata.Vulnerabilities;

            var capability = new VulnerablePackageMetadataCapability(packageMetadataRetrievalAdapterMock.Object);

            // Act
            await capability.PopulateDataAsync(CancellationToken.None);

            // Assert
            Assert.Equal(vulnExpected, capability.Vulnerabilities);
        }
    }
}
