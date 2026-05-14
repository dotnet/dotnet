// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using NuGet.Versioning;
using Xunit;

namespace NuGet.LibraryModel.Tests
{
    public class CentralPackageVersionNameComparerTests
    {
        [Fact]
        public void DistinctListOfObjects()
        {
            // Arrange
            var name1 = "name1";
            var name2 = "name2";
            var nameU1 = "Name1";
            var versionRange1 = VersionRange.Parse("1.0.0");
            var versionRange2 = VersionRange.Parse("2.0.0");
            var versionRange3 = VersionRange.Parse("3.0.0");

            var cpv11 = new CentralPackageVersion(name1, versionRange1);
            var cpv12 = new CentralPackageVersion(name1, versionRange2);
            var cpvU13 = new CentralPackageVersion(nameU1, versionRange3);
            var cpv21 = new CentralPackageVersion(name2, versionRange1);

            var cpvs = new List<CentralPackageVersion>() { cpv11, cpv12, cpvU13, cpv21 };

            // Act
            var distinctElements = cpvs.Distinct(CentralPackageVersionNameComparer.Default).ToList();

            // Assert
            // There should be only two elements distinct as the comparer is name comparer with StringComparison OrdinalIgnoreCase
            Assert.Equal(2, distinctElements.Count);
            Assert.Equal(name1, distinctElements[0].Name);
            Assert.Equal(name2, distinctElements[1].Name);
        }
    }
}
