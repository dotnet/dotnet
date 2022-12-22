// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class MachineLogMessageTests : LogMessageTests
    {
        [Fact]
        public void ToString_ReturnsJson()
        {
            var now = DateTimeOffset.UtcNow;

            var logMessage = new MachineLogMessage(now);

            var message = VerifyOuterMessageAndReturnInnerMessage(logMessage, now, "machine");

            Assert.Equal(1, message.Count);

            var actualLogicalProcessorCount = message.Value<int>("logical processor count");

            Assert.Equal(Environment.ProcessorCount, actualLogicalProcessorCount);
        }
    }
}
