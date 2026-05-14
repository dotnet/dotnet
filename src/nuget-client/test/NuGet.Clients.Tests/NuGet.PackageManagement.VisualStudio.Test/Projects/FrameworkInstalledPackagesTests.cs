// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Frameworks;
using NuGet.PackageManagement.VisualStudio.Utility;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using Xunit;

namespace NuGet.PackageManagement.VisualStudio.Test
{
    public class FrameworkInstalledPackagesTests
    {
        [Fact]
        public void FrameworkInstalledPackagesTests_Equals_Succeeds()
        {
            var a = new FrameworkInstalledPackages
            {
                TargetFramework = NuGetFramework.Parse("net472"),
                Packages = new()
                {
                    ["package1"] = new ProjectInstalledPackage(VersionRange.Parse("1.0.0"), new PackageIdentity("package1", NuGetVersion.Parse("1.0.0"))),
                    ["package2"] = new ProjectInstalledPackage(VersionRange.Parse("1.0.0"), new PackageIdentity("package2", NuGetVersion.Parse("1.0.0"))),
                }
            };

            var b = new FrameworkInstalledPackages
            {
                TargetFramework = NuGetFramework.Parse("net472"),
                Packages = new()
                {
                    ["package1"] = new ProjectInstalledPackage(VersionRange.Parse("1.0.0"), new PackageIdentity("package1", NuGetVersion.Parse("1.0.0"))),
                    ["package2"] = new ProjectInstalledPackage(VersionRange.Parse("1.0.0"), new PackageIdentity("package2", NuGetVersion.Parse("1.0.0"))),
                }
            };

            Assert.True(a.Equals(b));
            Assert.True(b.Equals(a));
            Assert.True(a.Equals(a));
            Assert.True(b.Equals(b));
            Assert.False(a.Equals(null));
            Assert.False(a == b);
        }
    }
}
