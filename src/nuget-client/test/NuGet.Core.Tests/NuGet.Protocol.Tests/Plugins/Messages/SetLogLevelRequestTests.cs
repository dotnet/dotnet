// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Newtonsoft.Json;
using NuGet.Common;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class SetLogLevelRequestTests
    {
        [Fact]
        public void Constructor_ThrowsForUndefinedLogLevel()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new SetLogLevelRequest((LogLevel)int.MaxValue));

            Assert.Equal("logLevel", exception.ParamName);
        }

        [Fact]
        public void Constructor_InitializesLogLevelProperty()
        {
            var request = new SetLogLevelRequest(LogLevel.Minimal);

            Assert.Equal(LogLevel.Minimal, request.LogLevel);
        }

        [Fact]
        public void JsonSerialization_ReturnsCorrectJson()
        {
            var request = new SetLogLevelRequest(LogLevel.Debug);

            var actualJson = TestUtilities.Serialize(request);
            var expectedJson = "{\"LogLevel\":\"Debug\"}";

            Assert.Equal(expectedJson, actualJson);
        }

        [Fact]
        public void JsonDeserialization_ReturnsCorrectObject()
        {
            var json = "{\"LogLevel\":\"Error\"}";

            var request = JsonSerializationUtilities.Deserialize<SetLogLevelRequest>(json);

            Assert.NotNull(request);
            Assert.Equal(LogLevel.Error, request.LogLevel);
        }

        [Theory]
        [InlineData("{}")]
        [InlineData("{\"LogLevel\":null}")]
        [InlineData("{\"LogLevel\":\"\"}")]
        [InlineData("{\"LogLevel\":\"abc\"}")]
        public void JsonDeserialization_ThrowsForInvalidLogLevel(string json)
        {
            Assert.Throws<JsonSerializationException>(
                () => JsonSerializationUtilities.Deserialize<SetLogLevelRequest>(json));
        }
    }
}
