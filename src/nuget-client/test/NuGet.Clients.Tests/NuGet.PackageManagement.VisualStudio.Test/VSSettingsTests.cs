// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using FluentAssertions;
using Microsoft.VisualStudio.Sdk.TestFramework;
using Moq;
using NuGet.Configuration;
using NuGet.PackageManagement.VisualStudio.Utility.FileWatchers;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.PackageManagement.VisualStudio.Test
{
    [Collection(MockedVS.Collection)]
    public class VSSettingsTests : MockedVSCollectionTests
    {
        public VSSettingsTests(GlobalServiceProvider globalServiceProvider) : base(globalServiceProvider)
        {
        }

        [Fact]
        public void SettingsChanged_WhenUserConfigWatcherNotifies_RaisesNotification()
        {
            // Arrange
            bool received = false;
            var solutionManager = new Mock<ISolutionManager>();
            var machineWideSettings = new Mock<IMachineWideSettings>();
            var slnConfigWatcher = new Mock<IFileWatcher>();
            var userConfigWatcher = new Mock<IFileWatcher>();

            var watcherFactory = new Mock<IFileWatcherFactory>();
            watcherFactory.Setup(f => f.CreateUserConfigFileWatcher()).Returns(userConfigWatcher.Object);
            watcherFactory.Setup(f => f.CreateSolutionConfigFileWatcher(It.IsAny<string>())).Returns(userConfigWatcher.Object);

            using var target = new VSSettings(solutionManager.Object, machineWideSettings.Object, watcherFactory.Object);
            target.SettingsChanged += (_, _) => received = true;

            // Act
            userConfigWatcher.Raise(w => w.FileChanged += null, EventArgs.Empty);

            // Assert
            received.Should().BeTrue();
        }

        [Fact]
        public void SettingsChanged_WhenSolutionConfigWatcherNotifies_RaisesNotification()
        {
            // Arrange
            using TestDirectory testDirectory = TestDirectory.Create();
            bool received = false;
            var solutionManager = new Mock<ISolutionManager>();
            solutionManager.SetupGet(sm => sm.IsSolutionOpen).Returns(true);
            solutionManager.SetupGet(sm => sm.SolutionDirectory).Returns(testDirectory.Path);

            var machineWideSettings = new Mock<IMachineWideSettings>();
            var slnConfigWatcher = new Mock<IFileWatcher>();
            var userConfigWatcher = new Mock<IFileWatcher>();

            var watcherFactory = new Mock<IFileWatcherFactory>();
            watcherFactory.Setup(f => f.CreateUserConfigFileWatcher()).Returns(userConfigWatcher.Object);
            watcherFactory.Setup(f => f.CreateSolutionConfigFileWatcher(It.IsAny<string>())).Returns(slnConfigWatcher.Object);

            var target = new VSSettings(solutionManager.Object, machineWideSettings.Object, watcherFactory.Object);
            target.SettingsChanged += (_, _) => received = true;
            // The solution watcher is initialized lazily, the first time the settings are actually used.
            _ = target.GetSection("config");

            // Act
            slnConfigWatcher.Raise(w => w.FileChanged += null, EventArgs.Empty);

            // Assert
            received.Should().BeTrue();
        }
    }
}
