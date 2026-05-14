// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class ThreadPoolLogMessageTests : LogMessageTests
    {
        [Fact]
        public void ToString_ReturnsJson()
        {
            var now = DateTimeOffset.UtcNow;

            var logMessage = new ThreadPoolLogMessage(now);

            var message = VerifyOuterMessageAndReturnInnerMessage(logMessage, now, "thread pool");

            Assert.Equal(4, message.Count);

            var actualWorkerThreadMinimumCount = message.Value<int>("worker thread minimum count");
            var actualWorkerThreadMaximumCount = message.Value<int>("worker thread maximum count");
            var actualCompletionPortThreadMinimumCount = message.Value<int>("completion port thread minimum count");
            var actualCompletionPortThreadMaximumCount = message.Value<int>("completion port thread maximum count");

            GetExpectedValues(
                out var expectedWorkerThreadMinimumCount,
                out var expectedWorkerThreadMaximumCount,
                out var expectedCompletionPortThreadMinimumCount,
                out var expectedCompletionPortThreadMaximumCount);

            Assert.Equal(expectedWorkerThreadMinimumCount, actualWorkerThreadMinimumCount);
            Assert.Equal(expectedWorkerThreadMaximumCount, actualWorkerThreadMaximumCount);
            Assert.Equal(expectedCompletionPortThreadMinimumCount, actualCompletionPortThreadMinimumCount);
            Assert.Equal(expectedCompletionPortThreadMaximumCount, actualCompletionPortThreadMaximumCount);
        }

        private void GetExpectedValues(
            out int expectedWorkerThreadMinimumCount,
            out int expectedWorkerThreadMaximumCount,
            out int expectedCompletionPortThreadMinimumCount,
            out int expectedCompletionPortThreadMaximumCount)
        {
            ThreadPool.GetMinThreads(out expectedWorkerThreadMinimumCount, out expectedCompletionPortThreadMinimumCount);
            ThreadPool.GetMaxThreads(out expectedWorkerThreadMaximumCount, out expectedCompletionPortThreadMaximumCount);
        }
    }
}
