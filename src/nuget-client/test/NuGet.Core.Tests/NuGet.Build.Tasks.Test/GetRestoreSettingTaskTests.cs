// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Microsoft.Build.Framework;
using Moq;
using NuGet.Configuration;
using NuGet.Configuration.Test;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Build.Tasks.Test
{
    public class GetRestoreSettingsTaskTests
    {
        class TestMachineWideSettings : IMachineWideSettings
        {
            public ISettings Settings { get; }

            public TestMachineWideSettings(Settings settings)
            {
                Settings = settings;
            }
        }

        [Fact]
        public void GetRestoreSettingsTask_GetValueGetFirstValue()
        {
            RestoreSettingsUtils.GetValue(
                () => "a",
                () => "b",
                () => null).Should().Be("a");
        }

        [Fact]
        public void GetRestoreSettingsTask_GetValueGetLastValue()
        {
            RestoreSettingsUtils.GetValue(
                () => null,
                () => null,
                () => Array.Empty<string>()).Should().BeEquivalentTo(Array.Empty<string>());
        }

        [Fact]
        public void GetRestoreSettingsTask_GetValueAllNull()
        {
            RestoreSettingsUtils.GetValue<string[]>(
                () => null,
                () => null).Should().BeNull();
        }

        [Fact]
        public void TestSolutionSettings()
        {
            // Arrange
            var subFolderConfig = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <configuration>
                <fallbackPackageFolders>
                    <add key=""a"" value=""C:\Temp\a"" />
                </fallbackPackageFolders>
            </configuration>";

            var baseConfig = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <configuration>
                <fallbackPackageFolders>
                    <add key=""b"" value=""C:\Temp\b"" />
                </fallbackPackageFolders>
                <packageSources>
                    <add key=""c"" value=""C:\Temp\c"" />
                </packageSources>
            </configuration>";



            var baseConfigPath = "NuGet.Config";

            using (var machineWide = TestDirectory.CreateInTemp())
            using (var mockBaseDirectory = TestDirectory.CreateInTemp())
            {
                var subFolder = Path.Combine(mockBaseDirectory, "sub");
                var solutionDirectoryConfig = Path.Combine(mockBaseDirectory, NuGetConstants.NuGetSolutionSettingsFolder);

                SettingsTestUtils.CreateConfigurationFile(baseConfigPath, solutionDirectoryConfig, baseConfig);
                SettingsTestUtils.CreateConfigurationFile(baseConfigPath, subFolder, subFolderConfig);
                SettingsTestUtils.CreateConfigurationFile(baseConfigPath, machineWide, MachineWideSettingsConfig);
                var machineWideSettings = new Lazy<IMachineWideSettings>(() => new TestMachineWideSettings(new Settings(machineWide, baseConfigPath, isMachineWide: true)));

                // Test

                var settings = RestoreSettingsUtils.ReadSettings(mockBaseDirectory, mockBaseDirectory, null, machineWideSettings);
                var filePaths = settings.GetConfigFilePaths();

                Assert.Equal(3, filePaths.Count()); // Solution, app data + machine wide
                Assert.True(filePaths.Contains(Path.Combine(solutionDirectoryConfig, baseConfigPath)));
                Assert.True(filePaths.Contains(Path.Combine(machineWide, baseConfigPath)));

                // Test 
                settings = RestoreSettingsUtils.ReadSettings(mockBaseDirectory, mockBaseDirectory, Path.Combine(subFolder, baseConfigPath), machineWideSettings);
                filePaths = settings.GetConfigFilePaths();

                Assert.Equal(1, filePaths.Count());
                Assert.True(filePaths.Contains(Path.Combine(subFolder, baseConfigPath)));
            }
        }

        [Fact]
        public void GetRestoreSettingsTask_FindConfigInProjectFolder()
        {
            // Verifies that we include any config file found in the project folder
            using (var machineWide = TestDirectory.CreateInTemp())
            using (var workingDir = TestDirectory.CreateInTemp())
            {
                // Arrange
                SettingsTestUtils.CreateConfigurationFile(Settings.DefaultSettingsFileName, machineWide, MachineWideSettingsConfig);
                var machineWideSettings = new Lazy<IMachineWideSettings>(() => new TestMachineWideSettings(new Settings(machineWide, Settings.DefaultSettingsFileName, isMachineWide: true)));

                var innerConfigFile = Path.Combine(workingDir, "sub", Settings.DefaultSettingsFileName);
                var outerConfigFile = Path.Combine(workingDir, Settings.DefaultSettingsFileName);

                var projectDirectory = Path.GetDirectoryName(innerConfigFile);
                Directory.CreateDirectory(projectDirectory);

                File.WriteAllText(innerConfigFile, InnerConfig);
                File.WriteAllText(outerConfigFile, OuterConfig);

                var settings = RestoreSettingsUtils.ReadSettings(null, projectDirectory, null, machineWideSettings);

                var innerValue = SettingsUtility.GetValueForAddItem(settings, "SectionName", "inner-key");
                var outerValue = SettingsUtility.GetValueForAddItem(settings, "SectionName", "outer-key");

                // Assert
                Assert.Equal("inner-value", innerValue);
                Assert.Equal("outer-value", outerValue);
                Assert.True(settings.GetConfigFilePaths().Contains(innerConfigFile));
                Assert.True(settings.GetConfigFilePaths().Contains(outerConfigFile));
            }
        }

        [Fact]
        public void GetRestoreSettingsTask_VerifyRestoreAdditionalProjectSourcesAreAppended()
        {
            using (var testDir = TestDirectory.CreateInTemp())
            {
                // Arrange
                var buildEngine = new TestBuildEngine();
                var testLogger = buildEngine.TestLogger;

                var settingsPerFramework = new List<ITaskItem>();
                var settings1 = new Mock<ITaskItem>();
                settings1.SetupGet(e => e.ItemSpec).Returns("a");
                settings1.Setup(e => e.GetMetadata("RestoreAdditionalProjectSources")).Returns(Path.Combine(testDir, "sourceC"));
                settingsPerFramework.Add(settings1.Object);

                var task = new GetRestoreSettingsTask()
                {
                    BuildEngine = buildEngine,
                    ProjectUniqueName = Path.Combine(testDir, "a.csproj"),
                    RestoreSources = new[] { Path.Combine(testDir, "sourceA"), Path.Combine(testDir, "sourceB") },
                    RestoreSettingsPerFramework = settingsPerFramework.ToArray()
                };

                // Act
                var result = task.Execute();

                // Assert
                result.Should().BeTrue();
                task.OutputSources.Should().BeEquivalentTo(new[] { Path.Combine(testDir, "sourceA"), Path.Combine(testDir, "sourceB"), Path.Combine(testDir, "sourceC") });
            }
        }

        [Fact]
        public void GetRestoreSettingsTask_VerifyRestoreAdditionalProjectFallbackFoldersAreAppended()
        {
            using (var testDir = TestDirectory.CreateInTemp())
            {
                // Arrange
                var buildEngine = new TestBuildEngine();
                var testLogger = buildEngine.TestLogger;

                var settingsPerFramework = new List<ITaskItem>();
                var settings1 = new Mock<ITaskItem>();
                settings1.SetupGet(e => e.ItemSpec).Returns("a");
                settings1.Setup(e => e.GetMetadata("RestoreAdditionalProjectFallbackFolders")).Returns(Path.Combine(testDir, "sourceC"));
                settingsPerFramework.Add(settings1.Object);

                var task = new GetRestoreSettingsTask()
                {
                    BuildEngine = buildEngine,
                    ProjectUniqueName = Path.Combine(testDir, "a.csproj"),
                    RestoreFallbackFolders = new[] { Path.Combine(testDir, "sourceA"), Path.Combine(testDir, "sourceB") },
                    RestoreSettingsPerFramework = settingsPerFramework.ToArray()
                };

                // Act
                var result = task.Execute();

                // Assert
                result.Should().BeTrue();
                task.OutputFallbackFolders.Should().BeEquivalentTo(new[] { Path.Combine(testDir, "sourceA"), Path.Combine(testDir, "sourceB"), Path.Combine(testDir, "sourceC") });
            }
        }

        [Fact]
        public void GetRestoreSettingsTask_VerifyRestoreAdditionalProjectFallbackFoldersWithExcludeAreNotAdded()
        {
            using (var testDir = TestDirectory.CreateInTemp())
            {
                // Arrange
                var buildEngine = new TestBuildEngine();
                var testLogger = buildEngine.TestLogger;

                var settingsPerFramework = new List<ITaskItem>();
                var settings1 = new Mock<ITaskItem>();
                settings1.SetupGet(e => e.ItemSpec).Returns("a");
                settings1.Setup(e => e.GetMetadata("RestoreAdditionalProjectFallbackFolders")).Returns(Path.Combine(testDir, "sourceC"));
                settingsPerFramework.Add(settings1.Object);

                var settings2 = new Mock<ITaskItem>();
                settings2.SetupGet(e => e.ItemSpec).Returns("b");
                settings2.Setup(e => e.GetMetadata("RestoreAdditionalProjectFallbackFoldersExcludes")).Returns(Path.Combine(testDir, "sourceC"));
                settingsPerFramework.Add(settings2.Object);

                var task = new GetRestoreSettingsTask()
                {
                    BuildEngine = buildEngine,
                    ProjectUniqueName = Path.Combine(testDir, "a.csproj"),
                    RestoreFallbackFolders = new[] { Path.Combine(testDir, "sourceA"), Path.Combine(testDir, "sourceB") },
                    RestoreSettingsPerFramework = settingsPerFramework.ToArray()
                };

                // Act
                var result = task.Execute();

                // Assert
                result.Should().BeTrue();
                task.OutputFallbackFolders.Should().BeEquivalentTo(new[] { Path.Combine(testDir, "sourceA"), Path.Combine(testDir, "sourceB") });
            }
        }

        [Fact]
        public void GetRestoreSettingsTask_VerifyAggregationAcrossFrameworks()
        {
            using (var testDir = TestDirectory.CreateInTemp())
            {
                // Arrange
                var buildEngine = new TestBuildEngine();
                var testLogger = buildEngine.TestLogger;

                var settingsPerFramework = new List<ITaskItem>();
                var settings1 = new Mock<ITaskItem>();
                settings1.SetupGet(e => e.ItemSpec).Returns("a");
                settings1.Setup(e => e.GetMetadata("RestoreAdditionalProjectSources")).Returns($"{Path.Combine(testDir, "a")};{Path.Combine(testDir, "b")}");
                settings1.Setup(e => e.GetMetadata("RestoreAdditionalProjectFallbackFolders")).Returns($"{Path.Combine(testDir, "m")};{Path.Combine(testDir, "n")}");
                settingsPerFramework.Add(settings1.Object);

                var settings2 = new Mock<ITaskItem>();
                settings2.SetupGet(e => e.ItemSpec).Returns("b");
                settings2.Setup(e => e.GetMetadata("RestoreAdditionalProjectSources")).Returns(Path.Combine(testDir, "c"));
                settings2.Setup(e => e.GetMetadata("RestoreAdditionalProjectFallbackFolders")).Returns(Path.Combine(testDir, "s"));
                settingsPerFramework.Add(settings2.Object);

                var settings3 = new Mock<ITaskItem>();
                settings3.SetupGet(e => e.ItemSpec).Returns("c");
                settings3.Setup(e => e.GetMetadata("RestoreAdditionalProjectSources")).Returns(Path.Combine(testDir, "d"));
                settingsPerFramework.Add(settings3.Object);

                var settings4 = new Mock<ITaskItem>();
                settings4.SetupGet(e => e.ItemSpec).Returns("d");
                settings4.Setup(e => e.GetMetadata("RestoreAdditionalProjectFallbackFolders")).Returns(Path.Combine(testDir, "t"));
                settingsPerFramework.Add(settings4.Object);

                var settings5 = new Mock<ITaskItem>();
                settings5.SetupGet(e => e.ItemSpec).Returns("e");
                settingsPerFramework.Add(settings5.Object);

                var task = new GetRestoreSettingsTask()
                {
                    BuildEngine = buildEngine,
                    ProjectUniqueName = Path.Combine(testDir, "a.csproj"),
                    RestoreSources = new[] { Path.Combine(testDir, "base") },
                    RestoreFallbackFolders = new[] { Path.Combine(testDir, "base") },
                    RestoreSettingsPerFramework = settingsPerFramework.ToArray()
                };

                // Act
                var result = task.Execute();

                // Assert
                result.Should().BeTrue();
                task.OutputSources.Should().BeEquivalentTo(new[] { Path.Combine(testDir, "base"), Path.Combine(testDir, "a"), Path.Combine(testDir, "b"), Path.Combine(testDir, "c"), Path.Combine(testDir, "d") });
                task.OutputFallbackFolders.Should().BeEquivalentTo(new[] { Path.Combine(testDir, "base"), Path.Combine(testDir, "m"), Path.Combine(testDir, "n"), Path.Combine(testDir, "s"), Path.Combine(testDir, "t") });
            }
        }

        [Fact]
        public void GetRestoreSettingsTask_VerifyNullPerFrameworkSettings()
        {
            using (var testDir = TestDirectory.CreateInTemp())
            {
                // Arrange
                var buildEngine = new TestBuildEngine();
                var testLogger = buildEngine.TestLogger;

                var task = new GetRestoreSettingsTask()
                {
                    BuildEngine = buildEngine,
                    ProjectUniqueName = Path.Combine(testDir, "a.csproj"),
                    RestoreSources = new[] { Path.Combine(testDir, "base") },
                    RestoreFallbackFolders = new[] { Path.Combine(testDir, "base") },
                    RestoreSettingsPerFramework = null
                };

                // Act
                var result = task.Execute();

                // Assert
                result.Should().BeTrue();
                task.OutputSources.Should().BeEquivalentTo(new[] { Path.Combine(testDir, "base") });
                task.OutputFallbackFolders.Should().BeEquivalentTo(new[] { Path.Combine(testDir, "base") });
            }
        }

        [Fact]
        public void GetRestoreSettingsTask_VerifyEmptyPerFrameworkSettings()
        {
            using (var testDir = TestDirectory.CreateInTemp())
            {
                // Arrange
                var buildEngine = new TestBuildEngine();
                var testLogger = buildEngine.TestLogger;

                var task = new GetRestoreSettingsTask()
                {
                    BuildEngine = buildEngine,
                    ProjectUniqueName = Path.Combine(testDir, "a.csproj"),
                    RestoreSources = new[] { Path.Combine(testDir, "base") },
                    RestoreFallbackFolders = new[] { Path.Combine(testDir, "base") },
                    RestoreSettingsPerFramework = new ITaskItem[0]
                };

                // Act
                var result = task.Execute();

                // Assert
                result.Should().BeTrue();
                task.OutputSources.Should().BeEquivalentTo(new[] { Path.Combine(testDir, "base") });
                task.OutputFallbackFolders.Should().BeEquivalentTo(new[] { Path.Combine(testDir, "base") });
            }
        }


        [Fact]
        public void GetRestoreSettingsTask_VerifyDisabledSourcesAreExcluded()
        {

            using (var testDir = TestDirectory.CreateInTemp())
            {
                // Arrange
                var buildEngine = new TestBuildEngine();
                var testLogger = buildEngine.TestLogger;


                var configFile = Path.Combine(testDir, Settings.DefaultSettingsFileName);

                var projectDirectory = Path.GetDirectoryName(configFile);
                Directory.CreateDirectory(projectDirectory);

                File.WriteAllText(configFile, DisableSourceConfig);

                var task = new GetRestoreSettingsTask()
                {
                    BuildEngine = buildEngine,
                    ProjectUniqueName = Path.Combine(testDir, "a.csproj"),
                    RestoreFallbackFolders = new[] { Path.Combine(testDir, "base") },
                    RestoreSettingsPerFramework = new ITaskItem[0]
                };

                // Act
                var result = task.Execute();

                // Assert
                result.Should().BeTrue();
                task.OutputSources.Should().BeEquivalentTo(new[] { @"https://nuget.org/v2/api" });
            }
        }

        [Fact]
        public void TestConfigFileProbingDirectory()
        {
            // Arrange
            var parentConfig = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <configuration>
                <fallbackPackageFolders>
                    <add key=""a"" value=""C:\Temp\a"" />
                </fallbackPackageFolders>
            </configuration>";

            var unreachableConfig = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <configuration>
                <fallbackPackageFolders>
                    <add key=""b"" value=""C:\Temp\b"" />
                </fallbackPackageFolders>
            </configuration>";

            var baseConfig = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <configuration>
                <packageSources>
                    <add key=""c"" value=""C:\Temp\c"" />
                </packageSources>
            </configuration>";

            var configName = "NuGet.Config";

            using (var machineWide = TestDirectory.CreateInTemp())
            using (var mockParentDirectory = TestDirectory.CreateInTemp())
            {
                // Parent
                //       Base
                //            Probe Path
                //       Unreachable
                var basePath = Path.Combine(mockParentDirectory, "base");
                var unreachablePath = Path.Combine(mockParentDirectory, "unreachable");
                var probePath = Path.Combine(basePath, "probe");
                Directory.CreateDirectory(basePath);
                Directory.CreateDirectory(unreachablePath);
                Directory.CreateDirectory(probePath);

                SettingsTestUtils.CreateConfigurationFile(configName, mockParentDirectory, parentConfig);
                SettingsTestUtils.CreateConfigurationFile(configName, basePath, baseConfig);
                SettingsTestUtils.CreateConfigurationFile(configName, unreachablePath, unreachableConfig);

                SettingsTestUtils.CreateConfigurationFile(configName, machineWide, MachineWideSettingsConfig);

                var machineWideSettings = new Lazy<IMachineWideSettings>(() => new TestMachineWideSettings(new Settings(machineWide, configName, isMachineWide: true)));

                // Test

                var settings = RestoreSettingsUtils.ReadSettings(null, probePath, null, machineWideSettings);
                var filePaths = settings.GetConfigFilePaths();

                Assert.Equal(4, filePaths.Count()); // base, parent, app data + machine wide
                Assert.Contains(Path.Combine(basePath, configName), filePaths);
                Assert.Contains(Path.Combine(mockParentDirectory, configName), filePaths);
                Assert.DoesNotContain(Path.Combine(unreachablePath, configName), filePaths);
            }
        }

        [Fact]
        public void TestConfigFileProbingDirectoryWithSettingsLoadingContext()
        {
            // Arrange
            var parentConfig = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <configuration>
                <fallbackPackageFolders>
                    <add key=""a"" value=""C:\Temp\a"" />
                </fallbackPackageFolders>
            </configuration>";

            var unreachableConfig = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <configuration>
                <fallbackPackageFolders>
                    <add key=""b"" value=""C:\Temp\b"" />
                </fallbackPackageFolders>
            </configuration>";

            var baseConfig = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <configuration>
                <packageSources>
                    <add key=""c"" value=""C:\Temp\c"" />
                </packageSources>
            </configuration>";

            var configName = "NuGet.Config";

            using (var machineWide = TestDirectory.CreateInTemp())
            using (var mockParentDirectory = TestDirectory.CreateInTemp())
            {
                // Parent
                //       Base
                //            Probe Path
                //       Unreachable
                var basePath = Path.Combine(mockParentDirectory, "base");
                var unreachablePath = Path.Combine(mockParentDirectory, "unreachable");
                var probePath = Path.Combine(basePath, "probe");
                Directory.CreateDirectory(basePath);
                Directory.CreateDirectory(unreachablePath);
                Directory.CreateDirectory(probePath);

                SettingsTestUtils.CreateConfigurationFile(configName, mockParentDirectory, parentConfig);
                SettingsTestUtils.CreateConfigurationFile(configName, basePath, baseConfig);
                SettingsTestUtils.CreateConfigurationFile(configName, unreachablePath, unreachableConfig);

                SettingsTestUtils.CreateConfigurationFile(configName, machineWide, MachineWideSettingsConfig);

                var machineWideSettings = new Lazy<IMachineWideSettings>(() => new TestMachineWideSettings(new Settings(machineWide, configName, isMachineWide: true)));

                // Test
                using (var settingsLoadingContext = new SettingsLoadingContext())
                {
                    var settings = RestoreSettingsUtils.ReadSettings(null, probePath, null, machineWideSettings, settingsLoadingContext);
                    var filePaths = settings.GetConfigFilePaths();

                    Assert.Equal(4, filePaths.Count()); // base, parent, app data + machine wide
                    Assert.Contains(Path.Combine(basePath, configName), filePaths);
                    Assert.Contains(Path.Combine(mockParentDirectory, configName), filePaths);
                    Assert.DoesNotContain(Path.Combine(unreachablePath, configName), filePaths);
                }
            }
        }

        /// <summary>
        /// This mimics the GetRestoreSettingsTask call when msbuild /t:restore is called.
        /// MSBuild /t:restore behaves the same regardless whether it's invoked on the project or solution level. 
        /// </summary>
        [Fact]
        public void GetRestoreSettingsTask_RestoreTaskBased_PackageReference_ProjectLevelConfig()
        {
            using (var testDir = TestDirectory.CreateInTemp())
            {
                // Arrange
                var buildEngine = new TestBuildEngine();
                var testLogger = buildEngine.TestLogger;

                var settingsPerFramework = new List<ITaskItem>();
                var settings1 = new Mock<ITaskItem>();
                settings1.SetupGet(e => e.ItemSpec).Returns("a");
                settingsPerFramework.Add(settings1.Object);

                var task = new GetRestoreSettingsTask()
                {
                    BuildEngine = buildEngine,
                    ProjectUniqueName = Path.Combine(testDir, "a.csproj"),
                    RestoreSettingsPerFramework = settingsPerFramework.ToArray()
                };

                var configFile = Path.Combine(testDir, Settings.DefaultSettingsFileName);
                File.WriteAllText(configFile, RootConfig);

                // Act
                var result = task.Execute();

                // Assert
                result.Should().BeTrue();
                task.OutputSources.Should().BeEquivalentTo(new[] { "https://api.nuget.org/v3/index.json" });
                task.OutputFallbackFolders.Should().BeEmpty();
                task.OutputConfigFilePaths.Should().Contain(configFile);
            }
        }

        /// <summary>
        /// This mimics the GetRestoreSettingsTask call when NuGet.exe on a solution is called.
        /// MSBuild /t:restore behaves the same regardless whether it's invoked on the project or solution level. 
        /// </summary>
        [Fact]
        public void GetRestoreSettingsTask_NuGetExeBased_PackageReference_ProjectLevelConfig_IsIgnored()
        {
            using (var testDir = TestDirectory.CreateInTemp())
            {
                var projectDir = Path.Combine(testDir, "project");
                Directory.CreateDirectory(projectDir);
                // Arrange
                var buildEngine = new TestBuildEngine();
                var testLogger = buildEngine.TestLogger;

                var settingsPerFramework = new List<ITaskItem>();
                var settings1 = new Mock<ITaskItem>();
                settings1.SetupGet(e => e.ItemSpec).Returns("a");
                settingsPerFramework.Add(settings1.Object);

                var task = new GetRestoreSettingsTask()
                {
                    BuildEngine = buildEngine,
                    ProjectUniqueName = Path.Combine(projectDir, "a.csproj"),
                    RestoreSolutionDirectory = testDir,
                    RestoreSettingsPerFramework = settingsPerFramework.ToArray()
                };

                var rootConfigFile = Path.Combine(testDir, Settings.DefaultSettingsFileName);
                var projectLevelConfigFile = Path.Combine(projectDir, Settings.DefaultSettingsFileName);
                File.WriteAllText(rootConfigFile, RootConfig);
                File.WriteAllText(projectLevelConfigFile, ProjectLevelConfig);
                // Act
                var result = task.Execute();

                // Assert
                result.Should().BeTrue();
                task.OutputSources.Should().BeEquivalentTo(new[] { "https://api.nuget.org/v3/index.json" });
                task.OutputFallbackFolders.Should().BeEmpty();
                task.OutputConfigFilePaths.Should().Contain(rootConfigFile);
                task.OutputConfigFilePaths.Should().NotContain(projectLevelConfigFile);

            }
        }

        /// <summary>
        /// This mimics the GetRestoreSettingsTask call when NuGet.exe on a solution is called.
        /// MSBuild /t:restore behaves the same regardless whether it's invoked on the project or solution level. 
        /// </summary>
        [Fact]
        public void GetRestoreSettingsTask_WithRestoreRootDirectory_ProjectLevelConfigIsIgnored()
        {
            using (var testDir = TestDirectory.CreateInTemp())
            {
                var projectDir = Path.Combine(testDir, "project");
                Directory.CreateDirectory(projectDir);
                // Arrange
                var buildEngine = new TestBuildEngine();
                var testLogger = buildEngine.TestLogger;

                var settingsPerFramework = new List<ITaskItem>();
                var settings1 = new Mock<ITaskItem>();
                settings1.SetupGet(e => e.ItemSpec).Returns("a");
                settingsPerFramework.Add(settings1.Object);

                var task = new GetRestoreSettingsTask()
                {
                    BuildEngine = buildEngine,
                    ProjectUniqueName = Path.Combine(projectDir, "a.csproj"),
                    RestoreRootConfigDirectory = testDir,
                    RestoreSettingsPerFramework = settingsPerFramework.ToArray()
                };

                var rootConfigFile = Path.Combine(testDir, Settings.DefaultSettingsFileName);
                var projectLevelConfigFile = Path.Combine(projectDir, Settings.DefaultSettingsFileName);
                File.WriteAllText(rootConfigFile, RootConfig);
                File.WriteAllText(projectLevelConfigFile, ProjectLevelConfig);
                // Act
                var result = task.Execute();

                // Assert
                result.Should().BeTrue();
                task.OutputSources.Should().BeEquivalentTo(new[] { "https://api.nuget.org/v3/index.json" });
                task.OutputFallbackFolders.Should().BeEmpty();
                task.OutputConfigFilePaths.Should().Contain(rootConfigFile);
                task.OutputConfigFilePaths.Should().NotContain(projectLevelConfigFile);

            }
        }

        [Fact]
        public void GetRestoreSettingsTask_WithRestoreSourcesOverride_ResolvesAgainstWorkingDirectory()
        {
            using (var testDir = TestDirectory.CreateInTemp())
            {
                // Arrange
                var buildEngine = new TestBuildEngine();
                var testLogger = buildEngine.TestLogger;

                var settingsPerFramework = new List<ITaskItem>();
                var settings1 = new Mock<ITaskItem>();
                settings1.SetupGet(e => e.ItemSpec).Returns("a");
                settingsPerFramework.Add(settings1.Object);
                var startupDirectory = Path.Combine(testDir, "innerPath");
                var relativePath = "relativeSource";

                var task = new GetRestoreSettingsTask()
                {
                    BuildEngine = buildEngine,
                    MSBuildStartupDirectory = startupDirectory,
                    ProjectUniqueName = Path.Combine(testDir, "a.csproj"),
                    RestoreSources = new[] { Path.Combine(testDir, "sourceA"), Path.Combine(testDir, "sourceB") },
                    RestoreSourcesOverride = new[] { relativePath },
                    RestoreSettingsPerFramework = settingsPerFramework.ToArray()
                };

                // Act
                var result = task.Execute();

                // Assert
                result.Should().BeTrue();
                task.OutputSources.Should().BeEquivalentTo(new[] { Path.Combine(startupDirectory, relativePath) });
            }
        }

        [Fact]
        public void GetRestoreSettingsTask_WithFallbackFoldersOverride_ResolvesAgainstWorkingDirectory()
        {
            using (var testDir = TestDirectory.CreateInTemp())
            {
                // Arrange
                var buildEngine = new TestBuildEngine();
                var testLogger = buildEngine.TestLogger;

                var settingsPerFramework = new List<ITaskItem>();
                var settings1 = new Mock<ITaskItem>();
                settings1.SetupGet(e => e.ItemSpec).Returns("a");
                settingsPerFramework.Add(settings1.Object);
                var startupDirectory = Path.Combine(testDir, "innerPath");
                var relativePath = "relativeSource";

                var task = new GetRestoreSettingsTask()
                {
                    BuildEngine = buildEngine,
                    MSBuildStartupDirectory = startupDirectory,
                    ProjectUniqueName = Path.Combine(testDir, "a.csproj"),
                    RestoreFallbackFolders = new[] { Path.Combine(testDir, "sourceA"), Path.Combine(testDir, "sourceB") },
                    RestoreFallbackFoldersOverride = new[] { relativePath },
                    RestoreSettingsPerFramework = settingsPerFramework.ToArray()
                };

                // Act
                var result = task.Execute();

                // Assert
                result.Should().BeTrue();
                task.OutputFallbackFolders.Should().BeEquivalentTo(new[] { Path.Combine(startupDirectory, relativePath) });
            }
        }

        private static readonly string MachineWideSettingsConfig = @"<?xml version=""1.0"" encoding=""utf-8""?>
                <configuration>
                </configuration>";

        private static readonly string InnerConfig =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
              <configuration>
                <SectionName>
                  <add key=""inner-key"" value=""inner-value"" />
                </SectionName>
              </configuration>";

        private static readonly string OuterConfig =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
              <configuration>
                <SectionName>
                  <add key=""outer-key"" value=""outer-value"" />
                </SectionName>
              </configuration>";

        private static readonly string DisableSourceConfig =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
             <configuration>
              <packageSources>
                <Clear/>
                <add key=""NuGet"" value=""https://api.nuget.org/v3/index.json"" />
                <add key=""NuGet.v2"" value=""https://nuget.org/v2/api"" />
              </packageSources>
              <disabledPackageSources>
                 <Clear/>
                 <add key=""NuGet"" value=""true"" />
              </disabledPackageSources>
            </configuration>";

        private static readonly string RootConfig =
        @"<?xml version=""1.0"" encoding=""utf-8""?>
             <configuration>
              <fallbackPackageFolders>
                <Clear/>
              </fallbackPackageFolders>
              <packageSources>
                <Clear/>
                <add key=""NuGet"" value=""https://api.nuget.org/v3/index.json"" />
              </packageSources>
              <disabledPackageSources>
                 <Clear/>
              </disabledPackageSources>
            </configuration>";


        private static readonly string ProjectLevelConfig =
        @"<?xml version=""1.0"" encoding=""utf-8""?>
             <configuration>
              <fallbackPackageFolders>
                <Clear/>
              </fallbackPackageFolders>
              <packageSources>
                <add key=""ProjectLevel"" value=""C:\Source"" />
              </packageSources>
              <disabledPackageSources>
                 <Clear/>
              </disabledPackageSources>
            </configuration>";
    }
}
