// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Common.Test
{
    public class ProjectJsonPathUtilitiesTests
    {
        [Fact]
        public void ProjectJsonPathUtilities_GetLockFilePathWithProjectNameOnly()
        {
            // Arrange
            using (var randomProjectFolderPath = TestDirectory.Create())
            {
                var projNameFile = Path.Combine(randomProjectFolderPath, "abc.project.json");
                CreateFile(projNameFile);

                // Act
                var path = ProjectJsonPathUtilities.GetProjectConfigPath(randomProjectFolderPath, "abc");
                var fileName = Path.GetFileName(path);

                // Assert
                Assert.Equal(fileName, "abc.project.json");
            }
        }

        [Fact]
        public void ProjectJsonPathUtilities_GetLockFilePathWithBothProjectJsonFiles()
        {
            // Arrange
            using (var randomProjectFolderPath = TestDirectory.Create())
            {
                var projNameFile = Path.Combine(randomProjectFolderPath, "abc.project.json");
                var projJsonFile = Path.Combine(randomProjectFolderPath, "project.json");
                CreateFile(projNameFile);
                CreateFile(projJsonFile);

                // Act
                var path = ProjectJsonPathUtilities.GetProjectConfigPath(randomProjectFolderPath, "abc");
                var fileName = Path.GetFileName(path);

                // Assert
                Assert.Equal(fileName, "abc.project.json");
            }
        }

        [Fact]
        public void ProjectJsonPathUtilities_GetLockFilePathWithProjectJsonFromAnotherProject()
        {
            // Arrange
            using (var randomProjectFolderPath = TestDirectory.Create())
            {
                var projNameFile = Path.Combine(randomProjectFolderPath, "xyz.project.json");
                var projJsonFile = Path.Combine(randomProjectFolderPath, "project.json");
                CreateFile(projNameFile);
                CreateFile(projJsonFile);

                // Act
                var path = ProjectJsonPathUtilities.GetProjectConfigPath(randomProjectFolderPath, "abc");
                var fileName = Path.GetFileName(path);

                // Assert
                Assert.Equal(fileName, "project.json");

                // Clean-up
            }
        }

        [Fact]
        public void ProjectJsonPathUtilities_GetLockFilePathWithProjectNameJsonAndAnotherProject()
        {
            // Arrange
            using (var randomProjectFolderPath = TestDirectory.Create())
            {
                var otherFile = Path.Combine(randomProjectFolderPath, "xyz.project.json");
                var projJsonFile = Path.Combine(randomProjectFolderPath, "abc.project.json");
                CreateFile(otherFile);
                CreateFile(projJsonFile);

                // Act
                var path = ProjectJsonPathUtilities.GetProjectConfigPath(randomProjectFolderPath, "abc");
                var fileName = Path.GetFileName(path);

                // Assert
                Assert.Equal(fileName, "abc.project.json");

                // Clean-up
            }
        }

        [Fact]
        public void ProjectJsonPathUtilities_GetLockFilePathWithNoFiles()
        {
            // Arrange
            using (var randomProjectFolderPath = TestDirectory.Create())
            {
                var expected = Path.Combine(randomProjectFolderPath, "project.json");

                // Act
                var path = ProjectJsonPathUtilities.GetProjectConfigPath(randomProjectFolderPath, "abc");

                // Assert
                Assert.Equal(expected, path);

                // Clean-up
            }
        }

        [Fact]
        public void ProjectJsonPathUtilities_GetLockFilePathWithProjectJsonOnly()
        {
            // Arrange
            using (var randomProjectFolderPath = TestDirectory.Create())
            {
                var projJsonFile = Path.Combine(randomProjectFolderPath, "project.json");
                CreateFile(projJsonFile);

                // Act
                var path = ProjectJsonPathUtilities.GetProjectConfigPath(randomProjectFolderPath, "abc");
                var fileName = Path.GetFileName(path);

                // Assert
                Assert.Equal(fileName, "project.json");

                // Clean-up
            }
        }

        [Theory]
        [InlineData("abc", "abc.project.json")]
        [InlineData("ABC", "ABC.project.json")]
        [InlineData("A B C", "A B C.project.json")]
        [InlineData("a.b.c", "a.b.c.project.json")]
        [InlineData(" ", " .project.json")]
        public void ProjectJsonPathUtilities_GetProjectConfigWithProjectName(string projectName, string fileName)
        {
            // Arrange & Act
            var generatedName = ProjectJsonPathUtilities.GetProjectConfigWithProjectName(projectName);

            // Assert
            Assert.Equal(fileName, generatedName);
        }

        [Theory]
        [InlineData("abc", "abc.project.lock.json")]
        [InlineData("ABC", "ABC.project.lock.json")]
        [InlineData("A B C", "A B C.project.lock.json")]
        [InlineData("a.b.c", "a.b.c.project.lock.json")]
        [InlineData(" ", " .project.lock.json")]
        public void ProjectJsonPathUtilities_GetProjectLockFileNameWithProjectName(
            string projectName,
            string fileName)
        {
            // Arrange & Act
            var generatedName = ProjectJsonPathUtilities.GetProjectLockFileNameWithProjectName(projectName);

            // Assert
            Assert.Equal(fileName, generatedName);
        }

        [Theory]
        [InlineData("abc", "abc.project.json")]
        [InlineData("ABC", "ABC.project.json")]
        [InlineData("A B C", "A B C.project.json")]
        [InlineData("a.b.c", "a.b.c.project.json")]
        [InlineData(" ", " .project.json")]
        [InlineData("", ".project.json")]
        public void ProjectJsonPathUtilities_GetProjectNameFromConfigFileName(
            string projectName,
            string fileName)
        {
            // Arrange & Act
            var result = ProjectJsonPathUtilities.GetProjectNameFromConfigFileName(fileName);

            // Assert
            Assert.Equal(projectName, result);
        }

        [Theory]
        [InlineData("abc.project.json")]
        [InlineData("a b c.project.json")]
        [InlineData("MY LONG PROJECT NAME 234234432.project.json")]
        [InlineData("packages.config.project.json")]
        [InlineData("111.project.json")]
        [InlineData("project.json")]
        [InlineData("prOject.JSon")]
        [InlineData("xyz.prOject.JSon")]
        [InlineData("c:\\users\\project.json")]
        [InlineData("dir\\project.json")]
        [InlineData("c:\\users\\abc.project.json")]
        [InlineData("dir\\abc.project.json")]
        [InlineData(".\\abc.project.json")]
        public void ProjectJsonPathUtilities_IsProjectConfig_True(string path)
        {
            // Arrange & Act
            var result = ProjectJsonPathUtilities.IsProjectConfig(ConvertToUnix(path));

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("abcproject.json")]
        [InlineData("a b c.project.jso")]
        [InlineData("abc.project..json")]
        [InlineData("packages.config")]
        [InlineData("project.json ")]
        [InlineData("c:\\users\\packages.config")]
        [InlineData("c:\\users\\abc.project..json")]
        [InlineData("c:\\users\\")]
        [InlineData("\t")]
        [InlineData("")]
        public void ProjectJsonPathUtilities_IsProjectConfig_False(string path)
        {
            // Arrange & Act
            var result = ProjectJsonPathUtilities.IsProjectConfig(ConvertToUnix(path));

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("project.json", "project.lock.json")]
        [InlineData("dir\\project.json", "dir\\project.lock.json")]
        [InlineData("c:\\users\\project.json", "c:\\users\\project.lock.json")]
        [InlineData("abc.project.json", "abc.project.lock.json")]
        [InlineData("dir\\abc.project.json", "dir\\abc.project.lock.json")]
        [InlineData("c:\\users\\abc.project.json", "c:\\users\\abc.project.lock.json")]
        public void ProjectJsonPathUtilities_GetLockFilePath(string configPath, string lockFilePath)
        {
            // Arrange & Act
            var result = ProjectJsonPathUtilities.GetLockFilePath(ConvertToUnix(configPath));

            // Assert
            Assert.Equal(ConvertToUnix(lockFilePath), result);
        }

        private static void CreateFile(string path)
        {
            File.WriteAllText(path, string.Empty);
        }

        private static string ConvertToUnix(string path)
        {
            if (!RuntimeEnvironmentHelper.IsWindows)
            {
                return path.Replace("c:\\", "/tmp/").Replace('\\', '/');
            }
            else
            {
                return path;
            }
        }
    }
}
