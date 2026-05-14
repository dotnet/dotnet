// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Moq;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class PluginEventArgsTests
    {
        [Fact]
        public void Constructor_ThrowsForNullPlugin()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new PluginEventArgs(plugin: null));

            Assert.Equal("plugin", exception.ParamName);
        }

        [Fact]
        public void Constructor_InitializesPluginProperty()
        {
            var plugin = Mock.Of<IPlugin>();
            var args = new PluginEventArgs(plugin);

            Assert.Same(plugin, args.Plugin);
        }
    }
}
