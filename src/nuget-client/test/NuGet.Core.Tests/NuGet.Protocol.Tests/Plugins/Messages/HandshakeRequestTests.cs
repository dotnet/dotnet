// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    using SemanticVersion = Versioning.SemanticVersion;

    public class HandshakeRequestTests
    {
        private static readonly SemanticVersion _version1_0_0 = new SemanticVersion(major: 1, minor: 0, patch: 0);
        private static readonly SemanticVersion _version2_0_0 = new SemanticVersion(major: 2, minor: 0, patch: 0);

        [Fact]
        public void Constructor_ThrowsForNullProtocolVersion()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new HandshakeRequest(protocolVersion: null, minimumProtocolVersion: _version1_0_0));

            Assert.Equal("protocolVersion", exception.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsForNullMinimumProtocolVersion()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new HandshakeRequest(_version1_0_0, minimumProtocolVersion: null));

            Assert.Equal("minimumProtocolVersion", exception.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsForInvalidVersionRange()
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(
                () => new HandshakeRequest(_version1_0_0, _version2_0_0));

            Assert.Equal("protocolVersion", exception.ParamName);
            Assert.Equal(_version1_0_0, exception.ActualValue);
        }

        [Fact]
        public void Constructor_InitializesProperties()
        {
            var request = new HandshakeRequest(_version2_0_0, _version1_0_0);

            Assert.Equal(_version2_0_0, request.ProtocolVersion);
            Assert.Equal(_version1_0_0, request.MinimumProtocolVersion);
        }

        [Fact]
        public void JsonSerialization_ReturnsCorrectJson()
        {
            var request = new HandshakeRequest(_version2_0_0, _version1_0_0);

            var json = TestUtilities.Serialize(request);

            Assert.Equal("{\"ProtocolVersion\":\"2.0.0\",\"MinimumProtocolVersion\":\"1.0.0\"}", json);
        }

        [Fact]
        public void JsonDeserialization_ReturnsCorrectObject()
        {
            var json = "{\"ProtocolVersion\":\"2.0.0\",\"MinimumProtocolVersion\":\"1.0.0\"}";

            var request = JsonSerializationUtilities.Deserialize<HandshakeRequest>(json);

            Assert.Equal(_version2_0_0, request.ProtocolVersion);
            Assert.Equal(_version1_0_0, request.MinimumProtocolVersion);
        }

        [Theory]
        [InlineData("{\"MinimumProtocolVersion\":\"1.0.0\"}", "protocolVersion")]
        [InlineData("{\"ProtocolVersion\":null,\"MinimumProtocolVersion\":\"1.0.0\"}", "protocolVersion")]
        [InlineData("{\"ProtocolVersion\":\"1.0.0\"}", "minimumProtocolVersion")]
        [InlineData("{\"ProtocolVersion\":\"1.0.0\",\"MinimumProtocolVersion\":null}", "minimumProtocolVersion")]
        public void JsonDeserialization_ThrowsForNullVersion(string json, string parameterName)
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => JsonSerializationUtilities.Deserialize<HandshakeRequest>(json));

            Assert.Equal(parameterName, exception.ParamName);
        }

        [Theory]
        [InlineData("{\"ProtocolVersion\":\"\",\"MinimumProtocolVersion\":\"1.0.0\"}")]
        [InlineData("{\"ProtocolVersion\":\" \",\"MinimumProtocolVersion\":\"1.0.0\"}")]
        [InlineData("{\"ProtocolVersion\":\"a\",\"MinimumProtocolVersion\":\"1.0.0\"}")]
        [InlineData("{\"ProtocolVersion\":3,\"MinimumProtocolVersion\":\"1.0.0\"}")]
        [InlineData("{\"ProtocolVersion\":false,\"MinimumProtocolVersion\":\"1.0.0\"}")]
        [InlineData("{\"ProtocolVersion\":\"1.0.0\",\"MinimumProtocolVersion\":\"\"}")]
        [InlineData("{\"ProtocolVersion\":\"1.0.0\",\"MinimumProtocolVersion\":\" \"}")]
        [InlineData("{\"ProtocolVersion\":\"1.0.0\",\"MinimumProtocolVersion\":\"a\"}")]
        [InlineData("{\"ProtocolVersion\":\"1.0.0\",\"MinimumProtocolVersion\":3,}")]
        [InlineData("{\"ProtocolVersion\":\"1.0.0\",\"MinimumProtocolVersion\":false}")]
        public void JsonDeserialization_ThrowsForInvalidVersion(string json)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => JsonSerializationUtilities.Deserialize<HandshakeRequest>(json));

            Assert.Equal("value", exception.ParamName);
        }
    }
}
