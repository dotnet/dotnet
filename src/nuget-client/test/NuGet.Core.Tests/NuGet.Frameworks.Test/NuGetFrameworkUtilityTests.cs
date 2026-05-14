// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using NuGet.Packaging;
using Xunit;

namespace NuGet.Frameworks.Test
{
    public class NuGetFrameworkUtilityTests
    {

        [Fact]
        public void NuGetFrameworkUtility_GetNearest_EmptyListReturnsNull()
        {
            // Arrange
            var project = NuGetFramework.Parse("net45");
            var items = new List<FrameworkSpecificGroup>();

            // Act
            var nearestWithSelector = NuGetFrameworkUtility.GetNearest<FrameworkSpecificGroup>(items, project, (item) => item.TargetFramework);
            var nearest = NuGetFrameworkUtility.GetNearest<FrameworkSpecificGroup>(items, project);

            // Assert
            Assert.Null(nearest);
            Assert.Null(nearestWithSelector);
        }

        [Fact]
        public void NuGetFrameworkUtility_GetNearest_NullReturnsNull()
        {
            // Arrange
            var project = NuGetFramework.Parse("net45");
            List<FrameworkSpecificGroup>? items = null;

            // Act
            var nearestWithSelector = NuGetFrameworkUtility.GetNearest<FrameworkSpecificGroup>(items!, project, (item) => item.TargetFramework);
            var nearest = NuGetFrameworkUtility.GetNearest<FrameworkSpecificGroup>(items!, project);

            // Assert
            Assert.Null(nearest);
            Assert.Null(nearestWithSelector);
        }

        [Fact]
        public void NuGetFrameworkUtility_GetNearest_ExactMatch()
        {
            // Arrange
            var project = NuGetFramework.Parse("net45");
            var items = new List<FrameworkSpecificGroup>();
            items.Add(new FrameworkSpecificGroup(NuGetFramework.Parse("net45"), new string[] { "lib/net45/test.dll" }));
            items.Add(new FrameworkSpecificGroup(NuGetFramework.Parse("net4"), new string[] { "lib/net4/test.dll" }));
            items.Add(new FrameworkSpecificGroup(NuGetFramework.Parse("win"), new string[] { "lib/win/test.dll" }));
            items.Add(new FrameworkSpecificGroup(NuGetFramework.Parse("any"), new string[] { "lib/test.dll" }));

            // Act
            var nearestWithSelector = NuGetFrameworkUtility.GetNearest<FrameworkSpecificGroup>(items, project, (item) => item.TargetFramework);
            var nearest = NuGetFrameworkUtility.GetNearest<FrameworkSpecificGroup>(items, project);

            // Assert
            Assert.Equal("net45", nearestWithSelector!.TargetFramework.GetShortFolderName());
            Assert.Equal("net45", nearest!.TargetFramework.GetShortFolderName());
        }


        [Fact]
        public void NuGetFrameworkUtility_GetNearest_Any()
        {
            // Arrange
            var project = NuGetFramework.Parse("net45");
            var items = new List<FrameworkSpecificGroup>();
            items.Add(new FrameworkSpecificGroup(NuGetFramework.Parse("win"), new string[] { "lib/win/test.dll" }));
            items.Add(new FrameworkSpecificGroup(NuGetFramework.Parse("any"), new string[] { "lib/test.dll" }));

            // Act
            var nearestWithSelector = NuGetFrameworkUtility.GetNearest<FrameworkSpecificGroup>(items, project, (item) => item.TargetFramework);
            var nearest = NuGetFrameworkUtility.GetNearest<FrameworkSpecificGroup>(items, project);

            // Assert
            Assert.Equal("any", nearestWithSelector!.TargetFramework.GetShortFolderName());
            Assert.Equal("any", nearest!.TargetFramework.GetShortFolderName());
        }

        [Fact]
        public void NuGetFrameworkUtility_GetNearest_NoMatch()
        {
            // Arrange
            var project = NuGetFramework.Parse("net45");
            var items = new List<FrameworkSpecificGroup>();
            items.Add(new FrameworkSpecificGroup(NuGetFramework.Parse("win"), new string[] { "lib/win/test.dll" }));
            items.Add(new FrameworkSpecificGroup(NuGetFramework.Parse("net46"), new string[] { "lib/net46/test.dll" }));

            // Act
            var nearestWithSelector = NuGetFrameworkUtility.GetNearest<FrameworkSpecificGroup>(items, project, (item) => item.TargetFramework);
            var nearest = NuGetFrameworkUtility.GetNearest<FrameworkSpecificGroup>(items, project);

            // Assert
            Assert.Null(nearestWithSelector);
            Assert.Null(nearest);
        }

        [Fact]
        public void NuGetFrameworkUtility_GetNearest_Duplicates()
        {
            // Arrange
            var project = NuGetFramework.Parse("net45");
            var items = new List<FrameworkSpecificGroup>();
            items.Add(new FrameworkSpecificGroup(NuGetFramework.UnsupportedFramework, new string[] { "lib/win/test.dll" }));
            items.Add(new FrameworkSpecificGroup(NuGetFramework.UnsupportedFramework, new string[] { "lib/net46/test.dll" }));
            items.Add(new FrameworkSpecificGroup(NuGetFramework.UnsupportedFramework, new string[] { "lib/net45/test.dll" }));

            // Act
            var nearestWithSelector = NuGetFrameworkUtility.GetNearest<FrameworkSpecificGroup>(items, project, (item) => item.TargetFramework);
            var nearest = NuGetFrameworkUtility.GetNearest<FrameworkSpecificGroup>(items, project);

            // Assert
            Assert.Null(nearestWithSelector);
            Assert.Null(nearest);
        }
    }
}
