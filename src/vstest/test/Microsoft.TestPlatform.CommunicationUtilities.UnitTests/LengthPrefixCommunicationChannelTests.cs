// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.UnitTests;

[TestClass]
public class LengthPrefixCommunicationChannelTests : IDisposable
{
    private const string Dummydata = "Dummy Data";

    private readonly ICommunicationChannel _channel;

    private readonly MemoryStream _stream;

    private readonly BinaryReader _reader;

    private readonly BinaryWriter _writer;

    public TestContext TestContext { get; set; } = null!;

    public LengthPrefixCommunicationChannelTests()
    {
        _stream = new MemoryStream();
        _channel = new LengthPrefixCommunicationChannel(_stream);

        _reader = new BinaryReader(_stream);
        _writer = new BinaryWriter(_stream);
    }

    public void Dispose()
    {
        _stream.Dispose();

        _reader.Dispose();
        _writer.Dispose();
        GC.SuppressFinalize(this);
    }

    [TestMethod]
    public async Task SendShouldWriteTheDataOnStream()
    {
        await _channel.Send(Dummydata);

        SeekToBeginning(_stream);
        Assert.AreEqual(Dummydata, _reader.ReadString());
    }

    [TestMethod]
    public async Task SendShouldWriteInLengthPrefixedFormat()
    {
        await _channel.Send(Dummydata);

        SeekToBeginning(_stream);
        Assert.AreEqual(Dummydata.Length, Read7BitEncodedInt(_reader));
    }

    [TestMethod]
    public async Task SendShouldBeAbleToWriteUnicodeData()
    {
        // Every day is a good day
        var utf8Data = "日日是好日";
        await _channel.Send(utf8Data);

        SeekToBeginning(_stream);
        Assert.AreEqual(utf8Data, _reader.ReadString());
    }

    [TestMethod]
    public async Task SendShouldFlushTheStream()
    {
        // A buffered stream doesn't immediately flush, it waits until buffer is filled in
        using var bufferedStream = new BufferedStream(_stream, 2048);
        var communicationChannel = new LengthPrefixCommunicationChannel(bufferedStream);

        await communicationChannel.Send("a");

        SeekToBeginning(_stream);
        Assert.AreEqual("a", _reader.ReadString());
    }

    [TestMethod]
    public async Task MessageReceivedShouldProvideDataOverStream()
    {
        var data = string.Empty;
        _channel.MessageReceived.Subscribe((sender, messageEventArgs) => data = messageEventArgs.Data);
        _writer.Write(Dummydata);
        SeekToBeginning(_stream);

        await _channel.NotifyDataAvailable(new CancellationToken());

        Assert.AreEqual(Dummydata, data);
    }

    [TestMethod]
    public async Task NotifyDataAvailableShouldNotReadStreamIfNoListenersAreRegistered()
    {
        _writer.Write(Dummydata);
        SeekToBeginning(_stream);

        await _channel.NotifyDataAvailable(new CancellationToken());

        // Data is read irrespective of listeners. See note in NotifyDataAvailable
        // implementation.
        Assert.AreEqual(0, _stream.Position);
    }

    [TestMethod]
    public void DisposeShouldNotCloseTheStream()
    {
        _channel.Dispose();

        // Should throw if stream is disposed.
        Assert.IsTrue(_stream.CanWrite);
    }

    /// <summary>
    /// Characterization test: it passes on main unchanged, and it passes here. It asserts no new
    /// behavior and claims no documented guarantee. Two implementation details combine to let a
    /// send after Dispose still reach the stream. BinaryWriter does not track its own disposal, so
    /// Write has nothing to reject, and the writer holds the stream with leaveOpen, so disposing
    /// the writer flushes rather than closing the stream underneath it. Shutdown leans on the
    /// resulting tolerance, because Dispose runs while timer driven sends are still in flight.
    /// Pinning it here means a future change surfaces as a failing test rather than as a silent
    /// change to shutdown.
    /// </summary>
    [TestMethod]
    public async Task SendAfterDisposeShouldStillWriteToTheStream()
    {
        using var stream = new MemoryStream();
        var channel = new LengthPrefixCommunicationChannel(stream);
        channel.Dispose();

        await channel.Send(Dummydata);

        Assert.IsTrue(stream.CanWrite);
        SeekToBeginning(stream);
        using var reader = new BinaryReader(stream);
        Assert.AreEqual(Dummydata, reader.ReadString());
    }

    [TestMethod]
    public async Task DoNotFailWhenWritingOnADisposedBaseStream()
    {
        // Dispose base stream
        _stream.Dispose();
        await _channel.Send(Dummydata);
    }

    [TestMethod]
    public async Task DoNotFailWhenReadingFromADisposedBaseStream()
    {
        var data = string.Empty;
        _channel.MessageReceived.Subscribe((sender, messageEventArgs) => data = messageEventArgs.Data);
        // Dispose base stream
        _stream.Dispose();
        await _channel.NotifyDataAvailable(new CancellationToken());
    }

    [TestMethod]
    public async Task SendShouldCloseStreamAndRejectLaterMessagesAfterPartialFrame()
    {
        using var stream = new FailingWriteStream(bytesBeforeFailure: 3);
        var channel = new LengthPrefixCommunicationChannel(stream);
        var message = new string('x', SocketConstants.BufferSize + 1);

        await Assert.ThrowsExactlyAsync<CommunicationException>(() => channel.Send(message));

        Assert.HasCount(3, stream.WrittenBytes);
        using (var prefixStream = new MemoryStream(stream.WrittenBytes))
        using (var prefixReader = new BinaryReader(prefixStream))
        {
            Assert.AreEqual(message.Length, Read7BitEncodedInt(prefixReader));
        }

        var writeCallCount = stream.WriteCallCount;
        await Assert.ThrowsExactlyAsync<CommunicationException>(() => channel.Send(Dummydata));

        Assert.IsTrue(stream.IsDisposed);
        Assert.AreEqual(writeCallCount, stream.WriteCallCount);
        Assert.HasCount(3, stream.WrittenBytes);
    }

    /// <summary>
    /// The corruption this change prevents, asserted at the peer instead of at the sender. A send
    /// that fails once its length prefix has reached the peer leaves a frame header on the wire
    /// promising a body that never arrives. On unmodified main the next message is consumed as that
    /// body, and the peer decodes a string nobody ever sent, which is where the
    /// "Unexpected character encountered while parsing value" JSON abort comes from. Every other
    /// test here asserts what the sender does; this one asserts what the peer can observe.
    /// </summary>
    [TestMethod]
    public async Task PeerShouldNotDecodeAMessageThatWasNeverSent()
    {
        var message1 = new string('a', SocketConstants.BufferSize + 1);
        var message2 = new string('b', SocketConstants.BufferSize + 1);

        // The payload does not fit in what is left of the buffer, so the length prefix is flushed as
        // its own write and reaches the peer before the payload write fails.
        using var wire = new FailingWriteStream(bytesBeforeFailure: 3);
        var sender = new LengthPrefixCommunicationChannel(wire);

        await Assert.ThrowsExactlyAsync<CommunicationException>(() => sender.Send(message1));

        try
        {
            // The injected failure fires once, so on unmodified main this second message is written
            // and lands directly behind the orphaned prefix.
            await sender.Send(message2);
        }
        catch (CommunicationException)
        {
            // Expected once the channel refuses to send after a failed frame.
        }

        var received = ReadFirstMessageFromWire(wire.WrittenBytes);

        var decoded = received is null ? string.Empty : DescribeFirstChars(received);
        Assert.IsNull(
            received,
            $"The peer decoded a {received?.Length ?? 0} character frame that was never sent as a message, starting {decoded}.");
    }

    private static string? ReadFirstMessageFromWire(byte[] bytesOnTheWire)
    {
        using var peerStream = new MemoryStream(bytesOnTheWire);
        using var peer = new LengthPrefixCommunicationChannel(peerStream);

        string? received = null;
        peer.MessageReceived.Subscribe((sender, messageEventArgs) => received = messageEventArgs.Data);

        try
        {
            peer.NotifyDataAvailable(CancellationToken.None).GetAwaiter().GetResult();
        }
        catch (EndOfStreamException)
        {
            // An incomplete frame is the correct outcome. The peer has nothing to act on.
        }

        return received;
    }

    private static string DescribeFirstChars(string value)
    {
        var count = Math.Min(3, value.Length);
        var parts = new string[count];
        for (var i = 0; i < count; i++)
        {
            parts[i] = $"U+{(int)value[i]:X4}";
        }

        return string.Join(" ", parts);
    }

    [TestMethod]
    public async Task DisposeShouldNotFlushBufferedDataAfterSendFailure()
    {
        using var stream = new FailingWriteStream(bytesBeforeFailure: 0);
        var channel = new LengthPrefixCommunicationChannel(stream);

        await Assert.ThrowsExactlyAsync<CommunicationException>(() => channel.Send(Dummydata));
        var writeCallCount = stream.WriteCallCount;

        channel.Dispose();

        Assert.AreEqual(writeCallCount, stream.WriteCallCount);
    }

    [TestMethod]
    public async Task ConcurrentSendShouldNotWriteAfterFirstSendFails()
    {
        using var stream = new FailingWriteStream(bytesBeforeFailure: 3, blockFirstWrite: true);
        var channel = new LengthPrefixCommunicationChannel(stream);
        var message = new string('x', SocketConstants.BufferSize + 1);

        var firstSend = Task.Run(() => channel.Send(message), TestContext.CancellationToken);
        Assert.IsTrue(stream.WriteStarted.Wait(TimeSpan.FromSeconds(5), TestContext.CancellationToken));

        var secondSend = Task.Run(() => channel.Send(Dummydata), TestContext.CancellationToken);
        await Task.Delay(50, TestContext.CancellationToken);
        Assert.IsFalse(secondSend.IsCompleted);

        stream.ReleaseWrite.Set();

        await Assert.ThrowsExactlyAsync<CommunicationException>(() => firstSend);
        await Assert.ThrowsExactlyAsync<CommunicationException>(() => secondSend);
        Assert.IsTrue(stream.IsDisposed);
        Assert.AreEqual(1, stream.WriteCallCount);
    }

    [TestMethod]
    public async Task DisposeShouldNeitherWriteNorBlockWhileASendIsInFlight()
    {
        using var stream = new FailingWriteStream(bytesBeforeFailure: 3, blockFirstWrite: true);
        var channel = new LengthPrefixCommunicationChannel(stream);
        var message = new string('x', SocketConstants.BufferSize + 1);

        var send = Task.Run(() => channel.Send(message), TestContext.CancellationToken);
        Assert.IsTrue(stream.WriteStarted.Wait(TimeSpan.FromSeconds(5), TestContext.CancellationToken));

        var writeCallCount = stream.WriteCallCount;
        var stopwatch = Stopwatch.StartNew();
        channel.Dispose();
        stopwatch.Stop();

        // The in-flight write is held for up to five seconds. Disposal must not wait for it,
        // and must not flush the writer underneath it.
        Assert.IsLessThan(TimeSpan.FromSeconds(2), stopwatch.Elapsed, $"Dispose blocked for {stopwatch.Elapsed}.");
        Assert.AreEqual(writeCallCount, stream.WriteCallCount);

        stream.ReleaseWrite.Set();
        await Assert.ThrowsExactlyAsync<CommunicationException>(() => send);
    }

    // TODO
    // WriteFromMultilpleThreadShouldBeInSequence
    private static void SeekToBeginning(Stream stream)
    {
        stream.Seek(0, SeekOrigin.Begin);
    }

    private static int Read7BitEncodedInt(BinaryReader reader)
    {
        // Copied from BinaryReader.Read7BitEncodedInt
        // https://referencesource.microsoft.com/#mscorlib/system/io/binaryreader.cs,f30b8b6e8ca06e0f
        // Read out an Int32 7 bits at a time.  The high bit
        // of the byte when on means to continue reading more bytes.
        int count = 0;
        int shift = 0;
        byte b;
        do
        {
            // Check for a corrupted stream.  Read a max of 5 bytes.
            // In a future version, add a DataFormatException.
            // 5 bytes max per Int32, shift += 7
            if (shift == 5 * 7)
            {
                throw new FormatException("Format_Bad7BitInt32");
            }

            // ReadByte handles end of stream cases for us.
            b = reader.ReadByte();
            count |= (b & 0x7F) << shift;
            shift += 7;
        }
        while ((b & 0x80) != 0);

        return count;
    }

    private sealed class FailingWriteStream : MemoryStream
    {
        private readonly int _bytesBeforeFailure;
        private readonly bool _blockFirstWrite;

        private int _acceptedBytes;
        private bool _failureInjected;

        public FailingWriteStream(int bytesBeforeFailure, bool blockFirstWrite = false)
        {
            _bytesBeforeFailure = bytesBeforeFailure;
            _blockFirstWrite = blockFirstWrite;
        }

        public bool IsDisposed { get; private set; }

        public int WriteCallCount { get; private set; }

        public byte[] WrittenBytes => ToArray();

        public ManualResetEventSlim WriteStarted { get; } = new(false);

        public ManualResetEventSlim ReleaseWrite { get; } = new(false);

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(nameof(FailingWriteStream));
            }

            WriteCallCount++;
            WriteStarted.Set();
            if (!_failureInjected)
            {
                if (_blockFirstWrite && !ReleaseWrite.Wait(TimeSpan.FromSeconds(5)))
                {
                    throw new TimeoutException("Timed out waiting to release the injected write failure.");
                }

                _failureInjected = true;
                var remainingBytes = _bytesBeforeFailure - _acceptedBytes;
                if (remainingBytes > 0)
                {
                    var acceptedCount = Math.Min(remainingBytes, count);
                    base.Write(buffer, offset, acceptedCount);
                    _acceptedBytes += acceptedCount;
                }

                throw new IOException("Injected write failure.");
            }

            base.Write(buffer, offset, count);
        }

        protected override void Dispose(bool disposing)
        {
            if (IsDisposed)
            {
                return;
            }

            IsDisposed = true;
            if (disposing)
            {
                WriteStarted.Dispose();
                ReleaseWrite.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
