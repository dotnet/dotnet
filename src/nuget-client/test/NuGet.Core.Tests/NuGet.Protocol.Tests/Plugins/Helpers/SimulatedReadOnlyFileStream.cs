// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Threading;

namespace NuGet.Protocol.Plugins.Tests
{
    internal sealed class SimulatedReadOnlyFileStream : Stream
    {
        private readonly CancellationToken _cancellationToken;
        private readonly ManualResetEventSlim _dataWrittenEvent;
        private readonly MemoryStream _stream;
        private readonly SemaphoreSlim _readWriteSemaphore;

        public override bool CanRead => true;
        public override bool CanSeek => false;
        public override bool CanTimeout => false;
        public override bool CanWrite => false;
        public override long Length => throw new NotSupportedException();
        public override long Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }
        public override int ReadTimeout { get => throw new InvalidOperationException(); set => throw new InvalidOperationException(); }
        public override int WriteTimeout { get => throw new InvalidOperationException(); set => throw new InvalidOperationException(); }

        internal SimulatedReadOnlyFileStream(
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
            _dataWrittenEvent.Wait(_cancellationToken);

            try
            {
                _readWriteSemaphore.Wait(_cancellationToken);

                return _stream.Read(buffer, offset, count);
            }
            finally
            {
                if (_stream.Position == _stream.Length)
                {
                    _dataWrittenEvent.Reset();
                }

                _readWriteSemaphore.Release();
            }
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
            throw new NotSupportedException();
        }
    }
}
