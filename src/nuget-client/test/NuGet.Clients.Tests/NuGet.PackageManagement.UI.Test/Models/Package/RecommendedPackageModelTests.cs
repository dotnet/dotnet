// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Moq;
using NuGet.PackageManagement.UI.Models.Package;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using Xunit;

namespace NuGet.PackageManagement.UI.Test.Models.Package
{
    public class RecommendedPackageModelTests
    {
        [Fact]
        public void Constructor_SetRecommenderVersion_InitializeRecommenderVersion()
        {
            // Arrange
            var identity = new PackageIdentity("TestPackage", new NuGetVersion("1.0.0"));
            var vulnerabilityCapability = new Mock<IVulnerableCapable>();
            var embeddedCapability = new Mock<IEmbeddedResourcesCapable>();
            var deprecatedCapability = new Mock<IDeprecationCapable>();
            (string modelVersion, string vsixVersion) recommenderVersion = ("1.0.0", "1.0.0");

            // Act
            var model = PackageModelCreationTestHelper.CreateRecommendedPackageModel(
                identity,
                vulnerabilityCapability.Object,
                deprecatedCapability.Object,
                embeddedCapability.Object,
                recommenderVersion);

            // Assert
            Assert.Equal(recommenderVersion, model.RecommenderVersion);
        }
    }
}
