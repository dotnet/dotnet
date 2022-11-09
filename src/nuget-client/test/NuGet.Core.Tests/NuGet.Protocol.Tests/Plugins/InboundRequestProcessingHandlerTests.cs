// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Protocol.Plugins;
using Xunit;

namespace NuGet.Protocol.Tests.Plugins
{
    public class InboundRequestProcessingHandlerTests
    {
        [Fact]
        public void Constructor_ThrowsForNullSet()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new InboundRequestProcessingHandler(fastProcessingMethods: null));

            Assert.Equal("fastProcessingMethods", exception.ParamName);
        }

        [Fact]
        public void Dispose_IsIdempotent()
        {
            using (var handler = new InboundRequestProcessingHandler(Enumerable.Empty<MessageMethod>()))
            {
                handler.Dispose();
                handler.Dispose();
            }
        }


        [Fact]
        public void Handle_NoFastProcessingMethods_ExecuteTask()
        {
            using (var handledEvent = new ManualResetEventSlim(initialState: false))
            using (var handler = new InboundRequestProcessingHandler(Enumerable.Empty<MessageMethod>()))
            {
                var executed = false;
                Func<Task> task = () => { executed = true; handledEvent.Set(); return Task.CompletedTask; };

                handler.Handle(MessageMethod.Handshake, task, CancellationToken.None);
                handledEvent.Wait();
                Assert.True(executed);
            }
        }

        [Fact]
        public void Handle_ForFastProcessingMethods_ExecuteTask()
        {
            var method = MessageMethod.Handshake;
            using (var handledEvent = new ManualResetEventSlim(initialState: false))
            using (var handler = new InboundRequestProcessingHandler(new HashSet<MessageMethod>() { method }))
            {
                var executed = false;
                Func<Task> task = () => { executed = true; handledEvent.Set(); return Task.CompletedTask; };

                handler.Handle(method, task, CancellationToken.None);
                handledEvent.Wait();
                Assert.True(executed);
            }
        }

        [Fact]
        public void Handle_ThrowsForDisposedObject()
        {
            using (var handledEvent = new ManualResetEventSlim(initialState: false))
            {
                var handler = new InboundRequestProcessingHandler(Enumerable.Empty<MessageMethod>());

                var executed = false;
                Func<Task> task = () => { executed = true; handledEvent.Set(); return Task.CompletedTask; };

                handler.Handle(MessageMethod.Handshake, task, CancellationToken.None);
                handledEvent.Wait();
                Assert.True(executed);
                handler.Dispose();

                // Act & Assert
                Assert.Throws<ObjectDisposedException>(() => handler.Handle(MessageMethod.Handshake, task, CancellationToken.None));
            }
        }
    }
}
