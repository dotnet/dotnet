// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using FluentAssertions;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Configuration.Test
{
    public class MinPublishAgeExceptionItemTests
    {
        [Fact]
        public void LoadMinPublishAgeExceptions_WithoutPattern_ThrowsClearError()
        {
            // Arrange
            using var directory = TestDirectory.Create();
            const string fileName = "NuGet.Config";
            SettingsTestUtils.CreateConfigurationFile(
                fileName,
                directory,
                """
                <configuration>
                    <minPublishAgeExceptions>
                        <package />
                    </minPublishAgeExceptions>
                </configuration>
                """);

            // Act
            var exception = Record.Exception(() => new SettingsFile(directory));

            // Assert
            exception.Should().BeOfType<NuGetConfigurationException>();
            exception.Message.Should().Contain("'pattern'");
            exception.Message.Should().Contain("'package'");
            exception.Message.Should().Contain(Path.Combine(directory.Path, fileName));
        }

        [Theory]
        [InlineData(" ")]
        [InlineData("\t")]
        public void MinPublishAgeException_WhitespacePattern_Throws(string pattern)
        {
            // Arrange
            using var directory = TestDirectory.Create();
            File.WriteAllText(
                Path.Combine(directory.Path, "NuGet.Config"),
                $"""
                <configuration>
                    <minPublishAgeExceptions>
                        <package pattern="{pattern}" />
                    </minPublishAgeExceptions>
                </configuration>
                """);

            // Act
            var fileException = Record.Exception(() => new SettingsFile(directory));
            var inMemoryException = Record.Exception(() => new MinPublishAgeExceptionItem { Pattern = pattern });

            // Assert
            fileException.Should().BeOfType<NuGetConfigurationException>();
            fileException!.Message.Should().Contain(Path.Combine(directory.Path, "NuGet.Config"));
            inMemoryException.Should().BeOfType<ArgumentException>();
        }

        [Fact]
        public void MinPublishAgeException_TooLongPattern_Throws()
        {
            // Arrange
            using var directory = TestDirectory.Create();
            string pattern = new string('a', 101);
            File.WriteAllText(
                Path.Combine(directory.Path, "NuGet.Config"),
                $"""
                <configuration>
                    <minPublishAgeExceptions>
                        <package pattern="{pattern}" />
                    </minPublishAgeExceptions>
                </configuration>
                """);

            // Act
            var fileException = Record.Exception(() => new SettingsFile(directory));
            var inMemoryException = Record.Exception(() => new MinPublishAgeExceptionItem { Pattern = pattern });

            // Assert
            fileException.Should().BeOfType<NuGetConfigurationException>();
            fileException!.Message.Should().Contain(Path.Combine(directory.Path, "NuGet.Config"));
            fileException.Message.Should().Contain(pattern);
            inMemoryException.Should().BeOfType<ArgumentOutOfRangeException>();
        }
    }
}
