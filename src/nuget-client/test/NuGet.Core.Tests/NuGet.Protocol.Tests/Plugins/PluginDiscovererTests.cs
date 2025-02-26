// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
# if !NET8_0_OR_GREATER
using Microsoft.Internal.NuGet.Testing.SignedPackages.ChildProcess;
#endif
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.Common;
using NuGet.Test.Utility;
using Xunit;
using Xunit.Abstractions;

namespace NuGet.Protocol.Plugins.Tests
{
    public class PluginDiscovererTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public PluginDiscovererTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Constructor_AcceptsAnyString(string rawPluginPaths)
        {
            var environmentalVariableReader = new Mock<IEnvironmentVariableReader>();
            environmentalVariableReader.Setup(env => env.GetEnvironmentVariable(EnvironmentVariableConstants.DesktopPluginPaths)).Returns(rawPluginPaths);
            environmentalVariableReader.Setup(env => env.GetEnvironmentVariable(EnvironmentVariableConstants.CorePluginPaths)).Returns(rawPluginPaths);

            Exception exception = Record.Exception(() =>
            {
                using (new PluginDiscoverer())
                {
                }
            });

            Assert.Null(exception);
        }

        [Fact]
        public async Task DiscoverAsync_ThrowsIfCancelled()
        {
            var environmentalVariableReader = new Mock<IEnvironmentVariableReader>();
            environmentalVariableReader.Setup(env => env.GetEnvironmentVariable(EnvironmentVariableConstants.DesktopPluginPaths)).Returns("");
            environmentalVariableReader.Setup(env => env.GetEnvironmentVariable(EnvironmentVariableConstants.CorePluginPaths)).Returns("");
            using (var discoverer = new PluginDiscoverer(environmentalVariableReader.Object))
            {
                await Assert.ThrowsAsync<OperationCanceledException>(
                    () => discoverer.DiscoverAsync(new CancellationToken(canceled: true)));
            }
        }

        [Fact]
        public async Task DiscoverAsync_DoesNotThrowIfNoValidFilePathsAndFallbackEmbeddedSignatureVerifier()
        {
            var environmentalVariableReader = new Mock<IEnvironmentVariableReader>();
            environmentalVariableReader.Setup(env => env.GetEnvironmentVariable(EnvironmentVariableConstants.DesktopPluginPaths)).Returns(Path.PathSeparator.ToString());
            environmentalVariableReader.Setup(env => env.GetEnvironmentVariable(EnvironmentVariableConstants.CorePluginPaths)).Returns(Path.PathSeparator.ToString());
            using (var discoverer = new PluginDiscoverer(environmentalVariableReader.Object))
            {
                var pluginFiles = await discoverer.DiscoverAsync(CancellationToken.None);

                Assert.Empty(pluginFiles);
            }
        }

        [Fact]
        public async Task DiscoverAsync_PerformsDiscoveryOnlyOnce()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                var pluginPath = Path.Combine(testDirectory.Path, "a");

                File.WriteAllText(pluginPath, string.Empty);
                var environmentalVariableReader = new Mock<IEnvironmentVariableReader>();
                environmentalVariableReader.Setup(env => env.GetEnvironmentVariable(EnvironmentVariableConstants.DesktopPluginPaths)).Returns(pluginPath);
                environmentalVariableReader.Setup(env => env.GetEnvironmentVariable(EnvironmentVariableConstants.CorePluginPaths)).Returns(pluginPath);

                using (var discoverer = new PluginDiscoverer(environmentalVariableReader.Object))
                {
                    var results = (await discoverer.DiscoverAsync(CancellationToken.None)).ToArray();

                    foreach (var result in results)
                    {
                        var pluginState = result.PluginFile.State.Value;
                    }

                    Assert.Equal(1, results.Length);

                    results = (await discoverer.DiscoverAsync(CancellationToken.None)).ToArray();

                    Assert.Equal(1, results.Length);
                }
            }
        }

        [Fact]
        public async Task DiscoverAsync_HandlesAllPluginFileStates()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                var pluginPaths = new[] { "a", "b", }
                    .Select(fileName => Path.Combine(testDirectory.Path, fileName))
                    .ToArray();

                File.WriteAllText(pluginPaths[1], string.Empty);

                string rawPluginPaths =
                    $"{pluginPaths[0]}{Path.PathSeparator}{pluginPaths[1]}{Path.PathSeparator}c";
                var environmentalVariableReader = new Mock<IEnvironmentVariableReader>();
                environmentalVariableReader.Setup(env => env.GetEnvironmentVariable(EnvironmentVariableConstants.DesktopPluginPaths)).Returns(rawPluginPaths);
                environmentalVariableReader.Setup(env => env.GetEnvironmentVariable(EnvironmentVariableConstants.CorePluginPaths)).Returns(rawPluginPaths);
                using (var discoverer = new PluginDiscoverer(environmentalVariableReader.Object))
                {
                    var results = (await discoverer.DiscoverAsync(CancellationToken.None)).ToArray();

                    Assert.Equal(3, results.Length);

                    Assert.Equal(pluginPaths[0], results[0].PluginFile.Path);
                    Assert.Equal(PluginFileState.NotFound, results[0].PluginFile.State.Value);
                    Assert.Equal($"A plugin was not found at path '{pluginPaths[0]}'.", results[0].Message);

                    Assert.Equal(pluginPaths[1], results[1].PluginFile.Path);
                    Assert.Equal(PluginFileState.Valid, results[1].PluginFile.State.Value);
                    Assert.Null(results[1].Message);

                    Assert.Equal("c", results[2].PluginFile.Path);
                    Assert.Equal(PluginFileState.InvalidFilePath, results[2].PluginFile.State.Value);
                    Assert.Equal($"The plugin file path 'c' is invalid.", results[2].Message);
                }
            }
        }

        [Theory]
        [InlineData("a")]
        [InlineData(@"\a")]
        [InlineData(@".\a")]
        [InlineData(@"..\a")]
        public async Task DiscoverAsync_DisallowsNonRootedFilePaths(string pluginPath)
        {
            var responses = new Dictionary<string, bool>() { { pluginPath, true } };
            var environmentalVariableReader = new Mock<IEnvironmentVariableReader>();
            environmentalVariableReader.Setup(env => env.GetEnvironmentVariable(EnvironmentVariableConstants.DesktopPluginPaths)).Returns(pluginPath);
            environmentalVariableReader.Setup(env => env.GetEnvironmentVariable(EnvironmentVariableConstants.CorePluginPaths)).Returns(pluginPath);
            using (var discoverer = new PluginDiscoverer(environmentalVariableReader.Object))
            {
                var results = (await discoverer.DiscoverAsync(CancellationToken.None)).ToArray();

                Assert.Equal(1, results.Length);
                Assert.Equal(pluginPath, results[0].PluginFile.Path);
                Assert.Equal(PluginFileState.InvalidFilePath, results[0].PluginFile.State.Value);
            }
        }

        [Fact]
        public async Task DiscoverAsync_IsIdempotent()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                var pluginPath = Path.Combine(testDirectory.Path, "a");

                File.WriteAllText(pluginPath, string.Empty);
                var environmentalVariableReader = new Mock<IEnvironmentVariableReader>();
                environmentalVariableReader.Setup(env => env.GetEnvironmentVariable(EnvironmentVariableConstants.DesktopPluginPaths)).Returns(pluginPath);
                environmentalVariableReader.Setup(env => env.GetEnvironmentVariable(EnvironmentVariableConstants.CorePluginPaths)).Returns(pluginPath);

                using (var discoverer = new PluginDiscoverer(environmentalVariableReader.Object))
                {
                    var firstResult = await discoverer.DiscoverAsync(CancellationToken.None);
                    var firstState = firstResult.SingleOrDefault().PluginFile.State.Value;

                    var secondResult = await discoverer.DiscoverAsync(CancellationToken.None);
                    var secondState = secondResult.SingleOrDefault().PluginFile.State.Value;

                    Assert.Same(firstResult, secondResult);
                }
            }
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("nuget-plugin-myPlugin.exe")]
        [InlineData("nuget-plugin-myPlugin.bat")]
        public async Task DiscoverAsync_withValidDotNetToolsPluginWindows_FindsThePlugin(string fileName)
        {
            using (var testDirectory = TestDirectory.Create())
            {
                // Arrange
                var pluginPath = Path.Combine(testDirectory.Path, "myPlugin");
                Directory.CreateDirectory(pluginPath);
                var myPlugin = Path.Combine(pluginPath, fileName);
                Mock<IEnvironmentVariableReader> environmentalVariableReader = new Mock<IEnvironmentVariableReader>();
                environmentalVariableReader.Setup(env => env.GetEnvironmentVariable(EnvironmentVariableConstants.PluginPaths)).Returns(pluginPath);

                File.WriteAllText(myPlugin, string.Empty);

                using (var discoverer = new PluginDiscoverer(environmentalVariableReader.Object))
                {
                    // Act
                    var result = await discoverer.DiscoverAsync(CancellationToken.None);

                    // Assert
                    Assert.Contains(result, discoveryResult => myPlugin == discoveryResult.PluginFile.Path);
                }
            }
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("nuget-plugin-myPlugin.exe")]
        [InlineData("nuget-plugin-myPlugin.bat")]
        public async Task DiscoverAsync_WithPluginPathSpecifiedInNuGetPluginPathsEnvVariableWindows_FindsThePlugin(string fileName)
        {
            using (var testDirectory = TestDirectory.Create())
            {
                // Arrange
                var pluginPath = Path.Combine(testDirectory.Path, "myPlugin");
                Directory.CreateDirectory(pluginPath);
                var myPlugin = Path.Combine(pluginPath, fileName);
                Mock<IEnvironmentVariableReader> environmentalVariableReader = new Mock<IEnvironmentVariableReader>();
                environmentalVariableReader.Setup(env => env.GetEnvironmentVariable(EnvironmentVariableConstants.PluginPaths)).Returns($"{Path.PathSeparator}{pluginPath}{Path.PathSeparator}");
                environmentalVariableReader.Setup(env => env.GetEnvironmentVariable("PATHS")).Returns("");
                File.WriteAllText(myPlugin, string.Empty);

                using (var discoverer = new PluginDiscoverer(environmentalVariableReader.Object))
                {
                    // Act
                    var result = await discoverer.DiscoverAsync(CancellationToken.None);

                    // Assert
                    Assert.Contains(result, discoveryResult => myPlugin == discoveryResult.PluginFile.Path);
                }
            }
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("nugetplugin-myPlugin.exe")]
        [InlineData("nugetplugin-myPlugin.bat")]
        public async Task DiscoverAsync_withInValidDotNetToolsPluginNameWindows_DoesNotFindThePlugin(string fileName)
        {
            using (var testDirectory = TestDirectory.Create())
            {
                // Arrange
                var pluginPath = Path.Combine(testDirectory.Path, "myPlugin");
                Directory.CreateDirectory(pluginPath);
                var myPlugin = Path.Combine(pluginPath, fileName);
                Mock<IEnvironmentVariableReader> environmentalVariableReader = new Mock<IEnvironmentVariableReader>();
                environmentalVariableReader.Setup(env => env.GetEnvironmentVariable(It.IsAny<string>())).Returns(pluginPath);

                File.WriteAllText(myPlugin, string.Empty);

                using (var discoverer = new PluginDiscoverer(environmentalVariableReader.Object))
                {
                    // Act
                    var result = await discoverer.DiscoverAsync(CancellationToken.None);

                    // Assert
                    Assert.DoesNotContain(result, discoveryResult => myPlugin == discoveryResult.PluginFile.Path);
                }
            }
        }

        [PlatformFact(Platform.Linux)]
        public async Task DiscoverAsync_withValidDotNetToolsPluginLinux_FindsThePlugin()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                // Arrange
                var pluginPath = Path.Combine(testDirectory.Path, "myPlugins");
                Directory.CreateDirectory(pluginPath);
                var myPlugin = Path.Combine(pluginPath, "nuget-plugin-MyPlugin");
                File.WriteAllText(myPlugin, string.Empty);
                Mock<IEnvironmentVariableReader> environmentalVariableReader = new Mock<IEnvironmentVariableReader>();
                environmentalVariableReader.Setup(env => env.GetEnvironmentVariable(EnvironmentVariableConstants.PluginPaths)).Returns(pluginPath);
                SetFileExecutable(myPlugin, true);
                using (var discoverer = new PluginDiscoverer(environmentalVariableReader.Object))
                {
                    // Act
                    var result = await discoverer.DiscoverAsync(CancellationToken.None);

                    // Assert
                    Assert.Contains(result, discoveryResult => myPlugin == discoveryResult.PluginFile.Path);
                }
            }
        }

        [PlatformFact(Platform.Linux)]
        public async Task DiscoverAsync_WithPluginPathSpecifiedInNuGetPluginPathsEnvVariableLinux_FindsThePlugin()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                // Arrange
                var pluginPath = Path.Combine(testDirectory.Path, "myPlugins");
                Directory.CreateDirectory(pluginPath);
                var myPlugin = Path.Combine(pluginPath, "nuget-plugin-MyPlugin");
                File.WriteAllText(myPlugin, string.Empty);
                Mock<IEnvironmentVariableReader> environmentalVariableReader = new Mock<IEnvironmentVariableReader>();
                environmentalVariableReader.Setup(env => env.GetEnvironmentVariable(EnvironmentVariableConstants.PluginPaths)).Returns(pluginPath);
                environmentalVariableReader.Setup(env => env.GetEnvironmentVariable("PATHS")).Returns("");

                SetFileExecutable(myPlugin, true);

                using (var discoverer = new PluginDiscoverer(environmentalVariableReader.Object))
                {
                    // Act
                    var result = await discoverer.DiscoverAsync(CancellationToken.None);

                    // Assert
                    Assert.Contains(result, discoveryResult => myPlugin == discoveryResult.PluginFile.Path);
                }
            }
        }

        [PlatformFact(Platform.Linux)]
        public async Task DiscoverAsync_withNoExecutableValidDotNetToolsPluginLinux_DoesNotFindThePlugin()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                // Arrange
                var pluginPath = Path.Combine(testDirectory.Path, "myPlugins");
                Directory.CreateDirectory(pluginPath);
                var myPlugin = Path.Combine(pluginPath, "nuget-plugin-MyPlugin");
                File.WriteAllText(myPlugin, string.Empty);
                Mock<IEnvironmentVariableReader> environmentalVariableReader = new Mock<IEnvironmentVariableReader>();
                environmentalVariableReader.Setup(env => env.GetEnvironmentVariable(EnvironmentVariableConstants.PluginPaths)).Returns(pluginPath);

                SetFileExecutable(myPlugin, false);

                using (var discoverer = new PluginDiscoverer(environmentalVariableReader.Object))
                {
                    // Act
                    var result = await discoverer.DiscoverAsync(CancellationToken.None);

                    // Assert
                    Assert.DoesNotContain(result, discoveryResult => myPlugin == discoveryResult.PluginFile.Path);
                }
            }
        }

        [PlatformFact(Platform.Windows)]
        public void GetPluginsInNuGetPluginPaths_WithNuGetPluginPathsSet_ReturnsPluginsInNuGetPluginPathOnly()
        {
            // Arrange
            using TestDirectory pluginPathDirectory = TestDirectory.Create();
            using TestDirectory pathDirectory = TestDirectory.Create();
            var pluginInNuGetPluginPathDirectoryFilePath = Path.Combine(pluginPathDirectory.Path, "nuget-plugin-auth.exe");
            var pluginInPathDirectoryFilePath = Path.Combine(pathDirectory.Path, "nuget-plugin-in-path-directory.exe");
            File.Create(pluginInNuGetPluginPathDirectoryFilePath);
            File.Create(pluginInPathDirectoryFilePath);
            Mock<IEnvironmentVariableReader> environmentalVariableReader = new Mock<IEnvironmentVariableReader>();
            environmentalVariableReader.Setup(env => env.GetEnvironmentVariable(EnvironmentVariableConstants.PluginPaths)).Returns(Directory.GetParent(pluginInNuGetPluginPathDirectoryFilePath).FullName);
            environmentalVariableReader.Setup(env => env.GetEnvironmentVariable("PATH")).Returns(Directory.GetParent(pluginInPathDirectoryFilePath).FullName);
            PluginDiscoverer pluginDiscoverer = new PluginDiscoverer(environmentalVariableReader.Object);

            // Act
            var plugins = pluginDiscoverer.GetPluginsInNuGetPluginPaths();

            // Assert
            Assert.Single(plugins);
            Assert.Equal(pluginInNuGetPluginPathDirectoryFilePath, plugins[0].Path);
            Assert.False(plugins[0].RequiresDotnetHost);
        }

        [PlatformFact(Platform.Windows)]
        public void GetPluginsInNuGetPluginPaths_WithoutNuGetPluginPaths_ReturnsEmpty()
        {
            // Arrange
            using var pathDirectory = TestDirectory.Create();
            var pluginFilePath = Path.Combine(pathDirectory.Path, "nuget-plugin-fallback.exe");
            File.Create(pluginFilePath);

            var environmentalVariableReader = new Mock<IEnvironmentVariableReader>();
            environmentalVariableReader.Setup(env => env.GetEnvironmentVariable("PATH")).Returns(pathDirectory.Path);

            var pluginDiscoverer = new PluginDiscoverer(environmentalVariableReader.Object);

            // Act
            var plugins = pluginDiscoverer.GetPluginsInNuGetPluginPaths();

            // Assert
            Assert.Empty(plugins);
        }

        [PlatformFact(Platform.Windows)]
        public void GetPluginsInPATH_WithPATHSet_ReturnsPlugin()
        {
            // Arrange
            using TestDirectory pluginPathDirectory = TestDirectory.Create();
            using TestDirectory pathDirectory = TestDirectory.Create();
            var pluginInNuGetPluginPathDirectoryFilePath = Path.Combine(pluginPathDirectory.Path, "nuget-plugin-auth.exe");
            var pluginInPathDirectoryFilePath = Path.Combine(pathDirectory.Path, "nuget-plugin-in-path-directory.exe");
            File.Create(pluginInNuGetPluginPathDirectoryFilePath);
            File.Create(pluginInPathDirectoryFilePath);
            Mock<IEnvironmentVariableReader> environmentalVariableReader = new Mock<IEnvironmentVariableReader>();
            environmentalVariableReader.Setup(env => env.GetEnvironmentVariable(EnvironmentVariableConstants.PluginPaths)).Returns(Directory.GetParent(pluginInNuGetPluginPathDirectoryFilePath).FullName);
            environmentalVariableReader.Setup(env => env.GetEnvironmentVariable("PATH")).Returns(Directory.GetParent(pluginInPathDirectoryFilePath).FullName);
            PluginDiscoverer pluginDiscoverer = new PluginDiscoverer(environmentalVariableReader.Object);

            // Act
            var plugins = pluginDiscoverer.GetPluginsInPath();

            // Assert
            Assert.Single(plugins);
            Assert.Equal(pluginInPathDirectoryFilePath, plugins[0].Path);
            Assert.False(plugins[0].RequiresDotnetHost);
        }

        [PlatformFact(Platform.Windows)]
        public void GetPluginsInPATH_WithOneValueInPathEmpty_ReturnsPlugin()
        {
            // Arrange
            using TestDirectory pluginPathDirectory = TestDirectory.Create();
            using TestDirectory pathDirectory = TestDirectory.Create();
            var pluginInNuGetPluginPathDirectoryFilePath = Path.Combine(pluginPathDirectory.Path, "nuget-plugin-auth.exe");
            var pluginInPathDirectoryFilePath = Path.Combine(pathDirectory.Path, "nuget-plugin-in-path-directory.exe");
            File.Create(pluginInNuGetPluginPathDirectoryFilePath);
            File.Create(pluginInPathDirectoryFilePath);
            Mock<IEnvironmentVariableReader> environmentalVariableReader = new Mock<IEnvironmentVariableReader>();
            environmentalVariableReader.Setup(env => env.GetEnvironmentVariable(EnvironmentVariableConstants.PluginPaths)).Returns(pluginPathDirectory.Path);
            environmentalVariableReader.Setup(env => env.GetEnvironmentVariable("PATH")).Returns($"{pathDirectory.Path}{Path.PathSeparator}");
            PluginDiscoverer pluginDiscoverer = new PluginDiscoverer(environmentalVariableReader.Object);

            // Act
            var plugins = pluginDiscoverer.GetPluginsInPath();

            // Assert
            Assert.Single(plugins);
            Assert.Equal(pluginInPathDirectoryFilePath, plugins[0].Path);
            Assert.False(plugins[0].RequiresDotnetHost);
        }

        [PlatformFact(Platform.Windows)]
        public void GetPluginsInNuGetPluginPaths_NuGetPluginPathsPointsToAFile_TreatsAsPlugin()
        {
            // Arrange
            using TestDirectory testDirectory = TestDirectory.Create();
            var pluginFilePath = Path.Combine(testDirectory.Path, "nuget-plugin-auth.exe");
            File.Create(pluginFilePath);

            var environmentalVariableReader = new Mock<IEnvironmentVariableReader>();
            environmentalVariableReader.Setup(env => env.GetEnvironmentVariable(EnvironmentVariableConstants.PluginPaths)).Returns(pluginFilePath);
            environmentalVariableReader.Setup(env => env.GetEnvironmentVariable("PATH")).Returns(string.Empty);

            var pluginDiscoverer = new PluginDiscoverer(environmentalVariableReader.Object);

            // Act
            var plugins = pluginDiscoverer.GetPluginsInNuGetPluginPaths();

            // Assert
            Assert.Single(plugins);
            Assert.Equal(pluginFilePath, plugins[0].Path);
            Assert.False(plugins[0].RequiresDotnetHost);
        }

        [PlatformFact(Platform.Windows)]
        public void GetPluginsInNuGetPluginPaths_NuGetPluginPathsPointsToAFileThatDoesNotStartWithNugetPlugin_ReturnsNonDotnetPlugin()
        {
            // Arrange
            using TestDirectory testDirectory = TestDirectory.Create();
            var pluginFilePath = Path.Combine(testDirectory.Path, "other-plugin.exe");
            File.Create(pluginFilePath);

            var environmentalVariableReader = new Mock<IEnvironmentVariableReader>();
            environmentalVariableReader.Setup(env => env.GetEnvironmentVariable(EnvironmentVariableConstants.PluginPaths)).Returns(pluginFilePath);
            environmentalVariableReader.Setup(env => env.GetEnvironmentVariable("PATH")).Returns(string.Empty);

            var pluginDiscoverer = new PluginDiscoverer(environmentalVariableReader.Object);

            // Act
            var plugins = pluginDiscoverer.GetPluginsInNuGetPluginPaths();

            // Assert
            Assert.Single(plugins);
        }

        [PlatformFact(Platform.Windows)]
        public void GetPluginsInPATH_PATHPointsToADirectory_ContainsValidPluginFiles()
        {
            // Arrange
            using var pluginPathDirectory = TestDirectory.Create();
            var validPluginFile = Path.Combine(pluginPathDirectory.Path, "nuget-plugin-auth.exe");
            var invalidPluginFile = Path.Combine(pluginPathDirectory.Path, "not-a-nuget-plugin.exe");
            File.Create(validPluginFile);
            File.Create(invalidPluginFile);

            var environmentalVariableReader = new Mock<IEnvironmentVariableReader>();
            environmentalVariableReader.Setup(env => env.GetEnvironmentVariable("PATH")).Returns(pluginPathDirectory.Path);

            var pluginDiscoverer = new PluginDiscoverer(environmentalVariableReader.Object);

            // Act
            var plugins = pluginDiscoverer.GetPluginsInPath();

            // Assert
            Assert.Single(plugins);
            Assert.Equal(validPluginFile, plugins[0].Path);
            Assert.False(plugins[0].RequiresDotnetHost);
        }

        [PlatformFact(Platform.Windows)]
        public void GetPluginsInNuGetPluginPaths_NoEnvironmentVariables_ReturnsNoPlugins()
        {
            // Arrange
            var environmentalVariableReader = new Mock<IEnvironmentVariableReader>();
            var pluginDiscoverer = new PluginDiscoverer(environmentalVariableReader.Object);

            // Act
            var plugins = pluginDiscoverer.GetPluginsInNuGetPluginPaths();

            // Assert
            Assert.Empty(plugins);
        }

        [PlatformFact(Platform.Windows)]
        public void IsValidPluginFile_ExeFile_ReturnsTrue()
        {
            // Arrange
            using TestDirectory testDirectory = TestDirectory.Create();
            var workingPath = testDirectory.Path;
            var pluginFilePath = Path.Combine(workingPath, "plugin.exe");
            File.Create(pluginFilePath);
            var fileInfo = new FileInfo(pluginFilePath);

            // Act
            bool result = PluginDiscoverer.IsValidPluginFile(fileInfo);

            // Assert
            Assert.True(result);
        }

        [PlatformFact(Platform.Windows)]
        public void IsValidPluginFile_Windows_NonExecutableFile_ReturnsFalse()
        {
            // Arrange
            using TestDirectory testDirectory = TestDirectory.Create();
            var workingPath = testDirectory.Path;
            var nonPluginFilePath = Path.Combine(workingPath, "plugin.txt");
            File.Create(nonPluginFilePath);
            var fileInfo = new FileInfo(nonPluginFilePath);

            // Act
            bool result = PluginDiscoverer.IsValidPluginFile(fileInfo);

            // Assert
            Assert.False(result);
        }

        [PlatformFact(Platform.Linux)]
        public void IsValidPluginFile_Unix_ExecutableFile_ReturnsTrue()
        {
            // Arrange
            using TestDirectory testDirectory = TestDirectory.Create();
            var workingPath = testDirectory.Path;
            var pluginFilePath = Path.Combine(workingPath, "plugin");
            File.Create(pluginFilePath).Dispose();

#if NET8_0_OR_GREATER
            // Set execute permissions
            File.SetUnixFileMode(pluginFilePath, UnixFileMode.UserExecute | UnixFileMode.UserRead);
#else
            SetFileExecutable(pluginFilePath, true);
#endif

            var fileInfo = new FileInfo(pluginFilePath);

            // Act
            bool result = PluginDiscoverer.IsValidPluginFile(fileInfo);

            // Assert
            Assert.True(result);
        }

#if !NET8_0_OR_GREATER
        [PlatformFact(Platform.Linux)]
        public void IsExecutable_FileIsExecutable_ReturnsTrue()
        {
            // Arrange
            using TestDirectory testDirectory = TestDirectory.Create();
            var workingPath = testDirectory.Path;
            var pluginFilePath = Path.Combine(workingPath, "plugin");
            File.Create(pluginFilePath);

            // Set execute permissions
            SetFileExecutable(pluginFilePath, true);

            var fileInfo = new FileInfo(pluginFilePath);

            // Act
            bool result = PluginDiscoverer.IsExecutable(fileInfo);

            // Assert
            Assert.True(result);
        }

        [PlatformFact(Platform.Linux)]
        public void IsExecutable_FileIsNotExecutable_ReturnsFalse()
        {
            // Arrange
            using TestDirectory testDirectory = TestDirectory.Create();
            var workingPath = testDirectory.Path;
            var pluginFilePath = Path.Combine(workingPath, "plugin");
            File.Create(pluginFilePath);

            // Remove execute permissions
            SetFileExecutable(pluginFilePath, false);

            var fileInfo = new FileInfo(pluginFilePath);

            // Act
            bool result = PluginDiscoverer.IsExecutable(fileInfo);

            // Assert
            Assert.False(result);
        }

        [PlatformFact(Platform.Linux)]
        public void IsExecutable_FileWithSpace_ReturnsTrue()
        {
            // Arrange
            using TestDirectory testDirectory = TestDirectory.Create();
            var workingPath = testDirectory.Path;
            var pluginFilePath = Path.Combine(workingPath, "plugin with space");
            File.Create(pluginFilePath).Dispose();

            // Set execute permissions
            SetFileExecutable(pluginFilePath, true);

            var fileInfo = new FileInfo(pluginFilePath);

            // Act
            bool result = PluginDiscoverer.IsExecutable(fileInfo);

            // Assert
            Assert.True(result);
        }

#endif

        private void SetFileExecutable(string filePath, bool executable)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File not found: {filePath}");
            }
#if NET8_0_OR_GREATER
            try
            {
                File.SetUnixFileMode(filePath, executable ?
                    UnixFileMode.UserExecute | UnixFileMode.UserRead | UnixFileMode.UserWrite :
                    UnixFileMode.UserRead | UnixFileMode.UserWrite);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error while setting file mode for: {filePath}", ex);
            }
#else
            try
            {
                CommandRunnerResult result = CommandRunner.Run(
                    filename: "/bin/bash",
                    arguments: $"-c \"chmod {(executable ? "+x" : "-x")} '{filePath}'\"",
                    testOutputHelper: _testOutputHelper);

                if (!result.Success)
                {
                    throw new InvalidOperationException($"Failed to set execute permissions for {filePath}. Error: {result.Errors}");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error setting file executable permission for {filePath}", ex);
            }
#endif
        }
    }
}
