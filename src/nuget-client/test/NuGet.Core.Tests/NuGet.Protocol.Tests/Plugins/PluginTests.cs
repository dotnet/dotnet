// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Threading;
using Moq;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class PluginTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_ThrowsForNullOrEmptyFilePath(string filePath)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new Plugin(
                    filePath,
                    Mock.Of<IConnection>(),
                    Mock.Of<IPluginProcess>(),
                    isOwnProcess: false,
                    idleTimeout: Timeout.InfiniteTimeSpan));

            Assert.Equal("filePath", exception.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsForNullConnection()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new Plugin(
                    filePath: "a",
                    connection: null,
                    process: Mock.Of<IPluginProcess>(),
                    isOwnProcess: false,
                    idleTimeout: Timeout.InfiniteTimeSpan));

            Assert.Equal("connection", exception.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsForNullProcess()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new Plugin(
                    filePath: "a",
                    connection: Mock.Of<IConnection>(),
                    process: null,
                    isOwnProcess: false,
                    idleTimeout: Timeout.InfiniteTimeSpan));

            Assert.Equal("process", exception.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsForTooSmallIdleTimeout()
        {
            var idleTimeout = TimeSpan.FromMilliseconds(Timeout.InfiniteTimeSpan.TotalMilliseconds - 1);
            var exception = Assert.Throws<ArgumentOutOfRangeException>(
                () => new Plugin(
                    filePath: "a",
                    connection: Mock.Of<IConnection>(),
                    process: Mock.Of<IPluginProcess>(),
                    isOwnProcess: false,
                    idleTimeout: idleTimeout));

            Assert.Equal("idleTimeout", exception.ParamName);
            Assert.Equal(idleTimeout, exception.ActualValue);
        }

        [Fact]
        public void Constructor_InitializesProperties()
        {
            var filePath = Path.Combine(".", "a", "b", "c.d");
            var connection = Mock.Of<IConnection>();
            var isOwnProcess = false;
            var idleTimeout = Timeout.InfiniteTimeSpan;

            using (var plugin = new Plugin(filePath, connection, Mock.Of<IPluginProcess>(), isOwnProcess, idleTimeout))
            {
                Assert.Same(connection, plugin.Connection);
                Assert.Equal(filePath, plugin.FilePath);
                Assert.Equal("c", plugin.Name);
            }
        }

        [Fact]
        public void Close_FiresBeforeCloseThenClosedEvents()
        {
            var filePath = Path.Combine(".", "a");
            var connection = Mock.Of<IConnection>();
            var isOwnProcess = false;
            var idleTimeout = Timeout.InfiniteTimeSpan;

            using (var plugin = new Plugin(filePath, connection, Mock.Of<IPluginProcess>(), isOwnProcess, idleTimeout))
            {
                var wasBeforeCloseFired = false;
                var wasClosedFired = false;

                plugin.BeforeClose += (sender, args) =>
                {
                    wasBeforeCloseFired = true;

                    Assert.False(wasClosedFired);
                };

                plugin.Closed += (sender, args) => wasClosedFired = true;

                plugin.Close();

                Assert.True(wasBeforeCloseFired);
                Assert.True(wasClosedFired);
            }
        }

        [Fact]
        public void Close_HandlesExceptionsInEventHandler()
        {
            var filePath = Path.Combine(".", "a");
            var connection = Mock.Of<IConnection>();
            var isOwnProcess = false;
            var idleTimeout = Timeout.InfiniteTimeSpan;

            using (var plugin = new Plugin(filePath, connection, Mock.Of<IPluginProcess>(), isOwnProcess, idleTimeout))
            {
                plugin.BeforeClose += (sender, args) => throw new Exception("thrown in event handler");
                plugin.Closed += (sender, args) => throw new Exception("thrown in event handler");

                plugin.Close();
            }
        }

        [Fact]
        public void Close_ClosesConnection()
        {
            var filePath = Path.Combine(".", "a");
            var connection = new Mock<IConnection>(MockBehavior.Strict);
            var isOwnProcess = false;
            var idleTimeout = Timeout.InfiniteTimeSpan;

            connection.Setup(x => x.Close());

            using (var plugin = new Plugin(
                filePath,
                connection.Object,
                Mock.Of<IPluginProcess>(),
                isOwnProcess,
                idleTimeout))
            {
                connection.Verify();

                connection.Setup(x => x.Dispose());
            }
        }

        [Fact]
        public void Close_IsIdempotent()
        {
            var filePath = Path.Combine(".", "a");
            var connection = Mock.Of<IConnection>();
            var isOwnProcess = false;
            var idleTimeout = Timeout.InfiniteTimeSpan;

            using (var plugin = new Plugin(filePath, connection, Mock.Of<IPluginProcess>(), isOwnProcess, idleTimeout))
            {
                var beforeCloseEventCount = 0;
                var closedEventCount = 0;

                plugin.BeforeClose += (sender, args) => ++beforeCloseEventCount;
                plugin.Closed += (sender, args) => ++closedEventCount;

                plugin.Close();
                plugin.Close();

                Assert.Equal(1, beforeCloseEventCount);
                Assert.Equal(1, closedEventCount);
            }
        }

        [Fact]
        public void Dispose_DisposesResources()
        {
            var connection = new Mock<IConnection>(MockBehavior.Strict);
            var process = new Mock<IPluginProcess>(MockBehavior.Strict);

            connection.Setup(x => x.Dispose());
            connection.Setup(x => x.Close());
            process.Setup(x => x.Kill());
            process.Setup(x => x.Dispose());

            using (var plugin = new Plugin(
                filePath: @"C:\a\b\c.d",
                connection: connection.Object,
                process: process.Object,
                isOwnProcess: false,
                idleTimeout: Timeout.InfiniteTimeSpan))
            {
            }

            connection.Verify(x => x.Dispose(), Times.Once);
            connection.Verify(x => x.Close(), Times.Once);
            process.Verify(x => x.Kill(), Times.Once);
            process.Verify(x => x.Dispose(), Times.Once);
        }

        [Fact]
        public void Dispose_CallsClose()
        {
            var filePath = Path.Combine(".", "a");
            var connection = Mock.Of<IConnection>();
            var isOwnProcess = false;
            var idleTimeout = Timeout.InfiniteTimeSpan;
            var wasBeforeCloseFired = false;
            var wasClosedFired = false;

            using (var plugin = new Plugin(filePath, connection, Mock.Of<IPluginProcess>(), isOwnProcess, idleTimeout))
            {
                plugin.BeforeClose += (sender, args) => wasBeforeCloseFired = true;
                plugin.Closed += (sender, args) => wasClosedFired = true;
            }

            Assert.True(wasBeforeCloseFired);
            Assert.True(wasClosedFired);
        }

        [Fact]
        public void Dispose_IsIdempotent()
        {
            using (var plugin = new Plugin(
                filePath: @"C:\a\b\c.d",
                connection: Mock.Of<IConnection>(),
                process: Mock.Of<IPluginProcess>(),
                isOwnProcess: false,
                idleTimeout: Timeout.InfiniteTimeSpan))
            {
                plugin.Dispose();
                plugin.Dispose();
            }
        }

        [Fact]
        public void Exited_RaisedOnProcessExited()
        {
            var process = new Mock<IPluginProcess>();

            using (var exitedEvent = new ManualResetEventSlim(initialState: false))
            using (var plugin = new Plugin(
                filePath: @"C:\a\b\c.d",
                connection: Mock.Of<IConnection>(),
                process: process.Object,
                isOwnProcess: false,
                idleTimeout: Timeout.InfiniteTimeSpan))
            {
                PluginEventArgs args = null;

                plugin.Exited += (object sender, PluginEventArgs e) =>
                {
                    args = e;

                    exitedEvent.Set();
                };

                process.Raise(x => x.Exited += null, process.Object, process.Object);

                exitedEvent.Wait();

                Assert.NotNull(args);
                Assert.Same(plugin, args.Plugin);
            }
        }

        [Fact]
        public void Fault_RaisedOnIdleTimeout()
        {
            var connection = new Mock<IConnection>();

            using (var faultedEvent = new ManualResetEventSlim(initialState: false))
            using (var plugin = new Plugin(
                filePath: @"C:\a\b\c.d",
                connection: connection.Object,
                process: Mock.Of<IPluginProcess>(),
                isOwnProcess: false,
                idleTimeout: Timeout.InfiniteTimeSpan))
            {
                FaultedPluginEventArgs args = null;

                plugin.Faulted += (object sender, FaultedPluginEventArgs e) =>
                {
                    args = e;

                    faultedEvent.Set();
                };

                var protocolErrorArgs = new ProtocolErrorEventArgs(new ProtocolException("test"));

                connection.Raise(x => x.Faulted += null, protocolErrorArgs);

                faultedEvent.Wait();

                Assert.NotNull(args);
                Assert.Same(plugin, args.Plugin);
                Assert.Same(protocolErrorArgs.Exception, args.Exception);
            }
        }

        [Fact]
        public void Idle_RaisedOnIdleTimeout()
        {
            using (var idleEvent = new ManualResetEventSlim(initialState: false))
            using (var plugin = new Plugin(
                filePath: @"C:\a\b\c.d",
                connection: Mock.Of<IConnection>(),
                process: Mock.Of<IPluginProcess>(),
                isOwnProcess: false,
                idleTimeout: TimeSpan.FromSeconds(1)))
            {
                var wasFired = false;

                plugin.Idle += (object sender, PluginEventArgs e) =>
                {
                    wasFired = true;

                    idleEvent.Set();
                };

                idleEvent.Wait();

                Assert.True(wasFired);
            }
        }
    }
}
