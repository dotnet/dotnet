// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Text;
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
    public class EmbeddedResourcesCapabilityTests
    {
        [Fact]
        public void Constructor_WithoutFileService_EnforcesRequiredParameter()
        {
            // Arrange
            var identity = new PackageIdentity("TestPackage", new NuGetVersion("1.0.0"));

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new EmbeddedResourcesCapability(null!, identity, null));
        }


        [Fact]
        public void Constructor_WithoutIdentity_EnforcesRequiredParameter()
        {
            // Arrange
            var mockPackageFileService = new Mock<INuGetPackageFileService>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new EmbeddedResourcesCapability(mockPackageFileService.Object, null!, null));
        }

        [Theory]
        [InlineData("stream")]
        [InlineData(null)]
        public async Task GetIconAsync_WithDifferentFileServiceResults_ReturnsFileServiceResults(string? streamText)
        {
            // Arrange
            using Stream? stream = streamText == null ? null : new MemoryStream(Encoding.UTF8.GetBytes(streamText));
            var identity = new PackageIdentity("TestPackage", new NuGetVersion("1.0.0"));
            var mockPackageFileService = new Mock<INuGetPackageFileService>();
            mockPackageFileService.Setup(x => x.GetPackageIconAsync(It.IsAny<PackageIdentity>(), default)).ReturnsAsync(stream);
            EmbeddedResourcesCapability test = new EmbeddedResourcesCapability(mockPackageFileService.Object, identity, null);

            // Act
            var result = await test.GetIconAsync(CancellationToken.None);

            // Assert
            Assert.Equal(stream, result);
            mockPackageFileService.Verify(x => x.GetPackageIconAsync(It.IsAny<PackageIdentity>(), default), Times.Once);
        }

        [Theory]
        [InlineData("stream")]
        [InlineData(null)]
        public async Task GetLicenseAsync_WithDifferentFileServiceResults_ReturnsFileServiceResults(string? streamText)
        {
            // Arrange
            using Stream? stream = streamText == null ? null : new MemoryStream(Encoding.UTF8.GetBytes(streamText));

            var identity = new PackageIdentity("TestPackage", new NuGetVersion("1.0.0"));
            var mockPackageFileService = new Mock<INuGetPackageFileService>();
            mockPackageFileService.Setup(x => x.GetEmbeddedLicenseAsync(It.IsAny<PackageIdentity>(), default)).ReturnsAsync(stream);
            EmbeddedResourcesCapability test = new EmbeddedResourcesCapability(mockPackageFileService.Object, identity, null);

            // Act
            var result = await test.GetLicenseAsync(CancellationToken.None);

            // Assert
            Assert.Equal(stream, result);
            mockPackageFileService.Verify(x => x.GetEmbeddedLicenseAsync(It.IsAny<PackageIdentity>(), default), Times.Once);
        }

        [Theory]
        [InlineData(@"C:\path\to\image.png")]
        [InlineData(null)]
        public async Task GetReadmeAsync_WithDifferentReadmeUri_ReturnStreamOrNull(string? readmePath)
        {
            // Arrange
            using Stream stream = new MemoryStream(Encoding.UTF8.GetBytes("stream"));
            var identity = new PackageIdentity("TestPackage", new NuGetVersion("1.0.0"));
            var mockPackageFileService = new Mock<INuGetPackageFileService>();
            var readmeUri = readmePath != null ? new Uri(@"C:\path\to\image.png") : null;
            var expectedTimes = readmePath != null ? Times.Once() : Times.Never();
            var expectedResult = readmePath != null ? stream : null;

            mockPackageFileService.Setup(x => x.GetReadmeAsync(It.IsAny<Uri>(), default)).ReturnsAsync(stream);
            EmbeddedResourcesCapability test = new EmbeddedResourcesCapability(mockPackageFileService.Object, identity, readmeUri);

            // Act
            var result = await test.GetReadmeAsync(CancellationToken.None);

            // Assert
            Assert.Equal(expectedResult, result);
            mockPackageFileService.Verify(x => x.GetReadmeAsync(It.IsAny<Uri>(), default), expectedTimes);
            Assert.Equal(readmeUri, test.ReadmeUri);
        }
    }
}
