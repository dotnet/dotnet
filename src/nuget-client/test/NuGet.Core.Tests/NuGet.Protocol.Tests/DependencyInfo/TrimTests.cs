// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class TrimTests
    {
        [Fact]
        public void TrimByAllowedVersions_HappyPath_Succeeds()
        {
            // Prepare
            var regInfo = new RegistrationInfo
            {
                Id = "package1",
                IncludePrerelease = true
            };

            var pkgInfo1 = new PackageInfo
            {
                Version = NuGetVersion.Parse("0.0.1")
            };
            regInfo.Add(pkgInfo1);

            var pkgInfo2 = new PackageInfo
            {
                Version = NuGetVersion.Parse("1.0.0")
            };
            regInfo.Add(pkgInfo2);

            var allowedVersions = new Dictionary<string, VersionRange>
            {
                ["package1"] = VersionRange.Parse("1.0.0"),
                ["package2"] = VersionRange.Parse("0.0.1") // not used info
            };

            // Act
            Trim.TrimByAllowedVersions(regInfo, allowedVersions);

            // Verify
            Assert.Equal(1, regInfo.Packages.Count);
        }
    }
}
