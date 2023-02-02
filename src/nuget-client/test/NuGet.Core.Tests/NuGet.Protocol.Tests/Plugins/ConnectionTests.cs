// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.Common;
using NuGet.Protocol.Tests;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    using SemanticVersion = Versioning.SemanticVersion;

    [Collection(nameof(NotThreadSafeResourceCollection))]
    public class ConnectionTests
    {
        [Fact]
        public void Constructor_ThrowsForNullDispatcher()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new Connection(
                    dispatcher: null,
                    sender: new Sender(TextWriter.Null),
                    receiver: new StandardInputReceiver(TextReader.Null),
                    options: ConnectionOptions.CreateDefault()));

            Assert.Equal("dispatcher", exception.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsForNullSender()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new Connection(
                    new MessageDispatcher(new RequestHandlers(), new RequestIdGenerator()),
                    sender: null,
                    receiver: new StandardInputReceiver(TextReader.Null),
                    options: ConnectionOptions.CreateDefault()));

            Assert.Equal("sender", exception.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsForNullReceiver()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new Connection(
                new MessageDispatcher(new RequestHandlers(), new RequestIdGenerator()),
                new Sender(TextWriter.Null),
                receiver: null,
                options: ConnectionOptions.CreateDefault()));

            Assert.Equal("receiver", exception.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsForNullOptions()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new Connection(
                    new MessageDispatcher(new RequestHandlers(), new RequestIdGenerator()),
                    new Sender(TextWriter.Null),
                    new StandardInputReceiver(TextReader.Null),
                    options: null));

            Assert.Equal("options", exception.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsForNullLogger()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new Connection(
                    new MessageDispatcher(new RequestHandlers(), new RequestIdGenerator()),
                    new Sender(TextWriter.Null),
                    new StandardInputReceiver(TextReader.Null),
                    ConnectionOptions.CreateDefault(),
                    logger: null));

            Assert.Equal("logger", exception.ParamName);
        }

        [Fact]
        public void Constructor_InitializesProperties()
        {
            var dispatcher = Mock.Of<IMessageDispatcher>();
            var sender = Mock.Of<ISender>();
            var receiver = Mock.Of<IReceiver>();
            var options = ConnectionOptions.CreateDefault();

            using (var connection = new Connection(dispatcher, sender, receiver, options))
            {
                Assert.Same(dispatcher, connection.MessageDispatcher);
                Assert.Same(options, connection.Options);
                Assert.Null(connection.ProtocolVersion);
                Assert.Equal(ConnectionState.ReadyToConnect, connection.State);
            }
        }

        [Fact]
        public void Dispose_IsIdempotent()
        {
            using (var test = new ConnectionTest())
            {
                test.Connection.Dispose();
                test.Connection.Dispose();
            }
        }

        [Fact]
        public void Dispose_ClosesConnection()
        {
            using (var test = new ConnectionTest())
            {
                Assert.Equal(ConnectionState.ReadyToConnect, test.Connection.State);

                test.Connection.Dispose();

                Assert.Equal(ConnectionState.Closed, test.Connection.State);
            }
        }

        [Fact]
        public void Dispose_DisposesDisposables()
        {
            using (var test = new MockConnectionTest())
            {
            }
        }

        [Fact]
        public async Task ConnectAsync_ThrowsIfCancelled()
        {
            using (var cancellationTokenSource = new CancellationTokenSource())
            using (var sender = new Sender(TextWriter.Null))
            using (var receiver = new StandardInputReceiver(TextReader.Null))
            using (var dispatcher = new MessageDispatcher(new RequestHandlers(), new RequestIdGenerator()))
            using (var connection = new Connection(dispatcher, sender, receiver, ConnectionOptions.CreateDefault()))
            {
                await Assert.ThrowsAsync<OperationCanceledException>(
                    () => connection.ConnectAsync(new CancellationToken(canceled: true)));
            }
        }

        [Fact]
        public async Task ConnectAsync_HandshakeNegotiatesProtocolVersionForIdenticalVersionRanges()
        {
            using (var test = new ConnectAsyncTest())
            {
                await test.ConnectAsync();

                Assert.NotNull(test.RemoteToLocalConnection.ProtocolVersion);
                Assert.NotNull(test.LocalToRemoteConnection.ProtocolVersion);
                Assert.Equal(test.RemoteToLocalConnection.ProtocolVersion, test.LocalToRemoteConnection.ProtocolVersion);
            }
        }

        [Fact]
        public async Task ConnectAsync_HandshakeNegotiatesHighestCompatibleProtocolVersion()
        {
            var localToRemoteOptions = new ConnectionOptions(
                new SemanticVersion(2, 0, 0),
                new SemanticVersion(1, 0, 0),
                TimeSpan.FromSeconds(10),
                TimeSpan.FromSeconds(10));
            var remoteToLocalOptions = new ConnectionOptions(
                new SemanticVersion(3, 0, 0),
                new SemanticVersion(1, 0, 0),
                TimeSpan.FromSeconds(10),
                TimeSpan.FromSeconds(10));

            using (var test = new ConnectAsyncTest(localToRemoteOptions, remoteToLocalOptions))
            {
                await test.ConnectAsync();

                Assert.NotNull(test.RemoteToLocalConnection.ProtocolVersion);
                Assert.NotNull(test.LocalToRemoteConnection.ProtocolVersion);
                Assert.Equal(test.RemoteToLocalConnection.ProtocolVersion, test.LocalToRemoteConnection.ProtocolVersion);
                Assert.Equal(test.RemoteToLocalConnection.ProtocolVersion, localToRemoteOptions.ProtocolVersion);
            }
        }

        [Fact]
        public async Task ConnectAsync_HandshakeFailsToNegotiateProtocolVersionIfVersionRangesDoNotOverlap()
        {
            var localToRemoteOptions = new ConnectionOptions(
                new SemanticVersion(2, 0, 0),
                new SemanticVersion(2, 0, 0),
                TimeSpan.FromSeconds(10),
                TimeSpan.FromSeconds(10));
            var remoteToLocalOptions = new ConnectionOptions(
                new SemanticVersion(1, 0, 0),
                new SemanticVersion(1, 0, 0),
                TimeSpan.FromSeconds(10),
                TimeSpan.FromSeconds(10));

            using (var test = new ConnectAsyncTest(localToRemoteOptions, remoteToLocalOptions))
            {
                var tasks = new[]
                {
                    test.RemoteToLocalConnection.ConnectAsync(test.CancellationToken),
                    test.LocalToRemoteConnection.ConnectAsync(test.CancellationToken)
                };

                await Assert.ThrowsAsync<ProtocolException>(() => Task.WhenAll(tasks));

                Assert.NotNull(tasks[0].Exception);
                Assert.NotNull(tasks[1].Exception);

                Assert.Null(test.RemoteToLocalConnection.ProtocolVersion);
                Assert.Equal(ConnectionState.FailedToHandshake, test.RemoteToLocalConnection.State);
                Assert.Null(test.LocalToRemoteConnection.ProtocolVersion);
                Assert.Equal(ConnectionState.FailedToHandshake, test.LocalToRemoteConnection.State);
            }
        }

        [Fact]
        public async Task ConnectAsync_HandshakeFailsToNegotiateProtocolVersionIfOnePartyFailsToRespondWithinTimeoutPeriod()
        {
            var protocolVersion = new SemanticVersion(1, 0, 0);
            var localToRemoteOptions = new ConnectionOptions(
                protocolVersion,
                protocolVersion,
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(10));
            var remoteToLocalOptions = new ConnectionOptions(
                protocolVersion,
                protocolVersion,
                TimeSpan.FromSeconds(10),
                TimeSpan.FromSeconds(10));

            using (var test = new ConnectAsyncTest(localToRemoteOptions, remoteToLocalOptions))
            {
                await Assert.ThrowsAsync<TaskCanceledException>(
                    () => test.LocalToRemoteConnection.ConnectAsync(test.CancellationToken));

                Assert.Null(test.LocalToRemoteConnection.ProtocolVersion);
                Assert.Equal(ConnectionState.FailedToHandshake, test.LocalToRemoteConnection.State);
            }
        }

        [Fact]
        public void Close_ClosesCloseables()
        {
            var dispatcher = new Mock<IMessageDispatcher>(MockBehavior.Strict);
            var sender = new Mock<ISender>(MockBehavior.Strict);
            var receiver = new Mock<IReceiver>(MockBehavior.Strict);

            dispatcher.Setup(x => x.SetConnection(It.IsNotNull<IConnection>()));
            dispatcher.Setup(x => x.Close());
            sender.Setup(x => x.Close());
            receiver.Setup(x => x.Close());

            using (var connection = new Connection(
                dispatcher.Object,
                sender.Object,
                receiver.Object,
                ConnectionOptions.CreateDefault()))
            {
                connection.Close();

                sender.Verify();
                receiver.Verify();
                dispatcher.Verify();

                dispatcher.Setup(x => x.Dispose());
                sender.Setup(x => x.Dispose());
                receiver.Setup(x => x.Dispose());
            }
        }

        [Fact]
        public async Task Close_SetsStateToClosed()
        {
            using (var test = new ConnectAsyncTest())
            {
                await test.ConnectAsync();

                Assert.Equal(ConnectionState.Connected, test.LocalToRemoteConnection.State);

                test.LocalToRemoteConnection.Close();

                Assert.Equal(ConnectionState.Closed, test.LocalToRemoteConnection.State);
            }
        }

        [Fact]
        public async Task Faulted_RaisedForProtocolError()
        {
            using (var test = new ConnectAsyncTest())
            {
                await test.ConnectAsync();

                using (var faultedEvent = new ManualResetEventSlim(initialState: false))
                {
                    ProtocolErrorEventArgs args = null;

                    test.LocalToRemoteConnection.Faulted += (object sender, ProtocolErrorEventArgs e) =>
                    {
                        args = e;

                        faultedEvent.Set();
                    };

                    test.LocalToRemoteConnection.MessageDispatcher.RequestHandlers.TryAdd(
                        MessageMethod.Initialize,
                        new RequestHandler<InitializeResponse>(new InitializeResponse(MessageResponseCode.Success)));

                    var message = new Message(
                        requestId: "a",
                        type: MessageType.Response,
                        method: MessageMethod.Initialize);

                    await test.RemoteToLocalConnection.SendAsync(message, test.CancellationToken);

                    faultedEvent.Wait();

                    Assert.NotNull(args);
                    Assert.IsType<ProtocolException>(args.Exception);
                    Assert.NotNull(args.Message);
                    Assert.Equal(message.RequestId, args.Message.RequestId);
                    Assert.Equal(message.Type, args.Message.Type);
                    Assert.Equal(message.Method, args.Message.Method);
                }
            }
        }

        [Fact]
        public async Task MessageReceived_RaisedForInboundMessage()
        {
            using (var test = new ConnectAsyncTest())
            {
                await test.ConnectAsync();

                using (var messageReceivedEvent = new ManualResetEventSlim(initialState: false))
                {
                    MessageEventArgs args = null;

                    test.LocalToRemoteConnection.MessageReceived += (object sender, MessageEventArgs e) =>
                    {
                        args = e;

                        messageReceivedEvent.Set();
                    };

                    test.LocalToRemoteConnection.MessageDispatcher.RequestHandlers.TryAdd(
                        MessageMethod.Initialize,
                        new RequestHandler<InitializeResponse>(new InitializeResponse(MessageResponseCode.Success)));

                    var message = new Message(
                        requestId: "a",
                        type: MessageType.Request,
                        method: MessageMethod.Initialize);

                    await test.RemoteToLocalConnection.SendAsync(message, test.CancellationToken);

                    messageReceivedEvent.Wait();

                    Assert.NotNull(args);
                    Assert.NotNull(args.Message);
                    Assert.Equal(message.RequestId, args.Message.RequestId);
                    Assert.Equal(message.Type, args.Message.Type);
                    Assert.Equal(message.Method, args.Message.Method);
                }
            }
        }

        [Fact]
        public async Task SendAsync_ThrowsForNullMessage()
        {
            using (var test = new ConnectionTest())
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => test.Connection.SendAsync(message: null, cancellationToken: CancellationToken.None));

                Assert.Equal("message", exception.ParamName);
            }
        }

        [Fact]
        public async Task SendAsync_ThrowsIfCancelled()
        {
            var message = new Message(
                requestId: "a",
                type: MessageType.Request,
                method: MessageMethod.Initialize);

            using (var test = new ConnectionTest())
            {
                await Assert.ThrowsAsync<OperationCanceledException>(
                    () => test.Connection.SendAsync(message, new CancellationToken(canceled: true)));
            }
        }

        [Fact]
        public async Task SendAsync_ThrowsIfNotConnected()
        {
            var message = new Message(
                requestId: "a",
                type: MessageType.Request,
                method: MessageMethod.Initialize);

            using (var test = new ConnectionTest())
            {
                await Assert.ThrowsAsync<InvalidOperationException>(
                    () => test.Connection.SendAsync(message, CancellationToken.None));
            }
        }

        [Fact]
        public async Task SendAsync_NoOpsIfClosed()
        {
            using (var test = new MockConnectionTest())
            {
                var message = new Message(
                    requestId: "a",
                    type: MessageType.Request,
                    method: MessageMethod.Initialize);

                test.Connection.Close();

                await test.Connection.SendAsync(message, CancellationToken.None);
            }
        }

        [Fact]
        public async Task SendRequestAndReceiveResponseAsync_ThrowsIfNotConnected()
        {
            using (var test = new ConnectionTest())
            {
                await Assert.ThrowsAsync<InvalidOperationException>(
                    () => test.Connection.SendRequestAndReceiveResponseAsync<HandshakeRequest, HandshakeResponse>(
                        MessageMethod.Handshake,
                        new HandshakeRequest(ProtocolConstants.CurrentVersion, ProtocolConstants.CurrentVersion),
                        CancellationToken.None));
            }
        }

        [Fact]
        public async Task SendRequestAndReceiveResponseAsync_ThrowsIfCancelled()
        {
            using (var test = new ConnectAsyncTest())
            {
                await test.ConnectAsync();

                await Assert.ThrowsAsync<OperationCanceledException>(
                    () => test.LocalToRemoteConnection.SendRequestAndReceiveResponseAsync<LogRequest, LogResponse>(
                        MessageMethod.Log,
                        new LogRequest(LogLevel.Debug, "a"),
                        new CancellationToken(canceled: true)));
            }
        }

        [Fact]
        public async Task SendRequestAndReceiveResponseAsync_NoOpsIfClosed()
        {
            using (var test = new MockConnectionTest())
            {
                var message = new Message(
                    requestId: "a",
                    type: MessageType.Request,
                    method: MessageMethod.Initialize);

                test.Connection.Close();

                var response = await test.Connection.SendRequestAndReceiveResponseAsync<HandshakeRequest, HandshakeResponse>(
                    MessageMethod.Handshake,
                    new HandshakeRequest(ProtocolConstants.CurrentVersion, ProtocolConstants.CurrentVersion),
                    CancellationToken.None);

                Assert.Null(response);
            }
        }

        private sealed class ConnectAsyncTest : IDisposable
        {
            private readonly CancellationTokenSource _combinedCancellationTokenSource;
            private readonly SimulatedIpc _simulatedIpc;
            private readonly Sender _remoteSender;
            private readonly Receiver _remoteReceiver;
            private readonly MessageDispatcher _remoteDispatcher;
            private readonly Sender _localSender;
            private readonly Receiver _localReceiver;
            private readonly MessageDispatcher _localDispatcher;
            private bool _isDisposed;

            internal Connection RemoteToLocalConnection { get; }
            internal Connection LocalToRemoteConnection { get; }
            internal TestPluginLogger Logger { get; }
            internal CancellationToken CancellationToken { get; }

            internal ConnectAsyncTest()
                : this(CreateOptions(), CreateOptions())
            {
            }

            internal ConnectAsyncTest(ConnectionOptions localToRemoteOptions, ConnectionOptions remoteToLocalOptions)
            {
                Logger = new TestPluginLogger();
                var localLogger = Logger.CreateLogger("A");
                _combinedCancellationTokenSource = new CancellationTokenSource();
                _simulatedIpc = SimulatedIpc.Create(_combinedCancellationTokenSource.Token);
                _remoteSender = new Sender(_simulatedIpc.RemoteStandardOutputForRemote);
                _remoteReceiver = new StandardInputReceiver(_simulatedIpc.RemoteStandardInputForRemote);
                _remoteDispatcher = new MessageDispatcher(new RequestHandlers(), new RequestIdGenerator(), new InboundRequestProcessingHandler(Enumerable.Empty<MessageMethod>()), localLogger);
                LocalToRemoteConnection = new Connection(_remoteDispatcher, _remoteSender, _remoteReceiver, localToRemoteOptions, localLogger);

                var remoteLogger = Logger.CreateLogger("B");
                _localSender = new Sender(_simulatedIpc.RemoteStandardInputForLocal);
                _localReceiver = new StandardInputReceiver(_simulatedIpc.RemoteStandardOutputForLocal);
                _localDispatcher = new MessageDispatcher(new RequestHandlers(), new RequestIdGenerator(), new InboundRequestProcessingHandler(Enumerable.Empty<MessageMethod>()), remoteLogger);
                RemoteToLocalConnection = new Connection(_localDispatcher, _localSender, _localReceiver, remoteToLocalOptions, remoteLogger);
                CancellationToken = _combinedCancellationTokenSource.Token;
            }

            public void Dispose()
            {
                if (_isDisposed)
                {
                    return;
                }

                using (_combinedCancellationTokenSource)
                {
                    _combinedCancellationTokenSource.Cancel();

                    LocalToRemoteConnection.Dispose();
                    RemoteToLocalConnection.Dispose();
                    _simulatedIpc.Dispose();

                    // Other IDisposable fields should be disposed by the connections.
                }

                _isDisposed = true;
            }

            internal async Task ConnectAsync()
            {
                try
                {
                    await Task.WhenAll(
                        RemoteToLocalConnection.ConnectAsync(CancellationToken),
                        LocalToRemoteConnection.ConnectAsync(CancellationToken));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    Console.WriteLine();
                    Console.WriteLine(Logger.Log);

                    throw;
                }
            }

            private static ConnectionOptions CreateOptions()
            {
                return new ConnectionOptions(
                    protocolVersion: ProtocolConstants.CurrentVersion,
                    minimumProtocolVersion: ProtocolConstants.Version100,
                    handshakeTimeout: TimeSpan.FromSeconds(30),
                    requestTimeout: ProtocolConstants.RequestTimeout);
            }
        }

        private sealed class ConnectionTest : IDisposable
        {
            private readonly SimulatedIpc _simulatedIpc;
            private bool _isDisposed;

            internal Connection Connection { get; }

            internal ConnectionTest()
            {
                var cancellationTokenSource = new CancellationTokenSource();
                _simulatedIpc = SimulatedIpc.Create(cancellationTokenSource.Token);
                var sender = new Sender(_simulatedIpc.RemoteStandardOutputForRemote);
                var receiver = new StandardInputReceiver(_simulatedIpc.RemoteStandardInputForRemote);
                var dispatcher = new MessageDispatcher(new RequestHandlers(), new RequestIdGenerator());
                var options = ConnectionOptions.CreateDefault();
                Connection = new Connection(dispatcher, sender, receiver, options);
            }

            public void Dispose()
            {
                if (_isDisposed)
                {
                    return;
                }

                _simulatedIpc.Dispose();
                Connection.Dispose();

                GC.SuppressFinalize(this);

                _isDisposed = true;
            }
        }

        private sealed class MockConnectionTest : IDisposable
        {
            internal Connection Connection { get; }
            internal Mock<IMessageDispatcher> Dispatcher { get; }
            internal Mock<IPluginLogger> Logger { get; }
            internal Mock<ISender> Sender { get; }
            internal Mock<IReceiver> Receiver { get; }

            internal MockConnectionTest()
            {
                Dispatcher = new Mock<IMessageDispatcher>(MockBehavior.Strict);
                Logger = new Mock<IPluginLogger>(MockBehavior.Strict);
                Sender = new Mock<ISender>(MockBehavior.Strict);
                Receiver = new Mock<IReceiver>(MockBehavior.Strict);

                Dispatcher.Setup(x => x.SetConnection(It.IsNotNull<IConnection>()));
                Dispatcher.Setup(x => x.Close());
                Dispatcher.Setup(x => x.Dispose());
                Sender.Setup(x => x.Close());
                Sender.Setup(x => x.Dispose());
                Receiver.Setup(x => x.Close());
                Receiver.Setup(x => x.Dispose());

                Connection = new Connection(
                    Dispatcher.Object,
                    Sender.Object,
                    Receiver.Object,
                    ConnectionOptions.CreateDefault(),
                    Logger.Object);
            }

            public void Dispose()
            {
                Connection.Dispose();

                Dispatcher.Verify();
                Logger.Verify(); // The logger should not be disposed.  The lack of a call to Setup(...) verifies this.
                Sender.Verify();
                Receiver.Verify();
            }
        }

        private sealed class RequestHandler<TPayload> : IRequestHandler
            where TPayload : class
        {
            private readonly TPayload _payload;

            public CancellationToken CancellationToken => CancellationToken.None;

            internal RequestHandler(TPayload payload)
            {
                _payload = payload;
            }

            public Task HandleResponseAsync(
                IConnection connection,
                Message request,
                IResponseHandler responseHandler,
                CancellationToken cancellationToken)
            {
                return responseHandler.SendResponseAsync(request, _payload, cancellationToken);
            }
        }

        private sealed class TestPluginLogger
        {
            private readonly object _lockObject;
            private readonly StringBuilder _log;
            private readonly DateTimeOffset _startTime;
            private readonly Stopwatch _stopwatch;

            public bool IsEnabled => true;
            public string Log => _log.ToString();
            public DateTimeOffset Now => _startTime.AddTicks(_stopwatch.ElapsedTicks);

            internal TestPluginLogger()
            {
                _lockObject = new object();
                _log = new StringBuilder();
                _startTime = DateTimeOffset.UtcNow;
                _stopwatch = Stopwatch.StartNew();

                Write(new AssemblyLogMessage(Now).ToString());
                Write(new MachineLogMessage(Now).ToString());
                Write(new EnvironmentVariablesLogMessage(Now).ToString());
                Write(new ProcessLogMessage(Now).ToString());
                Write(new ThreadPoolLogMessage(Now).ToString());
            }

            internal IPluginLogger CreateLogger(string tagName)
            {
                return new PluginLogger(this, tagName);
            }

            private void Write(string message)
            {
                lock (_lockObject)
                {
                    _log.AppendLine(message);
                }
            }

            private sealed class PluginLogger : IPluginLogger
            {
                private readonly TestPluginLogger _pluginLogger;
                private readonly string _tagName;

                public bool IsEnabled => _pluginLogger.IsEnabled;

                public DateTimeOffset Now => _pluginLogger.Now;

                internal PluginLogger(TestPluginLogger pluginLogger, string tagName)
                {
                    _pluginLogger = pluginLogger;
                    _tagName = tagName;
                }

                public void Dispose()
                {
                }

                public void Write(IPluginLogMessage message)
                {
                    _pluginLogger.Write($"{_tagName}:  {message.ToString()}");
                }
            }
        }
    }
}
