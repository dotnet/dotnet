// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NuGet.CommandLine.XPlat;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.CommandLine.Xplat.Tests.Utility
{
    public class XPlatUtilityTests
    {
        [Theory]
        [InlineData("", LogLevel.Minimal)]
        [InlineData(null, LogLevel.Minimal)]
        [InlineData("  ", LogLevel.Minimal)]
        [InlineData("qu", LogLevel.Minimal)]
        [InlineData("quiet ", LogLevel.Minimal)]
        [InlineData(" q", LogLevel.Minimal)]
        [InlineData("m", LogLevel.Minimal)]
        [InlineData("M", LogLevel.Minimal)]
        [InlineData("mInImAl", LogLevel.Minimal)]
        [InlineData("MINIMAL", LogLevel.Minimal)]
        [InlineData("something-else-entirely", LogLevel.Minimal)]
        [InlineData("q", LogLevel.Warning)]
        [InlineData("quiet", LogLevel.Warning)]
        [InlineData("Q", LogLevel.Warning)]
        [InlineData("QUIET", LogLevel.Warning)]
        [InlineData("n", LogLevel.Information)]
        [InlineData("normal", LogLevel.Information)]
        [InlineData("d", LogLevel.Debug)]
        [InlineData("detailed", LogLevel.Debug)]
        [InlineData("diag", LogLevel.Debug)]
        [InlineData("diagnostic", LogLevel.Debug)]
        public void MSBuildVerbosityToNuGetLogLevel_HasProperMapping(string verbosity, LogLevel expected)
        {
            LogLevel actual = XPlatUtility.MSBuildVerbosityToNuGetLogLevel(verbosity);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ProcessConfigFile_GetSettingsForWorkingDirectory(string emptyConfig)
        {
            ISettings settings = XPlatUtility.ProcessConfigFile(emptyConfig);
            var baseDirectory = NuGetEnvironment.GetFolderPath(NuGetFolderPath.UserSettingsDirectory);
            string baseNugetConfigPath = Path.Combine(baseDirectory, Settings.DefaultSettingsFileName);
            List<string> configPaths = settings.GetConfigFilePaths().ToList();
            // Since this command doesn't set specific working directory itself, it's just test binary folder,
            // so several nuget.config including user default nuget.config'll get loaded.
            Assert.True(configPaths.Count > 1);
            // Assert user default nuget.config is loaded
            Assert.Contains(baseNugetConfigPath, configPaths);
        }

        [Fact]
        public void ProcessConfigFile_PassConfigFile_OnlyPassedConfigLoaded()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                string currentFolderNugetConfigPath = Path.Combine(pathContext.WorkingDirectory, Settings.DefaultSettingsFileName);
                var tempFolder = Path.Combine(pathContext.WorkingDirectory, "Temp");
                string tempFolderNuGetConfigPath = Path.Combine(tempFolder, Settings.DefaultSettingsFileName);
                Directory.CreateDirectory(tempFolder);
                File.Copy(currentFolderNugetConfigPath, tempFolderNuGetConfigPath);
                ISettings settings = XPlatUtility.ProcessConfigFile(tempFolderNuGetConfigPath);
                List<string> configPaths = settings.GetConfigFilePaths().ToList();
                // If optional nuget.config passed then only that 1 file get loaded.
                Assert.Equal(1, configPaths.Count);
                Assert.Contains(tempFolderNuGetConfigPath, configPaths);
            }
        }

        [Theory]
        [InlineData(new string[] { "X.sln" }, "X.sln")]
        [InlineData(new string[] { "A.csproj" }, "A.csproj")]
        [InlineData(new string[] { "X.sln", "random.txt" }, "X.sln")]
        [InlineData(new string[] { "A.csproj", "random.txt" }, "A.csproj")]
        public void GetProjectOrSolutionFileFromDirectory_WithDirectoryWithSingleSolutionOrProject_ReturnsCorrectFile(string[] directoryFiles, string expectedFile)
        {
            // Arrange
            var pathContext = new SimpleTestPathContext();
            var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

            foreach (var filename in directoryFiles)
            {
                var filePath = Path.Combine(pathContext.SolutionRoot, filename);
                File.Create(filePath);
            }

            var expectedProjectOrSolutionFile = Path.Combine(pathContext.SolutionRoot, expectedFile);

            // Act
            var projectOrSolutionFile = XPlatUtility.GetProjectOrSolutionFileFromDirectory(pathContext.SolutionRoot);

            // Assert
            Assert.Equal(expectedProjectOrSolutionFile, projectOrSolutionFile);
        }

        [Theory]
        [InlineData("X.sln", "Y.sln")]
        [InlineData("A.csproj", "B.csproj")]
        [InlineData("X.sln", "A.csproj")]
        [InlineData()]
        [InlineData("random.txt")]
        public void GetProjectOrSolutionFileFromDirectory_WithDirectoryWithInvalidNumberOfSolutionsOrProjects_ThrowsException(params string[] directoryFiles)
        {
            // Arrange
            var pathContext = new SimpleTestPathContext();
            var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

            foreach (var filename in directoryFiles)
            {
                var filePath = Path.Combine(pathContext.SolutionRoot, filename);
                File.Create(filePath);
            }

            // Act & Assert
            Assert.Throws<ArgumentException>(() => XPlatUtility.GetProjectOrSolutionFileFromDirectory(pathContext.SolutionRoot));
        }
    }
}
