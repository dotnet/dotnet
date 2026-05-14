// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Test.Utility;
using Test.Utility;
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

        [Theory]
        [InlineData("0", "false", 0)]
        [InlineData("0", "true", 0)]
        [InlineData("false", "false", 0)]
        [InlineData("false", "true", 0)]
        [InlineData(" 2 ", "false", 0)]
        [InlineData(" 2 ", "true", 2)]
        [InlineData("true", "false", 0)]
        [InlineData("true", "true", 1)]
        [InlineData(" 1 ", "false", 0)]
        [InlineData(" 1 ", "true", 1)]
        [InlineData("", "false", 0)]
        [InlineData("", "true", 1)]
        [InlineData(null, "false", 0)]
        [InlineData(null, "true", 1)]
        public void GetFilesToEmbedInBinlogValue_WithValue_ReturnsExpectedValue(string value, string binaryLoggerEnabled, int expected)
        {
            IEnvironmentVariableReader environmentVariableReader = new TestEnvironmentVariableReader(new Dictionary<string, string>
            {
                ["MSBUILDBINARYLOGGERENABLED"] = binaryLoggerEnabled
            });

            BuildTasksUtility.GetFilesToEmbedInBinlogValue(value, environmentVariableReader).Should().Be(expected);
        }
    }
}
