// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Packaging.Signing;
using Xunit;

namespace NuGet.Packaging.Test
{
    public class ReadOnlyBufferedStreamTests
    {
        private const int _underlyingBufferSize = 1024;
        private const int _bufferedStreamBufferSize = 3;

        private static byte[] _defaultBuffer = CreateRandomBuffer(_underlyingBufferSize);

        [Fact]
        public void Constructor_TwoArguments_WithNullStream_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new ReadOnlyBufferedStream(stream: null, leaveOpen: false));

            Assert.Equal("stream", exception.ParamName);
        }

        [Fact]
        public void Constructor_TwoArguments_WithNonReadableStream_Throws()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new ReadOnlyBufferedStream(
                    new FakeStream(canRead: false, canSeek: true, canWrite: false),
                    leaveOpen: false));

            Assert.Equal("stream", exception.ParamName);
            Assert.StartsWith("The stream must be readable.", exception.Message);
        }

        [Fact]
        public void Constructor_TwoArguments_WithNonSeekableStream_Throws()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new ReadOnlyBufferedStream(
                    new FakeStream(canRead: true, canSeek: false, canWrite: false),
                    leaveOpen: false));

            Assert.Equal("stream", exception.ParamName);
            Assert.StartsWith("The stream must be seekable.", exception.Message);
        }

        [Fact]
        public void Constructor_ThreeArguments_WithNullStream_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new ReadOnlyBufferedStream(stream: null, leaveOpen: false, bufferSize: 4096));

            Assert.Equal("stream", exception.ParamName);
        }

        [Fact]
        public void Constructor_ThreeArguments_WithNonReadableStream_Throws()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new ReadOnlyBufferedStream(
                    new FakeStream(canRead: false, canSeek: true, canWrite: false),
                    leaveOpen: false,
                    bufferSize: 4096));

            Assert.Equal("stream", exception.ParamName);
            Assert.StartsWith("The stream must be readable.", exception.Message);
        }

        [Fact]
        public void Constructor_ThreeArguments_WithNonSeekableStream_Throws()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new ReadOnlyBufferedStream(
                    new FakeStream(canRead: true, canSeek: false, canWrite: false),
                    leaveOpen: false,
                    bufferSize: 4096));

            Assert.Equal("stream", exception.ParamName);
            Assert.StartsWith("The stream must be seekable.", exception.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Constructor_ThreeArguments_NonPositiveBufferSize_Throws(int bufferSize)
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(
                () => new ReadOnlyBufferedStream(Stream.Null, leaveOpen: false, bufferSize: bufferSize));

            Assert.Equal("bufferSize", exception.ParamName);
        }

        [Fact]
        public void Constructor_InitializesSelectProperties()
        {
            using (var test = CreateTest())
            {
                Assert.Equal(_defaultBuffer.Length, test.BufferedReadStream.Length);
                Assert.Equal(0, test.BufferedReadStream.Position);
            }
        }

        [Fact]
        public void CanRead_ReturnsValueFromUnderlyingStream()
        {
            using (var test = CreateTest())
            {
                // The constructor already called CanRead once.
                Assert.Equal(1, test.UnderlyingStream.CanReadCallCount);

                var canRead = test.BufferedReadStream.CanRead;

                Assert.Equal(2, test.UnderlyingStream.CanReadCallCount);
                Assert.Equal(test.UnderlyingStream.CanRead, canRead);
            }
        }

        [Fact]
        public void CanSeek_ReturnsValueFromUnderlyingStream()
        {
            using (var test = CreateTest())
            {
                // The constructor already called CanSeek once.
                Assert.Equal(1, test.UnderlyingStream.CanSeekCallCount);

                var canSeek = test.BufferedReadStream.CanSeek;

                Assert.Equal(2, test.UnderlyingStream.CanSeekCallCount);
                Assert.Equal(test.UnderlyingStream.CanRead, canSeek);
            }
        }

        [Fact]
        public void CanTimeout_ReturnsValueFromUnderlyingStream()
        {
            using (var test = CreateTest())
            {
                Assert.Equal(0, test.UnderlyingStream.CanTimeoutCallCount);

                var canTimeout = test.BufferedReadStream.CanTimeout;

                Assert.Equal(1, test.UnderlyingStream.CanTimeoutCallCount);
                Assert.Equal(test.UnderlyingStream.CanTimeout, canTimeout);
            }
        }

        [Fact]
        public void CanWrite_ReturnsFalse()
        {
            using (var test = CreateTest())
            {
                Assert.False(test.BufferedReadStream.CanWrite);
                Assert.Equal(0, test.UnderlyingStream.CanWriteCallCount);
            }
        }

        [Fact]
        public void Position_WithNegativeValue_Throws()
        {
            using (var test = CreateTest())
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => test.BufferedReadStream.Position = -1);
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(long.MaxValue)]
        public void Position_WithAnyNonNegativeValue_SetsPosition(long position)
        {
            using (var test = CreateTest())
            {
                test.BufferedReadStream.Position = position;

                Assert.Equal(position, test.BufferedReadStream.Position);
            }
        }

        [Fact]
        public void Length_ReturnsMemoizedValue()
        {
            using (var test = CreateTest())
            {
                for (var i = 0; i < 5; ++i)
                {
                    var length = test.BufferedReadStream.Length;
                }

                Assert.Equal(1, test.UnderlyingStream.LengthCallCount);
            }
        }

        [Fact]
        public void ReadTimeout_Getter_ReturnsValueFromUnderlyingStream()
        {
            using (var test = CreateTest())
            {
                Assert.Equal(0, test.UnderlyingStream.ReadTimeoutGetCallCount);

                var readTimeout = test.BufferedReadStream.ReadTimeout;

                Assert.Equal(1, test.UnderlyingStream.ReadTimeoutGetCallCount);
                Assert.Equal(test.UnderlyingStream.ReadTimeout, readTimeout);
            }
        }

        [Fact]
        public void ReadTimeout_Setter_SetsValueOnUnderlyingStream()
        {
            using (var test = CreateTest())
            {
                Assert.Equal(0, test.UnderlyingStream.ReadTimeoutSetCallCount);
                test.BufferedReadStream.ReadTimeout = 1;
                Assert.Equal(1, test.UnderlyingStream.ReadTimeoutSetCallCount);
            }
        }

        [Fact]
        public void WriteTimeout_Getter_ReturnsValueFromUnderlyingStream()
        {
            using (var test = CreateTest())
            {
                Assert.Equal(0, test.UnderlyingStream.WriteTimeoutGetCallCount);

                var writeTimeout = test.BufferedReadStream.WriteTimeout;

                Assert.Equal(1, test.UnderlyingStream.WriteTimeoutGetCallCount);
                Assert.Equal(test.UnderlyingStream.WriteTimeout, writeTimeout);
            }
        }

        [Fact]
        public void WriteTimeout_Setter_SetsValueOnUnderlyingStream()
        {
            using (var test = CreateTest())
            {
                Assert.Equal(0, test.UnderlyingStream.WriteTimeoutSetCallCount);
                test.BufferedReadStream.WriteTimeout = 1;
                Assert.Equal(1, test.UnderlyingStream.WriteTimeoutSetCallCount);
            }
        }

#if IS_SIGNING_SUPPORTED
        [Fact]
        public void Close_WhenLeaveOpenFalse_DisposesUnderlyingStream()
        {
            using (var test = CreateTest(leaveOpen: false))
            {
                Assert.Equal(0, test.UnderlyingStream.DisposeCallCount);
                test.BufferedReadStream.Close();
                Assert.Equal(1, test.UnderlyingStream.DisposeCallCount);
            }
        }

        [Fact]
        public void Close_WhenLeaveOpenTrue_DoesNotDisposeUnderlyingStream()
        {
            using (var test = CreateTest(leaveOpen: true))
            {
                Assert.Equal(0, test.UnderlyingStream.DisposeCallCount);
                test.BufferedReadStream.Close();
                Assert.Equal(0, test.UnderlyingStream.DisposeCallCount);
            }
        }
#endif

        [Fact]
        public void CopyTo_WithCurrentPositionOfSourceStreamAtStart_CopiesEntireStream()
        {
            using (var test = CreateTest())
            using (var stream = new MemoryStream())
            {
                test.BufferedReadStream.CopyTo(stream);

                Assert.Equal(_defaultBuffer, stream.ToArray());
            }
        }

        [Fact]
        public void CopyTo_WithCurrentPositionOfSourceStreamNotAtStart_CopiesPartialStream()
        {
            using (var test = CreateTest())
            using (var stream = new MemoryStream())
            {
                test.BufferedReadStream.Position = 100;
                test.BufferedReadStream.CopyTo(stream);

                Assert.Equal(_defaultBuffer.Skip(100).ToArray(), stream.ToArray());
            }
        }

        // Overload not tested:  public void CopyTo(Stream destination, int bufferSize);

        [Fact]
        public async Task CopyToAsync_WithCurrentPositionOfSourceStreamAtStart_CopiesEntireStream()
        {
            using (var test = CreateTest())
            using (var stream = new MemoryStream())
            {
                await test.BufferedReadStream.CopyToAsync(stream);

                Assert.Equal(_defaultBuffer, stream.ToArray());
            }
        }

        [Fact]
        public async Task CopyToAsync_WithCurrentPositionOfSourceStreamNotAtStart_CopiesPartialStream()
        {
            using (var test = CreateTest())
            using (var stream = new MemoryStream())
            {
                test.BufferedReadStream.Position = 100;
                await test.BufferedReadStream.CopyToAsync(stream);

                Assert.Equal(_defaultBuffer.Skip(100).ToArray(), stream.ToArray());
            }
        }

        // Overload not tested:  public Task CopyToAsync(Stream destination, int bufferSize);

        [Fact]
        public void Dispose_WhenLeaveOpenFalse_DisposesUnderlyingStream()
        {
            using (var test = CreateTest(leaveOpen: false))
            {
                Assert.Equal(0, test.UnderlyingStream.DisposeCallCount);
                test.BufferedReadStream.Dispose();
                Assert.Equal(1, test.UnderlyingStream.DisposeCallCount);
            }
        }

        [Fact]
        public void Dispose_WhenLeaveOpenTrue_DoesNotDisposeUnderlyingStream()
        {
            using (var test = CreateTest(leaveOpen: true))
            {
                Assert.Equal(0, test.UnderlyingStream.DisposeCallCount);
                test.BufferedReadStream.Dispose();
                Assert.Equal(0, test.UnderlyingStream.DisposeCallCount);
            }
        }

        [Fact]
        public void Flush_Throws()
        {
            using (var test = CreateTest())
            {
                Assert.Throws<NotSupportedException>(() => test.BufferedReadStream.Flush());
            }
        }

        [Fact]
        public async Task FlushAsync_Throws()
        {
            using (var test = CreateTest())
            {
                await Assert.ThrowsAsync<NotSupportedException>(
                    () => test.BufferedReadStream.FlushAsync(CancellationToken.None));
            }
        }

        [Fact]
        public void Read_WithNullBuffer_Throws()
        {
            using (var test = CreateTest())
            {
                var exception = Assert.Throws<ArgumentNullException>(
                    () => test.BufferedReadStream.Read(buffer: null, offset: 0, count: 0));

                Assert.Equal("buffer", exception.ParamName);
            }
        }

        [Fact]
        public void Read_WithNegativeOffset_Throws()
        {
            using (var test = CreateTest())
            {
                var exception = Assert.Throws<ArgumentOutOfRangeException>(
                    () => test.BufferedReadStream.Read(new byte[1], offset: -1, count: 1));

                Assert.Equal("offset", exception.ParamName);
            }
        }

        [Fact]
        public void Read_WithNegativeCount_Throws()
        {
            using (var test = CreateTest())
            {
                var exception = Assert.Throws<ArgumentOutOfRangeException>(
                    () => test.BufferedReadStream.Read(new byte[1], offset: 0, count: -1));

                Assert.Equal("count", exception.ParamName);
            }
        }

        [Theory]
        [InlineData(0, 2)]
        [InlineData(1, 1)]
        public void Read_WithRangeOutOfBounds_Throws(int offset, int count)
        {
            using (var test = CreateTest())
            {
                var exception = Assert.Throws<ArgumentException>(
                    () => test.BufferedReadStream.Read(new byte[1], offset, count));

                Assert.Equal("Arguments offset and count were out of bounds for the array.", exception.Message);
            }
        }

        [Fact]
        public void Read_WithEmptyUnderlyingBuffer_ReturnsZero()
        {
            using (var test = CreateTest(Array.Empty<byte>()))
            {
                var buffer = new byte[10];

                var bytesRead = test.BufferedReadStream.Read(buffer, offset: 0, count: buffer.Length);

                Assert.Equal(0, bytesRead);
                Assert.True(buffer.All(b => b == 0));
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1024)]
        public void Read_WithZeroCount_ReadsZeroBytes(long position)
        {
            using (var test = CreateTest())
            {
                test.BufferedReadStream.Position = position;

                var buffer = new byte[10];

                var bytesRead = test.BufferedReadStream.Read(buffer, offset: 0, count: 0);

                Assert.Equal(0, bytesRead);
                Assert.True(buffer.All(b => b == 0));
                Assert.Equal(0, test.UnderlyingStream.ReadCallCount);
                Assert.Equal(0, test.UnderlyingStream.ReadByteCallCount);
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Read_WithBufferSmallerThanStreamBufferSize_Reads(int bufferSize)
        {
            using (var test = CreateTest())
            {
                var buffer = new byte[bufferSize];
                var bytesRead = test.BufferedReadStream.Read(buffer, offset: 0, count: buffer.Length);

                Assert.Equal(buffer.Length, bytesRead);

                test.AssertBufferCorrectness(buffer, startingPositionInUnderlyingBuffer: 0, length: bytesRead);

                Assert.Equal(1, test.UnderlyingStream.ReadCallCount);
                Assert.Equal(0, test.UnderlyingStream.ReadByteCallCount);
            }
        }

        [Fact]
        public void Read_WithBufferEqualToStreamBuffer_Reads()
        {
            using (var test = CreateTest())
            {
                const int readCount = 5;

                for (var i = 0; i < readCount; ++i)
                {
                    var buffer = new byte[3];
                    var bytesRead = test.BufferedReadStream.Read(buffer, offset: 0, count: buffer.Length);

                    Assert.Equal(buffer.Length, bytesRead);

                    var startingPositionInUnderlyingBuffer = buffer.Length * i;

                    test.AssertBufferCorrectness(buffer, startingPositionInUnderlyingBuffer, length: bytesRead);
                }

                Assert.Equal(readCount, test.UnderlyingStream.ReadCallCount);
                Assert.Equal(0, test.UnderlyingStream.ReadByteCallCount);
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void Read_WithBufferSpanningStreamBuffers_Reads(int startingPosition)
        {
            using (var test = CreateTest())
            {
                test.BufferedReadStream.Position = startingPosition;

                var buffer = new byte[4];
                var bytesRead = test.BufferedReadStream.Read(buffer, offset: 0, count: buffer.Length);

                Assert.Equal(buffer.Length, bytesRead);

                test.AssertBufferCorrectness(buffer, startingPosition, bytesRead);

                Assert.Equal(2, test.UnderlyingStream.ReadCallCount);
                Assert.Equal(0, test.UnderlyingStream.ReadByteCallCount);
            }
        }

        [Fact]
        public void Read_RereadingSameUnderlyingBuffer_DoesNotMakeAdditionalReadCallsToUnderlyingStream()
        {
            using (var test = CreateTest(bufferedStreamBufferSize: 5))
            {
                const int readCount = 5;

                for (var i = 0; i < readCount; ++i)
                {
                    var buffer = new byte[1];
                    var bytesRead = test.BufferedReadStream.Read(buffer, offset: 0, count: buffer.Length);

                    Assert.Equal(buffer.Length, bytesRead);

                    var startingPositionInUnderlyingBuffer = buffer.Length * i;

                    test.AssertBufferCorrectness(buffer, startingPositionInUnderlyingBuffer, bytesRead);
                }

                Assert.Equal(1, test.UnderlyingStream.ReadCallCount);
                Assert.Equal(0, test.UnderlyingStream.ReadByteCallCount);
            }
        }

        [Fact]
        public void Read_FromBeforeEndOfStreamReadingToPastEndOfStream_ReadsUpToEndOfStream()
        {
            using (var test = CreateTest())
            {
                const long startingPosition = 1000;

                test.BufferedReadStream.Position = startingPosition;

                var buffer = new byte[100];
                const int expectedLength = 24;

                var bytesRead = test.BufferedReadStream.Read(buffer, offset: 0, count: buffer.Length);

                Assert.Equal(expectedLength, bytesRead);

                test.AssertBufferCorrectness(buffer, startingPosition, expectedLength);

                Assert.Equal(8, test.UnderlyingStream.ReadCallCount);  // 8 == 24 (expectedLength) / 3 (bufferSize)
                Assert.Equal(0, test.UnderlyingStream.ReadByteCallCount);
            }
        }

        [Fact]
        public void Read_FromAtEndOfStreamReadingToPastEndOfStream_ReadsZeroBytes()
        {
            using (var test = CreateTest())
            {
                test.BufferedReadStream.Position = 1024;

                var buffer = new byte[100];

                var bytesRead = test.BufferedReadStream.Read(buffer, offset: 0, count: buffer.Length);

                Assert.Equal(0, bytesRead);
                Assert.True(buffer.All(b => b == 0));
                Assert.Equal(0, test.UnderlyingStream.ReadCallCount);
                Assert.Equal(0, test.UnderlyingStream.ReadByteCallCount);
            }
        }

        [Fact]
        public void Read_FromAfterEndOfStream_ReadsZeroBytes()
        {
            using (var test = CreateTest())
            {
                test.BufferedReadStream.Position = 1025;

                var buffer = new byte[10];

                var bytesRead = test.BufferedReadStream.Read(buffer, offset: 0, count: buffer.Length);

                Assert.Equal(0, bytesRead);
                Assert.True(buffer.All(b => b == 0));
                Assert.Equal(0, test.UnderlyingStream.ReadCallCount);
                Assert.Equal(0, test.UnderlyingStream.ReadByteCallCount);
            }
        }

        [Theory]
        [InlineData(0, 128)]
        [InlineData(333, 46)]
        [InlineData(513, 511)]
        public void Read_WithVaryingOffsetsAndLengthsInDestinationBuffer_Reads(
            int offsetInDestinationBuffer,
            int lengthInDestinationBuffer)
        {
            using (var test = CreateTest())
            {
                var buffer = new byte[1024];
                var bytesRead = test.BufferedReadStream.Read(
                    buffer,
                    offsetInDestinationBuffer,
                    lengthInDestinationBuffer);

                Assert.Equal(lengthInDestinationBuffer, bytesRead);

                test.AssertBufferCorrectness(
                    buffer,
                    startingPositionInUnderlyingBuffer: 0,
                    startingPositionInDestinationBuffer: offsetInDestinationBuffer,
                    length: lengthInDestinationBuffer);
            }
        }

        [Fact]
        public async Task ReadAsync_Reads()
        {
            using (var test = CreateTest())
            {
                const long startingPosition = 0;

                var buffer = new byte[128];

                test.BufferedReadStream.Position = startingPosition;

                await test.BufferedReadStream.ReadAsync(buffer, offset: 0, count: buffer.Length);

                test.AssertBufferCorrectness(buffer, startingPosition, buffer.Length);
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(300)]
        [InlineData(1023)]
        public void ReadByte_WithCurrentPosition_ReadsByte(int position)
        {
            using (var test = CreateTest())
            {
                test.BufferedReadStream.Position = position;

                var actualByte = test.BufferedReadStream.ReadByte();
                var expectedByte = _defaultBuffer[position];

                Assert.Equal(expectedByte, actualByte);
            }
        }

        [Fact]
        public void ReadByte_WithPositionAtEndOfStream_ReadsByte()
        {
            using (var test = CreateTest())
            {
                test.BufferedReadStream.Position = test.BufferedReadStream.Length;

                Assert.Equal(-1, test.BufferedReadStream.ReadByte());
            }
        }

        [Fact]
        public void ReadByte_WithPositionAfterEndOfStream_ReadsByte()
        {
            using (var test = CreateTest())
            {
                test.BufferedReadStream.Position = int.MaxValue;

                Assert.Equal(-1, test.BufferedReadStream.ReadByte());
            }
        }

        [Fact]
        public void Seek_WithOriginBeginAndNegativeOffset_Throws()
        {
            using (var test = CreateTest())
            {
                Assert.Throws<ArgumentOutOfRangeException>(
                    () => test.BufferedReadStream.Seek(offset: -1, origin: SeekOrigin.Begin));
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(long.MaxValue)]
        public void Seek_WithOriginBeginAndNonNegativeOffset_Seeks(long offset)
        {
            using (var test = CreateTest())
            {
                var newPosition = test.BufferedReadStream.Seek(offset, SeekOrigin.Begin);

                Assert.Equal(offset, newPosition);
                Assert.Equal(offset, test.BufferedReadStream.Position);
            }
        }

        [Theory]
        [InlineData(0, -1)]
        [InlineData(1, -3)]
        public void Seek_WithOriginCurrentAndNegativePosition_Throws(long currentPosition, long offset)
        {
            using (var test = CreateTest())
            {
                test.BufferedReadStream.Position = currentPosition;

                Assert.Throws<ArgumentOutOfRangeException>(
                    () => test.BufferedReadStream.Seek(offset, SeekOrigin.Current));
            }
        }

        [Theory]
        [InlineData(0, 9)]
        [InlineData(7, -5)]
        [InlineData(13, 0)]
        public void Seek_WithOriginCurrentAndNonNegativePosition_Seeks(long currentPosition, long offset)
        {
            using (var test = CreateTest())
            {
                test.BufferedReadStream.Position = currentPosition;

                var expectedPosition = currentPosition + offset;

                var newPosition = test.BufferedReadStream.Seek(offset, SeekOrigin.Current);

                Assert.Equal(expectedPosition, newPosition);
                Assert.Equal(expectedPosition, test.BufferedReadStream.Position);
            }
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        public void Seek_WithOriginEndAndNegativePosition_Throws(long offset)
        {
            using (var test = CreateTest())
            {
                test.BufferedReadStream.Position = test.BufferedReadStream.Length;

                var expectedPosition = test.BufferedReadStream.Length + offset;

                var newPosition = test.BufferedReadStream.Seek(offset, SeekOrigin.End);

                Assert.Equal(expectedPosition, newPosition);
                Assert.Equal(expectedPosition, test.BufferedReadStream.Position);
            }
        }

        [Fact]
        public void SetLength_Throws()
        {
            using (var test = CreateTest())
            {
                Assert.Throws<NotSupportedException>(() => test.BufferedReadStream.SetLength(value: 1));
            }
        }

        [Fact]
        public void Write_Throws()
        {
            using (var test = CreateTest())
            {
                var buffer = new byte[] { 0 };

                Assert.Throws<NotSupportedException>(
                    () => test.BufferedReadStream.Write(buffer, offset: 0, count: buffer.Length));
            }
        }

        [Fact]
        public void WriteByte_Throws()
        {
            using (var test = CreateTest())
            {
                Assert.Throws<NotSupportedException>(() => test.BufferedReadStream.WriteByte(value: 1));
            }
        }

        [Fact]
        public async Task WriteAsync_Throws()
        {
            using (var test = CreateTest())
            {
                var buffer = new byte[] { 0 };

                await Assert.ThrowsAsync<NotSupportedException>(
                    () => test.BufferedReadStream.WriteAsync(buffer, offset: 0, count: buffer.Length));
            }
        }

        private static byte[] CreateRandomBuffer(int bufferSize)
        {
            var buffer = new byte[bufferSize];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(buffer);
            }

            return buffer;
        }

        private static Test<FakeStream> CreateTest(
            bool leaveOpen = false,
            int bufferedStreamBufferSize = _bufferedStreamBufferSize)
        {
            return CreateTest(_defaultBuffer, leaveOpen, bufferedStreamBufferSize);
        }

        private static Test<FakeStream> CreateTest(
            byte[] buffer,
            bool leaveOpen = false,
            int bufferedStreamBufferSize = _bufferedStreamBufferSize)
        {
            return new Test<FakeStream>(buffer, new FakeStream(buffer), leaveOpen, bufferedStreamBufferSize);
        }

        private sealed class FakeStream : Stream
        {
            private readonly byte[] _buffer;
            private readonly bool _canRead;
            private readonly bool _canSeek;
            private readonly bool _canWrite;

            private long _position;

            public override bool CanRead
            {
                get
                {
                    ++CanReadCallCount;

                    return _canRead;
                }
            }

            public override bool CanSeek
            {
                get
                {
                    ++CanSeekCallCount;

                    return _canSeek;
                }
            }

            public override bool CanTimeout
            {
                get
                {
                    ++CanTimeoutCallCount;

                    return false;
                }
            }

            public override bool CanWrite
            {
                get
                {
                    ++CanWriteCallCount;

                    return _canWrite;
                }
            }

            public override long Length
            {
                get
                {
                    ++LengthCallCount;

                    return _buffer.Length;
                }
            }

            public override long Position
            {
                get => _position;
                set => _position = value;
            }

            public override int ReadTimeout
            {
                get
                {
                    ++ReadTimeoutGetCallCount;

                    return 0;
                }
                set
                {
                    ++ReadTimeoutSetCallCount;
                }
            }

            public override int WriteTimeout
            {
                get
                {
                    ++WriteTimeoutGetCallCount;

                    return 0;
                }
                set
                {
                    ++WriteTimeoutSetCallCount;
                }
            }

            internal int CanReadCallCount { get; private set; }
            internal int CanSeekCallCount { get; private set; }
            internal int CanTimeoutCallCount { get; private set; }
            internal int CanWriteCallCount { get; private set; }
            internal int DisposeCallCount { get; private set; }
            internal int LengthCallCount { get; private set; }
            internal int ReadByteCallCount { get; private set; }
            internal int ReadCallCount { get; private set; }
            internal int ReadTimeoutGetCallCount { get; private set; }
            internal int ReadTimeoutSetCallCount { get; private set; }
            internal int WriteTimeoutGetCallCount { get; private set; }
            internal int WriteTimeoutSetCallCount { get; private set; }

            internal FakeStream(byte[] buffer) :
                this(buffer, canRead: true, canSeek: true, canWrite: false)
            {
            }

            internal FakeStream(bool canRead, bool canSeek, bool canWrite) :
                this(Array.Empty<byte>(), canRead, canSeek, canWrite)
            {
            }

            internal FakeStream(byte[] buffer, bool canRead, bool canSeek, bool canWrite)
            {
                _buffer = buffer;
                _canRead = canRead;
                _canSeek = canSeek;
                _canWrite = canWrite;
            }

            protected override void Dispose(bool disposing)
            {
                ++DisposeCallCount;

                base.Dispose(disposing);
            }

            public override void Flush()
            {
                throw new NotImplementedException();
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                ++ReadCallCount;

                Assert.NotNull(buffer);
                Assert.InRange(offset, 0, int.MaxValue);
                Assert.InRange(count, 0, int.MaxValue);
                Assert.InRange(count - offset, 0, buffer.Length);

                var bytesToCopy = (int)Math.Min(Length - _position, count);

                Buffer.BlockCopy(_buffer, (int)_position, buffer, offset, bytesToCopy);

                _position += bytesToCopy;

                return bytesToCopy;
            }

            public override int ReadByte()
            {
                ++ReadByteCallCount;

                Assert.InRange(_position, 0, _buffer.Length);

                var value = _buffer[_position];

                ++_position;

                return value;
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                switch (origin)
                {
                    case SeekOrigin.Begin:
                        Assert.InRange(offset, 0, long.MaxValue);

                        _position = offset;
                        break;

                    case SeekOrigin.Current:
                        Assert.InRange(_position + offset, 0, long.MaxValue);

                        _position += offset;
                        break;

                    case SeekOrigin.End:
                        Assert.InRange(Length + offset, 0, long.MaxValue);

                        _position = Length + offset;
                        break;

                    default:
                        break;
                }

                return _position;
            }

            public override void SetLength(long value)
            {
                throw new NotImplementedException();
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                throw new NotImplementedException();
            }
        }

        private sealed class Test<T> : IDisposable where T : Stream
        {
            private readonly byte[] _underlyingBuffer;
            private bool _isDisposed;

            internal T UnderlyingStream { get; }
            internal ReadOnlyBufferedStream BufferedReadStream { get; }

            internal Test(byte[] buffer, T stream, bool leaveOpen, int bufferedStreamBufferSize)
            {
                _underlyingBuffer = buffer;
                UnderlyingStream = stream;
                BufferedReadStream = new ReadOnlyBufferedStream(stream, leaveOpen, bufferedStreamBufferSize);
            }

            internal void AssertBufferCorrectness(
                byte[] actualBuffer,
                long startingPositionInUnderlyingBuffer,
                long length)
            {
                for (var i = startingPositionInUnderlyingBuffer; i < length; ++i)
                {
                    Assert.Equal(_underlyingBuffer[i], actualBuffer[i - startingPositionInUnderlyingBuffer]);
                }
            }

            internal void AssertBufferCorrectness(
                byte[] actualBuffer,
                long startingPositionInUnderlyingBuffer,
                long startingPositionInDestinationBuffer,
                long length)
            {
                for (var i = 0; i < startingPositionInDestinationBuffer; ++i)
                {
                    Assert.Equal(0, actualBuffer[i]);
                }

                for (var i = startingPositionInUnderlyingBuffer; i < length; ++i)
                {
                    var expectedByte = _underlyingBuffer[i];
                    var actualByte = actualBuffer[startingPositionInDestinationBuffer + i - startingPositionInUnderlyingBuffer];
                    Assert.Equal(expectedByte, actualByte);
                }

                for (var i = startingPositionInDestinationBuffer + length; i < actualBuffer.Length; ++i)
                {
                    Assert.Equal(0, actualBuffer[i]);
                }
            }

            public void Dispose()
            {
                if (!_isDisposed)
                {
                    UnderlyingStream.Dispose();
                    BufferedReadStream.Dispose();

                    GC.SuppressFinalize(this);

                    _isDisposed = true;
                }
            }
        }
    }
}
