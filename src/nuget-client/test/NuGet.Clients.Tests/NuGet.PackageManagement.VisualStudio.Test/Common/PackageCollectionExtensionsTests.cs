// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using NuGet.VisualStudio.Internal.Contracts;
using Xunit;

namespace NuGet.PackageManagement.VisualStudio.Test
{
    public class PackageCollectionExtensionsTests
    {
        public static IEnumerable<PackageCollectionItem> PackageCollectionItemTestData = new[]
        {
            new PackageCollectionItem("packageA", NuGetVersion.Parse("1.0.0"), new PackageReferenceContextInfo[]
            {
                new PackageReferenceContextInfo(new PackageIdentity("packageA", NuGetVersion.Parse("1.0.0")), NuGetFramework.Parse("net6.0"))
            }),
            new PackageCollectionItem("packageA", NuGetVersion.Parse("3.0.0"), new PackageReferenceContextInfo[]
            {
                new PackageReferenceContextInfo(new PackageIdentity("packageA", NuGetVersion.Parse("0.0.3")), NuGetFramework.Parse("net6.0"))
            }),
            new PackageCollectionItem("packageC", NuGetVersion.Parse("2.0.0"), new PackageReferenceContextInfo[0]),
        };

        public static IEnumerable<PackageIdentity> PackageIdentityTestData = new[]
        {
            new PackageIdentity("packageA", new NuGetVersion("1.0.0")),
            new PackageIdentity("packageA", new NuGetVersion("3.0.0")),
            new PackageIdentity("packageC", new NuGetVersion("2.0.0")),
        };

        public static IEnumerable<object[]> GetTestData()
        {
            yield return new object[]
            {
                PackageCollectionItemTestData, typeof(PackageCollectionItem)
            };

            yield return new object[]
            {
                PackageIdentityTestData, typeof(PackageIdentity)
            };
        }

        public static IEnumerable<object[]> GetEmptyTestData()
        {
            yield return new object[]
            {
                Enumerable.Empty<PackageIdentity>()
            };

            yield return new object[]
            {
                Enumerable.Empty<PackageCollectionItem>()
            };
        }

        [Theory]
        [MemberData(nameof(GetTestData))]
        public void GetEarliest_CollectionWithAllElementsOfSameType_ReturnsFirstOfSameTypeAsInput(IEnumerable<PackageIdentity> inputData, Type expectedType)
        {
            // Act
            IEnumerable<PackageIdentity> earliestPackages = inputData.GetEarliest();

            // Assert
            Assert.Collection(earliestPackages,
                e =>
                {
                    Assert.IsType(expectedType, e);
                    Assert.Equal("packageA", e.Id);
                    Assert.Equal("1.0.0", e.Version.ToNormalizedString());
                },
                e =>
                {
                    Assert.IsType(expectedType, e);
                    Assert.Equal("packageC", e.Id);
                    Assert.Equal("2.0.0", e.Version.ToNormalizedString());
                }
            );
        }

        [Fact]
        public void GetEarliest_NullData_Throws()
        {
            // Arrange
            IEnumerable<PackageIdentity> input = null;

            // Act and Assert
            Assert.Throws<ArgumentNullException>(() => input.GetEarliest());
        }

        [Theory]
        [MemberData(nameof(GetEmptyTestData))]
        public void GetEarliest_EmptyCollection_ReturnsEmptyCollection(IEnumerable<PackageIdentity> inputData)
        {
            // Act
            IEnumerable<PackageIdentity> result = inputData.GetEarliest();

            // Assert
            Assert.Empty(result);
        }

        [Theory]
        [MemberData(nameof(GetTestData))]
        public void GetLatest_CollectionWithAllElementsOfSameType_ReturnstLastOfSameTypeAsInput(IEnumerable<PackageIdentity> inputData, Type expectedType)
        {
            // Act
            IEnumerable<PackageIdentity> latestPackages = inputData.GetLatest();

            // Assert
            Assert.Collection(latestPackages,
                e =>
                {
                    Assert.IsType(expectedType, e);
                    Assert.Equal("packageA", e.Id);
                    Assert.Equal("3.0.0", e.Version.ToNormalizedString());
                },
                e =>
                {
                    Assert.IsType(expectedType, e);
                    Assert.Equal("packageC", e.Id);
                    Assert.Equal("2.0.0", e.Version.ToNormalizedString());
                }
            );
        }

        [Fact]
        public void GetLatest_NullData_Throws()
        {
            // Arrange
            IEnumerable<PackageCollectionItem> input = null;

            // Act and Assert
            Assert.Throws<ArgumentNullException>(() => input.GetLatest());
        }

        [Theory]
        [MemberData(nameof(GetTestData))]
        public void GroupById_CollectionWithAllElementsOfSameType_ReturnsGroupOfSameTypeAsInput<T>(IEnumerable<T> inputData, Type expectedType) where T : PackageIdentity
        {
            // Act
            IEnumerable<IGrouping<string, T>> grouped = inputData.GroupById();

            // Assert
            Assert.Collection(grouped,
                group =>
                {
                    Assert.Equal(2, group.Count());
                    Assert.All<object>(group, groupElem => Assert.IsType(expectedType, groupElem));
                },
                group =>
                {
                    Assert.Equal(1, group.Count());
                    Assert.All<object>(group, groupElem => Assert.IsType(expectedType, groupElem));
                });
        }

        [Theory]
        [MemberData(nameof(GetEmptyTestData))]
        public void GroupById_EmptyData_ReturnsEmptyCollection<T>(IEnumerable<T> inputData) where T : PackageIdentity
        {
            // Act
            IEnumerable<IGrouping<string, T>> grouped = inputData.GroupById();

            // Assert
            Assert.Empty(grouped);
        }

        [Fact]
        public void GroupById_NullData_Throws()
        {
            // Arrange
            IEnumerable<PackageIdentity> input = null;

            // Act and Assert
            Assert.Throws<ArgumentNullException>(() => input.GroupById());
        }
    }
}
