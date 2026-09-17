// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using FluentAssertions;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Configuration.Test
{
    public class MinPublishAgeExceptionsTests
    {
        [Fact]
        public void GetMinPublishAgeExceptions_WithNullSettings_Throws()
        {
            // Act
            var exception = Record.Exception(() => MinPublishAgeExceptions.GetMinPublishAgeExceptions(settings: null!));

            // Assert
            exception.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void GetMinPublishAgeExceptions_WithConfiguredPatterns_LoadsPatterns()
        {
            // Arrange
            using var directory = TestDirectory.Create();
            File.WriteAllText(
                Path.Combine(directory.Path, "NuGet.Config"),
                """
                <?xml version="1.0" encoding="utf-8"?>
                <configuration>
                    <minPublishAgeExceptions>
                        <package pattern="System.*" />
                        <package pattern="Fabrikam.WebApi.Client" />
                    </minPublishAgeExceptions>
                </configuration>
                """);
            var settings = new Settings(directory);

            // Act
            var exceptions = MinPublishAgeExceptions.GetMinPublishAgeExceptions(settings);

            // Assert
            exceptions.IsEnabled.Should().BeTrue();
            exceptions.FindException("System.Text.Json")!.Pattern.Should().Be("System.*");
            exceptions.FindException("Fabrikam.WebApi.Client")!.Pattern.Should().Be("Fabrikam.WebApi.Client");
            exceptions.FindException("Newtonsoft.Json").Should().BeNull();
        }

        [Fact]
        public void FindException_WithWhitespaceInPattern_TrimsPatternForMatching()
        {
            // Arrange
            var exceptions = new MinPublishAgeExceptions(new[]
            {
                new MinPublishAgeExceptionItem { Pattern = " System.* " },
            });

            // Act
            var exception = exceptions.FindException("System.Text.Json");

            // Assert
            exception.Should().NotBeNull();
            exception!.Pattern.Should().Be(" System.* ");
        }

        [Fact]
        public void GetMinPublishAgeExceptions_WithChildConfig_ReplacesParentConfig()
        {
            // Arrange
            using var directory = TestDirectory.Create();
            var childDirectory = Path.Combine(directory.Path, "child");
            var configContents = """
                <?xml version="1.0" encoding="utf-8"?>
                <configuration>
                    <minPublishAgeExceptions>
                        <package pattern="System.*" />
                    </minPublishAgeExceptions>
                </configuration>
                """;

            SettingsTestUtils.CreateConfigurationFile("NuGet.Config", directory, configContents);

            configContents = """
                <?xml version="1.0" encoding="utf-8"?>
                <configuration>
                    <minPublishAgeExceptions>
                        <package pattern="Fabrikam.*" />
                    </minPublishAgeExceptions>
                </configuration>
                """;

            SettingsTestUtils.CreateConfigurationFile("NuGet.Config", childDirectory, configContents);
            var settings = Settings.LoadSettings(
                childDirectory,
                configFileName: null,
                machineWideSettings: null,
                loadUserWideSettings: false,
                useTestingGlobalPath: false);

            // Act
            var exceptions = MinPublishAgeExceptions.GetMinPublishAgeExceptions(settings);

            // Assert
            exceptions.FindException("Fabrikam.WebApi.Client").Should().NotBeNull();
            exceptions.FindException("System.Text.Json").Should().BeNull();
        }

        [Fact]
        public void GetMinPublishAgeExceptions_WithEmptyChildConfig_ClearsParentConfig()
        {
            // Arrange
            using var directory = TestDirectory.Create();
            var childDirectory = Path.Combine(directory.Path, "child");
            var configContents = """
                <?xml version="1.0" encoding="utf-8"?>
                <configuration>
                    <minPublishAgeExceptions>
                        <package pattern="System.*" />
                    </minPublishAgeExceptions>
                </configuration>
                """;

            SettingsTestUtils.CreateConfigurationFile("NuGet.Config", directory, configContents);

            configContents = """
                <?xml version="1.0" encoding="utf-8"?>
                <configuration>
                    <minPublishAgeExceptions />
                </configuration>
                """;

            SettingsTestUtils.CreateConfigurationFile("NuGet.Config", childDirectory, configContents);
            var settings = Settings.LoadDefaultSettings(childDirectory);

            // Act
            var exceptions = MinPublishAgeExceptions.GetMinPublishAgeExceptions(settings);

            // Assert
            exceptions.IsEnabled.Should().BeFalse();
            exceptions.FindException("System.Text.Json").Should().BeNull();
        }
    }
}
