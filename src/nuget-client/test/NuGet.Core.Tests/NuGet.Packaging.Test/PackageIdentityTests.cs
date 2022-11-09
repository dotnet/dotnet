// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Versioning;
using Xunit;

namespace NuGet.Packaging.Core.Test
{
    public class PackageIdentityTests
    {
        [Fact]
        public void TestToString()
        {
            var packageIdentity = new PackageIdentity("packageA", new NuGetVersion("1.0.0"));
            Assert.Equal("packageA.1.0.0", packageIdentity.ToString());

            var packageIdentityNoVersion = new PackageIdentity("packageB", null);
            Assert.Equal("packageB", packageIdentityNoVersion.ToString());

            var formattedString = string.Format("This is package '{0}'", packageIdentity);
            Assert.Equal("This is package 'packageA.1.0.0'", formattedString);
        }
    }
}
