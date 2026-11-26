// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

/// <summary>
/// A communication channel using a length prefix packet frame for communication.
/// </summary>
public class LengthPrefixCommunicationChannel : ICommunicationChannel
{
    private readonly Stream _stream;

    private readonly BinaryReader _reader;

    private readonly BinaryWriter _writer;

    private Exception? _sendFailure;

    /// <summary>
    /// Sync object for sending messages
    /// Write for binarywriter is NOT thread-safe
    /// </summary>
    private readonly object _writeSyncObject = new();

    public LengthPrefixCommunicationChannel(Stream stream)
    {
        _stream = stream;
        _reader = new BinaryReader(stream, Encoding.UTF8, true);

        // Using the Buffered stream while writing, improves the write performance. By reducing the number of writes.
        _writer = new BinaryWriter(new PlatformStream().CreateBufferedStream(stream, SocketConstants.BufferSize), Encoding.UTF8, true);
    }

    /// <inheritdoc />
    public TrackableEvent<MessageReceivedEventArgs> MessageReceived { get; } = new TrackableEvent<MessageReceivedEventArgs>();

    /// <inheritdoc />
    public Task Send(string data)
    {
        // Writing Message on binarywriter is not Thread-Safe
        // Need to sync one by one to avoid buffer corruption
        lock (_writeSyncObject)
        {
            if (_sendFailure is not null)
            {
                throw new CommunicationException("Unable to send data over channel because a previous send failed.", _sendFailure);
            }

            try
            {
                _writer.Write(data);
                _writer.Flush();
            }
            catch (NotSupportedException ex) when (!_stream.CanWrite)
            {
                // As we are simply creating streams around some stream passed as ctor argument, we
                // end up in some unsynchronized behavior where it's possible that the outside stream
                // was disposed and we are still trying to write something. In such case we would fail
                // with "System.NotSupportedException: Stream does not support writing.".
                // To avoid being too generic in that catch, I am checking if the stream is not writable.
                EqtTrace.Verbose("LengthPrefixCommunicationChannel.Send: Stream is not writable (most likely it was disposed). {0}", ex);
            }
            catch (Exception ex)
            {
                FaultChannel(ex);
                throw new CommunicationException("Unable to send data over channel.", ex);
            }
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Marks the channel unusable after a failed write, and closes the transport.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <see cref="BinaryWriter.Write(string)"/> does not report how many bytes it wrote, so once it
    /// throws we cannot know whether the length prefix reached the peer without its payload. If it
    /// did, the peer is waiting for a body that will never arrive, and every later message would be
    /// consumed as part of that frame.
    /// </para>
    /// <para>
    /// There is no way to tell the peer in band, because the framing it is reading is the thing that
    /// broke. Closing the transport is the only signal left: the peer reads end of stream, which it
    /// already handles.
    /// </para>
    /// <para>
    /// Both callers, SocketClient and SocketServer, build this channel over a TcpClient stream and
    /// close that client in their own error paths, so this closes a stream that was already on its
    /// way down. Dispose still leaves the stream open, which DisposeShouldNotCloseTheStream pins.
    /// </para>
    /// </remarks>
    private void FaultChannel(Exception ex)
    {
        _sendFailure = ex;

        try
        {
            _stream.Dispose();
        }
        catch (Exception disposeException)
        {
            EqtTrace.Error("LengthPrefixCommunicationChannel.Send: Error closing stream after send failure: {0}.", disposeException);
        }

        EqtTrace.Error("LengthPrefixCommunicationChannel.Send: Error sending data: {0}.", ex);
    }

    /// <inheritdoc />
    public Task NotifyDataAvailable(CancellationToken cancellationToken)
    {
        try
        {
            // TODO: Review the comment below, because it says something different than what is
            // actually happening, and doing what it suggests would potentially lose messages.
            // For example in the case where we start testhost process, send it version, and
            // it responds, we then replace the handler with a new one, and there is quite a long time
            // (tens of milliseconds) when there is no handler present, which would pump the message
            // and dump it.
            //
            // Try read data even if no one is listening to the data stream. Some server
            // implementations (like Sockets) depend on the read operation to determine if a
            // connection is closed.
            if (MessageReceived.WaitForSubscriber(1000, cancellationToken))
            {
                var data = _reader.ReadString();
                MessageReceived.Notify(this, new MessageReceivedEventArgs { Data = data }, "LengthPrefixCommunicationChannel: MessageReceived");
            }
        }
        catch (ObjectDisposedException ex) when (!_reader.BaseStream.CanRead)
        {
            // As we are simply creating streams around some stream passed as ctor argument, we
            // end up in some unsynchronized behavior where it's possible that the outside stream
            // was disposed and we are still trying to write something. In such case we would fail
            // with "System.ObjectDisposedException: Cannot access a closed Stream.".
            // To avoid being too generic in that catch, I am checking if the stream is not readable.
            EqtTrace.Verbose("LengthPrefixCommunicationChannel.Send: BaseStream was disposed. {0}", ex);
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        // Dispose can run while another thread is inside Send. Take the write lock when it is
        // free, so disposal never flushes the writer mid-frame, but never wait for it: blocking
        // here would turn a stuck socket write into a shutdown hang.
        var lockTaken = false;
        try
        {
            Monitor.TryEnter(_writeSyncObject, ref lockTaken);

            EqtTrace.Verbose("LengthPrefixCommunicationChannel.Dispose: Dispose reader and writer.");
            _reader.Dispose();

            // BinaryWriter.Dispose flushes its BufferedStream. Skip that flush after a failed
            // send, where the buffer can still hold part of an incomplete frame that must not
            // reach the peer, and skip it while a send holds the lock, to avoid racing that
            // writer. Neither skip loses data: Send flushes after every message it completes.
            if (lockTaken && _sendFailure is null)
            {
                _writer.Dispose();
            }
        }
        catch (ObjectDisposedException)
        {
            // We don't own the underlying stream lifecycle so it's possible that it's already disposed.
        }
        finally
        {
            if (lockTaken)
            {
                Monitor.Exit(_writeSyncObject);
            }
        }

        GC.SuppressFinalize(this);
    }
}
