// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using NuGet.Packaging.Core;
using Xunit;
using Moq;
using NuGet.Versioning;
using NuGet.PackageManagement.UI.Models.Package;

namespace NuGet.PackageManagement.UI.Test.Models.Package
{
    public class TransitivelyReferencedPackageModelTests
    {
        [Fact]
        public void Constructor_InitializesTransitiveOrigins()
        {
            // Arrange
            var identity = new PackageIdentity("TestPackage", new NuGetVersion("1.0.0"));
            var mockVulnerableCapability = new Mock<IVulnerableCapable>();
            var mockDeprecationCapability = new Mock<IDeprecationCapable>();
            var mockEmbeddedCapability = new Mock<IEmbeddedResourcesCapable>();
            var transitiveOrigins = new List<PackageIdentity>
            {
                new PackageIdentity("OriginPackage1", new NuGetVersion("1.0.0")),
                new PackageIdentity("OriginPackage2", new NuGetVersion("2.0.0"))
            };

            // Act
            var model = PackageModelCreationTestHelper.CreateTransitivelyReferencedPackageModel(
                identity,
                mockVulnerableCapability.Object,
                mockEmbeddedCapability.Object,
                transitiveOrigins);

            // Assert
            Assert.Equal(transitiveOrigins, model.TransitiveOrigins);
        }
    }
}
