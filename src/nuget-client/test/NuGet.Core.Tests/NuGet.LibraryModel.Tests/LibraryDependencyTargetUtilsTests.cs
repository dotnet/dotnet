// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Xunit;

namespace NuGet.LibraryModel.Tests
{
    public class LibraryDependencyTargetUtilsTests
    {
        [Theory]
        [InlineData("assembly", LibraryDependencyTarget.Assembly)]
        [InlineData("ASSEMbly", LibraryDependencyTarget.Assembly)]
        [InlineData("   ASSEMbly  ", LibraryDependencyTarget.Assembly)]
        [InlineData("   ASSEMbly ,,,None ", LibraryDependencyTarget.Assembly)]
        [InlineData("   ASSEMbly ,,,assembly ", LibraryDependencyTarget.Assembly)]
        [InlineData("ExternalProject", LibraryDependencyTarget.ExternalProject)]
        [InlineData("Package", LibraryDependencyTarget.Package)]
        [InlineData("Project", LibraryDependencyTarget.Project)]
        [InlineData("Reference", LibraryDependencyTarget.Reference)]
        [InlineData("WinMD", LibraryDependencyTarget.WinMD)]
        [InlineData("winmd,assembly", LibraryDependencyTarget.WinMD | LibraryDependencyTarget.Assembly)]
        [InlineData("Package,ExternalProject,Project", LibraryDependencyTarget.PackageProjectExternal)]
        [InlineData("PackageProjectExternal", LibraryDependencyTarget.None)]
        [InlineData("PackageProjectExternal,Package", LibraryDependencyTarget.Package)]
        [InlineData("all", LibraryDependencyTarget.All)]
        [InlineData("all,package,assembly", LibraryDependencyTarget.All)]
        [InlineData("all,none", LibraryDependencyTarget.All)]
        [InlineData("none", LibraryDependencyTarget.None)]
        [InlineData("foo", LibraryDependencyTarget.None)]
        [InlineData("foo,bar", LibraryDependencyTarget.None)]
        [InlineData("assembly,foo,bar", LibraryDependencyTarget.Assembly)]
        [InlineData(null, LibraryDependencyTarget.All)]
        [InlineData("", LibraryDependencyTarget.All)]
        [InlineData(",", LibraryDependencyTarget.All)]
        [InlineData(" , ,,   ", LibraryDependencyTarget.All)]
        public void LibraryDependencyTargetUtils_Parse(string input, LibraryDependencyTarget expected)
        {
            // Arrange & Act
            var actual = LibraryDependencyTargetUtils.Parse(input);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
