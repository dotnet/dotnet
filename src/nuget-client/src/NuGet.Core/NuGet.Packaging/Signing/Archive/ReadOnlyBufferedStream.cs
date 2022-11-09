// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace NuGet.Packaging.Signing
{
    public sealed class ReadOnlyBufferedStream : Stream
    {
        private const int _defaultBufferSize = 4096;

        private readonly byte[] _buffer;
        private readonly bool _leaveOpen;
        private readonly Lazy<long> _length;
        private readonly Stream _stream;

        private long _bufferStartPosition;
        private int _bufferFillLength;
        private bool _isDisposed;
        private long _position;

        public override bool CanRead => _stream.CanRead;
        public override bool CanSeek => _stream.CanSeek;
        public override bool CanTimeout => _stream.CanTimeout;
        public override bool CanWrite => false;
        public override long Length => _length.Value;

        public override long Position
        {
            get
            {
                ThrowIfDisposed();

                return _position;
            }
            set
            {
                ThrowIfDisposed();

                // For stream implementations that support seeking, the Stream contract (per MSDN) is that
                // "[s]eeking to any location beyond the length of the stream is supported."
                // So, Position > Length is legal.
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                _position = value;
            }
        }

        public override int ReadTimeout
        {
            get
            {
                ThrowIfDisposed();

                return _stream.ReadTimeout;
            }
            set
            {
                ThrowIfDisposed();

                _stream.ReadTimeout = value;
            }
        }

        public override int WriteTimeout
        {
            get
            {
                ThrowIfDisposed();

                return _stream.WriteTimeout;
            }
            set
            {
                ThrowIfDisposed();

                _stream.WriteTimeout = value;
            }
        }

        public ReadOnlyBufferedStream(Stream stream, bool leaveOpen) :
            this(stream, leaveOpen, _defaultBufferSize)
        {
        }

        public ReadOnlyBufferedStream(Stream stream, bool leaveOpen, int bufferSize)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (!stream.CanRead)
            {
                throw new ArgumentException(Strings.StreamMustBeReadable, nameof(stream));
            }

            if (!stream.CanSeek)
            {
                throw new ArgumentException(Strings.StreamMustBeSeekable, nameof(stream));
            }

            if (bufferSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bufferSize));
            }

            _buffer = new byte[bufferSize];
            _stream = stream;
            _leaveOpen = leaveOpen;

            // Repeated calls to the Length property were found to be costly.
            _length = new Lazy<long>(() => _stream.Length);
        }

        public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            return base.CopyToAsync(destination, bufferSize, cancellationToken);
        }

        public override void Flush()
        {
            ThrowIfDisposed();

            // This is a read-only stream.
            throw new NotSupportedException();
        }

        public override Task FlushAsync(CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            // This is a read-only stream.
            throw new NotSupportedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            ThrowIfDisposed();

            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            if (count > buffer.Length - offset)
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.RangeOutOfBoundsForArray,
                        nameof(offset),
                        nameof(count)));
            }

            var bytesRead = 0;
            var destinationOffset = offset;
            var bytesWanted = count;

            while (bytesWanted > 0 && !IsPositionAfterEndOfStream())
            {
                int sourceStart;
                int bytesToCopy;

                if (IsPositionInBuffer())
                {
                    sourceStart = (int)(Position - _bufferStartPosition);
                }
                else
                {
                    FillBuffer();

                    sourceStart = 0;
                }

                bytesToCopy = (int)Math.Min(_bufferStartPosition + _bufferFillLength - Position, bytesWanted);

                Debug.Assert(bytesToCopy >= 0);

                if (bytesToCopy <= 0)
                {
                    break;
                }

                Buffer.BlockCopy(_buffer, sourceStart, buffer, destinationOffset, bytesToCopy);

                destinationOffset += bytesToCopy;
                bytesRead += bytesToCopy;
                bytesWanted -= bytesToCopy;

                Position += bytesToCopy;
            }

            return bytesRead;
        }

        public override int ReadByte()
        {
            ThrowIfDisposed();

            int offset;

            if (IsPositionInBuffer())
            {
                offset = (int)(Position - _bufferStartPosition);
            }
            else if (IsPositionAfterEndOfStream())
            {
                return -1;
            }
            else
            {
                FillBuffer();

                offset = 0;
            }

            var value = _buffer[offset];

            ++Position;

            return value;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            ThrowIfDisposed();

            long newOffset;

            switch (origin)
            {
                case SeekOrigin.Current:
                    newOffset = Position + offset;
                    break;

                case SeekOrigin.Begin:
                    newOffset = offset;
                    break;

                case SeekOrigin.End:
                    newOffset = Length + offset;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(origin));
            }

            if (newOffset < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            Position = _stream.Seek(newOffset, SeekOrigin.Begin);

            return Position;
        }

        public override void SetLength(long value)
        {
            ThrowIfDisposed();

            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            ThrowIfDisposed();

            throw new NotSupportedException();
        }

        public override void WriteByte(byte value)
        {
            ThrowIfDisposed();

            throw new NotSupportedException();
        }

        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            throw new NotSupportedException();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!_isDisposed)
                {
                    if (!_leaveOpen)
                    {
                        _stream.Dispose();
                    }

                    _isDisposed = true;
                }
            }

            base.Dispose(disposing);
        }

        private void FillBuffer()
        {
            _bufferStartPosition = Position;
            _bufferFillLength = 0;

            _stream.Position = _bufferStartPosition;

            var totalBytesRead = 0;
            var bytesRead = 0;
            var offset = 0;
            var count = _buffer.Length;

            // Read(...) does not guarantee that the requested number of bytes will be read, even if there are ample
            // bytes in the source.  From MSDN:
            //
            //     An implementation is free to return fewer bytes than requested even if the end of the stream has
            //     not been reached.
            do
            {
                bytesRead = _stream.Read(_buffer, offset, count);

                offset += bytesRead;
                count -= bytesRead;
                totalBytesRead += bytesRead;
            } while (bytesRead > 0 && count > 0);

            _bufferFillLength = totalBytesRead;
        }

        private bool IsPositionAfterEndOfStream()
        {
            return Position >= Length;
        }

        private bool IsPositionInBuffer()
        {
            return _bufferStartPosition <= Position && Position < _bufferStartPosition + _bufferFillLength;
        }

        private void ThrowIfDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(ReadOnlyBufferedStream));
            }
        }
    }
}
