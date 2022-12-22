// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class CommunicationLogMessageTests : LogMessageTests
    {
        [Fact]
        public void ToString_ReturnsJson()
        {
            const string requestId = "a";
            const MessageMethod method = MessageMethod.GetOperationClaims;
            const MessageType type = MessageType.Request;
            const MessageState state = MessageState.Sent;

            var now = DateTimeOffset.UtcNow;

            var logMessage = new CommunicationLogMessage(now, requestId, method, type, state);

            var message = VerifyOuterMessageAndReturnInnerMessage(logMessage, now, "communication");

            Assert.Equal(4, message.Count);

            var actualRequestId = message.Value<string>("request ID");
            var actualMethod = Enum.Parse(typeof(MessageMethod), message.Value<string>("method"));
            var actualType = Enum.Parse(typeof(MessageType), message.Value<string>("type"));
            var actualState = Enum.Parse(typeof(MessageState), message.Value<string>("state"));

            Assert.Equal(requestId, actualRequestId);
            Assert.Equal(method, actualMethod);
            Assert.Equal(type, actualType);
            Assert.Equal(state, actualState);
        }
    }
}
