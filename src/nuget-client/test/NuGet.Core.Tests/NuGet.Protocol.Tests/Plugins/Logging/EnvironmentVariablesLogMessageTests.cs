// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Moq;
using NuGet.Common;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class EnvironmentVariablesLogMessageTests : LogMessageTests
    {
        private readonly DateTimeOffset _now = DateTimeOffset.UtcNow;

        [Fact]
        public void ToString_WhenVariablesHaveIllegalValues_ReturnsJson()
        {
            const string expectedHandshakeTimeoutInSeconds = "a";
            const string expectedIdleTimeoutInSeconds = "b";
            const string expectedRequestTimeoutInSeconds = "c";

            var reader = new Mock<IEnvironmentVariableReader>(MockBehavior.Strict);

            reader.Setup(x => x.GetEnvironmentVariable(It.Is<string>(name => name == EnvironmentVariableConstants.HandshakeTimeout)))
                .Returns(expectedHandshakeTimeoutInSeconds);
            reader.Setup(x => x.GetEnvironmentVariable(It.Is<string>(name => name == EnvironmentVariableConstants.IdleTimeout)))
                .Returns(expectedIdleTimeoutInSeconds);
            reader.Setup(x => x.GetEnvironmentVariable(It.Is<string>(name => name == EnvironmentVariableConstants.RequestTimeout)))
                .Returns(expectedRequestTimeoutInSeconds);

            var logMessage = new EnvironmentVariablesLogMessage(_now, reader.Object);

            var message = VerifyOuterMessageAndReturnInnerMessage(logMessage, _now, "environment variables");

            Assert.Equal(0, message.Count);

            reader.VerifyAll();
        }

        [Fact]
        public void ToString_WhenNoVariablesAreDefined_ReturnsJson()
        {
            var reader = new Mock<IEnvironmentVariableReader>(MockBehavior.Strict);

            reader.Setup(x => x.GetEnvironmentVariable(It.Is<string>(name => name == EnvironmentVariableConstants.HandshakeTimeout)))
                .Returns((string)null);
            reader.Setup(x => x.GetEnvironmentVariable(It.Is<string>(name => name == EnvironmentVariableConstants.IdleTimeout)))
                .Returns((string)null);
            reader.Setup(x => x.GetEnvironmentVariable(It.Is<string>(name => name == EnvironmentVariableConstants.RequestTimeout)))
                .Returns((string)null);

            var logMessage = new EnvironmentVariablesLogMessage(_now, reader.Object);

            var message = VerifyOuterMessageAndReturnInnerMessage(logMessage, _now, "environment variables");

            Assert.Equal(0, message.Count);

            reader.VerifyAll();
        }

        [Fact]
        public void ToString_WhenAllVariablesAreDefined_ReturnsJson()
        {
            const int expectedHandshakeTimeoutInSeconds = 1;
            const int expectedIdleTimeoutInSeconds = 2;
            const int expectedRequestTimeoutInSeconds = 3;

            var reader = new Mock<IEnvironmentVariableReader>(MockBehavior.Strict);

            reader.Setup(x => x.GetEnvironmentVariable(It.Is<string>(name => name == EnvironmentVariableConstants.HandshakeTimeout)))
                .Returns(expectedHandshakeTimeoutInSeconds.ToString());
            reader.Setup(x => x.GetEnvironmentVariable(It.Is<string>(name => name == EnvironmentVariableConstants.IdleTimeout)))
                .Returns(expectedIdleTimeoutInSeconds.ToString());
            reader.Setup(x => x.GetEnvironmentVariable(It.Is<string>(name => name == EnvironmentVariableConstants.RequestTimeout)))
                .Returns(expectedRequestTimeoutInSeconds.ToString());

            var logMessage = new EnvironmentVariablesLogMessage(_now, reader.Object);

            var message = VerifyOuterMessageAndReturnInnerMessage(logMessage, _now, "environment variables");

            Assert.Equal(3, message.Count);

            var actualHandshakeTimeoutInSeconds = message.Value<int>("handshake timeout in seconds");
            var actualIdleTimeoutInSeconds = message.Value<int>("idle timeout in seconds");
            var actualRequestTimeoutInSeconds = message.Value<int>("request timeout in seconds");

            Assert.Equal(expectedHandshakeTimeoutInSeconds, actualHandshakeTimeoutInSeconds);
            Assert.Equal(expectedIdleTimeoutInSeconds, actualIdleTimeoutInSeconds);
            Assert.Equal(expectedRequestTimeoutInSeconds, actualRequestTimeoutInSeconds);

            reader.VerifyAll();
        }
    }
}
