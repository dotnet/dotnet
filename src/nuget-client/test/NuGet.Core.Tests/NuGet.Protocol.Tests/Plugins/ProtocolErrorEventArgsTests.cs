// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class ProtocolErrorEventArgsTests
    {
        [Fact]
        public void Constructor_ThrowsForNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ProtocolErrorEventArgs(exception: null));

            Assert.Equal("exception", exception.ParamName);
        }

        [Fact]
        public void Constructor_InitializesExceptionProperty()
        {
            var exception = new DivideByZeroException();
            var args = new ProtocolErrorEventArgs(exception);

            Assert.Same(exception, args.Exception);
        }
    }
}
