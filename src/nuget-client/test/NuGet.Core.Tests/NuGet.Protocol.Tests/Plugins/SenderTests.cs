// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class SenderTests
    {
        private readonly Message _message = new Message(requestId: "a", type: MessageType.Request, method: MessageMethod.None);

        [Fact]
        public void Constructor_ThrowsForNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new Sender(writer: null));

            Assert.Equal("writer", exception.ParamName);
        }

        [Fact]
        public void Constructor_AllowsTextWriterNull()
        {
            new Sender(TextWriter.Null);
        }

        [Fact]
        public void Dispose_ClosesUnderlyingStream()
        {
            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream))
            {
                Assert.True(stream.CanSeek);
                Assert.True(stream.CanRead);
                Assert.True(stream.CanWrite);

                var sender = new Sender(writer);

                sender.Dispose();

                Assert.False(stream.CanSeek);
                Assert.False(stream.CanRead);
                Assert.False(stream.CanWrite);
            }
        }

        [Fact]
        public void Dispose_IsIdempotent()
        {
            using (var sender = new Sender(TextWriter.Null))
            {
                sender.Dispose();
                sender.Dispose();
            }
        }

        [Fact]
        public void Connect_ThrowsIfDisposed()
        {
            var sender = new Sender(TextWriter.Null);

            sender.Dispose();

            var exception = Assert.Throws<ObjectDisposedException>(() => sender.Connect());

            Assert.Equal(nameof(Sender), exception.ObjectName);
        }

        [Fact]
        public void Connect_ThrowsIfClosed()
        {
            using (var sender = new Sender(TextWriter.Null))
            {
                sender.Close();

                Assert.Throws<InvalidOperationException>(() => sender.Connect());
            }
        }

        [Fact]
        public void Connect_ThrowsIfAlreadyConnected()
        {
            using (var sender = new Sender(TextWriter.Null))
            {
                sender.Connect();

                Assert.Throws<InvalidOperationException>(() => sender.Connect());
            }
        }

        [Fact]
        public async Task SendAsync_ThrowsForNull()
        {
            using (var sender = new Sender(TextWriter.Null))
            {
                sender.Connect();

                var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                    () => sender.SendAsync(message: null, cancellationToken: CancellationToken.None));

                Assert.Equal("message", exception.ParamName);
            }
        }

        [Fact]
        public async Task SendAsync_ThrowsIfDisposed()
        {
            var sender = new Sender(TextWriter.Null);

            sender.Dispose();

            var exception = await Assert.ThrowsAsync<ObjectDisposedException>(
                () => sender.SendAsync(_message, CancellationToken.None));

            Assert.Equal(nameof(Sender), exception.ObjectName);
        }

        [Fact]
        public async Task SendAsync_ThrowsIfCancelled()
        {
            using (var sender = new Sender(TextWriter.Null))
            {
                sender.Connect();

                await Assert.ThrowsAsync<OperationCanceledException>(
                    () => sender.SendAsync(_message, new CancellationToken(canceled: true)));
            }
        }

        [Fact]
        public async Task SendAsync_ThrowsForDisposedWriter()
        {
            using (var writer = new StringWriter())
            {
                writer.Dispose();

                using (var sender = new Sender(writer))
                {
                    sender.Connect();

                    await Assert.ThrowsAsync<ObjectDisposedException>(
                        () => sender.SendAsync(_message, CancellationToken.None));
                }
            }
        }

        [Fact]
        public async Task SendAsync_ThrowsIfNotConnected()
        {
            using (var sender = new Sender(TextWriter.Null))
            {
                await Assert.ThrowsAsync<InvalidOperationException>(
                    () => sender.SendAsync(_message, CancellationToken.None));
            }
        }

        [Fact]
        public async Task SendAsync_NoOpsIfClosed()
        {
            using (var writer = new StringWriter())
            using (var sender = new Sender(writer))
            {
                sender.Connect();
                sender.Close();

                await sender.SendAsync(_message, CancellationToken.None);

                var actualResult = writer.ToString();

                Assert.Equal(string.Empty, actualResult);
            }
        }

        [Fact]
        public async Task SendAsync_WritesMessageToWriter()
        {
            using (var writer = new StringWriter())
            using (var sender = new Sender(writer))
            {
                sender.Connect();

                await sender.SendAsync(_message, CancellationToken.None);

                var actualResult = writer.ToString();

                Assert.Equal($"{{\"RequestId\":\"a\",\"Type\":\"Request\",\"Method\":\"None\"}}{Environment.NewLine}", actualResult);
            }
        }

        [Fact]
        public void Close_DoesNotThrowIfDisposed()
        {
            using (var writer = new StringWriter())
            {
                var sender = new Sender(TextWriter.Null);

                sender.Dispose();

                sender.Close();
            }
        }

        [Fact]
        public void Close_IsIdempotent()
        {
            using (var sender = new Sender(TextWriter.Null))
            {
                sender.Connect();

                sender.Close();
                sender.Close();
            }
        }

        [Fact]
        public void Close_CanBeCalledWithoutConnectAsync()
        {
            using (var sender = new Sender(TextWriter.Null))
            {
                sender.Close();
            }
        }

        [Fact]
        public async Task Close_DoesNotCloseUnderlyingStream()
        {
            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream))
            using (var sender = new Sender(writer))
            {
                sender.Connect();
                await sender.SendAsync(_message, CancellationToken.None);

                sender.Close();

                Assert.True(stream.CanSeek);
                Assert.True(stream.CanRead);
                Assert.True(stream.CanWrite);
            }
        }
    }
}
