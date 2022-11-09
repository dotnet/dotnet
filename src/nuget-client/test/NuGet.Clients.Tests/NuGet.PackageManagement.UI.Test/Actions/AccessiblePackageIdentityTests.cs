// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Packaging.Core;
using NuGet.Versioning;
using Xunit;

namespace NuGet.PackageManagement.UI.Test
{
    public class AccessiblePackageIdentityTests
    {
        [Fact]
        public void AutomationName_WhenCultureIsNeutral_ReturnsMessage()
        {
            var pkgId = "test.package";
            var version = new NuGetVersion(0, 0, 1);
            var result = new AccessiblePackageIdentity(new PackageIdentity(pkgId, version));

            Assert.Equal($"{pkgId} version {version.ToNormalizedString()}", result.AutomationName);
        }
    }
}
