// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using NuGet.Common.Migrations;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Common.Test
{
    public class Migration1Tests
    {
        [Fact]
        public void DeleteMigratedDirectories_DeletesEmptyDirectories_Success()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                var v3cachePath = Path.Combine(testDirectory, "v3-cache");
                Directory.CreateDirectory(v3cachePath);
                var pluginsCachePath = Path.Combine(testDirectory, "plugins-cache");
                Directory.CreateDirectory(pluginsCachePath);

                Migration1.DeleteMigratedDirectories(testDirectory);

                Assert.False(Directory.Exists(v3cachePath));
                Assert.False(Directory.Exists(pluginsCachePath));
            }
        }

        [Fact]
        public void DeleteMigratedDirectories_DeletesNonEmptyDirectories_Success()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                var v3cachePath = Path.Combine(testDirectory, "v3-cache");
                var v3cacheSubDirectoryInfo = Directory.CreateDirectory(Path.Combine(v3cachePath, "subDirectory"));
                File.WriteAllText(Path.Combine(v3cacheSubDirectoryInfo.FullName, "temp.dat"), string.Empty);
                var pluginsCachePath = Path.Combine(testDirectory, "plugins-cache");
                Directory.CreateDirectory(pluginsCachePath);
                var pluginscacheSubDirectoryInfo = Directory.CreateDirectory(Path.Combine(pluginsCachePath, "subDirectory"));
                File.WriteAllText(Path.Combine(pluginscacheSubDirectoryInfo.FullName, "temp.dat"), string.Empty);

                Migration1.DeleteMigratedDirectories(testDirectory);

                Assert.False(Directory.Exists(v3cachePath));
                Assert.False(Directory.Exists(pluginsCachePath));
            }
        }

        [PlatformTheory(Platform.Darwin, Platform.Linux)]
        [InlineData("777", "022", "755")]
        [InlineData("775", "002", "775")]
        [InlineData("700", "022", "700")]
        [InlineData("700", "002", "700")]
        public void EnsureExpectedPermissions_Directories_Success(string currentPermissions, string umask, string newPermissions)
        {
            using (var testDirectory = TestDirectory.Create())
            {
                var v3cachePath = Path.Combine(testDirectory, "my documents", "v3-cache");
                var v3cacheSubDirectoryInfo = Directory.CreateDirectory(Path.Combine(v3cachePath, "subDirectory"));
                Migration1.Exec("chmod", currentPermissions + " " + testDirectory.Path);
                Migration1.Exec("chmod", currentPermissions + " " + v3cachePath.FormatWithDoubleQuotes());
                Migration1.Exec("chmod", currentPermissions + " " + v3cacheSubDirectoryInfo.FullName.FormatWithDoubleQuotes());
                HashSet<string> pathsToCheck = new HashSet<string>() { testDirectory.Path, v3cachePath, v3cacheSubDirectoryInfo.FullName };

                Migration1.EnsureExpectedPermissions(pathsToCheck, PosixPermissions.Parse(umask));

                string expectedPermissions = PosixPermissions.Parse(newPermissions).ToString();
                Assert.Equal(Migration1.GetPermissions(testDirectory.Path).ToString(), expectedPermissions);
                Assert.Equal(Migration1.GetPermissions(v3cachePath).ToString(), expectedPermissions);
                Assert.Equal(Migration1.GetPermissions(v3cacheSubDirectoryInfo.FullName).ToString(), expectedPermissions);
            }
        }

        [PlatformFact(Platform.Darwin, Platform.Linux)]
        public void EnsureConfigFilePermissions_NuGetConfig_Success()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                string testDirectoryConfigPath = Path.Combine(testDirectory.Path, "NuGet.Config");
                File.WriteAllText(testDirectoryConfigPath, string.Empty);
                Migration1.Exec("chmod", "666" + " " + testDirectoryConfigPath);
                var subDirectory = Directory.CreateDirectory(Path.Combine(testDirectory.Path, "config"));
                string subDirectoryConfigPath = Path.Combine(subDirectory.FullName, "NuGet.Config");
                File.WriteAllText(subDirectoryConfigPath, string.Empty);
                Migration1.Exec("chmod", "666" + " " + subDirectoryConfigPath);

                Migration1.EnsureConfigFilePermissions(testDirectory.Path, PosixPermissions.Parse("077"));

                string expectedPermissions = PosixPermissions.Parse("600").ToString();
                Assert.Equal(Migration1.GetPermissions(testDirectoryConfigPath).ToString(), expectedPermissions);
                Assert.Equal(Migration1.GetPermissions(subDirectoryConfigPath).ToString(), expectedPermissions);
            }
        }
    }
}
