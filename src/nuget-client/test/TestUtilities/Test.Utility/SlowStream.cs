// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Test.Utility
{
    public class SlowStream : Stream
    {
        private readonly Stream _innerStream;
        private readonly CancellationToken _cancellationToken;
        internal bool _isDisposed = false; // internal for testing purposes

        public SlowStream(Stream innerStream)
            : this(innerStream, CancellationToken.None)
        {
        }

        public SlowStream(Stream innerStream, CancellationToken cancellationToken)
        {
            _innerStream = innerStream;
            _cancellationToken = cancellationToken;
        }

        public TimeSpan DelayPerByte { get; set; }
        public Action<byte[], int, int> OnRead { get; set; }

        public override void Flush()
        {
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            OnRead?.Invoke(buffer, offset, count);
            var read = _innerStream.Read(buffer, offset, count);

            try
            {
                Task.Delay(new TimeSpan(DelayPerByte.Ticks * read)).Wait(_cancellationToken);
            }
            catch (OperationCanceledException)
            {
            }

            return read;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => false;

        public override long Length
        {
            get { throw new NotSupportedException(); }
        }

        public override long Position
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (_isDisposed)
            {
                return;
            }

            if (disposing)
            {
                // free managed resources
                _innerStream.Dispose();
            }

            _isDisposed = true;
        }
    }
}
