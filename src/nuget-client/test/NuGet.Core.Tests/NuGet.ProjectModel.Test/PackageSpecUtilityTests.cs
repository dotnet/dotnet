// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NuGet.Versioning;
using Xunit;

namespace NuGet.ProjectModel.Test
{
    public class PackageSpecUtilityTests
    {
        [Theory]
        [InlineData("1.0-*")]
        [InlineData("1.0.0-*")]
        [InlineData("1.0.0-beta-*")]
        [InlineData("1.2.3.4-beta-*")]
        [InlineData("1.2.3.4-beta-a-*")]
        [InlineData("1.2.3.4-beta.*")]
        [InlineData("1.2.3.4-beta.1.*")]
        public void PackageSpecUtility_IsSnapshotVersion_True(string version)
        {
            Assert.True(PackageSpecUtility.IsSnapshotVersion(version));
        }

        [Theory]
        [InlineData("")]
        [InlineData("a")]
        [InlineData("1.0.0-beta.01.*")]
        [InlineData("1.0.0-beta-*.*")]
        [InlineData("1.2.3.4-beta*")]
        [InlineData("1.2.3.4-beta-**")]
        [InlineData("1.2.3.4-beta.**")]
        [InlineData("1.2.3.4-beta.1.*+beta")]
        [InlineData("1.2.3.4-beta.1+5.*")]
        public void PackageSpecUtility_IsSnapshotVersion_False(string version)
        {
            Assert.False(PackageSpecUtility.IsSnapshotVersion(version));
        }

        [Theory]
        [InlineData("1.0-*", "", "1.0.0")]
        [InlineData("2.0.0-beta-*", "", "2.0.0-beta")]
        [InlineData("2.0.0-beta.*", "", "2.0.0-beta")]
        [InlineData("2.0.0-beta.*", "2", "2.0.0-beta.2")]
        [InlineData("2.0.0-beta.5.*", "0", "2.0.0-beta.5.0")]
        [InlineData("2.0.0-beta.*", "final.release-label+git.hash", "2.0.0-beta.final.release-label+git.hash")]
        public void PackageSpecUtility_SpecifySnapshotVersion(string version, string snapshotValue, string expected)
        {
            // Arrange && Act
            var actual = PackageSpecUtility.SpecifySnapshot(version, snapshotValue);

            // Assert
            Assert.Equal(NuGetVersion.Parse(expected), actual);
        }
    }
}
