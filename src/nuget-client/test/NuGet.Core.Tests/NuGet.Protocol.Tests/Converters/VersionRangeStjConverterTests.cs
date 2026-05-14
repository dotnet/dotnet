// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text.Json;
using FluentAssertions;
using NuGet.Protocol.Utility;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Protocol.Tests.Converters
{
    public class VersionRangeStjConverterTests
    {
        [Fact]
        public void Deserialize_ValidString_ReturnsVersionRange()
        {
            // Arrange
            const string range = "1.2.3";
            const string json = $"\"{range}\"";

            // Act
            VersionRange? actual = JsonSerializer.Deserialize(json, JsonContext.Default.VersionRange);

            // Assert
            actual.Should().NotBeNull();
            actual!.OriginalString.Should().Be(range);
        }

        [Fact]
        public void Deserialize_InvalidString_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => JsonSerializer.Deserialize<VersionRange>("\"not a range\"", JsonContext.Default.VersionRange));
        }

        [Fact]
        public void Deserialize_Null_ReturnsNull()
        {
            // Act
            var actual = JsonSerializer.Deserialize<VersionRange>("null");

            // Assert
            actual.Should().BeNull();
        }

        [Fact]
        public void Serialize_Range_ReturnsString()
        {
            // Arrange
            const string rangeString = "[1.2.3, )";
            var range = VersionRange.Parse(rangeString);

            // Act
            var actual = JsonSerializer.Serialize(range, JsonContext.Default.VersionRange);

            // Assert
            const string expected = $"\"{rangeString}\"";
            actual.Should().Be(expected);
        }

        [Fact]
        public void VersionRange_RoundTripsSerialization()
        {
            // Arrange
            var range = VersionRange.Parse("[1.0.0, 2.0.0)");

            // Act
            var json = JsonSerializer.Serialize(range, JsonContext.Default.VersionRange);
            var actual = JsonSerializer.Deserialize<VersionRange>(json, JsonContext.Default.VersionRange);

            // Assert
            actual.Should().BeEquivalentTo(range);
        }

        [Fact]
        public void Json_RoundTripsSerialization()
        {
            // Arrange
            var json = "\"[1.0.0, 2.0.0)\"";

            // Act
            var versionRange = JsonSerializer.Deserialize<VersionRange>(json, JsonContext.Default.VersionRange);
            versionRange.Should().NotBeNull();
            var actual = JsonSerializer.Serialize(versionRange, JsonContext.Default.VersionRange);

            // Assert
            actual.Should().Be(json);
        }
    }
}
