// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using Microsoft.ServiceHub.Framework;
using Moq;
using NuGet.Versioning;
using NuGet.VisualStudio.Internal.Contracts;
using Xunit;

namespace NuGet.PackageManagement.UI.Test
{
    public class DetailControlModelTests
    {
        [Theory]
        [InlineData("*", "ANewPackage")]
        [InlineData("*-*", "ANewPackage")]
        [InlineData("0.0.0", "ANewPackage")]
        [InlineData("[0.0.0,)", "ANewPackage")]
        [InlineData("(0.0.0,)", "ANewPackage > 0.0.0")]
        [InlineData("1.0.0", "ANewPackage >= 1.0.0")]
        public void DeprecationAlternativePackage_WithAsterisk_ShowsNoVersionInfo(string versionRange, string expected)
        {
            var model = new PackageDetailControlModel(
                serviceBroker: Mock.Of<IServiceBroker>(),
                solutionManager: Mock.Of<INuGetSolutionManagerService>(),
                projects: Enumerable.Empty<IProjectContextInfo>(),
                uiController: Mock.Of<INuGetUI>());

            var metadata = new DetailedPackageMetadata()
            {
                DeprecationMetadata = new PackageDeprecationMetadataContextInfo(
                    message: "package deprecated",
                    reasons: new[] { "package deprecated", "legacy" },
                    alternatePackageContextInfo: new AlternatePackageMetadataContextInfo(
                         packageId: "ANewPackage",
                         range: VersionRange.Parse(versionRange))
                )
            };
            model.PackageMetadata = metadata;

            Assert.NotNull(model.PackageDeprecationAlternatePackageText);
            Assert.Equal(expected, model.PackageDeprecationAlternatePackageText);
        }
    }
}
