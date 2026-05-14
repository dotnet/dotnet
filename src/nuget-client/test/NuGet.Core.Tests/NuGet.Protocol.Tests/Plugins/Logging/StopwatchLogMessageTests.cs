// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class StopwatchLogMessageTests : LogMessageTests
    {
        [Fact]
        public void ToString_ReturnsJson()
        {
            var now = DateTimeOffset.UtcNow;
            var frequency = 7;
            var logMessage = new StopwatchLogMessage(now, frequency);

            var message = VerifyOuterMessageAndReturnInnerMessage(logMessage, now, "stopwatch");

            Assert.Equal(1, message.Count);

            var actualFrequency = message.Value<long>("frequency");

            Assert.Equal(frequency, actualFrequency);
        }
    }
}
