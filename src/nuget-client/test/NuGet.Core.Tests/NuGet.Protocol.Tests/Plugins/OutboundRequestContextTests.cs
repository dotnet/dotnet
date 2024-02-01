// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class OutboundRequestContextTests
    {
        [Fact]
        public void Constructor_ThrowsForNullConnection()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new OutboundRequestContext<HandshakeResponse>(
                    connection: null,
                    request: new Message(
                        requestId: "a",
                        type: MessageType.Request,
                        method: MessageMethod.Handshake,
                        payload: null),
                    timeout: TimeSpan.FromMinutes(1),
                    isKeepAlive: true,
                    cancellationToken: CancellationToken.None));

            Assert.Equal("connection", exception.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsForNullRequest()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new OutboundRequestContext<HandshakeResponse>(
                    Mock.Of<IConnection>(),
                    request: null,
                    timeout: TimeSpan.FromMinutes(1),
                    isKeepAlive: true,
                    cancellationToken: CancellationToken.None));

            Assert.Equal("request", exception.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsForNullLogger()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new OutboundRequestContext<HandshakeResponse>(
                    Mock.Of<IConnection>(),
                    new Message(
                        requestId: "a",
                        type: MessageType.Request,
                        method: MessageMethod.Handshake,
                        payload: null),
                    TimeSpan.FromMinutes(1),
                    isKeepAlive: true,
                    cancellationToken: CancellationToken.None,
                    logger: null));

            Assert.Equal("logger", exception.ParamName);
        }

        [Fact]
        public void Constructor_InitializesProperties()
        {
            using (var test = new OutboundRequestContextTest())
            {
                Assert.Equal(test.Request.RequestId, test.Context.RequestId);
                Assert.NotNull(test.Context.CompletionTask);
                Assert.Equal(TaskStatus.WaitingForActivation, test.Context.CompletionTask.Status);
            }
        }

        [Fact]
        public async Task Constructor_ClosesIfTimedOut()
        {
            using (var test = new OutboundRequestContextTest(TimeSpan.FromMilliseconds(10)))
            {
                await Assert.ThrowsAsync<TaskCanceledException>(
                    () => test.Context.CompletionTask);
            }
        }

        [Fact]
        public void Constructor_CancelsCompletionTaskAndSendsCancelRequestIfCancelled()
        {
            using (var test = new OutboundRequestContextTest())
            using (var cancelEvent = new ManualResetEventSlim(initialState: false))
            {
                test.Connection.Setup(x => x.MessageDispatcher.DispatchCancelAsync(
                        It.Is<Message>(m => ReferenceEquals(m, test.Request)),
                        It.IsAny<CancellationToken>()))
                    .Callback<Message, CancellationToken>(
                        (message, cancellationToken) =>
                        {
                            cancelEvent.Set();
                        })
                    .Returns(Task.CompletedTask);

                test.CancellationTokenSource.Cancel();

                Assert.Equal(TaskStatus.Canceled, test.Context.CompletionTask.Status);

                cancelEvent.Wait();
            }
        }

        [Fact]
        public void CancellationToken_LinksOriginalCancellationToken()
        {
            using (var test = new OutboundRequestContextTest())
            {
                Assert.False(test.Context.CancellationToken.IsCancellationRequested);

                test.CancellationTokenSource.Cancel();

                Assert.True(test.Context.CancellationToken.IsCancellationRequested);
            }
        }

        [Fact]
        public void CancellationToken_CancelledWhenDisposed()
        {
            using (var test = new OutboundRequestContextTest())
            {
                Assert.False(test.Context.CancellationToken.IsCancellationRequested);

                test.Context.Dispose();

                Assert.True(test.Context.CancellationToken.IsCancellationRequested);
            }
        }

        [Fact]
        public void Dispose_DoesNotDisposeConnection()
        {
            using (var test = new OutboundRequestContextTest())
            {
                test.Context.Dispose();
                test.Connection.Verify();
            }
        }

        [Fact]
        public void Dispose_DoesNotDisposeLogger()
        {
            using (var test = new OutboundRequestContextTest())
            {
                test.Context.Dispose();
                test.Logger.Verify();
            }
        }

        [Fact]
        public void Dispose_IsIdempotent()
        {
            using (var test = new OutboundRequestContextTest())
            {
                test.Context.Dispose();
                test.Context.Dispose();
            }
        }

        [Fact]
        public void HandleCancelResponse_ThrowsIfNotCancelled()
        {
            using (var test = new OutboundRequestContextTest())
            {
                Assert.Throws<ProtocolException>(() => test.Context.HandleCancelResponse());
            }
        }

        [Fact]
        public void HandleProgress_ThrowsForNullProgress()
        {
            using (var test = new OutboundRequestContextTest())
            {
                var exception = Assert.Throws<ArgumentNullException>(
                    () => test.Context.HandleProgress(progress: null));

                Assert.Equal("progress", exception.ParamName);
            }
        }

        [Fact]
        public void HandleResponse_ThrowsForNullProgress()
        {
            using (var test = new OutboundRequestContextTest())
            {
                var exception = Assert.Throws<ArgumentNullException>(
                    () => test.Context.HandleResponse(response: null));

                Assert.Equal("response", exception.ParamName);
            }
        }

        [Fact]
        public void HandleResponse_CompletesCompletionTask()
        {
            using (var test = new OutboundRequestContextTest())
            {
                var payload = new HandshakeResponse(MessageResponseCode.Error, protocolVersion: null);
                var response = MessageUtilities.Create(
                    test.Request.RequestId,
                    MessageType.Response,
                    test.Request.Method,
                    payload);

                test.Context.HandleResponse(response);

                Assert.Equal(TaskStatus.RanToCompletion, test.Context.CompletionTask.Status);
#pragma warning disable xUnit1031 // Do not use blocking task operations in test method
                Assert.Equal(MessageResponseCode.Error, test.Context.CompletionTask.Result.ResponseCode);
                Assert.Null(test.Context.CompletionTask.Result.ProtocolVersion);
#pragma warning restore xUnit1031 // Do not use blocking task operations in test method
            }
        }

        [Fact]
        public void HandleResponse_SecondResponseIsIgnored()
        {
            using (var test = new OutboundRequestContextTest())
            {
                var payload = new HandshakeResponse(MessageResponseCode.Error, protocolVersion: null);
                var firstResponse = MessageUtilities.Create(
                    test.Request.RequestId,
                    MessageType.Response,
                    test.Request.Method,
                    payload);

                test.Context.HandleResponse(firstResponse);

#pragma warning disable xUnit1031 // Do not use blocking task operations in test method
                var response = test.Context.CompletionTask.Result;
#pragma warning restore xUnit1031 // Do not use blocking task operations in test method

                payload = new HandshakeResponse(MessageResponseCode.Success, ProtocolConstants.CurrentVersion);
                var secondResponse = MessageUtilities.Create(
                    test.Request.RequestId,
                    MessageType.Response,
                    test.Request.Method,
                    payload);

                test.Context.HandleResponse(secondResponse);

#pragma warning disable xUnit1031 // Do not use blocking task operations in test method
                Assert.Same(response, test.Context.CompletionTask.Result);
#pragma warning restore xUnit1031 // Do not use blocking task operations in test method
            }
        }

        [Fact]
        public void HandleFault_ThrowsForNullProgress()
        {
            using (var test = new OutboundRequestContextTest())
            {
                var exception = Assert.Throws<ArgumentNullException>(
                    () => test.Context.HandleFault(fault: null));

                Assert.Equal("fault", exception.ParamName);
            }
        }

        [Fact]
        public void HandleFault_ThrowsFault()
        {
            using (var test = new OutboundRequestContextTest())
            {
                var payload = new Fault("test");
                var response = MessageUtilities.Create(
                    test.Request.RequestId,
                    MessageType.Fault,
                    test.Request.Method,
                    payload);

                var exception = Assert.Throws<ProtocolException>(
                    () => test.Context.HandleFault(response));

                Assert.Equal("test", exception.Message);
            }
        }

        private sealed class OutboundRequestContextTest : IDisposable
        {
            internal CancellationTokenSource CancellationTokenSource { get; }
            internal Mock<IConnection> Connection { get; }
            internal OutboundRequestContext<HandshakeResponse> Context { get; }
            internal Mock<IPluginLogger> Logger { get; }
            internal Message Request { get; }

            internal OutboundRequestContextTest(TimeSpan? timeout = null)
            {
                CancellationTokenSource = new CancellationTokenSource();
                Connection = new Mock<IConnection>(MockBehavior.Strict);
                Logger = new Mock<IPluginLogger>(MockBehavior.Strict);
                Request = new Message(
                    requestId: "a",
                    type: MessageType.Request,
                    method: MessageMethod.Handshake,
                    payload: null);
                Context = new OutboundRequestContext<HandshakeResponse>(
                    Connection.Object,
                    Request,
                    timeout,
                    isKeepAlive: false,
                    cancellationToken: CancellationTokenSource.Token);
            }

            public void Dispose()
            {
                try
                {
                    using (CancellationTokenSource)
                    {
                        CancellationTokenSource.Cancel();
                    }
                }
                catch (Exception)
                {
                }

                Context.Dispose();

                GC.SuppressFinalize(this);

                Connection.Verify();
                Logger.Verify();
            }
        }
    }
}
