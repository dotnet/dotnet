// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Protocol.Tests;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    [Collection(nameof(NotThreadSafeResourceCollection))]
    public class MessageDispatcherTests
    {
        private readonly Mock<IConnection> _connection = new Mock<IConnection>(MockBehavior.Strict);
        private readonly ConstantIdGenerator _idGenerator = new ConstantIdGenerator();
        private readonly MessageMethod _method = MessageMethod.None;
        private readonly TimeSpan _timeout = TimeSpan.FromHours(1);

        [Fact]
        public void Constructor_ThrowsForNullRequestHandlers()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new MessageDispatcher(requestHandlers: null, idGenerator: _idGenerator));

            Assert.Equal("requestHandlers", exception.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsForNullIdGenerator()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new MessageDispatcher(new RequestHandlers(), idGenerator: null));

            Assert.Equal("idGenerator", exception.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsForNullLogger()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new MessageDispatcher(new RequestHandlers(), new ConstantIdGenerator(), new InboundRequestProcessingHandler(), logger: null));

            Assert.Equal("logger", exception.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsForNullInboundRequestProcessinHandler()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new MessageDispatcher(new RequestHandlers(), new ConstantIdGenerator(), inboundRequestProcessingHandler: null, logger: PluginLogger.DefaultInstance));

            Assert.Equal("inboundRequestProcessingHandler", exception.ParamName);
        }

        [Fact]
        public void Constructor_InitializesProperties()
        {
            using (var dispatcher = new MessageDispatcher(new RequestHandlers(), _idGenerator))
            {
                Assert.NotNull(dispatcher.RequestHandlers);
            }
        }

        [Fact]
        public async Task Close_DisposesAllActiveOutboundRequests()
        {
            using (var dispatcher = new MessageDispatcher(new RequestHandlers(), _idGenerator))
            using (var sentEvent = new ManualResetEventSlim(initialState: false))
            {
                var connection = new Mock<IConnection>(MockBehavior.Strict);

                connection.SetupGet(x => x.Options)
                    .Returns(ConnectionOptions.CreateDefault());

                connection.Setup(x => x.SendAsync(It.IsNotNull<Message>(), It.IsAny<CancellationToken>()))
                    .Callback<Message, CancellationToken>(
                        (message, cancellationToken) =>
                        {
                            sentEvent.Set();
                        })
                    .Returns(Task.FromResult(0));

                dispatcher.SetConnection(connection.Object);

                var outboundRequestTask = Task.Run(() => dispatcher.DispatchRequestAsync<HandshakeRequest, HandshakeResponse>(
                    MessageMethod.Handshake,
                    new HandshakeRequest(ProtocolConstants.CurrentVersion, ProtocolConstants.CurrentVersion),
                    CancellationToken.None));

                sentEvent.Wait();

                dispatcher.Close();

                await Assert.ThrowsAsync<TaskCanceledException>(() => outboundRequestTask);

                connection.Verify(x => x.SendAsync(It.IsNotNull<Message>(), It.IsAny<CancellationToken>()), Times.Once);
            }
        }

        [Fact]
        public void Close_DisposesAllActiveInboundRequests()
        {
            using (var dispatcher = new MessageDispatcher(new RequestHandlers(), _idGenerator))
            using (var handlingEvent = new ManualResetEventSlim(initialState: false))
            using (var blockingEvent = new ManualResetEventSlim(initialState: false))
            {
                var requestHandler = new RequestHandler();

                Assert.True(dispatcher.RequestHandlers.TryAdd(MessageMethod.Handshake, requestHandler));

                var connection = new Mock<IConnection>(MockBehavior.Strict);
                var payload = new HandshakeRequest(ProtocolConstants.CurrentVersion, ProtocolConstants.CurrentVersion);
                var request = dispatcher.CreateMessage(MessageType.Request, MessageMethod.Handshake, payload);

                dispatcher.SetConnection(connection.Object);

                var actualCancellationToken = default(CancellationToken);

                requestHandler.HandleResponseAsyncFunc = (conn, message, responseHandler, cancellationToken) =>
                    {
                        handlingEvent.Set();

                        actualCancellationToken = cancellationToken;

                        blockingEvent.Set();

                        return Task.FromResult(0);
                    };

                connection.Raise(x => x.MessageReceived += null, new MessageEventArgs(request));

                handlingEvent.Wait();

                dispatcher.Close();

                blockingEvent.Wait();

                Assert.True(actualCancellationToken.IsCancellationRequested);
            }
        }

        [Fact]
        public void Close_WithDedicatedProcessingContext_DisposesAllActiveInboundRequests()
        {
            using (var processingHandler = new InboundRequestProcessingHandler(new HashSet<MessageMethod>() { MessageMethod.Handshake }))
            using (var dispatcher = new MessageDispatcher(new RequestHandlers(), _idGenerator, processingHandler, PluginLogger.DefaultInstance))
            using (var handlingEvent = new ManualResetEventSlim(initialState: false))
            using (var blockingEvent = new ManualResetEventSlim(initialState: false))
            {
                var requestHandler = new RequestHandler();

                Assert.True(dispatcher.RequestHandlers.TryAdd(MessageMethod.Handshake, requestHandler));

                var connection = new Mock<IConnection>(MockBehavior.Strict);
                var payload = new HandshakeRequest(ProtocolConstants.CurrentVersion, ProtocolConstants.CurrentVersion);
                var request = dispatcher.CreateMessage(MessageType.Request, MessageMethod.Handshake, payload);

                dispatcher.SetConnection(connection.Object);

                var actualCancellationToken = default(CancellationToken);

                requestHandler.HandleResponseAsyncFunc = (conn, message, responseHandler, cancellationToken) =>
                {
                    handlingEvent.Set();

                    actualCancellationToken = cancellationToken;

                    blockingEvent.Set();

                    return Task.FromResult(0);
                };

                connection.Raise(x => x.MessageReceived += null, new MessageEventArgs(request));

                handlingEvent.Wait();

                dispatcher.Close();

                blockingEvent.Wait();

                Assert.True(actualCancellationToken.IsCancellationRequested);
            }
        }

        [Fact]
        public void Close_IsIdempotent()
        {
            using (var dispatcher = new MessageDispatcher(new RequestHandlers(), _idGenerator))
            {
                dispatcher.Close();
                dispatcher.Close();
            }
        }

        [Fact]
        public void CreateMessage_TypeMethod_ReturnsMessage()
        {
            using (var dispatcher = new MessageDispatcher(new RequestHandlers(), _idGenerator))
            {
                var message = dispatcher.CreateMessage(MessageType.Request, MessageMethod.Handshake);

                Assert.Equal(_idGenerator.Id, message.RequestId);
                Assert.Equal(MessageType.Request, message.Type);
                Assert.Equal(MessageMethod.Handshake, message.Method);
                Assert.Null(message.Payload);
            }
        }

        [Fact]
        public void CreateMessage_TypeMethodPayload_ThrowsForNullPayload()
        {
            using (var dispatcher = new MessageDispatcher(new RequestHandlers(), _idGenerator))
            {
                var exception = Assert.Throws<ArgumentNullException>(
                    () => dispatcher.CreateMessage<HandshakeRequest>(
                        MessageType.Request,
                        MessageMethod.Handshake,
                        payload: null));

                Assert.Equal("payload", exception.ParamName);
            }
        }

        [Fact]
        public void CreateMessage_TypeMethodPayload_ReturnsMessage()
        {
            using (var dispatcher = new MessageDispatcher(new RequestHandlers(), _idGenerator))
            {
                var payload = new HandshakeRequest(ProtocolConstants.Version100, ProtocolConstants.Version100);
                var message = dispatcher.CreateMessage(
                    MessageType.Request,
                    MessageMethod.Handshake,
                    payload);

                Assert.Equal(_idGenerator.Id, message.RequestId);
                Assert.Equal(MessageType.Request, message.Type);
                Assert.Equal(MessageMethod.Handshake, message.Method);
                Assert.Equal("{\"ProtocolVersion\":\"1.0.0\",\"MinimumProtocolVersion\":\"1.0.0\"}",
                    message.Payload.ToString(Formatting.None));
            }
        }

        [Fact]
        public void Dispose_DoesNotDisposeLogger()
        {
            var logger = new Mock<IPluginLogger>(MockBehavior.Strict);

            using (var dispatcher = new MessageDispatcher(new RequestHandlers(), _idGenerator, new InboundRequestProcessingHandler(), logger.Object))
            {
                dispatcher.Dispose();

                logger.Verify();
            }
        }

        [Fact]
        public void Dispose_DisposesInboundRequestProcessingHandler()
        {
            var processingHandler = new InboundRequestProcessingHandler(new HashSet<MessageMethod>());

            using (var dispatcher = new MessageDispatcher(new RequestHandlers(), _idGenerator, processingHandler, PluginLogger.DefaultInstance))
            {
                dispatcher.Dispose();
                Assert.Throws<ObjectDisposedException>(() => processingHandler.Handle(MessageMethod.Handshake, task: null, CancellationToken.None));
            }
        }

        [Fact]
        public void Dispose_IsIdempotent()
        {
            using (var dispatcher = new MessageDispatcher(new RequestHandlers(), _idGenerator))
            {
                dispatcher.Dispose();
                dispatcher.Dispose();
            }
        }

        [Fact]
        public void SetConnection_SubscribesToMessageReceived()
        {
            using (var dispatcher = new MessageDispatcher(new RequestHandlers(), _idGenerator))
            {
                var spy = new ConnectionEventRegistrationSpy();

                Assert.Equal(0, spy.MessageReceivedAddCount);

                dispatcher.SetConnection(spy);

                Assert.Equal(1, spy.MessageReceivedAddCount);
            }
        }

        [Fact]
        public void SetConnection_UnsubscribesFromMessageReceivedWithNullConnection()
        {
            using (var dispatcher = new MessageDispatcher(new RequestHandlers(), _idGenerator))
            {
                var spy = new ConnectionEventRegistrationSpy();

                dispatcher.SetConnection(spy);
                dispatcher.SetConnection(connection: null);

                Assert.Equal(1, spy.MessageReceivedRemoveCount);
            }
        }

        [Fact]
        public void SetConnection_UnsubscribesFromMessageReceivedWithNewConnection()
        {
            using (var dispatcher = new MessageDispatcher(new RequestHandlers(), _idGenerator))
            {
                var firstSpy = new ConnectionEventRegistrationSpy();

                dispatcher.SetConnection(firstSpy);

                var secondSpy = new ConnectionEventRegistrationSpy();

                dispatcher.SetConnection(secondSpy);

                Assert.Equal(1, firstSpy.MessageReceivedRemoveCount);
                Assert.Equal(1, secondSpy.MessageReceivedAddCount);
            }
        }

        [Fact]
        public void SetConnection_IsIdempotent()
        {
            using (var dispatcher = new MessageDispatcher(new RequestHandlers(), _idGenerator))
            {
                var spy = new ConnectionEventRegistrationSpy();

                dispatcher.SetConnection(spy);
                dispatcher.SetConnection(spy);
                dispatcher.SetConnection(spy);

                Assert.Equal(1, spy.MessageReceivedAddCount);
                Assert.Equal(0, spy.MessageReceivedRemoveCount);
            }
        }

        [Fact]
        public async Task DispatchCancelAsync_NoOpsIfNoConnection()
        {
            using (var dispatcher = new MessageDispatcher(new RequestHandlers(), _idGenerator))
            {
                var request = new Message(_idGenerator.Id, MessageType.Request, _method);

                await dispatcher.DispatchCancelAsync(request, CancellationToken.None);
            }
        }

        [Fact]
        public async Task DispatchCancelAsync_ThrowsForNullRequest()
        {
            using (var dispatcher = new MessageDispatcher(new RequestHandlers(), _idGenerator))
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => dispatcher.DispatchCancelAsync(request: null, cancellationToken: CancellationToken.None));

                Assert.Equal("request", exception.ParamName);
            }
        }

        [Fact]
        public async Task DispatchCancelAsync_ThrowsIfCancelled()
        {
            using (var dispatcher = new MessageDispatcher(new RequestHandlers(), _idGenerator))
            {
                await Assert.ThrowsAsync<OperationCanceledException>(
                    () => dispatcher.DispatchCancelAsync(
                        new Message(_idGenerator.Id, MessageType.Request, _method),
                        new CancellationToken(canceled: true)));
            }
        }

        [Fact]
        public async Task DispatchCancelAsync_ThrowsWithoutAssociatedRequestId()
        {
            using (var dispatcher = new MessageDispatcher(new RequestHandlers(), _idGenerator))
            using (var connection = new ConnectionMock())
            {
                dispatcher.SetConnection(connection);

                var request = new Message(_idGenerator.Id, MessageType.Request, _method);

                await Assert.ThrowsAsync<ProtocolException>(
                    () => dispatcher.DispatchCancelAsync(request, CancellationToken.None));
            }
        }

        [Fact]
        public async Task DispatchCancelAsync_SendsProgressWithAssociatedRequestId()
        {
            using (var dispatcher = new MessageDispatcher(new RequestHandlers(), _idGenerator))
            using (var connection = new ConnectionMock())
            {
                dispatcher.SetConnection(connection);

                Message message = null;

                connection.MessageSent += (object sender, MessageEventArgs e) =>
                {
                    message = e.Message;
                };

                var request = new Message(_idGenerator.Id, MessageType.Request, MessageMethod.GetOperationClaims);

                var requestTask = dispatcher.DispatchRequestAsync<Request, Response>(
                    request.Method,
                    new Request(),
                    CancellationToken.None);

                await dispatcher.DispatchCancelAsync(request, CancellationToken.None);

                Assert.NotNull(message);
                Assert.Equal(_idGenerator.Id, message.RequestId);
                Assert.Equal(MessageType.Cancel, message.Type);
                Assert.Equal(request.Method, message.Method);
                Assert.Null(message.Payload);
            }
        }

        [Fact]
        public async Task DispatchFaultAsync_NoOpsIfNoConnection()
        {
            using (var dispatcher = new MessageDispatcher(new RequestHandlers(), _idGenerator))
            {
                await dispatcher.DispatchFaultAsync(
                    request: null,
                    fault: new Fault(message: "a"),
                    cancellationToken: CancellationToken.None);
            }
        }

        [Fact]
        public async Task DispatchFaultAsync_ThrowsForNullFault()
        {
            using (var dispatcher = new MessageDispatcher(new RequestHandlers(), _idGenerator))
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => dispatcher.DispatchFaultAsync(
                        new Message(_idGenerator.Id, MessageType.Request, _method),
                        fault: null,
                        cancellationToken: CancellationToken.None));

                Assert.Equal("fault", exception.ParamName);
            }
        }

        [Fact]
        public async Task DispatchFaultAsync_ThrowsIfCancelled()
        {
            using (var dispatcher = new MessageDispatcher(new RequestHandlers(), _idGenerator))
            {
                await Assert.ThrowsAsync<OperationCanceledException>(
                    () => dispatcher.DispatchFaultAsync(
                        new Message(_idGenerator.Id, MessageType.Request, _method),
                        new Fault("test"),
                        new CancellationToken(canceled: true)));
            }
        }

        [Fact]
        public async Task DispatchFaultAsync_SendsFaultWithoutAssociatedRequestId()
        {
            using (var dispatcher = new MessageDispatcher(new RequestHandlers(), _idGenerator))
            using (var connection = new ConnectionMock())
            using (var messageSentEvent = new ManualResetEventSlim(initialState: false))
            {
                dispatcher.SetConnection(connection);

                var fault = new Fault(message: "a");

                Message message = null;

                connection.MessageSent += (object sender, MessageEventArgs e) =>
                {
                    message = e.Message;

                    messageSentEvent.Set();
                };

                await dispatcher.DispatchFaultAsync(
                    request: null,
                    fault: fault,
                    cancellationToken: CancellationToken.None);

                Assert.NotNull(message);
                Assert.Equal(_idGenerator.Id, message.RequestId);
                Assert.Equal(MessageType.Fault, message.Type);
                Assert.Equal(MessageMethod.None, message.Method);
                Assert.Equal("{\"Message\":\"a\"}", message.Payload.ToString(Formatting.None));
            }
        }

        [Fact]
        public async Task DispatchFaultAsync_SendsFaultWithAssociatedRequestId()
        {
            using (var dispatcher = new MessageDispatcher(new RequestHandlers(), _idGenerator))
            using (var connection = new ConnectionMock())
            {
                dispatcher.SetConnection(connection);

                Message message = null;

                connection.MessageSent += (object sender, MessageEventArgs e) =>
                {
                    message = e.Message;
                };

                var request = new Message(
                    _idGenerator.Id,
                    MessageType.Request,
                    MessageMethod.Handshake,
                    JObject.FromObject(new Request()));

                var requestTask = dispatcher.DispatchRequestAsync<Request, Response>(
                    request.Method,
                    new Request(),
                    CancellationToken.None);

                var fault = new Fault(message: "a");

                await dispatcher.DispatchFaultAsync(request, fault, CancellationToken.None);

                Assert.NotNull(message);
                Assert.Equal(_idGenerator.Id, message.RequestId);
                Assert.Equal(MessageType.Fault, message.Type);
                Assert.Equal(request.Method, message.Method);
                Assert.Equal("{\"Message\":\"a\"}", message.Payload.ToString(Formatting.None));
            }
        }

        [Fact]
        public async Task DispatchProgressAsync_NoOpsIfNoConnection()
        {
            using (var dispatcher = new MessageDispatcher(new RequestHandlers(), _idGenerator))
            {
                var payload = JObject.FromObject(new Request());
                var request = new Message(_idGenerator.Id, MessageType.Request, _method, payload);

                await dispatcher.DispatchProgressAsync(
                    request,
                    new Progress(percentage: 0.5),
                    CancellationToken.None);
            }
        }

        [Fact]
        public async Task DispatchProgressAsync_ThrowsForNullRequest()
        {
            using (var dispatcher = new MessageDispatcher(new RequestHandlers(), _idGenerator))
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => dispatcher.DispatchProgressAsync(
                        request: null,
                        progress: new Progress(),
                        cancellationToken: CancellationToken.None));

                Assert.Equal("request", exception.ParamName);
            }
        }

        [Fact]
        public async Task DispatchProgressAsync_ThrowsForNullProgress()
        {
            using (var dispatcher = new MessageDispatcher(new RequestHandlers(), _idGenerator))
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => dispatcher.DispatchProgressAsync(
                        new Message(_idGenerator.Id, MessageType.Request, _method),
                        progress: null,
                        cancellationToken: CancellationToken.None));

                Assert.Equal("progress", exception.ParamName);
            }
        }

        [Fact]
        public async Task DispatchProgressAsync_ThrowsIfCancelled()
        {
            using (var dispatcher = new MessageDispatcher(new RequestHandlers(), _idGenerator))
            {
                await Assert.ThrowsAsync<OperationCanceledException>(
                    () => dispatcher.DispatchProgressAsync(
                        new Message(_idGenerator.Id, MessageType.Request, _method),
                        new Progress(),
                        new CancellationToken(canceled: true)));
            }
        }

        [Fact]
        public async Task DispatchProgressAsync_ThrowsWithoutAssociatedRequestId()
        {
            using (var dispatcher = new MessageDispatcher(new RequestHandlers(), _idGenerator))
            using (var connection = new ConnectionMock())
            {
                dispatcher.SetConnection(connection);

                var payload = JObject.FromObject(new Request());
                var request = new Message(_idGenerator.Id, MessageType.Request, _method, payload);
                var progress = new Progress(percentage: 0.5);

                await Assert.ThrowsAsync<ProtocolException>(
                    () => dispatcher.DispatchProgressAsync(request, progress, CancellationToken.None));
            }
        }

        [Fact]
        public async Task DispatchProgressAsync_SendsProgressWithAssociatedRequestId()
        {
            using (var dispatcher = new MessageDispatcher(new RequestHandlers(), _idGenerator))
            using (var connection = new ConnectionMock())
            {
                dispatcher.SetConnection(connection);

                Message message = null;

                connection.MessageSent += (object sender, MessageEventArgs e) =>
                {
                    message = e.Message;
                };

                var payload = JObject.FromObject(new Request());
                var request = new Message(
                    _idGenerator.Id,
                    MessageType.Request,
                    MessageMethod.GetOperationClaims,
                    payload);
                var progress = new Progress(percentage: 0.5);

                var requestTask = dispatcher.DispatchRequestAsync<Request, Response>(
                    request.Method,
                    new Request(),
                    CancellationToken.None);

                await dispatcher.DispatchProgressAsync(request, progress, CancellationToken.None);

                Assert.NotNull(message);
                Assert.Equal(_idGenerator.Id, message.RequestId);
                Assert.Equal(MessageType.Progress, message.Type);
                Assert.Equal(request.Method, message.Method);
                Assert.Equal("{\"Percentage\":0.5}", message.Payload.ToString(Formatting.None));
            }
        }

        [Fact]
        public async Task DispatchRequestAsync_NoOpsIfNoConnection()
        {
            using (var dispatcher = new MessageDispatcher(new RequestHandlers(), _idGenerator))
            {
                var version = new SemanticVersion(major: 1, minor: 0, patch: 0);
                var result = await dispatcher.DispatchRequestAsync<HandshakeRequest, HandshakeResponse>(
                    MessageMethod.Handshake,
                    new HandshakeRequest(version, version),
                    CancellationToken.None);

                Assert.Null(result);
            }
        }

        [Fact]
        public async Task DispatchRequestAsync_ThrowsIfCancelled()
        {
            using (var dispatcher = new MessageDispatcher(new RequestHandlers(), _idGenerator))
            {
                var version = new SemanticVersion(major: 1, minor: 0, patch: 0);

                await Assert.ThrowsAsync<OperationCanceledException>(
                    () => dispatcher.DispatchRequestAsync<HandshakeRequest, HandshakeResponse>(
                        MessageMethod.Handshake,
                        new HandshakeRequest(version, version),
                        new CancellationToken(canceled: true)));
            }
        }

        [Fact]
        public async Task DispatchRequestAsync_ReturnsResponse()
        {
            using (var dispatcher = new MessageDispatcher(new RequestHandlers(), _idGenerator))
            using (var connection = new ConnectionMock())
            {
                var response = new Message(
                    _idGenerator.Id,
                    MessageType.Response,
                    _method,
                    JObject.FromObject(new Response()));

                dispatcher.SetConnection(connection);

                var requestTask = dispatcher.DispatchRequestAsync<Request, Response>(
                    _method,
                    new Request(),
                    CancellationToken.None);

                connection.SimulateResponse(response);

                await requestTask;

                Assert.IsType<Response>(requestTask.Result);
            }
        }

        [Fact]
        public async Task DispatchResponseAsync_NoOpsIfNoConnection()
        {
            using (var dispatcher = new MessageDispatcher(new RequestHandlers(), _idGenerator))
            {
                var version = new SemanticVersion(major: 1, minor: 0, patch: 0);
                var payload = JObject.FromObject(new HandshakeRequest(version, version));
                var request = new Message(_idGenerator.Id, MessageType.Request, _method, payload);

                await dispatcher.DispatchResponseAsync(
                    request,
                    new HandshakeResponse(MessageResponseCode.Error, protocolVersion: null),
                    CancellationToken.None);
            }
        }

        [Fact]
        public async Task DispatchResponseAsync_ThrowsForNullRequest()
        {
            using (var dispatcher = new MessageDispatcher(new RequestHandlers(), _idGenerator))
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => dispatcher.DispatchResponseAsync(
                        request: null,
                        responsePayload: new HandshakeResponse(
                            MessageResponseCode.Success,
                            ProtocolConstants.CurrentVersion),
                        cancellationToken: CancellationToken.None));

                Assert.Equal("request", exception.ParamName);
            }
        }

        [Fact]
        public async Task DispatchResponseAsync_ThrowsForNullResponsePayload()
        {
            using (var dispatcher = new MessageDispatcher(new RequestHandlers(), _idGenerator))
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => dispatcher.DispatchResponseAsync<HandshakeResponse>(
                        new Message(_idGenerator.Id, MessageType.Request, MessageMethod.Handshake),
                        responsePayload: null,
                        cancellationToken: CancellationToken.None));

                Assert.Equal("responsePayload", exception.ParamName);
            }
        }

        [Fact]
        public async Task DispatchResponseAsync_ThrowsIfCancelled()
        {
            using (var dispatcher = new MessageDispatcher(new RequestHandlers(), _idGenerator))
            {
                await Assert.ThrowsAsync<OperationCanceledException>(
                    () => dispatcher.DispatchResponseAsync(
                        new Message(_idGenerator.Id, MessageType.Request, MessageMethod.Handshake),
                        new HandshakeResponse(
                            MessageResponseCode.Success,
                            ProtocolConstants.CurrentVersion),
                        new CancellationToken(canceled: true)));
            }
        }

        [Fact]
        public async Task DispatchResponseAsync_ReturnsResponse()
        {
            using (var dispatcher = new MessageDispatcher(new RequestHandlers(), _idGenerator))
            using (var connection = new ConnectionMock())
            {
                dispatcher.SetConnection(connection);

                var requestTask = dispatcher.DispatchRequestAsync<Request, Response>(
                    _method,
                    new Request(),
                    CancellationToken.None);

                var response = new Message(
                    _idGenerator.Id,
                    MessageType.Response,
                    _method,
                    JObject.FromObject(new Response()));

                connection.SimulateResponse(response);

                await requestTask;

                Assert.IsType<Response>(requestTask.Result);
            }
        }

        [Fact]
        public void OnMessageReceived_ThrowsForInboundFault()
        {
            var connection = new Mock<IConnection>();

            using (var dispatcher = new MessageDispatcher(new RequestHandlers(), _idGenerator))
            {
                dispatcher.SetConnection(connection.Object);

                var fault = MessageUtilities.Create(
                    _idGenerator.Id,
                    MessageType.Fault,
                    MessageMethod.GetOperationClaims,
                    new Fault("test"));

                Assert.Throws<ProtocolException>(
                    () => connection.Raise(x => x.MessageReceived += null, new MessageEventArgs(fault)));
            }
        }

        [Fact]
        public async Task OnMessageReceived_DoesNotThrowForResponseAfterWaitForResponseIsCancelled()
        {
            using (var dispatcher = new MessageDispatcher(new RequestHandlers(), _idGenerator))
            using (var cancellationTokenSource = new CancellationTokenSource())
            using (var sentEvent = new ManualResetEventSlim(initialState: false))
            {
                var connection = new Mock<IConnection>(MockBehavior.Strict);

                connection.SetupGet(x => x.Options)
                    .Returns(ConnectionOptions.CreateDefault());

                connection.Setup(x => x.SendAsync(It.IsNotNull<Message>(), It.IsAny<CancellationToken>()))
                    .Callback<Message, CancellationToken>(
                        (message, cancellationToken) =>
                        {
                            sentEvent.Set();
                        })
                    .Returns(Task.FromResult(0));

                dispatcher.SetConnection(connection.Object);

                var outboundRequestTask = Task.Run(() => dispatcher.DispatchRequestAsync<PrefetchPackageRequest, PrefetchPackageResponse>(
                    MessageMethod.PrefetchPackage,
                    new PrefetchPackageRequest(
                        packageSourceRepository: "a",
                        packageId: "b",
                        packageVersion: "c"),
                    cancellationTokenSource.Token));

                sentEvent.Wait();

                cancellationTokenSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(() => outboundRequestTask);

                var response = MessageUtilities.Create(
                    _idGenerator.Id,
                    MessageType.Response,
                    MessageMethod.PrefetchPackage,
                    new PrefetchPackageResponse(MessageResponseCode.Success));

                connection.Raise(x => x.MessageReceived += null, new MessageEventArgs(response));
            }
        }

        [Fact]
        public async Task OnMessageReceived_WithDedicatedProcessingHandler_DoesNotThrowForResponseAfterWaitForResponseIsCancelled()
        {
            using (var processingHandler = new InboundRequestProcessingHandler(new HashSet<MessageMethod> { MessageMethod.PrefetchPackage }))
            using (var dispatcher = new MessageDispatcher(new RequestHandlers(), _idGenerator, processingHandler, PluginLogger.DefaultInstance))
            using (var cancellationTokenSource = new CancellationTokenSource())
            using (var sentEvent = new ManualResetEventSlim(initialState: false))
            {
                var connection = new Mock<IConnection>(MockBehavior.Strict);

                connection.SetupGet(x => x.Options)
                    .Returns(ConnectionOptions.CreateDefault());

                connection.Setup(x => x.SendAsync(It.IsNotNull<Message>(), It.IsAny<CancellationToken>()))
                    .Callback<Message, CancellationToken>(
                        (message, cancellationToken) =>
                        {
                            sentEvent.Set();
                        })
                    .Returns(Task.FromResult(0));

                dispatcher.SetConnection(connection.Object);

                var outboundRequestTask = Task.Run(() => dispatcher.DispatchRequestAsync<PrefetchPackageRequest, PrefetchPackageResponse>(
                    MessageMethod.PrefetchPackage,
                    new PrefetchPackageRequest(
                        packageSourceRepository: "a",
                        packageId: "b",
                        packageVersion: "c"),
                    cancellationTokenSource.Token));

                sentEvent.Wait();

                cancellationTokenSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(() => outboundRequestTask);

                var response = MessageUtilities.Create(
                    _idGenerator.Id,
                    MessageType.Response,
                    MessageMethod.PrefetchPackage,
                    new PrefetchPackageResponse(MessageResponseCode.Success));

                connection.Raise(x => x.MessageReceived += null, new MessageEventArgs(response));
            }
        }

        [Fact]
        public async Task OnMessageReceived_DoesNotThrowForCancelResponseAfterWaitForResponseIsCancelled()
        {
            using (var dispatcher = new MessageDispatcher(new RequestHandlers(), _idGenerator))
            using (var cancellationTokenSource = new CancellationTokenSource())
            using (var sentEvent = new ManualResetEventSlim(initialState: false))
            {
                var connection = new Mock<IConnection>(MockBehavior.Strict);

                connection.SetupGet(x => x.Options)
                    .Returns(ConnectionOptions.CreateDefault());

                connection.Setup(x => x.SendAsync(It.IsNotNull<Message>(), It.IsAny<CancellationToken>()))
                    .Callback<Message, CancellationToken>(
                        (message, cancellationToken) =>
                        {
                            sentEvent.Set();
                        })
                    .Returns(Task.FromResult(0));

                dispatcher.SetConnection(connection.Object);

                var outboundRequestTask = Task.Run(() => dispatcher.DispatchRequestAsync<PrefetchPackageRequest, PrefetchPackageResponse>(
                    MessageMethod.PrefetchPackage,
                    new PrefetchPackageRequest(
                        packageSourceRepository: "a",
                        packageId: "b",
                        packageVersion: "c"),
                    cancellationTokenSource.Token));

                sentEvent.Wait();

                cancellationTokenSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(() => outboundRequestTask);

                var response = MessageUtilities.Create(
                    _idGenerator.Id,
                    MessageType.Cancel,
                    MessageMethod.PrefetchPackage);

                connection.Raise(x => x.MessageReceived += null, new MessageEventArgs(response));
            }
        }

        [Fact]
        public void OnMessageReceived_CancelRequestIgnoredIfNoActiveRequest()
        {
            using (var dispatcher = new MessageDispatcher(new RequestHandlers(), _idGenerator))
            using (var handlingEvent = new ManualResetEventSlim(initialState: false))
            using (var cancelEvent = new ManualResetEventSlim(initialState: false))
            using (var sentEvent = new ManualResetEventSlim(initialState: false))
            {
                var cancellationRequest = MessageUtilities.Create(
                    _idGenerator.Id,
                    MessageType.Cancel,
                    MessageMethod.PrefetchPackage);

                var connection = new Mock<IConnection>(MockBehavior.Strict);

                connection.SetupGet(x => x.Options)
                    .Returns(ConnectionOptions.CreateDefault());

                dispatcher.SetConnection(connection.Object);

                connection.Raise(x => x.MessageReceived += null, new MessageEventArgs(cancellationRequest));

                connection.Verify();
            }
        }

        [Fact]
        public void OnMessageReceived_CancelRequestCancelsActiveRequest()
        {
            using (var dispatcher = new MessageDispatcher(new RequestHandlers(), _idGenerator))
            using (var handlingEvent = new ManualResetEventSlim(initialState: false))
            using (var respondingEvent = new ManualResetEventSlim(initialState: false))
            using (var cancelEvent = new ManualResetEventSlim(initialState: false))
            using (var sentEvent = new ManualResetEventSlim(initialState: false))
            {
                var requestHandler = new RequestHandler()
                {
                    HandleResponseAsyncFunc = (conn, message, responseHandler, cancellationToken) =>
                    {
                        cancellationToken.Register(() => cancelEvent.Set());

                        handlingEvent.Set();

                        respondingEvent.Wait(cancellationToken);

                        return Task.FromResult(0);
                    }
                };

                dispatcher.RequestHandlers.TryAdd(MessageMethod.PrefetchPackage, requestHandler);

                var request = MessageUtilities.Create(
                    _idGenerator.Id,
                    MessageType.Request,
                    MessageMethod.PrefetchPackage,
                    new PrefetchPackageRequest(
                        packageSourceRepository: "a",
                        packageId: "b",
                        packageVersion: "c"));

                var connection = new Mock<IConnection>(MockBehavior.Strict);

                connection.SetupGet(x => x.Options)
                    .Returns(ConnectionOptions.CreateDefault());

                connection.Setup(x => x.SendAsync(
                        It.Is<Message>(
                            m => m.RequestId == request.RequestId &&
                            m.Type == MessageType.Cancel &&
                            m.Method == request.Method &&
                            m.Payload == null),
                        It.IsAny<CancellationToken>()))
                    .Callback<Message, CancellationToken>(
                        (message, cancellationToken) =>
                        {
                            sentEvent.Set();
                        })
                    .Returns(Task.FromResult(0));

                dispatcher.SetConnection(connection.Object);

                connection.Raise(x => x.MessageReceived += null, new MessageEventArgs(request));

                handlingEvent.Wait();

                var cancellationRequest = MessageUtilities.Create(
                    _idGenerator.Id,
                    MessageType.Cancel,
                    MessageMethod.PrefetchPackage);

                connection.Raise(x => x.MessageReceived += null, new MessageEventArgs(cancellationRequest));

                cancelEvent.Wait();
                sentEvent.Wait();

                connection.Verify();
            }
        }

        [Fact]
        public void OnMessageReceived_WithDedicatedProcessingContext_CallsBackIntoHandler()
        {
            using (var processingHandler = new InboundRequestProcessingHandler(new HashSet<MessageMethod>() { MessageMethod.Handshake }))
            using (var dispatcher = new MessageDispatcher(new RequestHandlers(), _idGenerator, processingHandler, PluginLogger.DefaultInstance))
            using (var blockingEvent = new ManualResetEventSlim(initialState: false))
            {
                var requestHandler = new RequestHandler();

                Assert.True(dispatcher.RequestHandlers.TryAdd(MessageMethod.Handshake, requestHandler));

                var connection = new Mock<IConnection>(MockBehavior.Strict);
                var payload = new HandshakeRequest(ProtocolConstants.CurrentVersion, ProtocolConstants.CurrentVersion);
                var request = dispatcher.CreateMessage(MessageType.Request, MessageMethod.Handshake, payload);

                dispatcher.SetConnection(connection.Object);

                var responseReceived = false;

                requestHandler.HandleResponseAsyncFunc = (conn, message, responseHandler, cancellationToken) =>
                {
                    responseReceived = true;
                    blockingEvent.Set();
                    return Task.FromResult(0);
                };

                connection.Raise(x => x.MessageReceived += null, new MessageEventArgs(request));

                blockingEvent.Wait();

                Assert.True(responseReceived);
            }
        }

        private sealed class ConstantIdGenerator : IIdGenerator
        {
            internal string Id { get; }

            internal ConstantIdGenerator()
            {
                Id = "0";
            }

            public string GenerateUniqueId()
            {
                return Id;
            }
        }

        private sealed class ConnectionMock : IConnection, IDisposable
        {
            private readonly ManualResetEventSlim _event;
            private bool _isDisposed;

            public IMessageDispatcher MessageDispatcher => throw new NotImplementedException();
            public ConnectionOptions Options { get; }
            public SemanticVersion ProtocolVersion => throw new NotImplementedException();

#pragma warning disable 67
            public event EventHandler<ProtocolErrorEventArgs> Faulted;
#pragma warning restore 67
            public event EventHandler<MessageEventArgs> MessageReceived;
            public event EventHandler<MessageEventArgs> MessageSent;

            public ConnectionMock()
            {
                _event = new ManualResetEventSlim(initialState: false);

                Options = ConnectionOptions.CreateDefault();
            }

            public void Dispose()
            {
                if (!_isDisposed)
                {
                    _event.Dispose();
                    GC.SuppressFinalize(this);

                    _isDisposed = true;
                }
            }

            public void Close()
            {
            }

            public Task SendAsync(Message message, CancellationToken cancellationToken)
            {
                switch (message.Type)
                {
                    case MessageType.Fault:
                        MessageSent?.Invoke(this, new MessageEventArgs(message));
                        break;

                    case MessageType.Request:
                        _event.Set();
                        break;

                    case MessageType.Cancel:
                    case MessageType.Progress:
                    case MessageType.Response:
                        _event.Wait(cancellationToken);
                        MessageSent?.Invoke(this, new MessageEventArgs(message));
                        break;
                }

                return Task.FromResult(0);
            }

            public Task<TInbound> SendRequestAndReceiveResponseAsync<TOutbound, TInbound>(MessageMethod method, TOutbound payload, CancellationToken cancellationToken)
                where TOutbound : class
                where TInbound : class
            {
                throw new NotImplementedException();
            }

            internal void SimulateResponse(Message response)
            {
                MessageReceived?.Invoke(this, new MessageEventArgs(response));
            }
        }

        private sealed class ConnectionEventRegistrationSpy : IConnection
        {
            internal int FaultedAddCount { get; private set; }
            internal int FaultedRemoveCount { get; private set; }
            internal int MessageReceivedAddCount { get; private set; }
            internal int MessageReceivedRemoveCount { get; private set; }

            public IMessageDispatcher MessageDispatcher => throw new NotImplementedException();
            public ConnectionOptions Options => throw new NotImplementedException();
            public SemanticVersion ProtocolVersion => throw new NotImplementedException();

            public event EventHandler<ProtocolErrorEventArgs> Faulted
            {
                add
                {
                    ++FaultedAddCount;
                }
                remove
                {
                    ++FaultedRemoveCount;
                }
            }

            public event EventHandler<MessageEventArgs> MessageReceived
            {
                add
                {
                    ++MessageReceivedAddCount;
                }
                remove
                {
                    ++MessageReceivedRemoveCount;
                }
            }

            public void Dispose()
            {
            }

            public void Close()
            {
            }

            public Task SendAsync(Message message, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }

            public Task<TInbound> SendRequestAndReceiveResponseAsync<TOutbound, TInbound>(MessageMethod method, TOutbound payload, CancellationToken cancellationToken)
                where TOutbound : class
                where TInbound : class
            {
                throw new NotImplementedException();
            }
        }

        private sealed class Request
        {
        }

        private sealed class Response
        {
        }

        private sealed class RequestHandler : IRequestHandler
        {
            public CancellationToken CancellationToken { get; internal set; }

            internal Func<IConnection, Message, IResponseHandler, CancellationToken, Task> HandleResponseAsyncFunc { get; set; }

            internal RequestHandler()
            {
                CancellationToken = CancellationToken.None;
                HandleResponseAsyncFunc = (connection, request, responseHandler, cancellationToken) => throw new NotImplementedException();
            }

            public Task HandleResponseAsync(
                IConnection connection,
                Message request,
                IResponseHandler responseHandler,
                CancellationToken cancellationToken)
            {
                return HandleResponseAsyncFunc(connection, request, responseHandler, cancellationToken);
            }
        }
    }
}
