// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using NuGet.LibraryModel;
using NuGet.Versioning;
using Xunit;

namespace NuGet.DependencyResolver.Core.Tests
{
    public class GraphItemKeyComparerTests
    {
        [Theory]
        [MemberData(nameof(Data))]
        public void Equals_GraphItemsAreEqualOnlyIfKeysAreEqual(GraphItem<string> item1, GraphItem<string> item2, bool expectedResult)
        {
            // Assert
            if (expectedResult)
            {
                Assert.True(GraphItemKeyComparer<string>.Instance.Equals(item1, item2));
                Assert.Equal(GraphItemKeyComparer<string>.Instance.GetHashCode(item1), GraphItemKeyComparer<string>.Instance.GetHashCode(item2));
            }
            else
            {
                Assert.False(GraphItemKeyComparer<string>.Instance.Equals(item1, item2));
                Assert.NotEqual(GraphItemKeyComparer<string>.Instance.GetHashCode(item1), GraphItemKeyComparer<string>.Instance.GetHashCode(item2));
            }
        }

        public static IEnumerable<object[]> Data =>
           new List<object[]>
           {
                new object[] {new GraphItem<string>(new LibraryIdentity("lib1", NuGetVersion.Parse("1.0.0"), LibraryType.Package)), new GraphItem<string>(new LibraryIdentity("lib1", NuGetVersion.Parse("1.0.0"), LibraryType.Package)), true },
                new object[] {new GraphItem<string>(new LibraryIdentity("lib1", NuGetVersion.Parse("1.0.0"), LibraryType.Package)), new GraphItem<string>(new LibraryIdentity("lib1", NuGetVersion.Parse("1.0.0"), LibraryType.ExternalProject)), false },
                new object[] {new GraphItem<string>(new LibraryIdentity("lib1", NuGetVersion.Parse("1.0.0"), LibraryType.Package)), new GraphItem<string>(new LibraryIdentity("lib2", NuGetVersion.Parse("1.0.0"), LibraryType.Package)), false },
                new object[] {new GraphItem<string>(new LibraryIdentity("lib1", NuGetVersion.Parse("1.0.0"), LibraryType.Package)), new GraphItem<string>(new LibraryIdentity("lib1", NuGetVersion.Parse("2.0.0"), LibraryType.Package)), false },
                new object[] {new GraphItem<string>(new LibraryIdentity("lib1", NuGetVersion.Parse("1.0.0"), LibraryType.Package)), new GraphItem<string>(new LibraryIdentity("lib1", NuGetVersion.Parse("1.0.0"), LibraryType.Package)){ IsCentralTransitive = true }, true },
                new object[] {new GraphItem<string>(new LibraryIdentity("lib1", NuGetVersion.Parse("1.0.0"), LibraryType.Package)), new GraphItem<string>(new LibraryIdentity("lib1", NuGetVersion.Parse("1.0.0"), LibraryType.Package)){ Data = "foo" }, true },
           };

    }
}
