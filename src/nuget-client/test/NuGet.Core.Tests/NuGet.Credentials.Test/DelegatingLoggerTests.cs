// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using NuGet.Common;
using Xunit;

namespace NuGet.Credentials.Test
{
    public class DelegatingLoggerTests
    {
        [Fact]
        public void DelegateLogger_LogsAllMessages()
        {
            var catchAllLogger = new CatchAllLogger();

            var delegateLogger = new DelegatingLogger(catchAllLogger);

            var messageCount = 5;
            for (var i = 1; i <= messageCount; i++)
            {
                delegateLogger.LogInformation(i.ToString());

            }

            Assert.Equal(5, catchAllLogger.messages.Count);

            int count = 1;
            foreach (var message in catchAllLogger.messages)
            {
                Assert.Equal(count.ToString(), message);
                count++;
            }
        }

        [Fact]
        public async Task DelegateLogger_LogsAllMessagesAsync()
        {
            var catchAllLogger = new CatchAllLogger();

            var delegateLogger = new DelegatingLogger(catchAllLogger);

            var messageCount = 5;
            for (var i = 1; i <= messageCount; i++)
            {
                await delegateLogger.LogAsync(new RestoreLogMessage(LogLevel.Information, i.ToString()));
            }

            Assert.Equal(5, catchAllLogger.messages.Count);

            int count = 1;
            foreach (var message in catchAllLogger.messages)
            {
                Assert.Equal(count.ToString(), message);
                count++;
            }
        }

        [Fact]
        public async Task DelegateLogger_ReplacesAndLogsAllMessagesAsync()
        {
            // Set up
            var catchAllLogger1 = new CatchAllLogger();
            var catchAllLogger2 = new CatchAllLogger();

            var delegateLogger = new DelegatingLogger(catchAllLogger1);

            // Act
            var messageCount = 5;
            for (var i = 1; i <= messageCount; i++)
            {
                await delegateLogger.LogAsync(new RestoreLogMessage(LogLevel.Information, i.ToString()));
            }

            // Assert
            Assert.Equal(5, catchAllLogger1.messages.Count);

            int count = 1;
            foreach (var message in catchAllLogger1.messages)
            {
                Assert.Equal(count.ToString(), message);
                count++;
            }

            // Now replace and log 5 more messages.
            delegateLogger.UpdateDelegate(catchAllLogger2);

            // Set up 
            for (var i = 1; i <= messageCount; i++)
            {
                await delegateLogger.LogAsync(new RestoreLogMessage(LogLevel.Information, i.ToString()));
            }

            // Act
            Assert.Equal(5, catchAllLogger2.messages.Count);

            // Assert
            count = 1;
            foreach (var message in catchAllLogger2.messages)
            {
                Assert.Equal(count.ToString(), message);
                count++;
            }

        }
    }

    internal class CatchAllLogger : LoggerBase, ILogger
    {

        public IList<string> messages = new List<string>();

        public override void Log(ILogMessage message)
        {
            messages.Add(message.Message);
        }

        public override Task LogAsync(ILogMessage message)
        {
            Log(message);
            return Task.CompletedTask;
        }
    }
}
