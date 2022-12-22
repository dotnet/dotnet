// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Moq;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class NoOpDisposePluginTests
    {
        [Fact]
        public void Constructor_ThrowsForNullPlugin()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new NoOpDisposePlugin(plugin: null));

            Assert.Equal("plugin", exception.ParamName);
        }

        [Fact]
        public void Constructor_InitializesProperties()
        {
            var connection = Mock.Of<IConnection>();
            var plugin = new Mock<IPlugin>(MockBehavior.Strict);

            plugin.SetupGet(x => x.Connection)
                .Returns(connection);
            plugin.SetupGet(x => x.FilePath)
                .Returns("a");
            plugin.SetupGet(x => x.Id)
                .Returns("b");
            plugin.SetupGet(x => x.Name)
                .Returns("c");

            using (var noOpDisposePlugin = new NoOpDisposePlugin(plugin.Object))
            {
                Assert.Same(connection, noOpDisposePlugin.Connection);
                Assert.Equal("a", noOpDisposePlugin.FilePath);
                Assert.Equal("b", noOpDisposePlugin.Id);
                Assert.Equal("c", noOpDisposePlugin.Name);
            }

            plugin.Verify(x => x.Connection, Times.Once);
            plugin.Verify(x => x.FilePath, Times.Once);
            plugin.Verify(x => x.Id, Times.Once);
            plugin.Verify(x => x.Name, Times.Once);
        }

        [Fact]
        public void Dispose_DoesNotCallPluginDispose()
        {
            var plugin = new Mock<IPlugin>(MockBehavior.Strict);

            using (var noOpDisposePlugin = new NoOpDisposePlugin(plugin.Object))
            {
            }

            plugin.Verify();
        }

        [Fact]
        public void Close_ClosesPlugin()
        {
            var plugin = new Mock<IPlugin>(MockBehavior.Strict);

            plugin.Setup(x => x.Close());

            using (var noOpDisposePlugin = new NoOpDisposePlugin(plugin.Object))
            {
                noOpDisposePlugin.Close();

                plugin.Verify(x => x.Close(), Times.Once);
            }
        }

        [Fact]
        public void BeforeClose_Add_SubscribesToBeforeClose()
        {
            var plugin = new PluginStub();

            using (var noOpDisposePlugin = new NoOpDisposePlugin(plugin))
            {
                noOpDisposePlugin.BeforeClose += OnBeforeClose;

                Assert.Equal(1, plugin.BeforeCloseAddCallCount);
            }
        }

        [Fact]
        public void BeforeClose_Remove_UnsubscribesFromBeforeClose()
        {
            var plugin = new PluginStub();

            using (var noOpDisposePlugin = new NoOpDisposePlugin(plugin))
            {
                noOpDisposePlugin.BeforeClose -= OnBeforeClose;

                Assert.Equal(1, plugin.BeforeCloseRemoveCallCount);
            }
        }

        [Fact]
        public void Closed_Add_SubscribesToBeforeClose()
        {
            var plugin = new PluginStub();

            using (var noOpDisposePlugin = new NoOpDisposePlugin(plugin))
            {
                noOpDisposePlugin.Closed += OnClosed;

                Assert.Equal(1, plugin.ClosedAddCallCount);
            }
        }

        [Fact]
        public void Closed_Remove_UnsubscribesFromBeforeClose()
        {
            var plugin = new PluginStub();

            using (var noOpDisposePlugin = new NoOpDisposePlugin(plugin))
            {
                noOpDisposePlugin.Closed -= OnClosed;

                Assert.Equal(1, plugin.ClosedRemoveCallCount);
            }
        }

        private void OnBeforeClose(object sender, EventArgs e)
        {
        }

        private void OnClosed(object sender, EventArgs e)
        {
        }

        private sealed class PluginStub : IPlugin
        {
            public IConnection Connection => throw new NotImplementedException();
            public string FilePath => throw new NotImplementedException();
            public string Id => throw new NotImplementedException();
            public string Name => throw new NotImplementedException();

            internal int BeforeCloseAddCallCount { get; private set; }
            internal int BeforeCloseRemoveCallCount { get; private set; }
            internal int ClosedAddCallCount { get; private set; }
            internal int ClosedRemoveCallCount { get; private set; }

            public event EventHandler BeforeClose
            {
                add
                {
                    ++BeforeCloseAddCallCount;
                }
                remove
                {
                    ++BeforeCloseRemoveCallCount;
                }
            }

            public event EventHandler Closed
            {
                add
                {
                    ++ClosedAddCallCount;
                }
                remove
                {
                    ++ClosedRemoveCallCount;
                }
            }

            public void Close()
            {
                throw new NotImplementedException();
            }

            public void Dispose()
            {
                throw new NotImplementedException();
            }
        }
    }
}
