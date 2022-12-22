// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class PluginFileTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_ThrowsForNullOrEmptyFilePath(string filePath)
        {
            var exception = Assert.Throws<ArgumentException>(() => new PluginFile(filePath, state: new Lazy<PluginFileState>(() => PluginFileState.NotFound)));

            Assert.Equal("filePath", exception.ParamName);
        }

        [Fact]
        public void Constructor_InitializesProperties()
        {
            var pluginFile = new PluginFile(filePath: "a", state: new Lazy<PluginFileState>(() => PluginFileState.Valid));

            Assert.Equal("a", pluginFile.Path);
            Assert.Equal(PluginFileState.Valid, pluginFile.State.Value);
        }
    }
}
