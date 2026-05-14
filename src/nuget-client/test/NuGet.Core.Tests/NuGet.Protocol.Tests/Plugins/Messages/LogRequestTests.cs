// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Newtonsoft.Json;
using NuGet.Common;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class LogRequestTests
    {
        [Fact]
        public void Constructor_ThrowsForUndefinedLogLevel()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new LogRequest((LogLevel)int.MinValue, message: "a"));

            Assert.Equal("logLevel", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_ThrowsForNullOrEmptyMessage(string message)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new LogRequest(LogLevel.Information, message));

            Assert.Equal("message", exception.ParamName);
        }

        [Fact]
        public void Constructor_InitializesProperties()
        {
            var request = new LogRequest(LogLevel.Minimal, message: "a");

            Assert.Equal(LogLevel.Minimal, request.LogLevel);
            Assert.Equal("a", request.Message);
        }

        [Fact]
        public void JsonSerialization_ReturnsCorrectJson()
        {
            var request = new LogRequest(LogLevel.Verbose, message: "a");

            var json = TestUtilities.Serialize(request);

            Assert.Equal("{\"LogLevel\":\"Verbose\",\"Message\":\"a\"}", json);
        }

        [Theory]
        [InlineData("{\"LogLevel\":\"Warning\",\"Message\":\"a\"}", "a")]
        [InlineData("{\"LogLevel\":\"Warning\",\"Message\":3}", "3")]
        public void JsonDeserialization_ReturnsCorrectObject(string json, string message)
        {
            var request = JsonSerializationUtilities.Deserialize<LogRequest>(json);

            Assert.Equal(LogLevel.Warning, request.LogLevel);
            Assert.Equal(message, request.Message);
        }

        [Theory]
        [InlineData("", typeof(ArgumentException))]
        [InlineData("{}", typeof(ArgumentException))]
        [InlineData("{\"Message\":\"a\"}", typeof(JsonSerializationException))]
        [InlineData("{\"LogLevel\":null,\"Message\":\"a\"}", typeof(JsonSerializationException))]
        [InlineData("{\"LogLevel\":\"\",\"Message\":\"a\"}", typeof(JsonSerializationException))]
        [InlineData("{\"LogLevel\":\"abc\",\"Message\":\"a\"}", typeof(JsonSerializationException))]
        [InlineData("{\"LogLevel\":\"Debug\"}", typeof(ArgumentException))]
        [InlineData("{\"LogLevel\":\"Debug\",\"Message\":null}", typeof(ArgumentException))]
        [InlineData("{\"LogLevel\":\"Debug\",\"Message\":\"\"}", typeof(ArgumentException))]
        public void JsonDeserialization_ThrowsForInvalidJson(string json, Type exceptionType)
        {
            Assert.Throws(exceptionType, () => JsonSerializationUtilities.Deserialize<LogRequest>(json));
        }
    }
}
