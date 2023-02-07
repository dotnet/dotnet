// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    using SemanticVersion = Versioning.SemanticVersion;

    public class SymmetricHandshakeTests
    {
        private static readonly SemanticVersion _version1_0_0 = new SemanticVersion(major: 1, minor: 0, patch: 0);
        private static readonly SemanticVersion _version2_0_0 = new SemanticVersion(major: 2, minor: 0, patch: 0);
        private static readonly SemanticVersion _version3_0_0 = new SemanticVersion(major: 3, minor: 0, patch: 0);
        private static readonly SemanticVersion _version4_0_0 = new SemanticVersion(major: 4, minor: 0, patch: 0);

        private readonly Mock<IConnection> _connection;
        private readonly Mock<IMessageDispatcher> _messageDispatcher;
        private readonly RequestHandlers _handlers;

        public SymmetricHandshakeTests()
        {
            _connection = new Mock<IConnection>();
            _messageDispatcher = new Mock<IMessageDispatcher>();
            _handlers = new RequestHandlers();
        }

        [Fact]
        public void Constructor_ThrowsForNullConnection()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new SymmetricHandshake(
                    connection: null,
                    handshakeTimeout: TimeSpan.FromSeconds(10),
                    protocolVersion: _version1_0_0,
                    minimumProtocolVersion: _version1_0_0));

            Assert.Equal("connection", exception.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsForNullProtocolVersion()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new SymmetricHandshake(
                    Mock.Of<IConnection>(),
                    TimeSpan.FromSeconds(10),
                    protocolVersion: null,
                    minimumProtocolVersion: _version1_0_0));

            Assert.Equal("protocolVersion", exception.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsForZeroHandshakeTimeout()
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(
                () => new SymmetricHandshake(Mock.Of<IConnection>(), TimeSpan.Zero, _version1_0_0, _version1_0_0));

            Assert.Equal("handshakeTimeout", exception.ParamName);
            Assert.Equal(TimeSpan.Zero, exception.ActualValue);
        }

        [Fact]
        public void Constructor_ThrowsForNegativeHandshakeTimeout()
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(
                () => new SymmetricHandshake(
                    Mock.Of<IConnection>(),
                    TimeSpan.FromSeconds(-1),
                    _version1_0_0,
                    _version1_0_0));

            Assert.Equal("handshakeTimeout", exception.ParamName);
            Assert.Equal(TimeSpan.FromSeconds(-1), exception.ActualValue);
        }

        [Fact]
        public void Constructor_ThrowsForTooLargeHandshakeTimeout()
        {
            var milliseconds = int.MaxValue + 1L;

            var exception = Assert.Throws<ArgumentOutOfRangeException>(
                () => new SymmetricHandshake(
                    Mock.Of<IConnection>(),
                    TimeSpan.FromMilliseconds(milliseconds),
                    _version1_0_0,
                    _version1_0_0));

            Assert.Equal("handshakeTimeout", exception.ParamName);
            Assert.Equal(TimeSpan.FromMilliseconds(milliseconds), exception.ActualValue);
        }

        [Fact]
        public void Constructor_ThrowsForNullMinimumProtocolVersion()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new SymmetricHandshake(
                    Mock.Of<IConnection>(),
                    TimeSpan.FromSeconds(10),
                    protocolVersion: ProtocolConstants.CurrentVersion,
                    minimumProtocolVersion: null));

            Assert.Equal("minimumProtocolVersion", exception.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsIfHandshakeRequestHandlerAlreadyExists()
        {
            Assert.True(_handlers.TryAdd(MessageMethod.Handshake, Mock.Of<IRequestHandler>()));

            var exception = Assert.Throws<ArgumentException>(() => CreateHandshake());

            Assert.Equal("connection", exception.ParamName);
        }

        [Fact]
        public void Constructor_AddsItselfAsResponseHandler()
        {
            IRequestHandler handler;

            Assert.False(_handlers.TryGet(MessageMethod.Handshake, out handler));

            using (var handshake = CreateHandshake())
            {
                Assert.True(_handlers.TryGet(MessageMethod.Handshake, out handler));
                Assert.Same(handshake, handler);
            }
        }

        [Fact]
        public void Dispose_IsIdempotent()
        {
            using (var handshake = CreateHandshake())
            {
                handshake.Dispose();
                handshake.Dispose();
            }
        }

        [Fact]
        public void Dispose_RemovesItselfAsResponseHandler()
        {
            using (var handshake = CreateHandshake())
            {
            }

            IRequestHandler handler;

            Assert.False(_handlers.TryGet(MessageMethod.Handshake, out handler));
        }

        [Fact]
        public async Task HandleRequestAsync_ThrowsForNullConnection()
        {
            using (var handshake = CreateHandshake())
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => handshake.HandleResponseAsync(
                        connection: null,
                        request: new Message(
                            requestId: "a",
                            type: MessageType.Request,
                            method: MessageMethod.Handshake),
                        responseHandler: Mock.Of<IResponseHandler>(),
                        cancellationToken: CancellationToken.None));

                Assert.Equal("connection", exception.ParamName);
            }
        }

        [Fact]
        public async Task HandleRequestAsync_ThrowsForNullRequest()
        {
            using (var handshake = CreateHandshake())
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => handshake.HandleResponseAsync(
                        Mock.Of<IConnection>(),
                        request: null,
                        responseHandler: Mock.Of<IResponseHandler>(),
                        cancellationToken: CancellationToken.None));

                Assert.Equal("request", exception.ParamName);
            }
        }

        [Fact]
        public async Task HandleRequestAsync_ThrowsForNullResponseHandler()
        {
            using (var handshake = CreateHandshake())
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => handshake.HandleResponseAsync(
                        Mock.Of<IConnection>(),
                        new Message(requestId: "a", type: MessageType.Request, method: MessageMethod.Handshake),
                        responseHandler: null,
                        cancellationToken: CancellationToken.None));

                Assert.Equal("responseHandler", exception.ParamName);
            }
        }

        [Fact]
        public async Task HandleRequestAsync_ThrowsIfCancelled()
        {
            using (var handshake = CreateHandshake())
            {
                await Assert.ThrowsAsync<OperationCanceledException>(
                    () => handshake.HandleResponseAsync(
                        Mock.Of<IConnection>(),
                        new Message(requestId: "a", type: MessageType.Request, method: MessageMethod.Initialize),
                        Mock.Of<IResponseHandler>(),
                        new CancellationToken(canceled: true)));
            }
        }

        [Fact]
        public async Task HandleRequestAsync_SucceedsIfProtocolVersionIsSame()
        {
            await Verify(_version4_0_0, _version3_0_0, _version4_0_0);
        }

        [Fact]
        public async Task HandleRequestAsync_SucceedsIfHighestCommonDenominatorVersionExists()
        {
            await Verify(_version3_0_0, _version2_0_0, _version3_0_0);
        }

        [Fact]
        public async Task HandleRequestAsync_FailsIfNoCompatibleVersionExists()
        {
            await Verify(_version1_0_0, _version1_0_0, negotiatedProtocolVersion: null);
        }

        private async Task Verify(
            SemanticVersion requestedProtocolVersion,
            SemanticVersion requestedMinimumProtocolVersion,
            SemanticVersion negotiatedProtocolVersion)
        {
            using (var handshake = CreateHandshake())
            {
                var responseHandler = new Mock<IResponseHandler>(MockBehavior.Strict);

                responseHandler.Setup(x => x.SendResponseAsync(
                        It.IsNotNull<Message>(),
                        It.IsNotNull<HandshakeResponse>(),
                        It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(0));

                var request = new HandshakeRequest(requestedProtocolVersion, requestedMinimumProtocolVersion);
                var inboundMessage = new Message(
                    requestId: "a",
                    type: MessageType.Request,
                    method: MessageMethod.Handshake,
                    payload: JsonSerializationUtilities.FromObject(request));

                await handshake.HandleResponseAsync(
                    Mock.Of<IConnection>(),
                    inboundMessage,
                    responseHandler.Object,
                    CancellationToken.None);

                var expectedResponseCode = negotiatedProtocolVersion == null
                    ? MessageResponseCode.Error : MessageResponseCode.Success;

                responseHandler.Verify(x => x.SendResponseAsync(
                    It.Is<Message>(message => message == inboundMessage),
                    It.Is<HandshakeResponse>(response => response.ResponseCode == expectedResponseCode &&
                        response.ProtocolVersion == negotiatedProtocolVersion),
                    It.Is<CancellationToken>(token => !token.IsCancellationRequested)),
                    Times.Once);
            }
        }

        private SymmetricHandshake CreateHandshake()
        {
            _messageDispatcher.Setup(x => x.RequestHandlers)
                .Returns(_handlers);

            _connection.Setup(x => x.MessageDispatcher)
                .Returns(_messageDispatcher.Object);

            return new SymmetricHandshake(_connection.Object, TimeSpan.FromSeconds(1), _version4_0_0, _version3_0_0);
        }
    }
}
