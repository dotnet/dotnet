// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using Moq;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Configuration.Test
{
    public class MinPublishAgeExceptionsProviderTests
    {
        [Fact]
        public void Constructor_WithNullSettings_Throws()
        {
            // Act
            var exception = Record.Exception(() => new MinPublishAgeExceptionsProvider(settings: null!));

            // Assert
            exception.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void SaveMinPublishAgeExceptions_SavesAndLoadsPatterns()
        {
            // Arrange
            using var directory = TestDirectory.Create();
            File.WriteAllText(
                Path.Combine(directory.Path, "NuGet.Config"),
                """
                <?xml version="1.0" encoding="utf-8"?>
                <configuration>
                </configuration>
                """);

            var provider = new MinPublishAgeExceptionsProvider(new Settings(directory));

            // Act
            provider.SaveMinPublishAgeExceptions(new[]
            {
                new MinPublishAgeExceptionItem { Pattern = "System.*" },
                new MinPublishAgeExceptionItem { Pattern = "Fabrikam.WebApi.Client" },
            });

            // Assert
            var reloadedProvider = new MinPublishAgeExceptionsProvider(new Settings(directory));
            reloadedProvider.GetMinPublishAgeExceptionItems()
                .Select(exception => exception.Pattern)
                .Should()
                .BeEquivalentTo("System.*", "Fabrikam.WebApi.Client");

            var config = File.ReadAllText(Path.Combine(directory.Path, "NuGet.Config"));
            config.Should().Contain("<minPublishAgeExceptions>");
            config.Should().Contain("<package pattern=\"System.*\" />");
            config.Should().Contain("<package pattern=\"Fabrikam.WebApi.Client\" />");
            config.Should().NotContain("<clear />");
        }

        [Fact]
        public void SaveMinPublishAgeExceptions_UpdatesExistingProvider()
        {
            // Arrange
            using var directory = TestDirectory.Create();
            File.WriteAllText(
                Path.Combine(directory.Path, "NuGet.Config"),
                """
                <configuration>
                    <minPublishAgeExceptions>
                        <package pattern="Legacy.*" />
                    </minPublishAgeExceptions>
                </configuration>
                """);

            var provider = new MinPublishAgeExceptionsProvider(new Settings(directory));

            // Act
            provider.SaveMinPublishAgeExceptions(new[]
            {
                new MinPublishAgeExceptionItem { Pattern = "Contoso.*" },
            });

            // Assert
            provider.GetMinPublishAgeExceptionItems()
                .Select(exception => exception.Pattern)
                .Should()
                .BeEquivalentTo("Contoso.*");
        }

        [Fact]
        public void SaveMinPublishAgeExceptions_WithUnsupportedSettings_Throws()
        {
            // Arrange
            var settings = new Mock<ISettings>();
            var provider = new MinPublishAgeExceptionsProvider(settings.Object);

            // Act
            var exception = Record.Exception(() => provider.SaveMinPublishAgeExceptions(Array.Empty<MinPublishAgeExceptionItem>()));

            // Assert
            exception.Should().BeOfType<NotSupportedException>();
        }

        [Fact]
        public void SaveMinPublishAgeExceptions_WithEmptyList_SavesEmptySection()
        {
            // Arrange
            using var directory = TestDirectory.Create();
            File.WriteAllText(
                Path.Combine(directory.Path, "NuGet.Config"),
                """
                <configuration>
                    <minPublishAgeExceptions>
                        <package pattern="System.*" />
                    </minPublishAgeExceptions>
                </configuration>
                """);

            var provider = new MinPublishAgeExceptionsProvider(new Settings(directory));

            // Act
            provider.SaveMinPublishAgeExceptions(Array.Empty<MinPublishAgeExceptionItem>());

            // Assert
            var config = File.ReadAllText(Path.Combine(directory.Path, "NuGet.Config"));
            config.Should().Contain("<minPublishAgeExceptions />");
            config.Should().NotContain("<clear />");
            provider.GetMinPublishAgeExceptionItems().Should().BeEmpty();
        }

        [Fact]
        public void RemoveMinPublishAgeExceptions_RemovesAllExceptions()
        {
            // Arrange
            using var directory = TestDirectory.Create();
            File.WriteAllText(
                Path.Combine(directory.Path, "NuGet.Config"),
                """
                <configuration>
                    <minPublishAgeExceptions>
                        <package pattern="System.*" />
                        <package pattern="Fabrikam.*" />
                    </minPublishAgeExceptions>
                </configuration>
                """);

            var provider = new MinPublishAgeExceptionsProvider(new Settings(directory));

            // Act
            provider.RemoveMinPublishAgeExceptions();

            // Assert
            provider.GetMinPublishAgeExceptionItems().Should().BeEmpty();
        }

        [Fact]
        public void SaveMinPublishAgeExceptions_ReplacesExceptionsAcrossHierarchy()
        {
            // Arrange
            using var directory = TestDirectory.Create();
            var childDirectory = Path.Combine(directory.Path, "child");
            var parentConfigPath = Path.Combine(directory.Path, Settings.DefaultSettingsFileName);
            SettingsTestUtils.CreateConfigurationFile(
                Settings.DefaultSettingsFileName,
                directory,
                """
                <configuration>
                    <minPublishAgeExceptions>
                        <package pattern="Legacy.*" />
                    </minPublishAgeExceptions>
                </configuration>
                """);
            var originalParentConfig = File.ReadAllText(parentConfigPath);
            SettingsTestUtils.CreateConfigurationFile(
                Settings.DefaultSettingsFileName,
                childDirectory,
                """
                <configuration>
                    <minPublishAgeExceptions>
                        <package pattern="Contoso.*" />
                    </minPublishAgeExceptions>
                </configuration>
                """);

            var settings = Settings.LoadDefaultSettings(childDirectory);
            var provider = new MinPublishAgeExceptionsProvider(settings);

            // Act
            provider.SaveMinPublishAgeExceptions(new[]
            {
                new MinPublishAgeExceptionItem { Pattern = "Contoso.*" },
                new MinPublishAgeExceptionItem { Pattern = "Microsoft.*" },
            });

            // Assert
            var reloadedProvider = new MinPublishAgeExceptionsProvider(
                Settings.LoadSettings(
                    childDirectory,
                    configFileName: null,
                    machineWideSettings: null,
                    loadUserWideSettings: false,
                    useTestingGlobalPath: false));
            reloadedProvider.GetMinPublishAgeExceptionItems()
                .Select(exception => exception.Pattern)
                .Should()
                .BeEquivalentTo("Contoso.*", "Microsoft.*");
            File.ReadAllText(Path.Combine(childDirectory, Settings.DefaultSettingsFileName))
                .Should()
                .NotContain("<clear />");
            File.ReadAllText(parentConfigPath).Should().Be(originalParentConfig);
        }
    }
}
