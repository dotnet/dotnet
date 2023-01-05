// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using FluentAssertions;
using NuGet.PackageManagement.VisualStudio.Utility.FileWatchers;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.PackageManagement.VisualStudio.Test.Utility.FileWatchers
{
    public class FileWatcherFactoryTests
    {
        [Fact]
        public void CreateUserConfigFileWatcher_ReturnsNotNull()
        {
            // Arrange
            FileWatcherFactory target = new();

            // Act
            using IFileWatcher result = target.CreateUserConfigFileWatcher();

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void CreateSolutionConfigFileWatcher_ReturnsNotNull()
        {
            // Arrange
            using TestDirectory testDirectory = TestDirectory.Create();
            FileWatcherFactory target = new();

            // Act
            using IFileWatcher result = target.CreateSolutionConfigFileWatcher(testDirectory.Path);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
