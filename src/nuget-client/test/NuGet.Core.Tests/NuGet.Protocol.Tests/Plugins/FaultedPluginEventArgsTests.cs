// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Moq;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class FaultedPluginEventArgsTests
    {
        [Fact]
        public void Constructor_ThrowsForNullPlugin()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new FaultedPluginEventArgs(plugin: null, exception: new Exception()));

            Assert.Equal("plugin", exception.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsForNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new FaultedPluginEventArgs(Mock.Of<IPlugin>(), exception: null));

            Assert.Equal("exception", exception.ParamName);
        }

        [Fact]
        public void Constructor_InitializesProperties()
        {
            var plugin = Mock.Of<IPlugin>();
            var exception = new Exception();
            var args = new FaultedPluginEventArgs(plugin, exception);

            Assert.Same(plugin, args.Plugin);
            Assert.Same(exception, args.Exception);
        }
    }
}
