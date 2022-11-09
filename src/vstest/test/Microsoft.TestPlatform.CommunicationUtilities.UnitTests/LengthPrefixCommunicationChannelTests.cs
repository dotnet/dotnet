// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
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
        _channel.MessageReceived += (sender, messageEventArgs) => data = messageEventArgs.Data;
        _writer.Write(Dummydata);
        SeekToBeginning(_stream);

        await _channel.NotifyDataAvailable();

        Assert.AreEqual(Dummydata, data);
    }

    [TestMethod]
    public async Task NotifyDataAvailableShouldNotReadStreamIfNoListenersAreRegistered()
    {
        _writer.Write(Dummydata);
        SeekToBeginning(_stream);

        await _channel.NotifyDataAvailable();

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
        _channel.MessageReceived += (sender, messageEventArgs) => data = messageEventArgs.Data;
        // Dispose base stream
        _stream.Dispose();
        await _channel.NotifyDataAvailable();
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
}
