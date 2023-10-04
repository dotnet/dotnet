// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using Moq;
using NuGet.Configuration;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using Xunit;

namespace NuGet.PackageManagement.UI.Test.Utility
{
    public class PackageSourceMappingUtilityTests
    {
        [Fact]
        public void AddNewSourceMappingsFromAddedPackages_PackageNotMappedToMultipleExistingSources_CreatesSingleNewMapping()
        {
            // Arrange
            Dictionary<string, SortedSet<string>> newSourceMappings = new();

            // Existing Package Source Mappings.
            var dictionary = new Dictionary<string, IReadOnlyList<string>>
            {
                { "sourceA", new List<string>() { "b", "c", "d" } },
                { "sourceB", new List<string>() { "b", "c", "d" } }
            };
            var patterns = new ReadOnlyDictionary<string, IReadOnlyList<string>>(dictionary);
            var mockPackageSourceMapping = new Mock<PackageSourceMapping>(patterns);

            // Configure packages which are added by preview restore and may need new Package Source Mappings.
            string newMappingSourceName = "sourceC";
            List<AccessiblePackageIdentity> added = new List<AccessiblePackageIdentity>()
            {
                ConvertToAccessiblePackageIdentity("a"), // The only added package missing from existing mappings.
                ConvertToAccessiblePackageIdentity("b"),
                ConvertToAccessiblePackageIdentity("c"),
                ConvertToAccessiblePackageIdentity("d"),
            };

            // Repeat the Act to simulate multiple ProjectActions to ensure a unique resulting set of packages is created.
            for (int i = 0; i < 2; i++)
            {
                // Act
                PackageSourceMappingUtility.AddNewSourceMappingsFromAddedPackages(ref newSourceMappings, newMappingSourceName, added, mockPackageSourceMapping.Object);

                // Assert
                Assert.Equal(1, newSourceMappings.Count);
                Assert.True(newSourceMappings.ContainsKey(newMappingSourceName));
                Assert.Equal(1, newSourceMappings[newMappingSourceName].Count);
            }
        }

        private AccessiblePackageIdentity ConvertToAccessiblePackageIdentity(string packageId)
        {
            return new AccessiblePackageIdentity(new PackageIdentity(packageId, NuGetVersion.Parse("1.0.0")));
        }
    }
}
