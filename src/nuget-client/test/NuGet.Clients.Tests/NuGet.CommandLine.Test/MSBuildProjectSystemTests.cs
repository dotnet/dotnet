// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Text;
using System.Xml.Linq;
using Moq;
using NuGet.Common;
using NuGet.Test.Utility;
using Test.Utility;
using Xunit;
using Xunit.Abstractions;

namespace NuGet.CommandLine.Test
{
    public class MSBuildProjectSystemTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public MSBuildProjectSystemTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        private class TestInfo : IDisposable
        {
            private readonly TestDirectory _projectDirectory;
            private readonly string _msBuildDirectory;
            private readonly TestNuGetProjectContext _nuGetProjectContext;

            public MSBuildProjectSystem MSBuildProjectSystem { get; }

            public TestInfo(ITestOutputHelper testOutputHelper, string projectFileContent, string projectName = "proj1")
            {
                var console = new Mock<IConsole>();

                console.Setup(c => c.WriteLine(It.IsAny<string>(), It.IsAny<object[]>())).Callback<string, object[]>((format, args) => testOutputHelper.WriteLine(format, args));

                console.SetupGet(c => c.Verbosity).Returns(Verbosity.Detailed);

                _projectDirectory = TestDirectory.Create();
                _msBuildDirectory = MsBuildUtility.GetMsBuildToolset(null, console.Object).Path;
                _nuGetProjectContext = new TestNuGetProjectContext();

                var projectFilePath = Path.Combine(_projectDirectory, projectName + ".csproj");
                File.WriteAllText(projectFilePath, projectFileContent);

                MSBuildProjectSystem
                    = new MSBuildProjectSystem(_msBuildDirectory, projectFilePath, _nuGetProjectContext);
            }

            public void Dispose()
            {
                _projectDirectory.Dispose();
            }
        }

        [Fact]
        public void MSBuildProjectSystem_AddFile()
        {
            // Arrange
            var projectFileContent = Util.CreateProjFileContent();
            using (var testInfo = new TestInfo(_testOutputHelper, projectFileContent))
            {
                var expectedContent = "one two three";
                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(expectedContent)))
                {
                    // Act
                    testInfo.MSBuildProjectSystem.AddFile("a.js", stream);

                    // Assert
                    var path = Path.Combine(testInfo.MSBuildProjectSystem.ProjectFullPath, "a.js");
                    Assert.True(File.Exists(path), path + "should exist, but it does not.");

                    var actualContent = File.ReadAllText(path);
                    Assert.Equal(expectedContent, actualContent);
                }
            }
        }


        [Fact]
        public void MSBuildProjectSystem_RemoveFile()
        {
            // Arrange
            var projectFileContent = Util.CreateProjFileContent(
                "proj1",
                "v4.5",
                references: null,
                contentFiles: new[] { "a.js" });

            using (var testInfo = new TestInfo(_testOutputHelper, projectFileContent))
            {
                var path = Path.Combine(testInfo.MSBuildProjectSystem.ProjectFullPath, "a.js");
                var expectedContent = "one two three";
                File.WriteAllText(path, expectedContent);

                Assert.True(File.Exists(path), path + "should exist, but it does not.");

                // Act
                testInfo.MSBuildProjectSystem.RemoveFile("a.js");

                // Assert
                Assert.False(File.Exists(path), path + "should not exist, but it does.");
            }
        }


        [Fact]
        public void MSBuildProjectSystem_FileExistInProject()
        {
            // Arrange
            var projectFileContent = Util.CreateProjFileContent(
                "proj1",
                "v4.5",
                references: null,
                contentFiles: new[] { "a.js" });

            using (var testInfo = new TestInfo(_testOutputHelper, projectFileContent))
            {
                var path = Path.Combine(testInfo.MSBuildProjectSystem.ProjectFullPath, "a.js");
                var expectedContent = "one two three";
                File.WriteAllText(path, expectedContent);

                Assert.True(File.Exists(path), path + "should exist, but it does not.");

                // Act
                var fileExistsInProject = testInfo.MSBuildProjectSystem.FileExistsInProject("a.js");

                // Assert
                Assert.True(fileExistsInProject, "a.js does not exist in project");
            }
        }

        [Fact]
        public void MSBuildProjectSystem_RemoveImport()
        {
            // Arrange
            var import = @"packages\mypackage.1.0.0\build\mypackage.targets";
            var projectFileContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Project ToolsVersion=""14.0"" DefaultTargets=""Build"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
  <Import Project=""packages\mypackage.1.0.0\build\mypackage.targets"" Condition=""Exists('packages\mypackage.1.0.0\build\mypackage.targets')"" />
  <Target Name=""EnsureNuGetPackageBuildImports"" BeforeTargets=""PrepareForBuild"" >
    <PropertyGroup>   
      <ErrorText>This project references NuGet package(s) that are missing on this computer.Enable NuGet Package Restore to download them.For more information, see http://go.microsoft.com/fwlink/?LinkID=322105.The missing file is {0}.</ErrorText>
    </PropertyGroup>
    <Error Condition=""!Exists('packages\mypackage.1.0.0\build\mypackage.targets')"" Text=""$([System.String]::Format('$(ErrorText)', 'packages\mypackage.1.0.0\build\mypackage.targets'))"" />
  </Target>
</Project>";

            using (var testInfo = new TestInfo(_testOutputHelper, projectFileContent))
            {
                var targetFullPath = Path.Combine(testInfo.MSBuildProjectSystem.ProjectFullPath, import);

                // Act
                testInfo.MSBuildProjectSystem.RemoveImport(targetFullPath);

                // Assert
                var proj = XElement.Load(testInfo.MSBuildProjectSystem.ProjectFileFullPath);
                Assert.False(proj.HasElements, "The <Import /> and <Target Name=\"EnsureNuGetPackageBuildImports\" /> elements should have been removed.");
            }
        }

        // Verify for mono scenarios that / slash paths are also removed.
        [Fact]
        public void MSBuildProjectSystem_RemoveImportForwardSlashes()
        {
            // Arrange
            var import = @"packages\mypackage.1.0.0\build\mypackage.targets";
            var projectFileContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Project ToolsVersion=""14.0"" DefaultTargets=""Build"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
  <Import Project=""packages/mypackage.1.0.0/build/mypackage.targets"" Condition=""Exists('packages/mypackage.1.0.0/build/mypackage.targets')"" />
  <Target Name=""EnsureNuGetPackageBuildImports"" BeforeTargets=""PrepareForBuild"" >
    <PropertyGroup>   
      <ErrorText>This project references NuGet package(s) that are missing on this computer.Enable NuGet Package Restore to download them.For more information, see http://go.microsoft.com/fwlink/?LinkID=322105.The missing file is {0}.</ErrorText>
    </PropertyGroup>
    <Error Condition=""!Exists('packages/mypackage.1.0.0/build/mypackage.targets')"" Text=""$([System.String]::Format('$(ErrorText)', 'packages/mypackage.1.0.0/build/mypackage.targets'))"" />
  </Target>
</Project>";

            using (var testInfo = new TestInfo(_testOutputHelper, projectFileContent))
            {
                var targetFullPath = Path.Combine(testInfo.MSBuildProjectSystem.ProjectFullPath, import);

                // Act
                testInfo.MSBuildProjectSystem.RemoveImport(targetFullPath);

                // Assert
                var proj = XElement.Load(testInfo.MSBuildProjectSystem.ProjectFileFullPath);
                Assert.False(proj.HasElements, "The <Import /> and <Target Name=\"EnsureNuGetPackageBuildImports\" /> elements should have been removed.");
            }
        }
    }
}
