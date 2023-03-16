// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if IS_DESKTOP
extern alias MicrosoftBuildUtilitiesv4;
#endif

using System;
using FluentAssertions;
using Microsoft.Build.Framework;
#if IS_DESKTOP
using MicrosoftBuildUtilitiesv4::Microsoft.Build.Utilities;
#else
using Microsoft.Build.Utilities;
#endif
using NuGet.Common;
using Xunit;

namespace NuGet.Build.Tasks.Test
{
    public class TaskLoggingQueueTests
    {
        [Fact]
        public void TaskLoggingQueue_Process_HandleBadMessage()
        {
            const string badMessage = "{ 'Importance': [] }";

            var buildEngine = new TestBuildEngine();

            using (var loggingQueue = new TaskLoggingQueue(new TaskLoggingHelper(buildEngine, nameof(TaskLoggingQueueTests))))
            {
                loggingQueue.Enqueue(badMessage);
            }

            buildEngine.TestLogger.DebugMessages.Should().ContainSingle()
                .Which.Should().Be(badMessage);
        }

        [Theory]
        [InlineData(nameof(ConsoleOutLogMessageType.Error), "error", LogLevel.Error)]
        [InlineData(nameof(ConsoleOutLogMessageType.Warning), "warning", LogLevel.Warning)]
        [InlineData(nameof(ConsoleOutLogMessageType.Message), "low importance message", LogLevel.Debug, MessageImportance.Low)]
        [InlineData(nameof(ConsoleOutLogMessageType.Message), "normal importance message", LogLevel.Information, MessageImportance.Normal)]
        [InlineData(nameof(ConsoleOutLogMessageType.Message), "high importance message", LogLevel.Minimal, MessageImportance.High)]
        public void TaskLoggingQueue_Process_LogsMessages(string messageType, string message, LogLevel expectedLogLevel, MessageImportance? messageImportance = null)
        {
            var expected = new ConsoleOutLogMessage
            {
                MessageType = (ConsoleOutLogMessageType)Enum.Parse(typeof(ConsoleOutLogMessageType), messageType),
                Message = message,
            };

            if (messageImportance.HasValue)
            {
                expected.Importance = (ConsoleOutLogMessage.MessageImportance)messageImportance.Value;
            }

            var buildEngine = new TestBuildEngine();

            using (var loggingQueue = new TaskLoggingQueue(new TaskLoggingHelper(buildEngine, nameof(TaskLoggingQueueTests))))
            {
                loggingQueue.Enqueue(expected.ToJson());
            }

            var actual = buildEngine.TestLogger.LogMessages.Should().ContainSingle().Which;

            actual.Message.Should().Be(expected.Message);
            actual.Level.Should().Be(expectedLogLevel);
        }

        [Fact]
        public void TaskLoggingQueue_Process_LogsFilesToEmbedInBinlog()
        {
            var message = new ConsoleOutLogEmbedInBinlog(@"/path/to/file");

            var buildEngine = new TestBuildEngine();

            var loggingQueue = new TaskLoggingQueue(new TaskLoggingHelper(buildEngine, nameof(TaskLoggingQueueTests)));

            loggingQueue.Enqueue(message.ToJson());

            loggingQueue.Dispose();

            loggingQueue.FilesToEmbedInBinlog.Should().BeEquivalentTo(new string[] { "/path/to/file" });
        }

        [Fact]
        public void TaskLoggingQueue_Process_ThrowsIfInvalidMessageType()
        {
            const string badMessage = "{ 'MessageType': 200 }";

            var buildEngine = new TestBuildEngine();

            Action act = () =>
            {
                using (var loggingQueue = new TaskLoggingQueue(new TaskLoggingHelper(buildEngine, nameof(TaskLoggingQueueTests))))
                {
                    loggingQueue.Enqueue(badMessage);
                }
            };

            act.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}
