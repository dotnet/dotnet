// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using FluentAssertions;
using NuGet.Common;
using NuGet.Packaging.Signing;
using Xunit;

namespace NuGet.Packaging.Test
{
    public class SignatureLogTests
    {
        [Fact]
        public void InformationLog_InitializesProperties()
        {
            // Arrange
            var message = "unit_test_message";

            // Act
            var log = SignatureLog.InformationLog(message);

            // Assert
            log.Message.Should().Be(message);
            log.Code.Should().Be(NuGetLogCode.Undefined);
            log.Level.Should().Be(LogLevel.Information);
        }

        [Fact]
        public void DetailedLog_InitializesProperties()
        {
            // Arrange
            var message = "unit_test_message";

            // Act
            var log = SignatureLog.DetailedLog(message);

            // Assert
            log.Message.Should().Be(message);
            log.Code.Should().Be(NuGetLogCode.Undefined);
            log.Level.Should().Be(LogLevel.Verbose);
        }

        [Fact]
        public void DebugLog_InitializesProperties()
        {
            // Arrange
            var message = "unit_test_message";

            // Act
            var log = SignatureLog.DebugLog(message);

            // Assert
            log.Message.Should().Be(message);
            log.Code.Should().Be(NuGetLogCode.Undefined);
            log.Level.Should().Be(LogLevel.Debug);
        }

        [Theory]
        [InlineData(true, NuGetLogCode.NU3000, "unit_test_message")]
        [InlineData(false, NuGetLogCode.NU3000, "unit_test_message")]
        public void Issue_InitializesProperties(bool fatal, NuGetLogCode code, string message)
        {
            // Arrange
            var expectedLevel = fatal ? LogLevel.Error : LogLevel.Warning;

            // Act
            var log = SignatureLog.Issue(fatal, code, message);

            // Assert
            log.Message.Should().Be(message);
            log.Code.Should().Be(code);
            log.Level.Should().Be(expectedLevel);
        }

        [Fact]
        public void Error_InitializesProperties()
        {
            // Arrange
            var message = "unit_test_message";
            var code = NuGetLogCode.NU3000;

            // Act
            var log = SignatureLog.Error(code, message);

            // Assert
            log.Message.Should().Be(message);
            log.Code.Should().Be(code);
            log.Level.Should().Be(LogLevel.Error);
        }

        [Fact]
        public void Equals_OtherIsNull_ReturnsFalse()
        {
            // Arrange
            var message = "unit_test_message";
            var code = NuGetLogCode.NU3000;
            var log = SignatureLog.Error(code, message);

            // Act
            var equals = log.Equals(null);

            // Assert
            equals.Should().BeFalse();
        }

        [Theory]
        [MemberData(nameof(SignatureLogCombinations))]
        public void Equals_OtherIsDifferent_ReturnsFalse(SignatureLog other)
        {
            // Arrange
            var log = SignatureLog.Error(NuGetLogCode.NU3000, "unit_test_message");
            log.ProjectPath = Guid.NewGuid().ToString();
            log.LibraryId = Guid.NewGuid().ToString();

            // Act
            var equals = log.Equals(other);

            // Assert
            equals.Should().BeFalse();
        }

        [Fact]
        public void Equals_OtherIsSame_ReturnsTrue()
        {
            // Arrange
            var log = SignatureLog.Error(NuGetLogCode.NU3000, "unit_test_message");

            // Act
            var equals = log.Equals(log);

            // Assert
            equals.Should().BeTrue();
        }

        [Fact]
        public void Equals_OtherIsEquivalent_ReturnsFalse()
        {
            // Arrange
            var log = SignatureLog.Error(NuGetLogCode.NU3000, "unit_test_message");
            log.ProjectPath = "unit_test_project_path";
            log.LibraryId = "unit_test_library_id";

            var other = SignatureLog.Error(NuGetLogCode.NU3000, "unit_test_message");
            other.ProjectPath = "unit_test_project_path";
            other.LibraryId = "unit_test_library_id";

            // Act
            var equals = log.Equals(log);

            // Assert
            equals.Should().BeTrue();
        }

        public static IEnumerable<object[]> SignatureLogCombinations()
        {
            yield return new object[]
            { SignatureLog.DebugLog(string.Empty) };
            yield return new object[]
            { SignatureLog.InformationLog(string.Empty) };
            yield return new object[]
            { SignatureLog.DetailedLog(string.Empty) };
            yield return new object[]
            { SignatureLog.Issue(false, NuGetLogCode.NU1000, string.Empty) };
            yield return new object[]
            { SignatureLog.Issue(true, NuGetLogCode.NU1000, string.Empty) };
            yield return new object[]
            { SignatureLog.Issue(true, NuGetLogCode.NU3000, string.Empty) };
            yield return GenerateSignatureLogWithMetadata();
        }

        private static object[] GenerateSignatureLogWithMetadata()
        {
            var log = SignatureLog.Issue(true, NuGetLogCode.NU3000, string.Empty);
            log.ProjectPath = Guid.NewGuid().ToString();
            log.LibraryId = Guid.NewGuid().ToString();

            return new object[] { log };
        }
    }
}
