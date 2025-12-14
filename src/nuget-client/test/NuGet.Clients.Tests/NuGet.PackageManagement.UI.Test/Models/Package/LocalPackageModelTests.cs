// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.PackageManagement.UI.Models.Package;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using Xunit;

namespace NuGet.PackageManagement.UI.Test.Models.Package
{
    public class LocalPackageModelTests
    {
        private Mock<IEmbeddedResourcesCapable> _embeddedResourcesMock;

        public LocalPackageModelTests()
        {
            _embeddedResourcesMock = new Mock<IEmbeddedResourcesCapable>();
        }

        [Fact]
        public void LocalPackageModelCtr_IdAndVersion_ReturnsValueFromIdentity()
        {
            // Arrange
            var identity = new PackageIdentity("TestPackage", new NuGetVersion("1.0.0"));
            var packagePath = "C:\\TestPackage";

            // Act
            var package = PackageModelCreationTestHelper.CreateLocalPackageModel(identity, packagePath, _embeddedResourcesMock.Object);

            // Assert
            Assert.Equal("TestPackage", package.Id);
            Assert.Equal(new NuGetVersion("1.0.0"), package.Version);
        }

        [Fact]
        public void LocalPackageModelCtr_PackagePath_ReturnsExpected()
        {
            // Arrange
            var identity = new PackageIdentity("TestPackage", new NuGetVersion("1.0.0"));
            var packagePath = "C:\\TestPackage";

            // Act
            var package = PackageModelCreationTestHelper.CreateLocalPackageModel(identity, packagePath, _embeddedResourcesMock.Object);

            // Assert
            Assert.Equal(packagePath, package.PackagePath);
        }

        [Fact]
        public void PopulateDataAsync_WhenCalled_CompletesSuccessfully()
        {
            // Arrange
            var cancellationToken = new CancellationToken();
            var identity = new PackageIdentity("TestPackage", new NuGetVersion("1.0.0"));
            var packagePath = "C:\\TestPackage";
            var package = PackageModelCreationTestHelper.CreateLocalPackageModel(identity, packagePath, _embeddedResourcesMock.Object);

            // Act
            Task result = package.PopulateDataAsync(cancellationToken);

            // Assert
            Assert.Equal(Task.CompletedTask, result);
        }
    }
}
