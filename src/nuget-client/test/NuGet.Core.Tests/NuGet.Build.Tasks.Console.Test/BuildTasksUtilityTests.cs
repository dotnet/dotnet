// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using FluentAssertions;
using NuGet.Configuration;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Build.Tasks.Console.Test
{
    public class BuildTasksUtilityTests
    {
        [Fact]
        public void GetPackagesConfigFilePath_WithNonExistentPackagesConfig_ReturnsNull()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                BuildTasksUtility.GetPackagesConfigFilePath(testDirectory, "Test")
                    .Should().BeNull();
            }
        }

        [Fact]
        public void GetPackagesConfigFilePath_WithNullProjectDirectory_ThrowsArgumentException()
        {
            Action action = () => { BuildTasksUtility.GetPackagesConfigFilePath(projectDirectory: null, projectName: "ProjectA"); };

            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void GetPackagesConfigFilePath_WithNullProjectFullPath_ThrowsArgumentException()
        {
            Action action = () => { BuildTasksUtility.GetPackagesConfigFilePath(projectFullPath: null); };

            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void GetPackagesConfigFilePath_WithNullProjectName_ThrowsArgumentException()
        {
            Action action = () => { BuildTasksUtility.GetPackagesConfigFilePath(projectDirectory: "SomePath", projectName: null); };

            action.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData("packages.config")]
        [InlineData("packages.Test.config")]
        public void GetPackagesConfigFilePath_WithProjectDirectoryAndProjectName_ReturnsPackagesConfig(string packagesConfigFilename)
        {
            using (var testDirectory = TestDirectory.Create())
            {
                string projectFullPath = Path.Combine(testDirectory, "Test.csproj");

                string packagesConfigFilePath = Path.Combine(testDirectory, packagesConfigFilename);

                File.WriteAllText(packagesConfigFilePath, string.Empty);

                BuildTasksUtility.GetPackagesConfigFilePath(testDirectory, "Test")
                    .Should().Be(packagesConfigFilePath);
            }
        }

        [Theory]
        [InlineData("packages.config")]
        [InlineData("packages.Test.config")]
        public void GetPackagesConfigFilePath_WithProjectFullPath_ReturnsPackagesConfig(string packagesConfigFilename)
        {
            using (var testDirectory = TestDirectory.Create())
            {
                string projectFullPath = Path.Combine(testDirectory, "Test.csproj");

                string packagesConfigFilePath = Path.Combine(testDirectory, packagesConfigFilename);

                File.WriteAllText(packagesConfigFilePath, string.Empty);

                BuildTasksUtility.GetPackagesConfigFilePath(projectFullPath)
                    .Should().Be(packagesConfigFilePath);
            }
        }

        [Fact]
        public void GetSources_WithRestoreSourcesProperty_ResolvesAgainstProjectDirectory()
        {
            using (var testDir = TestDirectory.CreateInTemp())
            {
                // Arrange
                var startupDirectory = Path.Combine(testDir, "startup");
                var projectDirectory = Path.Combine(testDir, "project");
                var relativePath = "relativeSource";

                // Act
                var effectiveSources = BuildTasksUtility.GetSources(
                     startupDirectory: startupDirectory,
                     projectDirectory: projectDirectory,
                     sources: new string[] { relativePath },
                     sourcesOverride: null,
                     additionalProjectSources: Array.Empty<string>(),
                     settings: NullSettings.Instance
                     );

                // Assert
                effectiveSources.Should().BeEquivalentTo(new[] { Path.Combine(projectDirectory, relativePath) });
            }
        }

        [Fact]
        public void GetSources_WithRestoreSourcesGlobal_Property_ResolvesAgainstWorkingDirectory()
        {
            using (var testDir = TestDirectory.CreateInTemp())
            {
                // Arrange
                var startupDirectory = Path.Combine(testDir, "startup");
                var projectDirectory = Path.Combine(testDir, "project");
                var relativePath = "relativeSource";

                // Act
                var effectiveSources = BuildTasksUtility.GetSources(
                     startupDirectory: startupDirectory,
                     projectDirectory: projectDirectory,
                     sources: new string[] { relativePath },
                     sourcesOverride: new string[] { relativePath },
                     additionalProjectSources: Array.Empty<string>(),
                     settings: NullSettings.Instance
                     );

                // Assert
                effectiveSources.Should().BeEquivalentTo(new[] { Path.Combine(startupDirectory, relativePath) });
            }
        }
    }
}
