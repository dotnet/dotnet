// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Newtonsoft.Json;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class InitializeRequestTests
    {
        private static readonly TimeSpan _requestTimeout = new TimeSpan(days: 1, hours: 2, minutes: 3, seconds: 4, milliseconds: 5);

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_ThrowsForNullOrEmptyClientVersion(string clientVersion)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new InitializeRequest(
                    clientVersion,
                    culture: "a",
                    requestTimeout: TimeSpan.MaxValue));

            Assert.Equal("clientVersion", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_ThrowsForNullOrEmptyCulture(string culture)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new InitializeRequest(
                    clientVersion: "1.0.0",
                    culture: culture,
                    requestTimeout: TimeSpan.MaxValue));

            Assert.Equal("culture", exception.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsForZeroRequestTimeout()
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(
                () => new InitializeRequest(
                    clientVersion: "a",
                    culture: "b",
                    requestTimeout: TimeSpan.Zero));

            Assert.Equal("requestTimeout", exception.ParamName);
            Assert.Equal(TimeSpan.Zero, exception.ActualValue);
        }

        [Fact]
        public void Constructor_ThrowsForNegativeRequestTimeout()
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(
                () => new InitializeRequest(
                    clientVersion: "a",
                    culture: "b",
                    requestTimeout: TimeSpan.FromSeconds(-1)));

            Assert.Equal("requestTimeout", exception.ParamName);
            Assert.Equal(TimeSpan.FromSeconds(-1), exception.ActualValue);
        }

        [Fact]
        public void Constructor_ThrowsForTooLargeRequestTimeout()
        {
            var milliseconds = int.MaxValue + 1L;

            var exception = Assert.Throws<ArgumentOutOfRangeException>(
                () => new InitializeRequest(
                    clientVersion: "a",
                    culture: "b",
                    requestTimeout: TimeSpan.FromMilliseconds(milliseconds)));

            Assert.Equal("requestTimeout", exception.ParamName);
            Assert.Equal(TimeSpan.FromMilliseconds(milliseconds), exception.ActualValue);
        }

        [Fact]
        public void Constructor_InitializesProperties()
        {
            var request = new InitializeRequest(
                clientVersion: "a",
                culture: "b",
                requestTimeout: _requestTimeout);

            Assert.Equal("a", request.ClientVersion);
            Assert.Equal("b", request.Culture);
            Assert.Equal(_requestTimeout, request.RequestTimeout);
        }

        [Fact]
        public void JsonSerialization_ReturnsCorrectJson()
        {
            var request = new InitializeRequest(
                clientVersion: "a",
                culture: "b",
                requestTimeout: _requestTimeout);

            var actualJson = TestUtilities.Serialize(request);
            var expectedJson = "{\"ClientVersion\":\"a\",\"Culture\":\"b\",\"RequestTimeout\":\"1.02:03:04.0050000\"}";

            Assert.Equal(expectedJson, actualJson);
        }

        [Fact]
        public void JsonDeserialization_ReturnsCorrectObject()
        {
            var json = "{\"ClientVersion\":\"a\",\"Culture\":\"b\",\"RequestTimeout\":\"1.02:03:04.0050000\"}";

            var request = JsonSerializationUtilities.Deserialize<InitializeRequest>(json);

            Assert.NotNull(request);
            Assert.Equal("a", request.ClientVersion);
            Assert.Equal("b", request.Culture);
            Assert.Equal(_requestTimeout, request.RequestTimeout);
        }

        [Theory]
        [InlineData("{\"Culture\":\"b\",\"RequestTimeout\":\"1.02:03:04.0050000\"}", "clientVersion")]
        [InlineData("{\"ClientVersion\":null,\"Culture\":\"b\",\"RequestTimeout\":\"1.02:03:04.0050000\"}", "clientVersion")]
        [InlineData("{\"ClientVersion\":\"\",\"Culture\":\"b\",\"RequestTimeout\":\"1.02:03:04.0050000\"}", "clientVersion")]
        [InlineData("{\"ClientVersion\":\"a\",\"RequestTimeout\":\"1.02:03:04.0050000\"}", "culture")]
        [InlineData("{\"ClientVersion\":\"a\",\"Culture\":null,\"RequestTimeout\":\"1.02:03:04.0050000\"}", "culture")]
        [InlineData("{\"ClientVersion\":\"a\",\"Culture\":\"\",\"RequestTimeout\":\"1.02:03:04.0050000\"}", "culture")]
        public void JsonDeserialization_ThrowsForNullOrEmptyStringProperties(string json, string parameterName)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => JsonSerializationUtilities.Deserialize<InitializeRequest>(json));

            Assert.Equal(parameterName, exception.ParamName);
        }

        [Theory]
        [InlineData("{\"ClientVersion\":\"a\",\"Culture\":\"b\",\"RequestTimeout\":null}")]
        [InlineData("{\"ClientVersion\":\"a\",\"Culture\":\"b\",\"RequestTimeout\":\"a\"}")]
        public void JsonDeserialization_ThrowsForInvalidRequestTimeout(string json)
        {
            Assert.Throws<JsonSerializationException>(
                () => JsonSerializationUtilities.Deserialize<InitializeRequest>(json));
        }

        [Theory]
        [InlineData("{\"ClientVersion\":\"a\",\"Culture\":\"b\"}")]
        [InlineData("{\"ClientVersion\":\"a\",\"Culture\":\"b\",\"RequestTimeout\":\"-00:01:00\"}")]
        public void JsonDeserialization_ThrowsForInvalidRequestTimeoutValue(string json)
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(
                () => JsonSerializationUtilities.Deserialize<InitializeRequest>(json));

            Assert.Equal("requestTimeout", exception.ParamName);
        }
    }
}
