// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Test.Utility
{
    public class ReadTimeoutHonoringSlowStream : SlowStream
    {
        private readonly Stream _innerStream;
        private readonly CancellationToken _cancellationToken;
        internal new bool _isDisposed = false; // internal for testing purposes

        public ReadTimeoutHonoringSlowStream(Stream innerStream)
            : this(innerStream, CancellationToken.None)
        {
        }

        public ReadTimeoutHonoringSlowStream(Stream innerStream, CancellationToken cancellationToken)
            : base(innerStream, cancellationToken)
        {
            _innerStream = innerStream;
            _cancellationToken = cancellationToken;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            OnRead?.Invoke(buffer, offset, count);
            var read = _innerStream.Read(buffer, offset, count);

            try
            {
                var expectedDelayInMs = DelayPerByte.TotalMilliseconds * read;
                if (ReadTimeout > expectedDelayInMs)
                {
                    Task.Delay(new TimeSpan(DelayPerByte.Ticks * read)).Wait(_cancellationToken);
                }
                else
                {
                    Task.Delay(ReadTimeout).Wait(_cancellationToken);
                    throw new IOException($"..timed out because no data was received for {ReadTimeout}ms.", new TimeoutException());
                }
            }
            catch (OperationCanceledException)
            {
            }

            return read;
        }

        public override int ReadTimeout { get; set; } = Timeout.Infinite;
        public override bool CanTimeout => true;

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
