// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Newtonsoft.Json;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    using SemanticVersion = Versioning.SemanticVersion;

    public class MessageTests
    {
        [Fact]
        public void Constructor_InitializesProperties()
        {
            var payload = new { E = 7 };
            var message = MessageUtilities.Create("a", MessageType.Request, MessageMethod.None, payload);

            Assert.Equal("a", message.RequestId);
            Assert.Equal(MessageType.Request, message.Type);
            Assert.Equal(MessageMethod.None, message.Method);
            Assert.Equal("{\"E\":7}", MessageUtilities.SerializePayload(message));
        }

        [Fact]
        public void JsonSerialization_ReturnsCorrectJson()
        {
            var version = new SemanticVersion(major: 1, minor: 2, patch: 3);
            var request = new HandshakeRequest(protocolVersion: version, minimumProtocolVersion: version);
            var message = MessageUtilities.Create(
                requestId: "a",
                type: MessageType.Request,
                method: MessageMethod.None,
                payload: request);

            var actualJson = TestUtilities.Serialize(message);
            var expectedJson = "{\"RequestId\":\"a\",\"Type\":\"Request\",\"Method\":\"None\",\"Payload\":{\"ProtocolVersion\":\"1.2.3\",\"MinimumProtocolVersion\":\"1.2.3\"}}";

            Assert.Equal(expectedJson, actualJson);
        }

        [Fact]
        public void JsonDeserialization_ReturnsCorrectObject()
        {
            var json = "{\"RequestId\":\"a\",\"Type\":\"Request\",\"Method\":\"None\",\"Payload\":{\"ProtocolVersion\":\"c\",\"MinimumProtocolVersion\":\"d\"}}";

            var message = JsonSerializationUtilities.Deserialize<Message>(json);

            Assert.NotNull(message);
            Assert.Equal("a", message.RequestId);
            Assert.Equal(MessageType.Request, message.Type);
            Assert.Equal(MessageMethod.None, message.Method);
            Assert.Equal("{\"ProtocolVersion\":\"c\",\"MinimumProtocolVersion\":\"d\"}", MessageUtilities.SerializePayload(message));
        }

        [Theory]
        [InlineData("{\"Type\":\"Request\",\"Method\":\"None\",\"Payload\":{\"E\":1}}")]
        [InlineData("{\"RequestId\":null,\"Type\":\"Request\",\"Method\":\"None\",\"Payload\":{\"E\":1}}")]
        [InlineData("{\"RequestId\":\"\",\"Type\":\"Request\",\"Method\":\"None\",\"Payload\":{\"E\":1}}")]
        public void JsonDeserialization_ThrowsForNullOrEmptyRequestId(string json)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => JsonSerializationUtilities.Deserialize<Message>(json));

            Assert.Equal("requestId", exception.ParamName);
        }

        [Theory]
        [InlineData("{\"RequestId\":\"a\",\"Method\":\"None\"}")]
        [InlineData("{\"RequestId\":\"a\",\"Type\":null,\"Method\":\"None\"}")]
        [InlineData("{\"RequestId\":\"a\",\"Type\":\"\",\"Method\":\"None\"}")]
        [InlineData("{\"RequestId\":\"a\",\"Type\":\"abc\",\"Method\":\"None\"}")]
        public void JsonDeserialization_ThrowsForUndefinedType(string json)
        {
            Assert.Throws<JsonSerializationException>(() => JsonSerializationUtilities.Deserialize<Message>(json));
        }

        [Theory]
        [InlineData("{\"RequestId\":\"a\",\"Type\":\"Request\"}")]
        [InlineData("{\"RequestId\":\"a\",\"Type\":\"Request\",\"Method\":null}")]
        [InlineData("{\"RequestId\":\"a\",\"Type\":\"Request\",\"Method\":\"\"}")]
        [InlineData("{\"RequestId\":\"a\",\"Type\":\"Request\",\"Method\":\"abc\"}")]
        public void JsonDeserialization_ThrowsForUndefinedMethod(string json)
        {
            Assert.Throws<JsonSerializationException>(() => JsonSerializationUtilities.Deserialize<Message>(json));
        }

        [Fact]
        public void JsonDeserialization_ThrowsForPayloadParseError()
        {
            var json = "{\"RequestId\":\"a\",\"Type\":\"Request\",\"Method\":\"None\",\"Payload\":{\"E\":}}";

            Assert.Throws<JsonReaderException>(() => JsonSerializationUtilities.Deserialize<Message>(json));
        }
    }
}
