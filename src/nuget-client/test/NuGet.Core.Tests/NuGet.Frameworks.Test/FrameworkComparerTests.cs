// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using NuGet.Frameworks;
using Xunit;

namespace NuGet.Test
{
    public class FrameworkComparerTests
    {
        [Fact]
        public void FrameworkComparer_OrderingMixedFrameworksTypes()
        {
            // Arrange
            // non-package-based in the precedence list
            var fw1 = NuGetFramework.Parse("net45");
            var fw2 = NuGetFramework.Parse("netcore451");
            var fw3 = NuGetFramework.Parse("win81");
            var fw4 = NuGetFramework.Parse("wpa81");
            // non-package-based not in the precedence list
            var fw5 = NuGetFramework.Parse("xamarinios");
            var fw6 = NuGetFramework.Parse("sl5");
            // package-based in the precedence list
            var fw7 = NuGetFramework.Parse("netcoreapp1.0");
            var fw8 = NuGetFramework.Parse("netstandardapp1.1");
            var fw9 = NuGetFramework.Parse("netstandard1.1");
            var fw10 = NuGetFramework.Parse("dotnet5.2");
            // package-based not in the precedence list
            var fw11 = NuGetFramework.Parse("dnxcore50");

            var list = new List<NuGetFramework>
            {
                fw3,
                fw6,
                fw10,
                fw5,
                fw9,
                fw2,
                fw11,
                fw4,
                fw1,
                fw7,
                fw8,
            };

            // Act
            list = list
                .OrderBy(f => f, new FrameworkPrecedenceSorter(DefaultFrameworkNameProvider.Instance, false))
                .ThenByDescending(f => f, NuGetFrameworkSorter.Instance)
                .ToList();

            // Assert
            Assert.Equal(fw1, list[0]);
            Assert.Equal(fw2, list[1]);
            Assert.Equal(fw3, list[2]);
            Assert.Equal(fw4, list[3]);
            Assert.Equal(fw5, list[4]);
            Assert.Equal(fw6, list[5]);
            Assert.Equal(fw7, list[6]);
            Assert.Equal(fw8, list[7]);
            Assert.Equal(fw9, list[8]);
            Assert.Equal(fw10, list[9]);
            Assert.Equal(fw11, list[10]);
        }

        [Fact]
        public void FrameworkComparer_NonPackageBasedFrameworkPreferredAndNormalOrdering()
        {
            // Arrange
            var fw1 = NuGetFramework.Parse("net45");
            var fw2 = NuGetFramework.Parse("net40");
            var fw3 = NuGetFramework.Parse("netcore451");
            var fw4 = NuGetFramework.Parse("netcore45");
            var fw5 = NuGetFramework.Parse("win81");
            var fw6 = NuGetFramework.Parse("win8");
            var fw7 = NuGetFramework.Parse("wpa81");
            var fw8 = NuGetFramework.Parse("sl5");
            var fw9 = NuGetFramework.Parse("sl4");
            var fw10 = NuGetFramework.Parse("sl3");

            var list = new List<NuGetFramework>
            {
                fw3,
                fw5,
                fw10,
                fw9,
                fw2,
                fw4,
                fw1,
                fw6,
                fw7,
                fw8,
            };

            // Act
            list = list
                .OrderBy(f => f, new FrameworkPrecedenceSorter(DefaultFrameworkNameProvider.Instance, false))
                .ThenByDescending(f => f, NuGetFrameworkSorter.Instance)
                .ToList();

            // Assert
            Assert.Equal(fw1, list[0]);
            Assert.Equal(fw2, list[1]);
            Assert.Equal(fw3, list[2]);
            Assert.Equal(fw4, list[3]);
            Assert.Equal(fw5, list[4]);
            Assert.Equal(fw6, list[5]);
            Assert.Equal(fw7, list[6]);
            Assert.Equal(fw8, list[7]);
            Assert.Equal(fw9, list[8]);
            Assert.Equal(fw10, list[9]);
        }

        [Fact]
        public void FrameworkComparer_PackageBasedFrameworkPreferredAndNormalOrdering()
        {
            // Arrange
            var fw1 = NuGetFramework.Parse("netcoreapp1.1");
            var fw2 = NuGetFramework.Parse("netcoreapp1.0");
            var fw3 = NuGetFramework.Parse("netstandardapp1.1");
            var fw4 = NuGetFramework.Parse("netstandardapp1.0");
            var fw5 = NuGetFramework.Parse("netstandard1.1");
            var fw6 = NuGetFramework.Parse("netstandard1.0");
            var fw7 = NuGetFramework.Parse("dotnet5.2");
            var fw8 = NuGetFramework.Parse("dotnet5.1");
            var fw9 = NuGetFramework.Parse("dnxcore50");

            var list = new List<NuGetFramework>
            {
                fw9,
                fw6,
                fw3,
                fw5,
                fw8,
                fw2,
                fw7,
                fw4,
                fw1
            };

            // Act
            list = list
                .OrderBy(f => f, new FrameworkPrecedenceSorter(DefaultFrameworkNameProvider.Instance, false))
                .ThenByDescending(f => f, NuGetFrameworkSorter.Instance)
                .ToList();

            // Assert
            Assert.Equal(fw1, list[0]);
            Assert.Equal(fw2, list[1]);
            Assert.Equal(fw3, list[2]);
            Assert.Equal(fw4, list[3]);
            Assert.Equal(fw5, list[4]);
            Assert.Equal(fw6, list[5]);
            Assert.Equal(fw7, list[6]);
            Assert.Equal(fw8, list[7]);
            Assert.Equal(fw9, list[8]);
        }

        [Fact]
        public void FrameworkComparer_VersionNormalize()
        {
            var fw1 = NuGetFramework.Parse("net45");
            var fw2 = NuGetFramework.Parse("net4.5");
            var fw3 = NuGetFramework.Parse("net4.5.0");
            var fw4 = NuGetFramework.Parse("net450");
            var fw5 = NuGetFramework.Parse(".NETFramework45");

            var comparer = NuGetFrameworkFullComparer.Instance;

            Assert.True(comparer.Equals(fw1, fw2));
            Assert.True(comparer.Equals(fw1, fw3));
            Assert.True(comparer.Equals(fw1, fw4));
            Assert.True(comparer.Equals(fw1, fw5));
        }

        [Fact]
        public void FrameworkComparer_PCLNormalize()
        {
            var fw1 = NuGetFramework.Parse("portable-net45+win8+wp8+wpa81+monotouch+monoandroid");
            var fw2 = NuGetFramework.Parse("portable-net45+win8+wp8+wpa81");
            var fw3 = NuGetFramework.Parse(".NETPortable, Version=v0.0, Profile=Profile259");

            var comparer = NuGetFrameworkFullComparer.Instance;

            Assert.True(comparer.Equals(fw1, fw2), "2");
            Assert.True(comparer.Equals(fw1, fw3), "3");
        }

        [Fact]
        public void FrameworkSorter_OrderingFrameworksWithPlatform()
        {
            // Arrange
            var fw1 = NuGetFramework.Parse("net5.0");
            var fw2 = NuGetFramework.Parse("net6.0");
            var fw3 = NuGetFramework.Parse("net6.0-android10.0");
            var fw4 = NuGetFramework.Parse("net6.0-android13.0");
            var fw5 = NuGetFramework.Parse("net6.0-ios");
            var fw6 = NuGetFramework.Parse("net6.0-ios1.0");
            var fw7 = NuGetFramework.Parse("net7.0-android11.0");
            var fw8 = NuGetFramework.Parse("net8.0");

            var list = new List<NuGetFramework>
            {
                fw3,
                fw6,
                fw5,
                fw2,
                fw4,
                fw1,
                fw7,
                fw8,
            };

            // Act
            list = list
                .OrderBy(f => f, new FrameworkPrecedenceSorter(DefaultFrameworkNameProvider.Instance, false))
                .ThenBy(f => f, NuGetFrameworkSorter.Instance)
                .ToList();

            // Assert
            Assert.Equal(fw1, list[0]);
            Assert.Equal(fw2, list[1]);
            Assert.Equal(fw3, list[2]);
            Assert.Equal(fw4, list[3]);
            Assert.Equal(fw5, list[4]);
            Assert.Equal(fw6, list[5]);
            Assert.Equal(fw7, list[6]);
            Assert.Equal(fw8, list[7]);
        }
    }
}
