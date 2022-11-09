// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using Xunit;

namespace NuGet.PackageManagement.UI.Test.Models
{
    public class PackageDependencySetMetadataTests
    {
        [Theory]
        [InlineData("net472", ".NETFramework,Version=v4.7.2")]
        [InlineData("netstandard2.0", ".NETStandard,Version=v2.0")]
        [InlineData("netcoreapp3.1", ".NETCoreApp,Version=v3.1")]
        [InlineData("net5.0", "net5.0")]
        [InlineData("net5.0-windows7.0", "net5.0-windows7.0")]
        public void ctor_WithDependencyGroup_UsesExpectedTargetFrameworkDisplay(string targetFrameworkShortName, string expectedDisplay)
        {
            // Arrange
            var framework = NuGetFramework.Parse(targetFrameworkShortName);
            var group = new PackageDependencyGroup(framework, Array.Empty<PackageDependency>());

            // Act
            var target = new PackageDependencySetMetadata(group);

            // Assert
            Assert.Equal(expectedDisplay, target.TargetFrameworkDisplay);
        }
    }
}
