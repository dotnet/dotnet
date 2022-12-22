// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class CloseRequestHandlerTests
    {
        [Fact]
        public void Constructor_ThrowsForNullPlugin()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new CloseRequestHandler(plugin: null));

            Assert.Equal("plugin", exception.ParamName);
        }

        [Fact]
        public void CancellationToken_IsNone()
        {
            var handler = new CloseRequestHandler(Mock.Of<IPlugin>());

            Assert.Equal(CancellationToken.None, handler.CancellationToken);
        }

        [Fact]
        public void Dispose_DisposesDisposables()
        {
            using (var test = new CloseRequestHandlerTest())
            {
            }
        }

        [Fact]
        public void Dispose_IsIdempotent()
        {
            using (var test = new CloseRequestHandlerTest())
            {
                test.Handler.Dispose();
                test.Handler.Dispose();
            }
        }

        [Fact]
        public async Task HandleResponseAsync_ThrowsForNullConnection()
        {
            using (var test = new CloseRequestHandlerTest())
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => test.Handler.HandleResponseAsync(
                        connection: null,
                        request: new Message(
                            requestId: "a",
                            type: MessageType.Request,
                            method: MessageMethod.Close,
                            payload: null),
                        responseHandler: Mock.Of<IResponseHandler>(),
                        cancellationToken: CancellationToken.None));

                Assert.Equal("connection", exception.ParamName);
            }
        }

        [Fact]
        public async Task HandleResponseAsync_ThrowsForNullRequest()
        {
            using (var test = new CloseRequestHandlerTest())
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => test.Handler.HandleResponseAsync(
                        Mock.Of<IConnection>(),
                        request: null,
                        responseHandler: Mock.Of<IResponseHandler>(),
                        cancellationToken: CancellationToken.None));

                Assert.Equal("request", exception.ParamName);
            }
        }

        [Fact]
        public async Task HandleResponseAsync_ThrowsForNullResponseHandler()
        {
            using (var test = new CloseRequestHandlerTest())
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => test.Handler.HandleResponseAsync(
                        Mock.Of<IConnection>(),
                        new Message(
                            requestId: "a",
                            type: MessageType.Request,
                            method: MessageMethod.Close,
                            payload: null),
                        responseHandler: null,
                        cancellationToken: CancellationToken.None));

                Assert.Equal("responseHandler", exception.ParamName);
            }
        }

        [Fact]
        public async Task HandleResponseAsync_ThrowsIfCancelled()
        {
            using (var test = new CloseRequestHandlerTest())
            {
                await Assert.ThrowsAsync<OperationCanceledException>(
                    () => test.Handler.HandleResponseAsync(
                        Mock.Of<IConnection>(),
                        new Message(
                            requestId: "a",
                            type: MessageType.Request,
                            method: MessageMethod.Close,
                            payload: null),
                        Mock.Of<IResponseHandler>(),
                        new CancellationToken(canceled: true)));
            }
        }

        [Fact]
        public async Task HandleResponseAsync_ClosesPlugin()
        {
            using (var test = new CloseRequestHandlerTest())
            {
                test.Plugin.Setup(x => x.Close());

                await test.Handler.HandleResponseAsync(
                    Mock.Of<IConnection>(),
                    new Message(
                        requestId: "a",
                        type: MessageType.Request,
                        method: MessageMethod.Close,
                        payload: null),
                    Mock.Of<IResponseHandler>(),
                    CancellationToken.None);

                test.Plugin.Verify(x => x.Close(), Times.Once);
            }
        }

        private sealed class CloseRequestHandlerTest : IDisposable
        {
            internal CloseRequestHandler Handler { get; }
            internal Mock<IPlugin> Plugin { get; }

            internal CloseRequestHandlerTest()
            {
                Plugin = new Mock<IPlugin>(MockBehavior.Strict);

                Plugin.Setup(x => x.Dispose());

                Handler = new CloseRequestHandler(Plugin.Object);
            }

            public void Dispose()
            {
                Handler.Dispose();

                GC.SuppressFinalize(this);

                Plugin.Verify(x => x.Dispose(), Times.Once);
            }
        }
    }
}
