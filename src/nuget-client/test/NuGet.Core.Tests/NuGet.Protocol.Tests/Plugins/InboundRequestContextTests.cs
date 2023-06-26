// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class InboundRequestContextTests
    {
        [Fact]
        public void Constructor_ThrowsForNullConnection()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new InboundRequestContext(
                    connection: null,
                    requestId: "a",
                    cancellationToken: CancellationToken.None));

            Assert.Equal("connection", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_ThrowsForNullOrEmptyRequestId(string requestId)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new InboundRequestContext(
                    Mock.Of<IConnection>(),
                    requestId,
                    CancellationToken.None));

            Assert.Equal("requestId", exception.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsForNullInboundRequestProcessingHandler()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new InboundRequestContext(
                    Mock.Of<IConnection>(),
                    requestId: "a",
                    cancellationToken: CancellationToken.None,
                    inboundRequestProcessingHandler: null,
                    Mock.Of<IPluginLogger>()));

            Assert.Equal("inboundRequestProcessingHandler", exception.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsForNullLogger()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new InboundRequestContext(
                    Mock.Of<IConnection>(),
                    requestId: "a",
                    cancellationToken: CancellationToken.None,
                    inboundRequestProcessingHandler: new InboundRequestProcessingHandler(Enumerable.Empty<MessageMethod>()),
                    logger: null));

            Assert.Equal("logger", exception.ParamName);
        }

        [Fact]
        public void Constructor_InitializesRequestIdProperty()
        {
            using (var test = new InboundRequestContextTest())
            {
                Assert.Equal(test.RequestId, test.Context.RequestId);
            }
        }

        [Fact]
        public void Dispose_DoesNotDisposeConnection()
        {
            using (var test = new InboundRequestContextTest())
            {
                test.Context.Dispose();
                test.Connection.Verify();
            }
        }

        [Fact]
        public void Dispose_DoesNotDisposeOfInboundRequestProcessingHandler()
        {
            using (var test = new InboundRequestContextTest())
            {
                test.Context.Dispose();
                test.Handler.Verify();
            }
        }


        [Fact]
        public void Dispose_DoesNotDisposeLogger()
        {
            using (var test = new InboundRequestContextTest())
            {
                test.Context.Dispose();
                test.Logger.Verify();
            }
        }

        [Fact]
        public void Dispose_IsIdempotent()
        {
            using (var test = new InboundRequestContextTest())
            {
                test.Context.Dispose();
                test.Context.Dispose();
            }
        }

        [Fact]
        public void Cancel_SendsCancelResponse()
        {
            using (var test = new InboundRequestContextTest())
            using (var handledEvent = new ManualResetEventSlim(initialState: false))
            {
                test.Connection.Setup(
                        x => x.SendAsync(
                            It.Is<Message>(m => m.Type == MessageType.Cancel),
                            It.IsAny<CancellationToken>()))
                    .Returns(Task.CompletedTask);

                var requestHandler = new Mock<IRequestHandler>(MockBehavior.Strict);

                requestHandler.Setup(x => x.HandleResponseAsync(
                        It.IsNotNull<IConnection>(),
                        It.IsNotNull<Message>(),
                        It.IsNotNull<IResponseHandler>(),
                        It.IsAny<CancellationToken>()))
                    .Callback<IConnection, Message, IResponseHandler, CancellationToken>(
                        (connection, requestMessage, responseHandler, cancellationToken) =>
                        {
                            handledEvent.Set();
                        })
                    .Returns(Task.CompletedTask);

                var request = MessageUtilities.Create(
                    test.RequestId,
                    MessageType.Cancel,
                    MessageMethod.PrefetchPackage,
                    new PrefetchPackageRequest(
                        packageSourceRepository: "a",
                        packageId: "b",
                        packageVersion: "c"));

                test.Context.BeginResponseAsync(request, requestHandler.Object, Mock.Of<IResponseHandler>());

                handledEvent.Wait();

                test.Context.Cancel();

                requestHandler.Verify(x => x.HandleResponseAsync(
                    It.IsNotNull<IConnection>(),
                    It.IsNotNull<Message>(),
                    It.IsNotNull<IResponseHandler>(),
                    It.IsAny<CancellationToken>()), Times.Once);
            }
        }

        [Fact]
        public void Cancel_IsIdempotent()
        {
            using (var test = new InboundRequestContextTest())
            {
                test.Context.Cancel();
                test.Context.Cancel();
            }
        }

        [Fact]
        public void BeginFaultAsync_ThrowsForNullRequest()
        {
            using (var test = new InboundRequestContextTest())
            {
                var exception = Assert.Throws<ArgumentNullException>(
                    () => test.Context.BeginFaultAsync(request: null, exception: new Exception()));

                Assert.Equal("request", exception.ParamName);
            }
        }

        [Fact]
        public void BeginFaultAsync_ThrowsForNullException()
        {
            using (var test = new InboundRequestContextTest())
            {
                var exception = Assert.Throws<ArgumentNullException>(
                    () => test.Context.BeginFaultAsync(
                        new Message(
                            test.RequestId,
                            MessageType.Request,
                            MessageMethod.GetOperationClaims,
                            payload: null),
                        exception: null));

                Assert.Equal("exception", exception.ParamName);
            }
        }

        [Fact]
        public void BeginFaultAsync_SendsReponse()
        {
            using (var test = new InboundRequestContextTest())
            using (var sentEvent = new ManualResetEventSlim(initialState: false))
            {
                Message response = null;

                test.Connection.Setup(x => x.SendAsync(It.IsNotNull<Message>(), It.IsAny<CancellationToken>()))
                    .Callback<Message, CancellationToken>(
                        (message, cancellationToken) =>
                        {
                            response = message;

                            sentEvent.Set();
                        })
                    .Returns(Task.CompletedTask);

                test.Context.BeginFaultAsync(
                    new Message(
                        test.RequestId,
                        MessageType.Request,
                        MessageMethod.GetOperationClaims,
                        payload: null),
                    new Exception("test"));

                sentEvent.Wait();

                test.Connection.Verify(
                    x => x.SendAsync(It.IsNotNull<Message>(), It.IsAny<CancellationToken>()),
                    Times.Once);

                Assert.NotNull(response);
                Assert.Equal(test.RequestId, response.RequestId);
                Assert.Equal(MessageType.Fault, response.Type);
                Assert.Equal(MessageMethod.GetOperationClaims, response.Method);

                var responsePayload = MessageUtilities.DeserializePayload<Fault>(response);

                Assert.Equal("test", responsePayload.Message);
            }
        }

        [Fact]
        public void BeginResponseAsync_ThrowsForNullRequest()
        {
            using (var test = new InboundRequestContextTest())
            {
                var exception = Assert.Throws<ArgumentNullException>(
                    () => test.Context.BeginResponseAsync(
                        request: null,
                        requestHandler: Mock.Of<IRequestHandler>(),
                        responseHandler: Mock.Of<IResponseHandler>()));

                Assert.Equal("request", exception.ParamName);
            }
        }

        [Fact]
        public void BeginResponseAsync_ThrowsForNullRequestHandler()
        {
            using (var test = new InboundRequestContextTest())
            {
                var exception = Assert.Throws<ArgumentNullException>(
                    () => test.Context.BeginResponseAsync(
                        new Message(
                            test.RequestId,
                            MessageType.Progress,
                            MessageMethod.GetOperationClaims,
                            payload: null),
                        requestHandler: null,
                        responseHandler: Mock.Of<IResponseHandler>()));

                Assert.Equal("requestHandler", exception.ParamName);
            }
        }

        [Fact]
        public void BeginResponseAsync_ThrowsForNullResponseHandler()
        {
            using (var test = new InboundRequestContextTest())
            {
                var exception = Assert.Throws<ArgumentNullException>(
                    () => test.Context.BeginResponseAsync(
                        new Message(
                            test.RequestId,
                            type: MessageType.Progress,
                            method: MessageMethod.GetOperationClaims,
                            payload: null),
                        requestHandler: Mock.Of<IRequestHandler>(),
                        responseHandler: null));

                Assert.Equal("responseHandler", exception.ParamName);
            }
        }

        [Fact]
        public void BeginResponseAsync_CallsRequestHandler()
        {
            using (var test = new InboundRequestContextTest())
            using (var handledEvent = new ManualResetEventSlim(initialState: false))
            {
                var requestHandler = new Mock<IRequestHandler>(MockBehavior.Strict);

                requestHandler.Setup(x => x.HandleResponseAsync(
                        It.IsNotNull<IConnection>(),
                        It.IsNotNull<Message>(),
                        It.IsNotNull<IResponseHandler>(),
                        It.IsAny<CancellationToken>()))
                    .Callback<IConnection, Message, IResponseHandler, CancellationToken>(
                        (connection, message, responseHandler, cancellationToken) =>
                        {
                            handledEvent.Set();
                        })
                    .Returns(Task.CompletedTask);

                test.Context.BeginResponseAsync(
                    new Message(
                        test.RequestId,
                        MessageType.Request,
                        MessageMethod.GetOperationClaims,
                        payload: null),
                    requestHandler.Object,
                    Mock.Of<IResponseHandler>());

                handledEvent.Wait();

                requestHandler.Verify(x => x.HandleResponseAsync(
                    It.IsNotNull<IConnection>(),
                    It.IsNotNull<Message>(),
                    It.IsNotNull<IResponseHandler>(),
                    It.IsAny<CancellationToken>()), Times.Once);
            }
        }

        [Fact]
        public void BeginResponseAsync_TranslatesRequestHandlerExceptionIntoFaultResponse()
        {
            using (var test = new InboundRequestContextTest())
            using (var sentEvent = new ManualResetEventSlim(initialState: false))
            {
                var requestHandler = new Mock<IRequestHandler>(MockBehavior.Strict);

                requestHandler.Setup(x => x.HandleResponseAsync(
                        It.IsNotNull<IConnection>(),
                        It.IsNotNull<Message>(),
                        It.IsNotNull<IResponseHandler>(),
                        It.IsAny<CancellationToken>()))
                    .Throws(new Exception("test"));

                Message response = null;

                test.Connection.Setup(x => x.SendAsync(It.IsNotNull<Message>(), It.IsAny<CancellationToken>()))
                    .Callback<Message, CancellationToken>(
                        (message, cancellationToken) =>
                        {
                            response = message;

                            sentEvent.Set();
                        })
                    .Returns(Task.CompletedTask);

                test.Context.BeginResponseAsync(
                    new Message(
                        test.RequestId,
                        MessageType.Request,
                        MessageMethod.GetOperationClaims,
                        payload: null),
                    requestHandler.Object,
                    Mock.Of<IResponseHandler>());

                sentEvent.Wait();

                requestHandler.Verify(x => x.HandleResponseAsync(
                    It.IsNotNull<IConnection>(),
                    It.IsNotNull<Message>(),
                    It.IsNotNull<IResponseHandler>(),
                    It.IsAny<CancellationToken>()), Times.Once);

                test.Connection.Verify(
                    x => x.SendAsync(It.IsNotNull<Message>(), It.IsAny<CancellationToken>()),
                    Times.Once);

                Assert.NotNull(response);
                Assert.Equal(test.RequestId, response.RequestId);
                Assert.Equal(MessageType.Fault, response.Type);
                Assert.Equal(MessageMethod.GetOperationClaims, response.Method);

                var responsePayload = MessageUtilities.DeserializePayload<Fault>(response);

                Assert.Equal("test", responsePayload.Message);
            }
        }

        private sealed class InboundRequestContextTest : IDisposable
        {
            internal CancellationTokenSource CancellationTokenSource { get; }
            internal Mock<IConnection> Connection { get; }
            internal InboundRequestContext Context { get; }
            internal Mock<IPluginLogger> Logger { get; }
            internal string RequestId { get; }
            internal Mock<InboundRequestProcessingHandler> Handler { get; }
            internal InboundRequestContextTest()
            {
                CancellationTokenSource = new CancellationTokenSource();
                Connection = new Mock<IConnection>(MockBehavior.Strict);
                Logger = new Mock<IPluginLogger>();
                RequestId = "a";
                Handler = new Mock<InboundRequestProcessingHandler>(MockBehavior.Strict);
                Context = new InboundRequestContext(
                    Connection.Object,
                    RequestId,
                    CancellationTokenSource.Token,
                    Handler.Object,
                    Logger.Object);
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
                Handler.Verify();
            }
        }
    }
}
