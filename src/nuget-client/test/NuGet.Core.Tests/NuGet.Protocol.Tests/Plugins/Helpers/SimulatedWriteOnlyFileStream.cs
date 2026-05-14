// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Threading;

namespace NuGet.Protocol.Plugins.Tests
{
    internal sealed class SimulatedWriteOnlyFileStream : Stream
    {
        private readonly CancellationToken _cancellationToken;
        private readonly ManualResetEventSlim _dataWrittenEvent;
        private readonly MemoryStream _stream;
        private readonly SemaphoreSlim _readWriteSemaphore;
        private long _writePosition;

        public override bool CanRead => false;
        public override bool CanSeek => false;
        public override bool CanTimeout => false;
        public override bool CanWrite => true;
        public override long Length => throw new NotSupportedException();
        public override long Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }
        public override int ReadTimeout { get => throw new InvalidOperationException(); set => throw new InvalidOperationException(); }
        public override int WriteTimeout { get => throw new InvalidOperationException(); set => throw new InvalidOperationException(); }

        internal SimulatedWriteOnlyFileStream(
            MemoryStream stream,
            SemaphoreSlim readWriteSemaphore,
            ManualResetEventSlim dataWrittenEvent,
            CancellationToken cancellationToken)
        {
            _stream = stream;
            _readWriteSemaphore = readWriteSemaphore;
            _dataWrittenEvent = dataWrittenEvent;
            _cancellationToken = cancellationToken;
        }

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            try
            {
                _readWriteSemaphore.Wait(_cancellationToken);

                //System.Diagnostics.Debug.WriteLine($"Position:  {_stream.Position}.  Length:  {_stream.Length}.  Writing...");

                if (_stream.Position == _writePosition)
                {
                    // Read has caught up with write.
                    // Reset the stream so it doesn't grow indefinitely.
                    _stream.Position = 0;
                    _stream.SetLength(0);
                    _writePosition = 0;

                    //System.Diagnostics.Debug.WriteLine($"Position:  {_stream.Position}.  Length:  {_stream.Length}.  Reset stream.");
                }

                var readPosition = _stream.Position;

                _stream.Position = _writePosition;

                _stream.Write(buffer, offset, count);

                _writePosition = _stream.Position;

                _stream.Position = readPosition;

                //System.Diagnostics.Debug.WriteLine($"Position:  {_stream.Position}.  Length:  {_stream.Length}.  Wrote {count} bytes.");

                _dataWrittenEvent.Set();
            }
            finally
            {
                _readWriteSemaphore.Release();
            }
        }
    }
}
