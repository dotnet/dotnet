// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Threading;
using Moq;
using NuGet.Common;
using NuGet.Shared;
using Test.Utility;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class StandardOutputReceiverTests
    {
        private static IEnvironmentVariableReader CreateEnvReader(bool useStj) =>
            useStj
                ? new TestEnvironmentVariableReader(
                    new Dictionary<string, string> { [NuGetFeatureFlags.UseSystemTextJsonDeserializationEnvVar] = "true" })
                : TestEnvironmentVariableReader.EmptyInstance;

        [Fact]
        public void Constructor_ThrowsForNullProcess()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new StandardOutputReceiver(
                    process: null));

            Assert.Equal("process", exception.ParamName);
        }

        [Fact]
        public void Dispose_CancelsReading()
        {
            var process = new Mock<IPluginProcess>(MockBehavior.Strict);

            process.Setup(x => x.CancelRead());

            using (var receiver = new StandardOutputReceiver(process.Object))
            {
            }

            process.Verify(x => x.CancelRead(), Times.Once);
        }

        [Fact]
        public void Dispose_IsIdempotent()
        {
            using (var receiver = new StandardOutputReceiver(Mock.Of<IPluginProcess>()))
            {
                receiver.Dispose();
                receiver.Dispose();
            }
        }

        [Fact]
        public void Connect_ThrowsIfDisposed()
        {
            var receiver = new StandardOutputReceiver(Mock.Of<IPluginProcess>());

            receiver.Dispose();

            var exception = Assert.Throws<ObjectDisposedException>(() => receiver.Connect());

            Assert.Equal(nameof(StandardOutputReceiver), exception.ObjectName);
        }

        [Fact]
        public void Connect_ThrowsIfClosed()
        {
            using (var receiver = new StandardOutputReceiver(Mock.Of<IPluginProcess>()))
            {
                receiver.Close();

                Assert.Throws<InvalidOperationException>(() => receiver.Connect());
            }
        }

        [Fact]
        public void Connect_ThrowsIfAlreadyConnected()
        {
            using (var receiver = new StandardOutputReceiver(Mock.Of<IPluginProcess>()))
            {
                receiver.Connect();

                Assert.Throws<InvalidOperationException>(() => receiver.Connect());
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void MessageReceived_RaisedForSingleMessage(bool useStj)
        {
            var json = "{\"RequestId\":\"a\",\"Type\":\"Response\",\"Method\":\"None\"}";
            var requestId = "a";
            var type = MessageType.Response;
            var method = MessageMethod.None;
            var process = new Mock<IPluginProcess>();

            process.Setup(x => x.BeginReadLine())
                .Callback(() => process.Raise(x => x.LineRead += null, new LineReadEventArgs(json)));

            using (var receivedEvent = new ManualResetEventSlim(initialState: false))
            using (var receiver = new StandardOutputReceiver(process.Object, CreateEnvReader(useStj)))
            {
                MessageEventArgs args = null;

                receiver.MessageReceived += (object sender, MessageEventArgs e) =>
                {
                    args = e;

                    receivedEvent.Set();
                };

                receiver.Connect();

                receivedEvent.Wait();

                Assert.NotNull(args);
                Assert.NotNull(args.Message);
                Assert.Equal(requestId, args.Message.RequestId);
                Assert.Equal(type, args.Message.Type);
                Assert.Equal(method, args.Message.Method);
                Assert.Null(MessageUtilities.SerializePayload(args.Message));
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void MessageReceived_HandlesNullAndEmptyString(bool useStj)
        {
            var json = "{\"RequestId\":\"a\",\"Type\":\"Response\",\"Method\":\"None\"}";
            var requestId = "a";
            var type = MessageType.Response;
            var method = MessageMethod.None;
            var process = new Mock<IPluginProcess>();

            process.Setup(x => x.BeginReadLine())
                .Callback(() =>
                {
                    // We can't directly verify handling of null and empty string except by
                    // passing a valid line after them and ensuring it gets processed correctly.
                    process.Raise(x => x.LineRead += null, new LineReadEventArgs(null));
                    process.Raise(x => x.LineRead += null, new LineReadEventArgs(""));
                    process.Raise(x => x.LineRead += null, new LineReadEventArgs(json));
                });

            using (var receivedEvent = new ManualResetEventSlim(initialState: false))
            using (var receiver = new StandardOutputReceiver(process.Object, CreateEnvReader(useStj)))
            {
                MessageEventArgs args = null;

                receiver.MessageReceived += (object sender, MessageEventArgs e) =>
                {
                    args = e;

                    receivedEvent.Set();
                };

                receiver.Connect();

                receivedEvent.Wait();

                Assert.NotNull(args);
                Assert.NotNull(args.Message);
                Assert.Equal(requestId, args.Message.RequestId);
                Assert.Equal(type, args.Message.Type);
                Assert.Equal(method, args.Message.Method);
                Assert.Null(MessageUtilities.SerializePayload(args.Message));
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Faulted_RaisedForParseError(bool useStj)
        {
            var invalidJson = "text";
            var process = new Mock<IPluginProcess>();

            process.Setup(x => x.BeginReadLine())
                .Callback(() => process.Raise(x => x.LineRead += null, new LineReadEventArgs(invalidJson)));

            using (var faultedEvent = new ManualResetEventSlim(initialState: false))
            using (var receiver = new StandardOutputReceiver(process.Object, CreateEnvReader(useStj)))
            {
                ProtocolErrorEventArgs args = null;

                receiver.Faulted += (object sender, ProtocolErrorEventArgs e) =>
                {
                    args = e;

                    faultedEvent.Set();
                };

                receiver.Connect();

                faultedEvent.Wait();

                Assert.NotNull(args);
                Assert.NotNull(args.Exception);
                Assert.IsType<ProtocolException>(args.Exception);
                Assert.Null(args.Message);
            }
        }

        [Theory]
        [InlineData("1", false)]
        [InlineData("1", true)]
        [InlineData("[]", false)]
        [InlineData("[]", true)]
        public void Faulted_RaisedForDeserializationOfInvalidJson(string invalidJson, bool useStj)
        {
            var process = new Mock<IPluginProcess>();

            process.Setup(x => x.BeginReadLine())
                .Callback(() => process.Raise(x => x.LineRead += null, new LineReadEventArgs(invalidJson)));

            using (var faultedEvent = new ManualResetEventSlim(initialState: false))
            using (var receiver = new StandardOutputReceiver(process.Object, CreateEnvReader(useStj)))
            {
                ProtocolErrorEventArgs args = null;

                receiver.Faulted += (object sender, ProtocolErrorEventArgs e) =>
                {
                    args = e;

                    faultedEvent.Set();
                };

                receiver.Connect();

                faultedEvent.Wait();

                Assert.NotNull(args);
                Assert.NotNull(args.Exception);
                Assert.IsType<ProtocolException>(args.Exception);
                Assert.Null(args.Message);
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Faulted_RaisedForDeserializationError(bool useStj)
        {
            var json = "{\"RequestId\":\"a\",\"Type\":\"Response\",\"Method\":\"None\",\"Payload\":\"{\\\"d\\\":\\\"e\\\"}\"}\r\n";

            var process = new Mock<IPluginProcess>();

            process.Setup(x => x.BeginReadLine())
                .Callback(() => process.Raise(x => x.LineRead += null, new LineReadEventArgs(json)));

            using (var faultedEvent = new ManualResetEventSlim(initialState: false))
            using (var receiver = new StandardOutputReceiver(process.Object, CreateEnvReader(useStj)))
            {
                ProtocolErrorEventArgs args = null;

                receiver.Faulted += (object sender, ProtocolErrorEventArgs e) =>
                {
                    args = e;

                    faultedEvent.Set();
                };

                receiver.Connect();

                faultedEvent.Wait();

                Assert.NotNull(args);
                Assert.NotNull(args.Exception);
                Assert.IsType<ProtocolException>(args.Exception);
                Assert.Null(args.Message);
            }
        }

        [Theory]
        [InlineData("{\"Type\":\"Response\",\"Method\":\"None\"}\r\n", false)]
        [InlineData("{\"Type\":\"Response\",\"Method\":\"None\"}\r\n", true)]
        [InlineData("{\"RequestId\":null,\"Type\":\"Response\",\"Method\":\"None\"}\r\n", false)]
        [InlineData("{\"RequestId\":null,\"Type\":\"Response\",\"Method\":\"None\"}\r\n", true)]
        [InlineData("{\"RequestId\":\"\",\"Type\":\"Response\",\"Method\":\"None\"}\r\n", false)]
        [InlineData("{\"RequestId\":\"\",\"Type\":\"Response\",\"Method\":\"None\"}\r\n", true)]
        [InlineData("{\"RequestId\":\"a\",\"Method\":\"None\"}\r\n", false)]
        [InlineData("{\"RequestId\":\"a\",\"Method\":\"None\"}\r\n", true)]
        [InlineData("{\"RequestId\":\"a\",\"Type\":null,\"Method\":\"None\"}\r\n", false)]
        [InlineData("{\"RequestId\":\"a\",\"Type\":null,\"Method\":\"None\"}\r\n", true)]
        [InlineData("{\"RequestId\":\"a\",\"Type\":\"\",\"Method\":\"None\"}\r\n", false)]
        [InlineData("{\"RequestId\":\"a\",\"Type\":\"\",\"Method\":\"None\"}\r\n", true)]
        [InlineData("{\"RequestId\":\"a\",\"Type\":\" \",\"Method\":\"None\"}\r\n", false)]
        [InlineData("{\"RequestId\":\"a\",\"Type\":\" \",\"Method\":\"None\"}\r\n", true)]
        [InlineData("{\"RequestId\":\"a\",\"Type\":\"abc\",\"Method\":\"None\"}\r\n", false)]
        [InlineData("{\"RequestId\":\"a\",\"Type\":\"abc\",\"Method\":\"None\"}\r\n", true)]
        [InlineData("{\"RequestId\":\"a\",\"Type\":\"Response\"}\r\n", false)]
        [InlineData("{\"RequestId\":\"a\",\"Type\":\"Response\"}\r\n", true)]
        [InlineData("{\"RequestId\":\"a\",\"Type\":\"Response\",\"Method\":null}\r\n", false)]
        [InlineData("{\"RequestId\":\"a\",\"Type\":\"Response\",\"Method\":null}\r\n", true)]
        [InlineData("{\"RequestId\":\"a\",\"Type\":\"Response\",\"Method\":\"\"}\r\n", false)]
        [InlineData("{\"RequestId\":\"a\",\"Type\":\"Response\",\"Method\":\"\"}\r\n", true)]
        [InlineData("{\"RequestId\":\"a\",\"Type\":\"Response\",\"Method\":\"abc\"}\r\n", false)]
        [InlineData("{\"RequestId\":\"a\",\"Type\":\"Response\",\"Method\":\"abc\"}\r\n", true)]
        public void Faulted_RaisedForInvalidMessage(string json, bool useStj)
        {
            var process = new Mock<IPluginProcess>();

            process.Setup(x => x.BeginReadLine())
                .Callback(() => process.Raise(x => x.LineRead += null, new LineReadEventArgs(json)));

            using (var faultedEvent = new ManualResetEventSlim(initialState: false))
            using (var receiver = new StandardOutputReceiver(process.Object, CreateEnvReader(useStj)))
            {
                ProtocolErrorEventArgs args = null;

                receiver.Faulted += (object sender, ProtocolErrorEventArgs e) =>
                {
                    args = e;

                    faultedEvent.Set();
                };

                receiver.Connect();

                faultedEvent.Wait();

                Assert.NotNull(args);
                Assert.IsType<ProtocolException>(args.Exception);
            }
        }

        [Fact]
        public void Close_IsIdempotent()
        {
            using (var receiver = new StandardOutputReceiver(Mock.Of<IPluginProcess>()))
            {
                receiver.Connect();

                receiver.Close();
                receiver.Close();
            }
        }

        [Fact]
        public void Close_CanBeCalledWithoutConnectAsync()
        {
            using (var receiver = new StandardOutputReceiver(Mock.Of<IPluginProcess>()))
            {
                receiver.Close();
            }
        }

        [Fact]
        public void Close_CancelsReading()
        {
            var process = new Mock<IPluginProcess>(MockBehavior.Strict);

            process.Setup(x => x.BeginReadLine());
            process.Setup(x => x.CancelRead());

            using (var receiver = new StandardOutputReceiver(process.Object))
            {
                receiver.Connect();
                receiver.Close();
            }

            process.Verify(x => x.CancelRead(), Times.Once);
        }
    }
}
