// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class PluginInstanceLogMessageTests : LogMessageTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData(7)]
        public void ToString_ReturnsJson(int? expectedProcessId)
        {
            const string expectedPluginId = "a";
            const PluginState expectedState = PluginState.Started;

            var now = DateTimeOffset.UtcNow;

            var logMessage = new PluginInstanceLogMessage(now, expectedPluginId, expectedState, expectedProcessId);

            var message = VerifyOuterMessageAndReturnInnerMessage(logMessage, now, "plugin instance");

            var expectedCount = 2 + (expectedProcessId.HasValue ? 1 : 0);

            Assert.Equal(expectedCount, message.Count);

            var actualPluginId = message.Value<string>("plugin ID");
            var actualState = Enum.Parse(typeof(PluginState), message.Value<string>("state"));

            Assert.Equal(expectedPluginId, actualPluginId);
            Assert.Equal(expectedState, actualState);

            if (expectedProcessId.HasValue)
            {
                var actualProcessId = message.Value<int>("process ID");

                Assert.Equal(expectedProcessId, actualProcessId);
            }
            else
            {
                Assert.Null(message["process ID"]);
            }
        }
    }
}
