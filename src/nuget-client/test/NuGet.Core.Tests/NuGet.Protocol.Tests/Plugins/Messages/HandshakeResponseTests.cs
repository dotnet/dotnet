// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    using SemanticVersion = Versioning.SemanticVersion;

    public class HandshakeResponseTests
    {
        private static readonly SemanticVersion _version = new SemanticVersion(major: 1, minor: 0, patch: 0);

        [Fact]
        public void Constructor_ThrowsForUndefinedMessageResponseCode()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new HandshakeResponse((MessageResponseCode)int.MinValue, _version));

            Assert.Equal("responseCode", exception.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsForNullProtocolVersionIfResponseCodeIsSuccess()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new HandshakeResponse(MessageResponseCode.Success, protocolVersion: null));

            Assert.Equal("protocolVersion", exception.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsForNonNullProtocolVersionIfResponseCodeIsError()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new HandshakeResponse(MessageResponseCode.Error, _version));

            Assert.Equal("protocolVersion", exception.ParamName);
        }

        [Fact]
        public void Constructor_InitializesPropertiesForSuccess()
        {
            var response = new HandshakeResponse(MessageResponseCode.Success, _version);

            Assert.Equal(MessageResponseCode.Success, response.ResponseCode);
            Assert.Equal(_version, response.ProtocolVersion);
        }

        [Fact]
        public void Constructor_InitializesPropertiesForError()
        {
            var response = new HandshakeResponse(MessageResponseCode.Error, protocolVersion: null);

            Assert.Equal(MessageResponseCode.Error, response.ResponseCode);
            Assert.Null(response.ProtocolVersion);
        }

        [Fact]
        public void JsonSerialization_ReturnsCorrectJson()
        {
            var response = new HandshakeResponse(MessageResponseCode.Success, _version);

            var json = TestUtilities.Serialize(response);

            Assert.Equal("{\"ResponseCode\":\"Success\",\"ProtocolVersion\":\"1.0.0\"}", json);
        }

        [Theory]
        [InlineData("{\"ResponseCode\":\"Success\",\"ProtocolVersion\":\"1.0.0\"}", MessageResponseCode.Success, "1.0.0")]
        [InlineData("{\"ResponseCode\":\"Error\"}", MessageResponseCode.Error, null)]
        public void JsonDeserialization_ReturnsCorrectObject(
            string json,
            MessageResponseCode responseCode,
            string versionString)
        {
            var version = versionString == null ? null : SemanticVersion.Parse(versionString);

            var response = JsonSerializationUtilities.Deserialize<HandshakeResponse>(json);

            Assert.Equal(responseCode, response.ResponseCode);
            Assert.Equal(version, response.ProtocolVersion);
        }

        [Theory]
        [InlineData("{\"ResponseCode\":\"Success\"}", "protocolVersion")]
        [InlineData("{\"ResponseCode\":\"Success\",\"ProtocolVersion\":null}", "protocolVersion")]
        [InlineData("{\"ResponseCode\":\"Success\",\"ProtocolVersion\":\"\"}", "value")]
        [InlineData("{\"ResponseCode\":\"Success\",\"ProtocolVersion\":\"a\"}", "value")]
        [InlineData("{\"ResponseCode\":\"Error\",\"ProtocolVersion\":\"1.0.0\"}", "protocolVersion")]
        public void JsonDeserialization_ThrowsForInvalidProtocolVersion(string json, string parameterName)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => JsonSerializationUtilities.Deserialize<HandshakeResponse>(json));

            Assert.Equal(parameterName, exception.ParamName);
        }
    }
}
