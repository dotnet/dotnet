// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class MessageUtilitiesTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Create_RequestIdTypeMethod_ThrowsForNullOrEmptyRequestId(string requestId)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => MessageUtilities.Create(requestId, MessageType.Request, MessageMethod.Handshake));

            Assert.Equal("requestId", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Create_RequestIdTypeMethodPayload_ThrowsForNullOrEmptyRequestId(string requestId)
        {
            var payload = new Payload("a", 3, true, D.F);
            var exception = Assert.Throws<ArgumentException>(
                () => MessageUtilities.Create(requestId, MessageType.Request, MessageMethod.Handshake, payload));

            Assert.Equal("requestId", exception.ParamName);
        }

        [Fact]
        public void Create_RequestIdTypeMethodPayload_ThrowsForNullPayload()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => MessageUtilities.Create<Payload>(
                    requestId: "a",
                    type: MessageType.Request,
                    method: MessageMethod.Handshake,
                    payload: null));

            Assert.Equal("payload", exception.ParamName);
        }

        [Fact]
        public void Create_RequestIdTypeMethod_InitializesProperties()
        {
            var message = MessageUtilities.Create(
                requestId: "a",
                type: MessageType.Request,
                method: MessageMethod.Handshake);

            Assert.Equal("a", message.RequestId);
            Assert.Equal(MessageType.Request, message.Type);
            Assert.Equal(MessageMethod.Handshake, message.Method);
            Assert.Null(message.Payload);
        }

        [Fact]
        public void Create_RequestIdTypeMethodPayload_InitializesProperties()
        {
            var payload = new Payload("a", 3, true, D.F);
            var message = MessageUtilities.Create(
                requestId: "a",
                type: MessageType.Request,
                method: MessageMethod.Handshake,
                payload: payload);

            Assert.Equal("a", message.RequestId);
            Assert.Equal(MessageType.Request, message.Type);
            Assert.Equal(MessageMethod.Handshake, message.Method);
            Assert.NotNull(message.Payload);
        }

        [Fact]
        public void Create_SerializesPayload()
        {
            var payload = new Payload("a", 3, true, D.F);
            var message = MessageUtilities.Create(
                requestId: "a",
                type: MessageType.Request,
                method: MessageMethod.Handshake,
                payload: payload);

            Assert.NotNull(message);
            Assert.NotNull(message.Payload);
            Assert.Equal("{\"A\":\"a\",\"B\":3,\"C\":true,\"D\":\"F\"}", message.Payload.ToString(Formatting.None));
        }

        [Fact]
        public void DeserializePayload_ThrowsForNullMessage()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => MessageUtilities.DeserializePayload<Payload>(message: null));

            Assert.Equal("message", exception.ParamName);
        }

        [Fact]
        public void DeserializePayload_SupportsNullPayload()
        {
            var message = new Message(requestId: "a", type: MessageType.Fault, method: MessageMethod.None, payload: null);

            var payload = MessageUtilities.DeserializePayload<Payload>(message);

            Assert.Null(payload);
        }

        [Fact]
        public void DeserializePayload_UsesDefaultSerializationOptions()
        {
            var payload = new Payload("a", 3, true, D.F);
            var serializedPayload = JObject.FromObject(payload);
            var message = new Message(
                requestId: "a",
                type: MessageType.Cancel,
                method: MessageMethod.None,
                payload: serializedPayload);

            var deserializedPayload = MessageUtilities.DeserializePayload<Payload>(message);

            Assert.NotNull(deserializedPayload);
            Assert.Equal(payload.A, deserializedPayload.A);
            Assert.Equal(payload.B, deserializedPayload.B);
            Assert.Equal(payload.C, deserializedPayload.C);
            Assert.Equal(payload.D, deserializedPayload.D);
        }

        private sealed class Payload
        {
            public string A { get; }
            public int B { get; }
            public bool C { get; }
            public D D { get; }

            [JsonConstructor]
            public Payload(string a, int b, bool c, D d)
            {
                A = a;
                B = b;
                C = c;
                D = d;
            }
        }

        private enum D
        {
            E,
            F
        }
    }
}
