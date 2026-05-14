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
    private readonly BinaryReader _reader;

    private readonly BinaryWriter _writer;

    /// <summary>
    /// Sync object for sending messages
    /// Write for binarywriter is NOT thread-safe
    /// </summary>
    private readonly object _writeSyncObject = new();

    public LengthPrefixCommunicationChannel(Stream stream)
    {
        _reader = new BinaryReader(stream, Encoding.UTF8, true);

        // Using the Buffered stream while writing, improves the write performance. By reducing the number of writes.
        _writer = new BinaryWriter(new PlatformStream().CreateBufferedStream(stream, SocketConstants.BufferSize), Encoding.UTF8, true);
    }

    /// <inheritdoc />
    public TrackableEvent<MessageReceivedEventArgs> MessageReceived { get; } = new TrackableEvent<MessageReceivedEventArgs>();

    /// <inheritdoc />
    public Task Send(string data)
    {
        try
        {
            // Writing Message on binarywriter is not Thread-Safe
            // Need to sync one by one to avoid buffer corruption
            lock (_writeSyncObject)
            {
                _writer.Write(data);
                _writer.Flush();
            }
        }
        catch (NotSupportedException ex) when (!_writer.BaseStream.CanWrite)
        {
            // As we are simply creating streams around some stream passed as ctor argument, we
            // end up in some unsynchronized behavior where it's possible that the outside stream
            // was disposed and we are still trying to write something. In such case we would fail
            // with "System.NotSupportedException: Stream does not support writing.".
            // To avoid being too generic in that catch, I am checking if the stream is not writable.
            EqtTrace.Verbose("LengthPrefixCommunicationChannel.Send: BaseStream is not writable (most likely it was dispose). {0}", ex);
        }
        catch (Exception ex)
        {
            EqtTrace.Error("LengthPrefixCommunicationChannel.Send: Error sending data: {0}.", ex);
            throw new CommunicationException("Unable to send data over channel.", ex);
        }

        return Task.FromResult(0);
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

        return Task.FromResult(0);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        try
        {
            EqtTrace.Verbose("LengthPrefixCommunicationChannel.Dispose: Dispose reader and writer.");
            _reader.Dispose();
            _writer.Dispose();
        }
        catch (ObjectDisposedException)
        {
            // We don't own the underlying stream lifecycle so it's possible that it's already disposed.
        }

        GC.SuppressFinalize(this);
    }
}
