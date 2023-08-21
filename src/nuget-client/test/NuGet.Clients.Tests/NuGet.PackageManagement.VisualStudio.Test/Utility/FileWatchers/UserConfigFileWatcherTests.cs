// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Threading;
using FluentAssertions;
using NuGet.Configuration;
using NuGet.PackageManagement.VisualStudio.Utility.FileWatchers;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.PackageManagement.VisualStudio.Test.Utility.FileWatchers
{
    public class UserConfigFileWatcherTests
    {
        [Fact]
        public void FileChanged_WhenFileCreated_NotificationReceived()
        {
            // Arrange
            using TestDirectory testDirectory = TestDirectory.Create();
            using UserConfigFileWatcher target = new(testDirectory.Path);
            ManualResetEventSlim mre = new(initialState: false, spinCount: 0);
            target.FileChanged += (s, e) => mre.Set();
            string configPath = Path.Combine(testDirectory.Path, Settings.DefaultSettingsFileName);

            // Act
            File.WriteAllText(configPath, string.Empty);
            // File system watchers are not real time, so we need to give a little time for all the async file IO to happen
            bool obtained = mre.Wait(TimeSpan.FromSeconds(10));

            // Assert
            obtained.Should().BeTrue();
        }

        [Fact]
        public void FileChanged_WhenFileModified_NotificationReceivedAsync()
        {
            // Arrange
            using TestDirectory testDirectory = TestDirectory.Create();
            string configPath = Path.Combine(testDirectory.Path, Settings.DefaultSettingsFileName);
            File.WriteAllText(configPath, string.Empty);
            ManualResetEventSlim mre = new(initialState: false, spinCount: 0);
            using UserConfigFileWatcher target = new(testDirectory.Path);
            target.FileChanged += (s, e) => mre.Set();

            // Act
            mre.Wait(0).Should().BeFalse();
            File.WriteAllText(configPath, "1");
            // File system watchers are not real time, so we need to give a little time for all the async file IO to happen
            bool obtained = mre.Wait(TimeSpan.FromSeconds(10));

            // Assert
            obtained.Should().BeTrue();
        }

        [Fact]
        public void FileChanged_WhenFileDeleted_NotificationReceivedAsync()
        {
            // Arrange
            using TestDirectory testDirectory = TestDirectory.Create();
            string configPath = Path.Combine(testDirectory.Path, Settings.DefaultSettingsFileName);
            File.WriteAllText(configPath, string.Empty);
            ManualResetEventSlim mre = new(initialState: false, spinCount: 0);
            using UserConfigFileWatcher target = new(testDirectory.Path);
            target.FileChanged += (s, e) => mre.Set();

            // Act
            mre.Wait(0).Should().BeFalse();
            File.Delete(configPath);
            // File system watchers are not real time, so we need to give a little time for all the async file IO to happen
            bool obtained = mre.Wait(TimeSpan.FromSeconds(10));

            // Assert
            obtained.Should().BeTrue();
        }

        [Theory]
        [InlineData("nuget.config", "nuget.config.old")]
        [InlineData("nuget.config.old", "nuget.config")]
        public void FileChanged_WhenFileRenamed_NotificationReceived(string filename1, string filename2)
        {
            // Arrange
            using TestDirectory testDirectory = TestDirectory.Create();
            string configPath1 = Path.Combine(testDirectory.Path, filename1);
            string configPath2 = Path.Combine(testDirectory.Path, filename2);
            File.WriteAllText(configPath1, string.Empty);
            ManualResetEventSlim mre = new(initialState: false, spinCount: 0);
            using UserConfigFileWatcher target = new(testDirectory.Path);
            target.FileChanged += (s, e) => mre.Set();

            // Act
            mre.Wait(0).Should().BeFalse();
            File.Move(configPath1, configPath2);
            // File system watchers are not real time, so we need to give a little time for all the async file IO to happen
            bool obtained = mre.Wait(TimeSpan.FromSeconds(10));

            // Assert
            obtained.Should().BeTrue();
        }

        [Fact]
        public void FileChanged_WhenFileCreatedInConfigSubdirectory_NotificationReceived()
        {
            // Arrange
            using TestDirectory testDirectory = TestDirectory.Create();
            var configDirectory = Path.Combine(testDirectory, "config");
            using UserConfigFileWatcher target = new(testDirectory.Path);
            ManualResetEventSlim mre = new(initialState: false, spinCount: 0);
            target.FileChanged += (s, e) => mre.Set();
            string configPath = Path.Combine(configDirectory, "contoso.config");

            // Act
            File.WriteAllText(configPath, string.Empty);
            // File system watchers are not real time, so we need to give a little time for all the async file IO to happen
            bool obtained = mre.Wait(TimeSpan.FromSeconds(10));

            // Assert
            obtained.Should().BeTrue();
        }

        [Fact]
        public void FileChanged_NonConfigFileChanged_NotificationNotReceived()
        {
            // Arrange
            using TestDirectory testDirectory = TestDirectory.Create();
            ManualResetEventSlim mre = new(initialState: false, spinCount: 0);
            using (UserConfigFileWatcher target = new(testDirectory.Path))
            {
                target.FileChanged += (s, e) => mre.Set();
            }
            string nugetConfigBakPath = Path.Combine(testDirectory.Path, Settings.DefaultSettingsFileName + ".bak");
            string otherConfigPath = Path.Combine(testDirectory.Path, "other.config");
            string customConfigBak = Path.Combine(testDirectory.Path, "config", "custom.config.bak");

            // Act
            File.WriteAllText(nugetConfigBakPath, string.Empty);
            File.WriteAllText(otherConfigPath, string.Empty);
            File.WriteAllText(customConfigBak, string.Empty);
            // File system watchers are not real time, so we need to give a little time for all the async file IO to happen
            bool obtained = mre.Wait(TimeSpan.FromMilliseconds(10));

            // Assert
            obtained.Should().BeFalse();
        }

        [Fact]
        public void FileChanged_FileChangedAfterDispose_NotificationNotReceived()
        {
            // Arrange
            using TestDirectory testDirectory = TestDirectory.Create();
            ManualResetEventSlim mre = new(initialState: false, spinCount: 0);
            string configPath = Path.Combine(testDirectory.Path, Settings.DefaultSettingsFileName);

            // Act
            using (UserConfigFileWatcher target = new(testDirectory.Path))
            {
                target.FileChanged += (s, e) => mre.Set();
            }
            File.WriteAllText(configPath, string.Empty);
            // File system watchers are not real time, so we need to give a little time for all the async file IO to happen
            bool obtained = mre.Wait(TimeSpan.FromMilliseconds(10));

            // Assert
            obtained.Should().BeFalse();
        }

        [Fact]
        public void Delete_ConfigDirectory_Fails()
        {
            // Arrange
            using TestDirectory testDirectory = TestDirectory.Create();
            using (UserConfigFileWatcher target = new(testDirectory.Path))
            {
                // Act & Assert
                Assert.ThrowsAny<Exception>(() => Directory.Delete(testDirectory.Path, recursive: true));
            }
        }

        // Simulate two instances of Visual Studio being open at the same time by having two instances of the class at the same time.
        [Fact]
        public void ctor_TwoInstances_DoesNotFail()
        {
            // Arrange
            using TestDirectory testDirectory = TestDirectory.Create();

            // Act & Assert (does not throw)
            using UserConfigFileWatcher target1 = new(testDirectory.Path);
            using UserConfigFileWatcher target2 = new(testDirectory.Path);
        }
    }
}
