// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class MessageEventArgsTests
    {
        [Fact]
        public void Constructor_ThrowsForNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new MessageEventArgs(message: null));

            Assert.Equal("message", exception.ParamName);
        }

        [Fact]
        public void Constructor_InitializesMessageProperty()
        {
            var message = new Message(requestId: "a", type: MessageType.Request, method: MessageMethod.None);
            var args = new MessageEventArgs(message);

            Assert.Same(message, args.Message);
        }
    }
}
