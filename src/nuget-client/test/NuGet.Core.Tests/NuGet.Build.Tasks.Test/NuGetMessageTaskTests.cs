// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using FluentAssertions;
using NuGet.Common;
using Xunit;

namespace NuGet.Build.Tasks.Test
{
    public class NuGetMessageTaskTests
    {
        [Fact]
        public void Execute_WithArgs_LogsMessageWithCorrectReplacements()
        {
            var buildEngine = new TestBuildEngine();
            var testLogger = buildEngine.TestLogger;

            var task = new NuGetMessageTask
            {
                BuildEngine = buildEngine,
                Name = nameof(TestStrings.Execute_WithArgs_LogsMessageWithCorrectReplacements),
                Args = new[] { "One", "two" }
            };

            task.Log.TaskResources = TestStrings.ResourceManager;

            var result = task.Execute();

            result.Should().BeTrue();

            var message = testLogger.LogMessages.Should().ContainSingle().Which;

            message.Level.Should().Be(LogLevel.Information);
            message.Message.Should().Be("This is One two");
        }

        [Theory]
        [InlineData("high", LogLevel.Minimal)]
        [InlineData("normal", LogLevel.Information)]
        [InlineData("low", LogLevel.Debug)]
        [InlineData("invalid", LogLevel.Information)]
        public void Execute_WithImportance_LogsMessageWithCorrectLogLevel(string importance, LogLevel expectedLogLevel)
        {
            var buildEngine = new TestBuildEngine();
            var testLogger = buildEngine.TestLogger;

            var task = new NuGetMessageTask
            {
                BuildEngine = buildEngine,
                Name = nameof(TestStrings.Execute_WithImportance_LogsMessageWithCorrectLogLevel),
                Importance = importance
            };

            task.Log.TaskResources = TestStrings.ResourceManager;

            var result = task.Execute();

            result.Should().BeTrue();

            var message = testLogger.LogMessages.Should().ContainSingle().Which;

            message.Level.Should().Be(expectedLogLevel);
            message.Message.Should().Be(TestStrings.Execute_WithImportance_LogsMessageWithCorrectLogLevel);
        }
    }
}
