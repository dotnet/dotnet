// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class LineReadEventArgsTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("a")]
        public void Constructor_InitializesLineProperty(string line)
        {
            var args = new LineReadEventArgs(line);

            Assert.Equal(line, args.Line);
        }
    }
}
