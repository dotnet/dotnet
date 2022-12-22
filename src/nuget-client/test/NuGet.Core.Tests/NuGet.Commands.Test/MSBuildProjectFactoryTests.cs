// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using NuGet.Commands;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Commands.Test
{
    public class MSBuildProjectFactoryTests
    {
        [PlatformTheory(Platform.Windows)]
        [InlineData("D:\\temp\\project1\\source.cs", "D:\\temp\\project1", "src\\project1\\source.cs")]
        [InlineData("D:\\temp\\project1\\folder1\\source.cs", "D:\\temp\\project1", "src\\project1\\folder1\\source.cs")]
        [InlineData("D:\\temp\\project1\\folder1\\folder2\\source.cs", "D:\\temp\\project1", "src\\project1\\folder1\\folder2\\source.cs")]
        [InlineData("D:\\temp\\source.cs", "D:\\temp\\project1", "src\\project1\\source.cs")]
        [InlineData("D:/temp/project1/source.cs", "D:/temp/project1", "src\\project1\\source.cs")]
        [InlineData("D:/temp/project1/folder1/source.cs", "D:/temp/project1", "src\\project1\\folder1\\source.cs")]
        [InlineData("D:/temp/project1/folder1/folder2/source.cs", "D:/temp/project1", "src\\project1\\folder1\\folder2\\source.cs")]
        [InlineData("D:/temp/source.cs", "D:/temp/project1", "src\\project1\\source.cs")]
        public void GetTargetPathForSourceFiles_Windows(string sourcePath, string projectDirectory, string expectedResult)
        {
            var result = MSBuildProjectFactory.GetTargetPathForSourceFile(sourcePath, projectDirectory);
            Assert.Equal(expectedResult, result);
        }

        [PlatformTheory(Platform.Linux)]
        [InlineData("/mnt/d/temp/project1/source.cs", "/mnt/d/temp/project1", "src/project1/source.cs")]
        [InlineData("/mnt/d/temp/project1/folder1/source.cs", "/mnt/d/temp/project1", "src/project1/folder1/source.cs")]
        [InlineData("/mnt/d/temp/project1/folder1/folder2/source.cs", "/mnt/d/temp/project1", "src/project1/folder1/folder2/source.cs")]
        [InlineData("/mnt/d/temp/source.cs", "/mnt/d/temp/project1", "src/project1/source.cs")]

        public void GetTargetPathForSourceFiles_Linux(string sourcePath, string projectDirectory, string expectedResult)
        {
            var result = MSBuildProjectFactory.GetTargetPathForSourceFile(sourcePath, projectDirectory);
            Assert.Equal(expectedResult, result);
        }

        [PlatformTheory(Platform.Darwin)]
        [InlineData("/mnt/d/temp/project1/source.cs", "/mnt/d/temp/project1", "src/project1/source.cs")]
        [InlineData("/mnt/d/temp/project1/folder1/source.cs", "/mnt/d/temp/project1", "src/project1/folder1/source.cs")]
        [InlineData("/mnt/d/temp/project1/folder1/folder2/source.cs", "/mnt/d/temp/project1", "src/project1/folder1/folder2/source.cs")]
        [InlineData("/mnt/d/temp/source.cs", "/mnt/d/temp/project1", "src/project1/source.cs")]
        public void GetTargetPathForSourceFiles_Darwin(string sourcePath, string projectDirectory, string expectedResult)
        {
            var result = MSBuildProjectFactory.GetTargetPathForSourceFile(sourcePath, projectDirectory);
            Assert.Equal(expectedResult, result);
        }
    }
}
