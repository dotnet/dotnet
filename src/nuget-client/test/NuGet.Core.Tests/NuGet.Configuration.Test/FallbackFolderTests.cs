// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Configuration.Test
{
    public class FallbackFolderTests
    {
        [Fact]
        public void GetFallbackPackageFolders_DefaultHasNoFallbackFolders()
        {
            // Arrange & Act
            var paths = SettingsUtility.GetFallbackPackageFolders(NullSettings.Instance);

            // Assert
            Assert.Equal(0, paths.Count);
        }

        [Fact]
        public void GetFallbackPackageFolders_SingleFolderFromNuGetConfig()
        {
            // Arrange
            var config = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <fallbackPackageFolders>
        <add key=""shared"" value=""a"" />
    </fallbackPackageFolders>
</configuration>";

            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                Settings settings = new Settings(mockBaseDirectory);

                // Act
                var paths = SettingsUtility.GetFallbackPackageFolders(settings);

                // Assert
                Assert.Equal(Path.Combine(mockBaseDirectory, "a"), paths.Single());
            }
        }

        [Fact]
        public void GetFallbackPackageFolders_RelativePath()
        {
            // Arrange
            var config = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <fallbackPackageFolders>
        <add key=""shared"" value=""../test"" />
    </fallbackPackageFolders>
</configuration>";

            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                var subFolder = Path.Combine(mockBaseDirectory, "sub");
                var testFolder = Path.Combine(mockBaseDirectory, "test");


                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, subFolder, config);
                Settings settings = new Settings(subFolder);

                // Act
                var paths = SettingsUtility.GetFallbackPackageFolders(settings);

                // Assert
                Assert.Equal(testFolder, paths.Single());
            }
        }

        [Fact]
        public void GetFallbackPackageFolders_RelativePathChild()
        {
            // Arrange
            var config = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <fallbackPackageFolders>
        <add key=""shared"" value=""test"" />
    </fallbackPackageFolders>
</configuration>";

            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                var testFolder = Path.Combine(mockBaseDirectory, "test");


                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                Settings settings = new Settings(mockBaseDirectory);

                // Act
                var paths = SettingsUtility.GetFallbackPackageFolders(settings);

                // Assert
                Assert.Equal(testFolder, paths.Single());
            }
        }

        [Fact]
        public void GetFallbackPackageFolders_MultipleFoldersFromNuGetConfig()
        {
            // Arrange
            var config = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <fallbackPackageFolders>
        <add key=""d"" value=""C:\Temp\d"" />
        <add key=""b"" value=""C:\Temp\b"" />
        <add key=""c"" value=""C:\Temp\c"" />
    </fallbackPackageFolders>
</configuration>";

            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                var settings = new Settings(mockBaseDirectory);

                // Act
                var paths = SettingsUtility.GetFallbackPackageFolders(settings).ToArray();

                // Assert
                Assert.Equal(3, paths.Length);
                Assert.Equal("d", GetFileName(paths[0]));
                Assert.Equal("b", GetFileName(paths[1]));
                Assert.Equal("c", GetFileName(paths[2]));
            }
        }

        [Fact]
        public void GetFallbackPackageFolders_MultipleFoldersFromMultipleNuGetConfigs()
        {
            // Arrange
            var configA = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <fallbackPackageFolders>
        <add key=""a"" value=""C:\Temp\a"" />
        <add key=""b"" value=""C:\Temp\b"" />
    </fallbackPackageFolders>
</configuration>";

            var configB = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <fallbackPackageFolders>
        <add key=""c"" value=""C:\Temp\c"" />
        <add key=""d"" value=""C:\Temp\d"" />
    </fallbackPackageFolders>
</configuration>";

            var configC = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <fallbackPackageFolders>
        <add key=""x"" value=""C:\Temp\x"" />
        <add key=""y"" value=""C:\Temp\y"" />
    </fallbackPackageFolders>
</configuration>";

            var nugetConfigPath = "NuGet.Config";
            using (var machineWide = TestDirectory.Create())
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                var subFolder = Path.Combine(mockBaseDirectory, "sub");

                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, subFolder, configA);
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, configB);
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, machineWide, configC);

                var settings = Settings.LoadSettingsGivenConfigPaths(new[] {
                    Path.Combine(subFolder, nugetConfigPath),
                    Path.Combine(mockBaseDirectory, nugetConfigPath),
                    Path.Combine(machineWide, nugetConfigPath)
                });

                // Act
                var actual = SettingsUtility
                    .GetFallbackPackageFolders(settings)
                    .Select(GetFileName);

                var expected = new[] { "a", "b", "c", "d", "x", "y" };

                // Ignore any extra folders on the machine
                var actualFiltered = Enumerable.Intersect(actual, expected);
                Assert.Equal(expected, actualFiltered);
            }
        }

        [Fact]
        public void GetFallbackPackageFolders_ClearTag()
        {
            // Arrange
            var configA = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <fallbackPackageFolders>
        <clear />
        <add key=""a"" value=""C:\Temp\a"" />
        <add key=""b"" value=""C:\Temp\b"" />
    </fallbackPackageFolders>
</configuration>";

            var configB = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <fallbackPackageFolders>
        <add key=""c"" value=""C:\Temp\c"" />
        <add key=""d"" value=""C:\Temp\d"" />
    </fallbackPackageFolders>
</configuration>";

            var configC = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <fallbackPackageFolders>
        <add key=""x"" value=""C:\Temp\x"" />
        <add key=""y"" value=""C:\Temp\y"" />
    </fallbackPackageFolders>
</configuration>";

            var nugetConfigPath = "NuGet.Config";
            using (var machineWide = TestDirectory.Create())
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                var subFolder = Path.Combine(mockBaseDirectory, "sub");

                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, subFolder, configA);
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, configB);
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, machineWide, configC);

                var settings = Settings.LoadSettingsGivenConfigPaths(new[] {
                    Path.Combine(subFolder, nugetConfigPath),
                    Path.Combine(mockBaseDirectory, nugetConfigPath),
                    Path.Combine(machineWide, nugetConfigPath)
                });

                // Act
                var paths = SettingsUtility.GetFallbackPackageFolders(settings).ToArray();

                // Assert
                Assert.Equal(2, paths.Length);
                Assert.Equal("a", GetFileName(paths[0]));
                Assert.Equal("b", GetFileName(paths[1]));
            }
        }

        private static string? GetFileName(string path)
        {
            return path.Split(new char[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
        }
    }
}
